using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SchoolRegister.Model.DataModels;

// dotnet ef migrations add <name> --project .\SchoolRegister.DAL\SchoolRegister.DAL.csproj --startup-project .\SchoolRegister.Web\SchoolRegister.Web.csproj
// dotnet ef database update --project .\SchoolRegister.DAL\SchoolRegister.DAL.csproj --startup-project .\SchoolRegister.Web\SchoolRegister.Web.csproj

namespace SchoolRegister.DAL.EF {
  public class ApplicationDbContext : IdentityDbContext<User, Role, int> {
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
      : base(options) { }

    // Table properties e.g
    public virtual DbSet<Group> Groups { get; set; }
    public virtual DbSet<Subject> Subjects { get; set; }
    public virtual DbSet<SubjectGroup> SubjectGroups { get; set; }
    public virtual DbSet<Grade> Grades { get; set; }
    public virtual DbSet<Parent> Parents { get; set; }
    public virtual DbSet<Student> Students { get; set; }
    public virtual DbSet<Teacher> Teachers { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
      base.OnConfiguring(optionsBuilder);
      //configuration commands
      optionsBuilder.UseLazyLoadingProxies(); //enable lazy loading proxies
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder) {
      base.OnModelCreating(modelBuilder);
      // Fluent API commands
      modelBuilder.Entity<User>()
        .ToTable("AspNetUsers")
        .HasDiscriminator<int>("UserType")
        .HasValue<User>((int) RoleValue.User)
        .HasValue<Student>((int) RoleValue.Student)
        .HasValue<Parent>((int) RoleValue.Parent)
        .HasValue<Teacher>((int) RoleValue.Teacher);

      modelBuilder.Entity<Grade>()
        .HasKey(g => new {g.DateOfIssue, g.SubjectId, g.StudentId});

      modelBuilder.Entity<SubjectGroup>()
        .HasKey(sg => new {sg.GroupId, sg.SubjectId});

      modelBuilder.Entity<Subject>()
        .HasKey(s => s.Id);

      modelBuilder.Entity<Group>()
        .HasKey(s => s.Id);

      modelBuilder.Entity<Subject>()
        .HasMany(t => t.Grades)
        .WithOne(s => s.Subject)
        .HasForeignKey(x => x.SubjectId)
        .IsRequired();

      modelBuilder.Entity<Subject>()
        .HasMany(t => t.SubjectGroups)
        .WithOne(s => s.Subject)
        .HasForeignKey(x => x.SubjectId)
        .IsRequired();

      modelBuilder.Entity<Group>()
        .HasMany(g => g.Students)
        .WithOne(s => s.Group)
        .HasForeignKey(x => x.GroupId);

      modelBuilder.Entity<Group>()
        .HasMany(g => g.SubjectGroups)
        .WithOne(s => s.Group)
        .HasForeignKey(x => x.GroupId)
        .IsRequired();

      modelBuilder.Entity<Student>()
        .HasMany(s => s.Grades)
        .WithOne(g => g.Student)
        .HasForeignKey(x => x.StudentId)
        .IsRequired();

      modelBuilder.Entity<Parent>()
        .HasMany(s => s.Students)
        .WithOne(g => g.Parent)
        .HasForeignKey(x => x.ParentId)
        .IsRequired()
        .OnDelete(DeleteBehavior.NoAction);

      modelBuilder.Entity<Teacher>()
        .HasMany(t => t.Subjects)
        .WithOne(s => s.Teacher)
        .HasForeignKey(x => x.TeacherId)
        .IsRequired()
        .OnDelete(DeleteBehavior.NoAction);
    }
  }
}
