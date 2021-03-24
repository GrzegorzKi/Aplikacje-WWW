﻿using System;
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

        var teacher = DbContext.Users.OfType<Teacher>().First(t => t.Id == addGradeToStudentVm.TeacherId);

        if (!await UserManager.IsInRoleAsync(teacher, "Teacher")) {
          throw new InvalidOperationException("TeacherId must correspond to user with \"Teacher\" role");
        }

        var groupEntity = Mapper.Map<Grade>(addGradeToStudentVm);

        await DbContext.Grades.AddAsync(groupEntity);
        await DbContext.SaveChangesAsync();

        return Mapper.Map<GradeVm>(groupEntity);
      }
      catch (Exception ex) {
        Logger.LogError(ex, ex.Message);
        throw;
      }
    }

    public GradeVm GetGrade(Expression<Func<Grade, bool>> filterExpression) {
      try {
        if (filterExpression == null)
          throw new ArgumentNullException(nameof(filterExpression), "FilterExpression is null");

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
  }
}
