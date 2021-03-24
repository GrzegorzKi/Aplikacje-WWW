using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Mail;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SchoolRegister.DAL.EF;
using SchoolRegister.Model.DataModels;
using SchoolRegister.Services.Interfaces;
using SchoolRegister.ViewModels.VM;

namespace SchoolRegister.Services.Services {
  public class TeacherService : BaseService, ITeacherService {
    private UserManager<User> UserManager { get; }

    private SmtpClient SmtpClient { get; }

    public TeacherService(ApplicationDbContext dbContext, IMapper mapper, ILogger logger, UserManager<User> userManager, SmtpClient smtpClient)
      : base(dbContext, mapper, logger) {
      UserManager = userManager;
      SmtpClient = smtpClient;
    }

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

    public async Task<EmailVm> SendEmailToParent(CreateEmailVm createEmailVm) {
      try {
        if (createEmailVm == null) {
          throw new ArgumentNullException(nameof(createEmailVm), "View model parameter is null");
        }

        var teacherTask = DbContext.Users.OfType<Teacher>().FirstAsync(t => t.Id == createEmailVm.SenderId);
        var parentTask = DbContext.Users.OfType<Parent>().FirstAsync(p => p.Id == createEmailVm.RecipientId);

        var teacher = teacherTask.Result;
        var parent = parentTask.Result;

        if (!await UserManager.IsInRoleAsync(teacher, "Teacher")) {
          throw new InvalidOperationException("SenderId must correspond to user with \"Teacher\" role");
        }
        if (!await UserManager.IsInRoleAsync(parent, "Parent")) {
          throw new InvalidOperationException("RecipientId must correspond to user with \"Parent\" role");
        }

        var mailMessage = new MailMessage(teacher.Email, parent.Email, createEmailVm.Subject, createEmailVm.Body);
        await SmtpClient.SendMailAsync(mailMessage);

        var emailVm = Mapper.Map<EmailVm>(createEmailVm);
        emailVm = Mapper.Map(teacher, emailVm);
        emailVm = Mapper.Map(parent, emailVm);

        return emailVm;
      }
      catch (Exception ex) {
        Logger.LogError(ex, ex.Message);
        throw;
      }
    }
  }
}
