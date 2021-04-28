using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using SchoolRegister.Model.DataModels;
using SchoolRegister.ViewModels.VM;

namespace SchoolRegister.Services.Interfaces {
  public interface IGradeService {
    Task<GradeVm> AddGradeToStudent(AddGradeToStudentVm addGradeToStudentVm);
    GradeVm GetGrade(Expression<Func<Grade, bool>> filterExpression);
    IEnumerable<GradeVm> GetGrades(Expression<Func<Grade, bool>> filterExpression = null);
    Task<IEnumerable<GradeVm>> GetGradesReportForStudent(GetGradesReportVm getGradesReportVm);
  }
}
