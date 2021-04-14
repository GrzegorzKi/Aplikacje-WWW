using System;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using SchoolRegister.DAL.EF;
using SchoolRegister.Model.DataModels;

namespace SchoolRegister.Tests {
  public static class Extensions {
    // Create sample data
    public static async void SeedData(this IServiceCollection services) {
      var serviceProvider = services.BuildServiceProvider();
      var dbContext = serviceProvider.GetRequiredService<ApplicationDbContext>();
      var userManager = serviceProvider.GetRequiredService<UserManager<User>>();
      var roleManager = serviceProvider.GetRequiredService<RoleManager<Role>>();

      // Roles
      var studentRole = new Role {Id = 1, Name = "Student", RoleValue = RoleValue.Student};
      var parentRole = new Role {Id = 2, Name = "Parent", RoleValue = RoleValue.Parent};
      var teacherRole = new Role {Id = 3, Name = "Teacher", RoleValue = RoleValue.Teacher};
      var adminRole = new Role {Id = 4, Name = "Admin", RoleValue = RoleValue.Admin};

      // Groups
      var groupIo = new Group {Id = 1, Name = "IO"};
      var groupPai = new Group {Id = 2, Name = "PAI"};
      var groupAipErasmus = new Group {Id = 3, Name = "AIPErasmus"};

      await roleManager.CreateAsync(studentRole);
      await roleManager.CreateAsync(parentRole);
      await roleManager.CreateAsync(teacherRole);
      await roleManager.CreateAsync(adminRole);
      await dbContext.Groups.AddAsync(groupIo);
      await dbContext.Groups.AddAsync(groupPai);
      await dbContext.Groups.AddAsync(groupAipErasmus);

      // All users, including teachers, students and parents
      const string userPassword = "User1234"; // in lab env we use the same pass for all users :)
      var teachers = new Teacher[] {
        new() {
          Id = 1,
          FirstName = "Adam",
          LastName = "Bednarski",
          UserName = "t1@eg.eg",
          Email = "real_email@eg.eg",
          Title = "mgr inż.",
          RegistrationDate = new DateTime(2010, 1, 1)
        },
        new() {
          Id = 2,
          FirstName = "Jan",
          LastName = "Nowak",
          UserName = "t2@eg.eg",
          Email = "t2@eg.eg",
          Title = "mgr",
          RegistrationDate = new DateTime(2010, 11, 12)
        },
        new() {
          Id = 12,
          FirstName = "Stanisław",
          LastName = "Nowakowski",
          UserName = "t11@eg.eg",
          Email = "t11@eg.eg",
          Title = "mgr inż.",
          RegistrationDate = new DateTime(2010, 11, 12),
        }
      };

      foreach (var teacher in teachers) {
        await userManager.CreateAsync(teacher, userPassword);
        await userManager.AddToRoleAsync(teacher, teacherRole.Name);
      }

      var parents = new Parent[] {
        new() {
          Id = 3,
          FirstName = "Zbigniew",
          LastName = "Kowalski",
          UserName = "p1@eg.eg",
          Email = "real_email@eg.eg",
          RegistrationDate = new DateTime(2014, 03, 20)
        },
        new() {
          Id = 4,
          FirstName = "Anna",
          LastName = "Nowakowska",
          UserName = "p2@eg.eg",
          Email = "p2@eg.eg",
          RegistrationDate = new DateTime(2014, 06, 21)
        }
      };

      foreach (var parent in parents) {
        await userManager.CreateAsync(parent, userPassword);
        await userManager.AddToRoleAsync(parent, parentRole.Name);
      }

      var students = new Student[] {
        new() {
          Id = 5,
          FirstName = "Tomasz",
          LastName = "Kowalski",
          UserName = "s1@eg.eg",
          Email = "s1@eg.eg",
          RegistrationDate = new DateTime(2016, 05, 11),
          GroupId = 1,
          ParentId = 3
        },
        new() {
          Id = 6,
          FirstName = "Krzysztof",
          LastName = "Kowalski",
          UserName = "s2@eg.eg",
          Email = "s2@eg.eg",
          RegistrationDate = new DateTime(2015, 09, 18),
          GroupId = 1,
          ParentId = 3
        },
        new() {
          Id = 7,
          FirstName = "Natalia",
          LastName = "Kowalska",
          UserName = "s3@eg.eg",
          Email = "s3@eg.eg",
          RegistrationDate = new DateTime(2017, 07, 16),
          GroupId = 2,
          ParentId = 3
        },
        new() {
          Id = 8,
          FirstName = "Magdalena",
          LastName = "Wiśniewska",
          UserName = "s4@eg.eg",
          Email = "s4@eg.eg",
          RegistrationDate = new DateTime(2018, 05, 14),
          GroupId = 2,
          ParentId = 4
        },
        new() {
          Id = 9,
          FirstName = "Jan",
          LastName = "Wiśniewski",
          UserName = "s5@eg.eg",
          Email = "s5@eg.eg",
          RegistrationDate = new DateTime(2019, 02, 19),
          GroupId = 3,
          ParentId = 4
        },
        new() {
          Id = 10,
          FirstName = "Krystian",
          LastName = "Wiśniewski",
          UserName = "s6@eg.eg",
          Email = "s6@eg.eg",
          RegistrationDate = new DateTime(2019, 05, 1),
          GroupId = null,
          ParentId = 4
        }
      };

      foreach (var student in students) {
        await userManager.CreateAsync(student, userPassword);
        await userManager.AddToRoleAsync(student, studentRole.Name);
      }

      var admin = new User() {
        Id = 11,
        FirstName = "Jacek",
        LastName = "Kowalczyk",
        UserName = "a1@eg.eg",
        Email = "a1@eg.eg",
        RegistrationDate = new DateTime(2009, 1, 1)
      };
      await userManager.CreateAsync(admin, userPassword);
      await userManager.AddToRoleAsync(admin, adminRole.Name);

      // Subject
      var subjects = new Subject[] {
        new() {
          Id = 1,
          Name = "Aplikacje WWW",
          Description = "Aplikacje webowe",
          TeacherId = 1
        },
        new() {
          Id = 2,
          Name = "Programowanie obiektowe",
          Description = "Programowanie obiektowe jest przedmiotem realizującym przykłady programowanie obiektowego",
          TeacherId = 1
        },
        new() {
          Id = 3,
          Name = "Advanced Internet Programming",
          Description = "Advanced Internet Programming is a course for ERASMUS+ students",
          TeacherId = 2
        },
        new() {
          Id = 4,
          Name = "Administracja Internetowymi Systemami Baz Danych",
          Description =
            "Administracja Internetowymi Systemami Baz Danych jest kontynuacją przedmiotu Bazy danych na studiach stacjonarnych I-go stopnia spec. PAI",
          TeacherId = 2,
        },
        new() {
          Id = 5,
          Name = "Programowanie interaktywnej grafiki dla stron WWW",
          TeacherId = 12
        }
      };

      foreach (var subject in subjects) {
        await dbContext.AddAsync(subject);
      }

      // SubjectGroups
      var subjectGroups = new SubjectGroup[] {
        new() {
          SubjectId = 1,
          GroupId = 1
        },
        new() {
          SubjectId = 1,
          GroupId = 2
        },
        new() {
          SubjectId = 2,
          GroupId = 1
        },
        new() {
          SubjectId = 2,
          GroupId = 2
        },
        new() {
          SubjectId = 2,
          GroupId = 3
        },
        new() {
          SubjectId = 3,
          GroupId = 3
        },
        new() {
          SubjectId = 4,
          GroupId = 2
        },
        new() {
          SubjectId = 4,
          GroupId = 3
        }
      };

      foreach (var subjectGroup in subjectGroups) {
        await dbContext.SubjectGroups.AddAsync(subjectGroup);
      }

      var grade1 = new Grade {
        DateOfIssue = new DateTime(2019, 03, 21, 17, 46, 38),
        StudentId = 5,
        SubjectId = 1,
        GradeValue = GradeScale.DB
      };

      await dbContext.Grades.AddAsync(grade1);

      await dbContext.SaveChangesAsync();
    }
  }
}
