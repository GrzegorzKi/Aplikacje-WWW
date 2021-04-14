using SchoolRegister.DAL.EF;
using SchoolRegister.Services.Interfaces;

namespace SchoolRegister.Tests.UnitTests {
  public class EmailSenderServiceUnitTests : BaseUnitTests {
    private readonly IEmailSenderService _emailSenderService;

    public EmailSenderServiceUnitTests(ApplicationDbContext dbContext, IEmailSenderService emailSenderService) : base(dbContext) {
      _emailSenderService = emailSenderService;
    }
  }
}
