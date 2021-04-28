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
  public class ParentService : BaseService, IParentService {
    public ParentService(ApplicationDbContext dbContext, IMapper mapper, ILogger logger)
        : base(dbContext, mapper, logger) { }
    public ParentVm GetParent(Expression<Func<Parent, bool>> filterExpression) {
      try {
        if (filterExpression == null) {
          throw new ArgumentNullException(nameof(filterExpression), "FilterExpression is null");
        }

        var parentEntity = DbContext.Parents.FirstOrDefault(filterExpression);

        return Mapper.Map<ParentVm>(parentEntity);
      }
      catch (Exception ex) {
        Logger.LogError(ex, ex.Message);
        throw;
      }
    }

    public IEnumerable<ParentVm> GetParents(Expression<Func<Parent, bool>> filterExpression = null) {
      try {
        var parentsEntities = DbContext.Parents.AsQueryable();
        if (filterExpression != null) parentsEntities = parentsEntities.Where(filterExpression);

        return Mapper.Map<IEnumerable<ParentVm>>(parentsEntities);
      }
      catch (Exception ex) {
        Logger.LogError(ex, ex.Message);
        throw;
      }
    }
  }
}
