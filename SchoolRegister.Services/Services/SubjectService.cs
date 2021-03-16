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
  public class SubjectService : BaseService, ISubjectService {
    public SubjectService(ApplicationDbContext dbContext, IMapper mapper, ILogger logger)
      : base(dbContext, mapper, logger) { }

    public SubjectVm AddOrUpdateSubject(AddOrUpdateSubjectVm addOrUpdateSubjectVm) {
      try {
        if (addOrUpdateSubjectVm == null)
          throw new ArgumentNullException(nameof(addOrUpdateSubjectVm), "View model parameter is null");

        var subjectEntity = Mapper.Map<Subject>(addOrUpdateSubjectVm);
        if (!addOrUpdateSubjectVm.Id.HasValue || addOrUpdateSubjectVm.Id == 0)
          DbContext.Subjects.Add(subjectEntity);
        else
          DbContext.Subjects.Update(subjectEntity);

        DbContext.SaveChanges();
        return Mapper.Map<SubjectVm>(subjectEntity);
      }
      catch (Exception ex) {
        Logger.LogError(ex, ex.Message);
        throw;
      }
    }

    public SubjectVm GetSubject(Expression<Func<Subject, bool>> filterExpression) {
      try {
        if (filterExpression == null)
          throw new ArgumentNullException(nameof(filterExpression), "FilterExpression is null");

        var subjectEntity = DbContext.Subjects.FirstOrDefault(filterExpression);

        return Mapper.Map<SubjectVm>(subjectEntity);
      }
      catch (Exception ex) {
        Logger.LogError(ex, ex.Message);
        throw;
      }
    }

    public IEnumerable<SubjectVm> GetSubjects(Expression<Func<Subject, bool>> filterExpression = null) {
      try {
        var subjectEntities = DbContext.Subjects.AsQueryable();
        if (filterExpression != null) subjectEntities = subjectEntities.Where(filterExpression);

        return Mapper.Map<IEnumerable<SubjectVm>>(subjectEntities);
      }
      catch (Exception ex) {
        Logger.LogError(ex, ex.Message);
        throw;
      }
    }
  }
}
