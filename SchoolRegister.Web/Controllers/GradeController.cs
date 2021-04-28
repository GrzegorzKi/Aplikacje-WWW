using System;
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
  [Authorize(Roles = "Student, Parent, Teacher")]
  [AutoValidateAntiforgeryToken]
  public class GradeController : BaseController {
    private readonly IGradeService _gradeService;
    private readonly ISubjectService _subjectService;
    private readonly IStudentService _studentService;
    private readonly UserManager<User> _userManager;

    public GradeController(
        IGradeService gradeService,
        ISubjectService subjectService,
        IStudentService studentService,
        UserManager<User> userManager,
        ILogger logger,
        IMapper mapper,
        IStringLocalizer localizer) : base(logger, mapper, localizer) {
      _gradeService = gradeService;
      _subjectService = subjectService;
      _studentService = studentService;
      _userManager = userManager;
    }

    public IActionResult Index(int? studentId = null) {
      try {
        var userId = int.Parse(_userManager.GetUserId(User));
        if (!studentId.HasValue) {
          if (User.IsInRole("Student")) {
            studentId = userId;
          } else {
            return new BadRequestResult();
          }
        }

        var getGradesReportVm = new GetGradesReportVm() {
            StudentId = studentId.Value,
            GetterUserId = userId
        };
        var gradeVms = _gradeService.GetGradesReportForStudent(getGradesReportVm).Result;
        return View(gradeVms);
      } catch (Exception ex) {
        Logger.LogError(ex, ex.Message);
        return View("Error");
      }
    }

    [Authorize(Roles = "Teacher")]
    public IActionResult AddGradeToStudent(int studentId) {
      var studentVm = _studentService.GetStudent(s => s.Id == studentId);
      if (studentVm is null) {
        return new NotFoundResult();
      }

      var subjectsVm = _subjectService.GetSubjects();
      ViewBag.SubjectsSelectList = new SelectList(subjectsVm.Select(t => new {
          Text = t.Name,
          Value = t.Id
      }), "Value", "Text");

      var teacherId = int.Parse(_userManager.GetUserId(User));
      var modelVm = new AddGradeToStudentVm {
          TeacherId = teacherId,
          StudentId = studentVm.Id
      };

      return View(modelVm);
    }

    [HttpPost]
    [Authorize(Roles = "Teacher")]
    public IActionResult AddGradeToStudent(AddGradeToStudentVm addGradeToStudentVm) {
      if (ModelState.IsValid) {
        _gradeService.AddGradeToStudent(addGradeToStudentVm);
        return RedirectToAction("Index");
      }

      return View();
    }
  }
}
