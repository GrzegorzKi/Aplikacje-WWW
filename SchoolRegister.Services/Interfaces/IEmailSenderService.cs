using System.Threading.Tasks;
using SchoolRegister.ViewModels.VM;

namespace SchoolRegister.Services.Interfaces {
  public interface IEmailSenderService {
    public Task<EmailVm> SendEmailToParent(CreateEmailVm createEmailVm);
  }
}
