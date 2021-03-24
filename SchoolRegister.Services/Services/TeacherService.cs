﻿using System;
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
  public class TeacherService : BaseService, ITeacherService {
    public TeacherService(ApplicationDbContext dbContext, IMapper mapper, ILogger logger)
      : base(dbContext, mapper, logger) { }

    public TeacherVm AddOrUpdateTeacher(AddOrUpdateTeacherVm addOrUpdateTeacherVm) {
      try {
        if (addOrUpdateTeacherVm == null)
          throw new ArgumentNullException(nameof(addOrUpdateTeacherVm), "View model parameter is null");

        var teacherEntity = Mapper.Map<Teacher>(addOrUpdateTeacherVm);
        if (!addOrUpdateTeacherVm.Id.HasValue || addOrUpdateTeacherVm.Id == 0)
          DbContext.Teachers.Add(teacherEntity);
        else
          DbContext.Teachers.Update(teacherEntity);

        DbContext.SaveChanges();
        return Mapper.Map<TeacherVm>(teacherEntity);
      }
      catch (Exception ex) {
        Logger.LogError(ex, ex.Message);
        throw;
      }
    }

    public TeacherVm GetTeacher(Expression<Func<Teacher, bool>> filterExpression) {
      try {
        if (filterExpression == null)
          throw new ArgumentNullException(nameof(filterExpression), "FilterExpression is null");

        var subjectEntity = DbContext.Teachers.FirstOrDefault(filterExpression);

        return Mapper.Map<TeacherVm>(subjectEntity);
      }
      catch (Exception ex) {
        Logger.LogError(ex, ex.Message);
        throw;
      }
    }

    public IEnumerable<TeacherVm> GetTeachers(Expression<Func<Teacher, bool>> filterExpression = null) {
      try {
        var teacherEntities = DbContext.Teachers.AsQueryable();
        if (filterExpression != null) teacherEntities = teacherEntities.Where(filterExpression);

        return Mapper.Map<IEnumerable<TeacherVm>>(teacherEntities);
      }
      catch (Exception ex) {
        Logger.LogError(ex, ex.Message);
        throw;
      }
    }
  }
}