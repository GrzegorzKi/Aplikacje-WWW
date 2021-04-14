using System;
using System.Linq;
using SchoolRegister.DAL.EF;
using SchoolRegister.Model.DataModels;
using SchoolRegister.Services.Interfaces;
using SchoolRegister.ViewModels.VM;
using Xunit;

namespace SchoolRegister.Tests.UnitTests {
  public class GradeServiceUnitTests : BaseUnitTests {
    private readonly IGradeService _gradeService;

    public GradeServiceUnitTests(ApplicationDbContext dbContext, IGradeService gradeService) : base(dbContext) {
      _gradeService = gradeService;
    }

    [Fact]
    public void AddGradeToStudent() {
      var gradeVm = new AddGradeToStudentVm {
        StudentId = 5,
        SubjectId = 1,
        GradeValue = GradeScale.DB,
        TeacherId = 1
      };

      var gradeTask = _gradeService.AddGradeToStudent(gradeVm);
      var grade = gradeTask.Result;

      Assert.NotNull(grade);
      Assert.Equal(2, DbContext.Grades.Count());
    }

    [Fact]
    public void AddGradeToStudentWithUserOtherThanTeacherShouldThrowException() {
      var gradeVm = new AddGradeToStudentVm {
        StudentId = 5,
        SubjectId = 1,
        GradeValue = GradeScale.BDB,
        TeacherId = 5
      };

      var gradeTask = _gradeService.AddGradeToStudent(gradeVm);
      var exception = Assert.Throws<AggregateException>(() => gradeTask.Result);
      Assert.IsType<InvalidOperationException>(exception.InnerExceptions.First());
    }

    [Fact]
    public void AddGradeWithNullArgumentShouldThrowException() {
      var gradeTask = _gradeService.AddGradeToStudent(null);
      var exception = Assert.Throws<AggregateException>(() => gradeTask.Result);
      Assert.IsType<ArgumentNullException>(exception.InnerExceptions.First());
    }

    [Fact]
    public void GetGradeWithNonExistentUser() {
      var grade = _gradeService.GetGrade(g => g.StudentId == 0);
      Assert.Null(grade);
    }

    [Fact]
    public void GetGrade() {
      var grade = _gradeService.GetGrade(g => g.StudentId == 5);
      Assert.NotNull(grade);
    }

    [Fact]
    public void GetGradeWithNullArgumentShouldThrowException() {
      Assert.Throws<ArgumentNullException>(() => _gradeService.GetGrade(null));
    }

    [Fact]
    public void GetGrades() {
      var grades = _gradeService.GetGrades(g => g.StudentId == 5);

      Assert.NotNull(grades);
      Assert.NotEmpty(grades);
    }

    [Fact]
    public void GetGradesWithNullArgument() {
      var grades = _gradeService.GetGrades(null);

      Assert.NotNull(grades);
      Assert.NotEmpty(grades);
    }

    [Fact]
    public void GetGradesForNonExistentUser() {
      var grades = _gradeService.GetGrades(g => g.StudentId == 0);

      Assert.NotNull(grades);
      Assert.Empty(grades);
    }

    [Fact]
    public void GetGradesReportForStudentByTeacher() {
      var getGradesReportForStudent = new GetGradesReportVm {
        StudentId = 5,
        GetterUserId = 1
      };

      var gradesReportTask = _gradeService.GetGradesReportForStudent(getGradesReportForStudent);
      var gradesReport = gradesReportTask.Result;

      Assert.NotNull(gradesReport);
      Assert.NotEmpty(gradesReport);
    }

    [Fact]
    public void GetGradesReportForStudentByStudent() {
      var getGradesReportForStudent = new GetGradesReportVm {
        StudentId = 5,
        GetterUserId = 5
      };

      var gradesReportTask = _gradeService.GetGradesReportForStudent(getGradesReportForStudent);
      var gradesReport = gradesReportTask.Result;

      Assert.NotNull(gradesReport);
      Assert.NotEmpty(gradesReport);
    }

    [Fact]
    public void GetGradesReportForStudentByParent() {
      var getGradesReportForStudent = new GetGradesReportVm {
        StudentId = 5,
        GetterUserId = 3
      };

      var gradesReportTask = _gradeService.GetGradesReportForStudent(getGradesReportForStudent);
      var gradesReport = gradesReportTask.Result;

      Assert.NotNull(gradesReport);
      Assert.NotEmpty(gradesReport);
    }

    [Fact]
    public void GetGradesReportForStudentWithNullArgumentShouldThrowException() {
      var gradeTask = _gradeService.GetGradesReportForStudent(null);
      var exception = Assert.Throws<AggregateException>(() => gradeTask.Result);
      Assert.IsType<ArgumentNullException>(exception.InnerExceptions.First());
    }

    [Fact]
    public void GetGradesReportForStudentWithGetterUserOtherThanAcceptedShouldThrowException() {
      var getGradesReportVm = new GetGradesReportVm {
        StudentId = 5,
        GetterUserId = 11 // Admin
      };

      var gradeTask = _gradeService.GetGradesReportForStudent(getGradesReportVm);
      var exception = Assert.Throws<AggregateException>(() => gradeTask.Result);
      Assert.IsType<InvalidOperationException>(exception.InnerExceptions.First());
    }
  }
}
