using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using SchoolRegister.Model.DataModels;
using SchoolRegister.Services.Interfaces;
using SchoolRegister.ViewModels.VM;

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

    public async Task<IActionResult> Index(string filterValue = null) {
      IEnumerable<StudentVm> studentVms;
      var filterExpression = GetFilterExpression(filterValue);

      var user = await _userManager.GetUserAsync(User);
      if (await _userManager.IsInRoleAsync(user, "Admin")
          || await _userManager.IsInRoleAsync(user, "Teacher")) {
        studentVms = _studentService.GetStudents(filterExpression);
      } else if (await _userManager.IsInRoleAsync(user, "Parent")) {
        if (user is Parent parent) {
          Expression<Func<Student, bool>> filterTeacherExpression = s => s.ParentId == parent.Id;
          var finalFilterExpression = filterTeacherExpression.AndAlso(filterExpression);
          studentVms = _studentService.GetStudents(finalFilterExpression);
        } else {
          return BadRequest("Parent role is assigned to user, but user is not of Parent type");
        }
      } else {
        return View("Error");
      }

      if (IsAjaxRequest())
        return PartialView("_StudentsTableDataPartial", studentVms);
      return View(studentVms);
    }

    private Expression<Func<Student, bool>> GetFilterExpression(string filterValue) {
      if (!string.IsNullOrWhiteSpace(filterValue)) {
        return s => s.FirstName.Contains(filterValue) || s.LastName.Contains(filterValue);
      } else {
        return null;
      }
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
