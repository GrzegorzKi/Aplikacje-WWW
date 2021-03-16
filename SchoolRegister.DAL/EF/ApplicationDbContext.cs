using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SchoolRegister.Model.DataModels;

namespace SchoolRegister.DAL.EF
{
  public class ApplicationDbContext : IdentityDbContext<User, Role, int>
  {
    // Table properties e.g
    public virtual DbSet<Group> Groups { get; set; }
    public virtual DbSet<Grade> Grades { get; set; }
    public virtual DbSet<Parent> Parents { get; set; }
    public virtual DbSet<Student> Students { get; set; }
    public virtual DbSet<Subject> Subjects { get; set; }
    public virtual DbSet<Teacher> Teachers { get; set; }

    // more properties need to added...
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
    : base(options)
    { }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
      base.OnConfiguring(optionsBuilder);
      //configuration commands
      optionsBuilder.UseLazyLoadingProxies(); //enable lazy loading proxies
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
      base.OnModelCreating(modelBuilder);
      // Fluent API commands
      modelBuilder.Entity<User>()
        .ToTable("AspNetUsers")
        .HasDiscriminator<int>("UserType")
        .HasValue<User>((int)RoleValue.User)
        .HasValue<Student>((int)RoleValue.Student)
        .HasValue<Parent>((int)RoleValue.Parent)
        .HasValue<Teacher>((int)RoleValue.Teacher);

      modelBuilder.Entity<Grade>()
        .HasKey(g => new {g.DateOfIssue, g.SubjectId, g.StudentId});

      modelBuilder.Entity<SubjectGroup>()
        .HasKey(sg => new {sg.GroupId, sg.SubjectId});
    }
  }
}
