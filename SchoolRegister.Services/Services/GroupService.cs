using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Linq.Expressions;
using AutoMapper;
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
        if (addOrUpdateGroupVm == null)
          throw new ArgumentNullException(nameof(addOrUpdateGroupVm), "View model parameter is null");

        var groupEntity = Mapper.Map<Group>(addOrUpdateGroupVm);
        if (!addOrUpdateGroupVm.Id.HasValue || addOrUpdateGroupVm.Id == 0)
          DbContext.Groups.Add(groupEntity);
        else
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
        if (deleteGroupVm == null)
          throw new ArgumentNullException(nameof(deleteGroupVm), "View model parameter is null");

        var groupEntity = DbContext.Groups.First(g => g.Id == deleteGroupVm.Id);

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
  }
}
