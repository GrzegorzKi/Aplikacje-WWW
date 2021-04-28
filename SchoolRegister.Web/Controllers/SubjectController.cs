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
  [Authorize(Roles = "Teacher, Admin")]
  [AutoValidateAntiforgeryToken]
  public class SubjectController : BaseController {
    private readonly ISubjectService _subjectService;
    private readonly ITeacherService _teacherService;
    private readonly UserManager<User> _userManager;

    public SubjectController(
        ISubjectService subjectService,
        ITeacherService teacherService,
        UserManager<User> userManager,
        ILogger logger,
        IMapper mapper,
        IStringLocalizer localizer) : base(logger, mapper, localizer) {
      _subjectService = subjectService;
      _teacherService = teacherService;
      _userManager = userManager;
    }

    public IActionResult Index() {
      if (User.IsInRole("Admin")) {
        return View(_subjectService.GetSubjects());
      }

      if (User.IsInRole("Teacher")) {
        var teacherId = int.Parse(_userManager.GetUserId(User));
        return View(_subjectService.GetSubjects(x => x.TeacherId == teacherId));
      }

      return View("Error");
    }

    public IActionResult Details(int id) {
      var subjectVm = _subjectService.GetSubject(x => x.Id == id);
      if (subjectVm is null) {
        return new NotFoundResult();
      }
      return View(subjectVm);
    }

    public IActionResult AddOrEditSubject(int? id = null) {
      var teachersVm = _teacherService.GetTeachers();
      ViewBag.TeachersSelectList = new SelectList(teachersVm.Select(t => new {
          Text = t.TeacherName,
          Value = t.Id
      }), "Value", "Text");

      if (id.HasValue) {
        var subjectVm = _subjectService.GetSubject(x => x.Id == id);
        if (subjectVm is null) {
          return new NotFoundResult();
        }
        ViewBag.ActionType = "Edit";
        return View(Mapper.Map<AddOrUpdateSubjectVm>(subjectVm));
      }

      ViewBag.ActionType = "Add";
      return View();
    }

    [HttpPost]
    public IActionResult AddOrEditSubject(AddOrUpdateSubjectVm addOrUpdateSubjectVm) {
      if (ModelState.IsValid) {
        _subjectService.AddOrUpdateSubject(addOrUpdateSubjectVm);
        return RedirectToAction("Index");
      }

      return View();
    }
  }
}
