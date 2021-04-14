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
  }
}
