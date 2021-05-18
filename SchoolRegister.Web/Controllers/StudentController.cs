using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using SchoolRegister.Model.DataModels;
using SchoolRegister.Services.Interfaces;

namespace SchoolRegister.Web.Controllers {
  [Authorize(Roles = "Admin, Parent, Teacher")]
  [AutoValidateAntiforgeryToken]
  public class StudentController : BaseController {
    private readonly IStudentService _studentService;
    private readonly UserManager<User> _userManager;

    public StudentController(
        IStudentService studentService,
        UserManager<User> userManager,
        ILogger logger,
        IMapper mapper,
        IStringLocalizer localizer) : base(logger, mapper, localizer) {
      _studentService = studentService;
      _userManager = userManager;
    }

    public async Task<IActionResult> Index() {
      var user = await _userManager.FindByNameAsync(User?.Identity?.Name);
      if (await _userManager.IsInRoleAsync(user, "Admin")
          || await _userManager.IsInRoleAsync(user, "Teacher")) {
        return View(_studentService.GetStudents());
      } else if (await _userManager.IsInRoleAsync(user, "Parent")) {
        if (user is Parent parent) {
          return View(_studentService.GetStudents(s => s.ParentId == parent.Id));
        } else {
          return BadRequest("Parent role is assigned to user, but user is not of Parent type");
        }
      }

      return View("Error");
    }

    public IActionResult Details(int id) {
      var studentVm = _studentService.GetStudent(s => s.Id == id);
      if (studentVm is null) {
        return new NotFoundResult();
      }

      return View(studentVm);
    }
  }
}
