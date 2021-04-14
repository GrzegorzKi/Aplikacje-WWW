using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using AutoMapper;
using Microsoft.Extensions.Logging;
using SchoolRegister.DAL.EF;
using SchoolRegister.Model.DataModels;
using SchoolRegister.Services.Interfaces;
using SchoolRegister.ViewModels.VM;

namespace SchoolRegister.Services.Services {
  public class StudentService : BaseService, IStudentService {
    public StudentService(ApplicationDbContext dbContext, IMapper mapper, ILogger logger)
      : base(dbContext, mapper, logger) { }

    public StudentVm AddOrUpdateStudent(AddOrUpdateStudentVm addOrUpdateStudentVm) {
      try {
        if (addOrUpdateStudentVm == null)
          throw new ArgumentNullException(nameof(addOrUpdateStudentVm), "View model parameter is null");

        var studentEntity = Mapper.Map<Student>(addOrUpdateStudentVm);
        if (!addOrUpdateStudentVm.Id.HasValue || addOrUpdateStudentVm.Id == 0)
          DbContext.Students.Add(studentEntity);
        else
          DbContext.Students.Update(studentEntity);

        DbContext.SaveChanges();
        return Mapper.Map<StudentVm>(studentEntity);
      }
      catch (Exception ex) {
        Logger.LogError(ex, ex.Message);
        throw;
      }
    }

    public StudentVm GetStudent(Expression<Func<Student, bool>> filterExpression) {
      try {
        if (filterExpression == null)
          throw new ArgumentNullException(nameof(filterExpression), "FilterExpression is null");

        var subjectEntity = DbContext.Students.FirstOrDefault(filterExpression);

        return Mapper.Map<StudentVm>(subjectEntity);
      }
      catch (Exception ex) {
        Logger.LogError(ex, ex.Message);
        throw;
      }
    }

    public IEnumerable<StudentVm> GetStudents(Expression<Func<Student, bool>> filterExpression = null) {
      try {
        var studentsEntities = DbContext.Students.AsQueryable();
        if (filterExpression != null) studentsEntities = studentsEntities.Where(filterExpression);

        return Mapper.Map<IEnumerable<StudentVm>>(studentsEntities);
      }
      catch (Exception ex) {
        Logger.LogError(ex, ex.Message);
        throw;
      }
    }
  }
}
