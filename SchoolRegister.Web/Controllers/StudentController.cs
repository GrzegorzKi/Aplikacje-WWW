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
  [Authorize(Roles = "Admin, Parent, Teacher")]
  [AutoValidateAntiforgeryToken]
  public class StudentController : BaseController {
    private readonly IParentService _parentService;
    private readonly IStudentService _studentService;
    private readonly UserManager<User> _userManager;

    public StudentController(
        IParentService parentService,
        IStudentService studentService,
        UserManager<User> userManager,
        ILogger logger,
        IMapper mapper,
        IStringLocalizer localizer) : base(logger, mapper, localizer) {
      _parentService = parentService;
      _studentService = studentService;
      _userManager = userManager;
    }

    public IActionResult Index() {
      if (User.IsInRole("Teacher") || User.IsInRole("Admin")) {
        return View(_studentService.GetStudents());
      }

      if (User.IsInRole("Parent")) {
        var parentId = int.Parse(_userManager.GetUserId(User));
        return View(_studentService.GetStudents(student => student.ParentId == parentId));
      }

      return View("Error");
    }

    public IActionResult Details(int id) {
      var studentVm = _studentService.GetStudent(x => x.Id == id);
      if (studentVm is null) {
        return new NotFoundResult();
      }

      return View(studentVm);
    }

    [Authorize(Roles = "Admin")]
    public IActionResult AddOrEditStudent(int? id = null) {
      var parentVms = _parentService.GetParents();
      ViewBag.ParentsSelectList = new SelectList(parentVms.Select(p => new {
          Text = p.ParentName,
          Value = p.Id
      }), "Value", "Text");

      if (id.HasValue) {
        var studentVm = _studentService.GetStudent(x => x.Id == id);
        if (studentVm is null) {
          return new NotFoundResult();
        }

        ViewBag.ActionType = "Edit";
        return View(Mapper.Map<AddOrUpdateStudentVm>(studentVm));
      }

      ViewBag.ActionType = "Add";
      return View();
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public IActionResult AddOrEditStudent(AddOrUpdateStudentVm addOrUpdateStudentVm) {
      if (ModelState.IsValid) {
        _studentService.AddOrUpdateStudent(addOrUpdateStudentVm);
        return RedirectToAction("Index");
      }

      return View();
    }
  }
}
