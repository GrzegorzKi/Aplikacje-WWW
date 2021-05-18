using System;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SchoolRegister.Services.Interfaces;
using SchoolRegister.ViewModels.VM;

namespace SchoolRegister.Api.Controllers {
  [Authorize(Roles = "Teacher")]
  public class SendEmailController : BaseApiController {

    private readonly IEmailSenderService _emailSenderService;

    private static readonly string ERROR_MESSAGE = "An error occurred";

    public SendEmailController(ILogger logger,
        IMapper mapper,
        IEmailSenderService emailSenderService) : base(logger, mapper) {
      _emailSenderService = emailSenderService;
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] CreateEmailVm createEmailVm) {
      try {
        if (ModelState == null || !ModelState.IsValid) {
          return BadRequest(ModelState);
        }
        var emailVm = await _emailSenderService.SendEmailToParent(createEmailVm);
        return Ok(emailVm);
      }
      catch (ArgumentNullException) {
        return NotFound();
      }
      catch (Exception) {
        return BadRequest(ERROR_MESSAGE);
      }
    }
  }
}
