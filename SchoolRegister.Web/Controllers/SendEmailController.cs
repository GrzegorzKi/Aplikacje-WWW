using System.Linq;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using SchoolRegister.Model.DataModels;
using SchoolRegister.Services.Interfaces;
using SchoolRegister.ViewModels.VM;

namespace SchoolRegister.Web.Controllers {
  [Authorize(Roles = "Teacher")]
  [AutoValidateAntiforgeryToken]
  public class SendEmailController : BaseController {
    private readonly IEmailSenderService _emailSenderService;
    private readonly IParentService _parentService;
    private readonly UserManager<User> _userManager;

    public SendEmailController(
        IEmailSenderService emailSenderService,
        IParentService parentService,
        UserManager<User> userManager,
        ILogger logger,
        IMapper mapper,
        IStringLocalizer localizer) : base(logger, mapper, localizer) {
      _emailSenderService = emailSenderService;
      _parentService = parentService;
      _userManager = userManager;
    }

    public IActionResult Index() {
      var parentVms = _parentService.GetParents();
      ViewBag.RecipientsSelectList = new SelectList(parentVms.Select(p => new {
          Text = p.ParentName,
          Value = p.Id
      }), "Value", "Text");

      var teacherId = int.Parse(_userManager.GetUserId(User));
      var modelVm = new CreateEmailVm {
          SenderId = teacherId
      };

      return View(modelVm);
    }

    [HttpPost]
    public IActionResult Index(CreateEmailVm createEmailVm) {
      if (ModelState.IsValid) {
        _emailSenderService.SendEmailToParent(createEmailVm);
        return RedirectToAction("Index");
      }

      return View();
    }
  }
}
