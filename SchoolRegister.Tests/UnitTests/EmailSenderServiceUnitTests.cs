using System;
using System.Linq;
using SchoolRegister.DAL.EF;
using SchoolRegister.Services.Interfaces;
using SchoolRegister.ViewModels.VM;
using Xunit;

namespace SchoolRegister.Tests.UnitTests {
  public class EmailSenderServiceUnitTests : BaseUnitTests {
    private readonly IEmailSenderService _emailSenderService;

    public EmailSenderServiceUnitTests(ApplicationDbContext dbContext, IEmailSenderService emailSenderService) : base(dbContext) {
      _emailSenderService = emailSenderService;
    }

    [Fact]
    public void CreateEmail() {
      var createEmailVm = new CreateEmailVm {
        SenderId = 1,
        RecipientId = 4,
        Subject = "Test email message",
        Body = "This is a test message body."
      };

      var emailTask = _emailSenderService.SendEmailToParent(createEmailVm);
      var email = emailTask.Result;

      Assert.NotNull(email);
    }

    [Fact]
    public void CreateEmailWithNullSubjectAndBody() {
      var createEmailVm = new CreateEmailVm {
        SenderId = 1,
        RecipientId = 4,
        Subject = null,
        Body = null
      };

      var emailTask = _emailSenderService.SendEmailToParent(createEmailVm);
      var email = emailTask.Result;

      Assert.NotNull(email);
    }

    [Fact]
    public void CreateEmailWithNonTeacherSenderIdShouldThrowException() {
      var createEmailVm = new CreateEmailVm {
        SenderId = -1,
        RecipientId = 1,
        Subject = "Test email message",
        Body = "This is a test message body."
      };

      var emailTask = _emailSenderService.SendEmailToParent(createEmailVm);
      var exception = Assert.Throws<AggregateException>(() => emailTask.Result);
      Assert.IsType<InvalidOperationException>(exception.InnerExceptions.First());
      Assert.Contains("SenderId", exception.Message);
    }

    [Fact]
    public void CreateEmailWithNonParentRecipientIdShouldThrowException() {
      var createEmailVm = new CreateEmailVm {
        SenderId = 1,
        RecipientId = -1,
        Subject = "Test email message",
        Body = "This is a test message body."
      };

      var emailTask = _emailSenderService.SendEmailToParent(createEmailVm);
      var exception = Assert.Throws<AggregateException>(() => emailTask.Result);
      Assert.IsType<InvalidOperationException>(exception.InnerExceptions.First());
      Assert.Contains("RecipientId", exception.Message);
    }

    [Fact]
    public void CreateEmailWithNullArgumentShouldThrowException() {
      var gradeTask = _emailSenderService.SendEmailToParent(null);
      var exception = Assert.Throws<AggregateException>(() => gradeTask.Result);
      Assert.IsType<ArgumentNullException>(exception.InnerExceptions.First());
    }
  }
}
