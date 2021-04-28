using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using SchoolRegister.DAL.EF;
using SchoolRegister.Model.DataModels;
using SchoolRegister.Services.Interfaces;
using SchoolRegister.ViewModels.VM;

namespace SchoolRegister.Services.Services {
  public class GradeService : BaseService, IGradeService {
    private UserManager<User> UserManager { get; }
    public GradeService(ApplicationDbContext dbContext, IMapper mapper, ILogger logger, UserManager<User> userManager)
      : base(dbContext, mapper, logger) {
      UserManager = userManager;
    }

    public async Task<GradeVm> AddGradeToStudent(AddGradeToStudentVm addGradeToStudentVm) {
      try {
        if (addGradeToStudentVm == null)
          throw new ArgumentNullException(nameof(addGradeToStudentVm), "View model parameter is null");

        var teacher = DbContext.Users.OfType<Teacher>().FirstOrDefault(t => t.Id == addGradeToStudentVm.TeacherId);
        var student = DbContext.Users.OfType<Student>().FirstOrDefault(s => s.Id == addGradeToStudentVm.StudentId);

        if (teacher == null || !await UserManager.IsInRoleAsync(teacher, "Teacher")) {
          throw new InvalidOperationException("TeacherId must correspond to user with \"Teacher\" role");
        }
        if (student == null || !await UserManager.IsInRoleAsync(student, "Student")) {
          throw new InvalidOperationException("StudentId must correspond to user with \"Student\" role");
        }

        var subject = DbContext.Subjects.FirstOrDefault(s => s.Id == addGradeToStudentVm.SubjectId);

        if (subject == null) {
          throw new InvalidOperationException($"Subject with id {addGradeToStudentVm.SubjectId} not found");
        }
        if (!teacher.Subjects.Contains(subject)) {
          throw new InvalidOperationException($"Subject with id {addGradeToStudentVm.SubjectId} is not assigned to teacher {teacher.FirstName} {teacher.LastName}");
        }

        var gradeEntity = Mapper.Map<Grade>(addGradeToStudentVm);

        await DbContext.Grades.AddAsync(gradeEntity);
        await DbContext.SaveChangesAsync();

        return Mapper.Map<GradeVm>(gradeEntity);
      }
      catch (Exception ex) {
        Logger.LogError(ex, ex.Message);
        throw;
      }
    }

    public GradeVm GetGrade(Expression<Func<Grade, bool>> filterExpression) {
      try {
        if (filterExpression == null) {
          throw new ArgumentNullException(nameof(filterExpression), "FilterExpression is null");
        }

        var gradeEntity = DbContext.Grades.FirstOrDefault(filterExpression);

        return Mapper.Map<GradeVm>(gradeEntity);
      }
      catch (Exception ex) {
        Logger.LogError(ex, ex.Message);
        throw;
      }
    }

    public IEnumerable<GradeVm> GetGrades(Expression<Func<Grade, bool>> filterExpression = null) {
      try {
        var gradesEntities = DbContext.Grades.AsQueryable();
        if (filterExpression != null) gradesEntities = gradesEntities.Where(filterExpression);

        return Mapper.Map<IEnumerable<GradeVm>>(gradesEntities);
      }
      catch (Exception ex) {
        Logger.LogError(ex, ex.Message);
        throw;
      }
    }

    public async Task<IEnumerable<GradeVm>> GetGradesReportForStudent(GetGradesReportVm getGradesReportVm) {
      try {
        if (getGradesReportVm == null) {
          throw new ArgumentNullException(nameof(getGradesReportVm), "GetGradesReportVm is null");
        }

        var student = DbContext.Users.OfType<Student>().FirstOrDefault(t => t.Id == getGradesReportVm.StudentId);

        if (student == null || !await UserManager.IsInRoleAsync(student, "Student")) {
          throw new InvalidOperationException("StudentId must correspond to user with \"Student\" role");
        }

        var getterUser = DbContext.Users.FirstOrDefault(t => t.Id == getGradesReportVm.GetterUserId);

        if (getterUser == null
            || !(await UserManager.IsInRoleAsync(getterUser, "Student")
                 || await UserManager.IsInRoleAsync(getterUser, "Parent")
                 || await UserManager.IsInRoleAsync(getterUser, "Teacher"))) {
          throw new InvalidOperationException("GetterUserId must correspond to user with any of roles: \"Student\", \"Parent\", \"Teacher\"");
        }

        if (await UserManager.IsInRoleAsync(getterUser, "Student") &&
            getGradesReportVm.StudentId != getGradesReportVm.GetterUserId) {
          throw new InvalidOperationException("Student can browse only his own grades");
        }

        if (await UserManager.IsInRoleAsync(getterUser, "Teacher")) {
          var teacher = (Teacher) getterUser;
          var filteredGrades = student.Grades.Where(grade => teacher.Subjects.Contains(grade.Subject)).OrderBy(grade => grade.DateOfIssue);
          return Mapper.Map<IEnumerable<GradeVm>>(filteredGrades);
        }

        return Mapper.Map<IEnumerable<GradeVm>>(student.Grades.OrderBy(grade => grade.DateOfIssue));
      }
      catch (Exception ex) {
        Logger.LogError(ex, ex.Message);
        throw;
      }
    }
  }
}
