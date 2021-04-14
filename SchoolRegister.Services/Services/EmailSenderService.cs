using System;
using System.Linq;
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
  public class EmailSenderService : BaseService, IEmailSenderService {

    private UserManager<User> UserManager { get; }
    private SmtpClient SmtpClient { get; }
    public EmailSenderService(ApplicationDbContext dbContext, IMapper mapper, ILogger logger, SmtpClient smtpClient, UserManager<User> userManager)
      : base(dbContext, mapper, logger) {
      SmtpClient = smtpClient;
      UserManager = userManager;
    }

    public async Task<EmailVm> SendEmailToParent(CreateEmailVm createEmailVm) {
      try {
        if (createEmailVm == null) {
          throw new ArgumentNullException(nameof(createEmailVm), "View model parameter is null");
        }

        var teacher = await DbContext.Users.OfType<Teacher>().FirstOrDefaultAsync(t => t.Id == createEmailVm.SenderId);
        var parent = await DbContext.Users.OfType<Parent>().FirstOrDefaultAsync(p => p.Id == createEmailVm.RecipientId);

        if (teacher == null || !await UserManager.IsInRoleAsync(teacher, "Teacher")) {
          throw new InvalidOperationException("SenderId must correspond to user with \"Teacher\" role");
        }
        if (parent == null || !await UserManager.IsInRoleAsync(parent, "Parent")) {
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
