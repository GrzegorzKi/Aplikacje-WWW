using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;

namespace SchoolRegister.Web.Controllers {
  public abstract class BaseController : Controller {
    protected readonly ILogger Logger;
    protected readonly IMapper Mapper;
    protected readonly IStringLocalizer Localizer;

    protected BaseController(ILogger logger, IMapper mapper, IStringLocalizer localizer) {
      Logger = logger;
      Mapper = mapper;
      Localizer = localizer;
    }

    protected bool IsAjaxRequest() {
      return HttpContext.Request.Headers["x-requested-with"] == "XMLHttpRequest";
    }

  }
}
