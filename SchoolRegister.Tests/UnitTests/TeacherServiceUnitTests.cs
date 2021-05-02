using System;
using SchoolRegister.DAL.EF;
using SchoolRegister.Services.Interfaces;
using SchoolRegister.ViewModels.VM;
using Xunit;

namespace SchoolRegister.Tests.UnitTests {
  public class TeacherServiceUnitTests : BaseUnitTests {
    private readonly ITeacherService _teacherService;

    public TeacherServiceUnitTests(ApplicationDbContext dbContext, ITeacherService teacherService) : base(dbContext) {
      _teacherService = teacherService;
    }
        
    [Fact]
    public void AddTeacher() {
      var addOrUpdateTeacherVm = new AddOrUpdateTeacherVm {
        FirstName = "Jolanta",
        LastName = "Lojalna",
        Title = "mgr inż.",
        RegistrationDate = new DateTime(2020, 01, 10)
      };

      var teacher = _teacherService.AddOrUpdateTeacher(addOrUpdateTeacherVm);

      Assert.NotNull(teacher);
      Assert.Equal("Jolanta Lojalna", teacher.TeacherName);
      Assert.Equal("mgr inż.", teacher.Title);
    }

    [Fact]
    public void UpdateTeacher() {
      var addOrUpdateTeacherVm = new AddOrUpdateTeacherVm {
        Id = 2,
        Title = "dr inż."
      };

      var teacher = _teacherService.AddOrUpdateTeacher(addOrUpdateTeacherVm);

      Assert.NotNull(teacher);
      Assert.Equal(2, teacher.Id);
      Assert.Equal("dr inż.", teacher.Title);
    }

    [Fact]
    public void UpdateTeacherWithInvalidId() {
      var addOrUpdateTeacherVm = new AddOrUpdateTeacherVm {
        Id = 5,
        Title = "dr inż."
      };

      Assert.Throws<InvalidOperationException>(() => _teacherService.AddOrUpdateTeacher(addOrUpdateTeacherVm));
    }

    [Fact]
    public void UpdateSubjectWithNullArgumentShouldThrowException() {
      Assert.Throws<ArgumentNullException>(() => _teacherService.AddOrUpdateTeacher(null));
    }
    
    [Fact]
    public void GetTeacher() {
      var teacher = _teacherService.GetTeacher(s => s.Id == 1);

      Assert.NotNull(teacher);
      Assert.Equal("Adam Bednarski", teacher.TeacherName);
      Assert.Equal("mgr inż.", teacher.Title);
    }

    [Fact]
    public void GetTeacherForNonExistentResults() {
      var teacher = _teacherService.GetTeacher(s => s.Id == -1);

      Assert.Null(teacher);
    }

    [Fact]
    public void GetTeacherWithNullArgumentShouldThrowException() {
      Assert.Throws<ArgumentNullException>(() => _teacherService.GetTeacher(null));
    }

    [Fact]
    public void GetTeachers() {
      var teachers = _teacherService.GetTeachers(g => g.Id == 1);

      Assert.NotNull(teachers);
      Assert.NotEmpty(teachers);
      Assert.All(teachers, vm => Assert.Equal(1, vm.Id));
    }

    [Fact]
    public void GetTeachersWithNullArgument() {
      var group = _teacherService.GetTeachers();

      Assert.NotNull(group);
      Assert.NotEmpty(group);
    }

    [Fact]
    public void GetTeachersForNonExistentResults() {
      var teachers = _teacherService.GetTeachers(s => s.Id == -1);

      Assert.NotNull(teachers);
      Assert.Empty(teachers);
    }
  }
}
