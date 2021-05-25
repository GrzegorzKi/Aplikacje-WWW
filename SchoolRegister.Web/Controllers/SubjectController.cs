using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
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

    public async Task<IActionResult> Index(string filterValue = null) {
      IEnumerable<SubjectVm> subjectVms;
      var filterExpression = GetFilterExpression(filterValue);

      var user = await _userManager.GetUserAsync(User);
      if (await _userManager.IsInRoleAsync(user, "Admin")) {
        subjectVms = _subjectService.GetSubjects(filterExpression);
      } else if (await _userManager.IsInRoleAsync(user, "Teacher")) {
        if (user is Teacher teacher) {
          Expression<Func<Subject, bool>> filterTeacherExpression = s => s.TeacherId == teacher.Id;
          var finalFilterExpression = filterTeacherExpression.AndAlso(filterExpression);
          subjectVms = _subjectService.GetSubjects(finalFilterExpression);
        } else {
          return BadRequest("Teacher role is assigned to user, but user is not of Teacher type");
        }
      } else {
        return View("Error");
      }

      if (IsAjaxRequest())
        return PartialView("_SubjectsTableDataPartial", subjectVms);
      return View(subjectVms);
    }

    private Expression<Func<Subject, bool>> GetFilterExpression(string filterValue) {
      if (!string.IsNullOrWhiteSpace(filterValue)) {
        return s => s.Name.Contains(filterValue);
      } else {
        return null;
      }
    }

    public async Task<IActionResult> Details(int id) {
      var user = await _userManager.FindByNameAsync(User?.Identity?.Name);
      SubjectVm subjectVm = null;
      if (await _userManager.IsInRoleAsync(user, "Admin")) {
        subjectVm = _subjectService.GetSubject(s => s.Id == id);
      } else if (await _userManager.IsInRoleAsync(user, "Teacher")) {
        if (user is Teacher teacher) {
          subjectVm = _subjectService.GetSubject(s => s.TeacherId == teacher.Id && s.Id == id);
        } else {
          return BadRequest("Teacher is assigned to role, but to the Teacher type.");
        }
      }

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
        var subjectVm = _subjectService.GetSubject(s => s.Id == id);
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
