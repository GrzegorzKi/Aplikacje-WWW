using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SchoolRegister.DAL.EF;
using SchoolRegister.Model.DataModels;
using SchoolRegister.Services.Interfaces;
using SchoolRegister.ViewModels.VM;

namespace SchoolRegister.Services.Services {
  public class GroupService : BaseService, IGroupService {
    public GroupService(ApplicationDbContext dbContext, IMapper mapper, ILogger logger)
      : base(dbContext, mapper, logger) { }

    public GroupVm AddOrUpdateGroup(AddOrUpdateGroupVm addOrUpdateGroupVm) {
      try {
        if (addOrUpdateGroupVm == null) {
          throw new ArgumentNullException(nameof(addOrUpdateGroupVm), "View model parameter is null");
        }

        Group groupEntity;

        if (!addOrUpdateGroupVm.Id.HasValue || addOrUpdateGroupVm.Id == 0) {
          groupEntity = DbContext.Groups.CreateProxy();
        } else {
          groupEntity = DbContext.Groups.First(group => group.Id == addOrUpdateGroupVm.Id);
        }

        Mapper.Map(addOrUpdateGroupVm, groupEntity);
        DbContext.Groups.Update(groupEntity);

        DbContext.SaveChanges();

        return Mapper.Map<GroupVm>(groupEntity);
      }
      catch (Exception ex) {
        Logger.LogError(ex, ex.Message);
        throw;
      }
    }

    public GroupVm DeleteGroup(DeleteGroupVm deleteGroupVm) {
      try {
        if (deleteGroupVm == null) {
          throw new ArgumentNullException(nameof(deleteGroupVm), "View model parameter is null");
        }

        var groupEntity = DbContext.Groups.Find(deleteGroupVm.Id);

        if (groupEntity == null) {
          throw new InvalidOperationException( $"Id {deleteGroupVm.Id} does not exist in database.");
        }

        DbContext.Groups.Remove(groupEntity);
        DbContext.SaveChanges();

        return Mapper.Map<GroupVm>(groupEntity);
      }
      catch (Exception ex) {
        Logger.LogError(ex, ex.Message);
        throw;
      }
    }

    public GroupVm GetGroup(Expression<Func<Group, bool>> filterExpression) {
      try {
        if (filterExpression == null)
          throw new ArgumentNullException(nameof(filterExpression), "FilterExpression is null");

        var groupEntity = DbContext.Groups.FirstOrDefault(filterExpression);

        return Mapper.Map<GroupVm>(groupEntity);
      }
      catch (Exception ex) {
        Logger.LogError(ex, ex.Message);
        throw;
      }
    }

    public IEnumerable<GroupVm> GetGroups(Expression<Func<Group, bool>> filterExpression = null) {
      try {
        var groupsEntities = DbContext.Groups.AsQueryable();
        if (filterExpression != null) groupsEntities = groupsEntities.Where(filterExpression);

        return Mapper.Map<IEnumerable<GroupVm>>(groupsEntities);
      }
      catch (Exception ex) {
        Logger.LogError(ex, ex.Message);
        throw;
      }
    }

    public StudentVm AddStudentToGroup(AddStudentToGroupVm addStudentToGroupVm) {
      try {
        if (addStudentToGroupVm == null) {
          throw new ArgumentNullException(nameof(addStudentToGroupVm), "View model parameter is null");
        }

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
