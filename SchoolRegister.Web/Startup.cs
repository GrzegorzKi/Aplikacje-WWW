using System.Net;
using System.Net.Mail;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SchoolRegister.DAL.EF;
using SchoolRegister.Model.DataModels;
using SchoolRegister.Services.Interfaces;
using SchoolRegister.Services.Services;

namespace SchoolRegister.Web {
  public class Startup {
    public Startup(IConfiguration configuration) {
      Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services) {
      services.AddAutoMapper(typeof(Startup));
      services.AddDbContext<ApplicationDbContext>(options =>
        options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"))
      );
      services.AddDatabaseDeveloperPageExceptionFilter();
      services.AddDefaultIdentity<User>(options => options.SignIn.RequireConfirmedAccount = false)
        .AddRoles<Role>()
        .AddRoleManager<RoleManager<Role>>()
        .AddUserManager<UserManager<User>>()
        .AddEntityFrameworkStores<ApplicationDbContext>();

      services.AddTransient(typeof(ILogger), typeof(Logger<Startup>));
      services.AddScoped<ISubjectService, SubjectService>();
      services.AddScoped<IEmailSenderService, EmailSenderService>();
      services.AddScoped<IGradeService, GradeService>();
      services.AddScoped<IGroupService, GroupService>();
      services.AddScoped<IStudentService, StudentService>();
      services.AddScoped<ITeacherService, TeacherService>();
      services.AddScoped(serviceProvider => {
        // Czy nie potrzeba nam też Singleton IConfiguration?
        var config = serviceProvider.GetRequiredService<IConfiguration>();
        return new SmtpClient {
          Host = config.GetValue<string>("Email:Smtp:Host"),
          Port = config.GetValue<int>("Email:Smtp:Port"),
          EnableSsl = true,
          DeliveryMethod = SmtpDeliveryMethod.Network,
          UseDefaultCredentials = false,
          Credentials = new NetworkCredential(
            config.GetValue<string>("Email:Smtp:Username"),
            config.GetValue<string>("Email:Smtp:Password")
          )
        };
      });
      services.AddControllersWithViews();
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env) {
      if (env.IsDevelopment()) {
        app.UseDeveloperExceptionPage();
        app.UseMigrationsEndPoint();
      } else {
        app.UseExceptionHandler("/Error");
        // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
        app.UseHsts();
      }

      app.UseHttpsRedirection();
      app.UseStaticFiles();

      app.UseRouting();

      app.UseAuthentication();
      app.UseAuthorization();

      app.UseEndpoints(endpoints =>
      {
        endpoints.MapControllerRoute(
          "default",
          "{controller=Home}/{action=Index}/{id?}");
        endpoints.MapRazorPages();
      });
    }
  }
}
