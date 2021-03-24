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

    public StudentVm AddStudentToGroup(AddStudentToGroupVm addStudentToGroupVm) {
      try {
        if (addStudentToGroupVm == null)
          throw new ArgumentNullException(nameof(addStudentToGroupVm), "View model parameter is null");

        var studentEntity = DbContext.Students.First(s => s.Id == addStudentToGroupVm.StudentId);
        if (studentEntity.Group != null) {
          throw new InvalidOperationException("Student is already in a group. Remove group first before assigning new one");
        }

        studentEntity.GroupId = addStudentToGroupVm.GroupId;
        DbContext.SaveChanges();

        return Mapper.Map<StudentVm>(studentEntity);
      }
      catch (Exception ex) {
        Logger.LogError(ex, ex.Message);
        throw;
      }
    }

    public StudentVm RemoveStudentFromGroup(RemoveStudentFromGroupVm removeStudentFromGroupVm) {
      try {
        if (removeStudentFromGroupVm == null)
          throw new ArgumentNullException(nameof(removeStudentFromGroupVm), "View model parameter is null");

        var studentEntity = DbContext.Students.First(s => s.Id == removeStudentFromGroupVm.StudentId);
        if (studentEntity.GroupId != removeStudentFromGroupVm.GroupId) {
          throw new InvalidOperationException($"Student is not in a group of id {removeStudentFromGroupVm.GroupId}");
        }

        studentEntity.GroupId = null;
        DbContext.SaveChanges();

        return Mapper.Map<StudentVm>(studentEntity);
      }
      catch (Exception ex) {
        Logger.LogError(ex, ex.Message);
        throw;
      }
    }
  }
}
