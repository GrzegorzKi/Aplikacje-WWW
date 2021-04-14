using System;
using SchoolRegister.DAL.EF;
using SchoolRegister.Services.Interfaces;
using SchoolRegister.ViewModels.VM;
using Xunit;

namespace SchoolRegister.Tests.UnitTests {
  public class StudentServiceUnitTests : BaseUnitTests {
    private readonly IStudentService _studentService;

    public StudentServiceUnitTests(ApplicationDbContext dbContext, IStudentService studentService) : base(dbContext) {
      _studentService = studentService;
    }

    [Fact]
    public void AddStudent() {
      var addOrUpdateStudentVm = new AddOrUpdateStudentVm {
        FirstName = "Jan",
        LastName = "Kowalski",
        GroupId = 1,
        ParentId = 4
      };

      var student = _studentService.AddOrUpdateStudent(addOrUpdateStudentVm);

      Assert.NotNull(student);
      Assert.Equal("Jan Kowalski", student.StudentName);
      Assert.Equal(1, student.Group.Id);
      Assert.Equal(4, student.Parent.Id);
    }

    [Fact]
    public void UpdateStudent() {
      var addOrUpdateStudentVm = new AddOrUpdateStudentVm {
        Id = 5,
        FirstName = "Tomasz",
        LastName = "Kowalski",
        GroupId = 1,
        ParentId = 4
      };

      var student = _studentService.AddOrUpdateStudent(addOrUpdateStudentVm);

      Assert.NotNull(student);
      Assert.Equal("Tomasz Kowalski", student.StudentName);
      Assert.Equal(1, student.Group.Id);
      Assert.Equal(4, student.Parent.Id);
    }

    [Fact]
    public void UpdateStudentWithNullArgumentShouldThrowException() {

      Assert.Throws<ArgumentNullException>(() => _studentService.AddOrUpdateStudent(null));
    }

    [Fact]
    public void GetStudent() {

      var student = _studentService.GetStudent(s => s.Id == 5);

      Assert.NotNull(student);
      Assert.Equal(5, student.Id);
    }

    [Fact]
    public void GetStudentWithNullArgumentShouldThrowException() {

      Assert.Throws<ArgumentNullException>(() => _studentService.GetStudent(null));
    }

    [Fact]
    public void GetStudents() {

      var students = _studentService.GetStudents(s => s.Id == 5);

      Assert.NotNull(students);
      Assert.NotEmpty(students);
      Assert.All(students, vm => Assert.Equal(5, vm.Id));
    }

    [Fact]
    public void GetStudentsWithNullArgument() {

      var students = _studentService.GetStudents(null);

      Assert.NotNull(students);
      Assert.NotEmpty(students);
    }

    [Fact]
    public void GetStudentsForNonExistentStudent() {
      var students = _studentService.GetStudents(s => s.Id == 0);

      Assert.NotNull(students);
      Assert.Empty(students);
    }
  }
}
