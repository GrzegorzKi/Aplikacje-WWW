using System.Linq;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using SchoolRegister.Services.Interfaces;
using SchoolRegister.ViewModels.VM;

namespace SchoolRegister.Web.Controllers {
  [Authorize(Roles = "Teacher, Admin")]
  [AutoValidateAntiforgeryToken]
  public class GroupController : BaseController {
    private readonly IGroupService _groupService;
    private readonly ISubjectService _subjectService;
    private readonly IStudentService _studentService;

    public GroupController(IGroupService groupService,
      ISubjectService subjectService,
      IStudentService studentService,
      IStringLocalizer localizer,
      ILogger logger,
      IMapper mapper) : base(logger, mapper, localizer) {
      _groupService = groupService;
      _subjectService = subjectService;
      _studentService = studentService;
    }

    public IActionResult Index() {
      return View(_groupService.GetGroups());
    }

    public IActionResult Details(int id) {
      var subjectVm = _groupService.GetGroup(x => x.Id == id);
      return View(subjectVm);
    }

    public IActionResult AddOrEditGroup(int? id = null) {
      if (id.HasValue) {
        var groupVm = _groupService.GetGroup(x => x.Id == id);
        ViewBag.ActionType = "Edit";
        return View(Mapper.Map<AddOrUpdateGroupVm>(groupVm));
      } else {
        ViewBag.ActionType = "Add";
        return View();
      }
    }

    [HttpPost]
    public IActionResult AddOrEditGroup(AddOrUpdateGroupVm addOrUpdateGroupVm) {
      if (ModelState.IsValid) {
        _groupService.AddOrUpdateGroup(addOrUpdateGroupVm);
        return RedirectToAction("Index");
      }

      return View();
    }

    public IActionResult AttachSubjectToGroup(int subjectId) {
      var subject = _subjectService.GetSubject(s => s.Id == subjectId);
      ViewBag.SubjectName = subject.Name;

      var groups = _groupService.GetGroups(
          g => !g.SubjectGroups.Any(
              sg => sg.SubjectId == subject.Id));
      ViewBag.GroupsSelectList = new SelectList(groups.Select(t => new {
          Text = t.Name,
          Value = t.Id
      }), "Value", "Text");

      return View();
    }

    [HttpPost]
    public IActionResult AttachSubjectToGroup(AttachSubjectToGroupVm attachSubjectToGroupVm) {
      if (ModelState.IsValid) {
        _groupService.AttachSubjectToGroup(attachSubjectToGroupVm);
        return RedirectToAction("Index");
      }

      return View();
    }

    public IActionResult DetachSubjectFromGroup(int subjectId) {
      var subject = _subjectService.GetSubject(s => s.Id == subjectId);
      ViewBag.SubjectName = subject.Name;

      var groups = _groupService.GetGroups(
          g => g.SubjectGroups.Any(
              sg => sg.SubjectId == subject.Id));
      ViewBag.GroupsSelectList = new SelectList(groups.Select(t => new {
          Text = t.Name,
          Value = t.Id
      }), "Value", "Text");

      return View();
    }

    [HttpPost]
    public IActionResult DetachSubjectFromGroup(DetachSubjectFromGroupVm detachSubjectFromGroupVm) {
      if (ModelState.IsValid) {
        _groupService.DetachSubjectFromGroup(detachSubjectFromGroupVm);
        return RedirectToAction("Index");
      }

      return View();
    }

    public IActionResult AttachStudentToGroup(int studentId) {
      var student = _studentService.GetStudent(s => s.Id == studentId);
      ViewBag.SubjectName = student.StudentName;

      var groups = _groupService.GetGroups(
          g => !g.SubjectGroups.Any(
              sg => sg.SubjectId == student.Id));
      ViewBag.GroupsSelectList = new SelectList(groups.Select(t => new {
          Text = t.Name,
          Value = t.Id
      }), "Value", "Text");

      return View();
    }

    [HttpPost]
    public IActionResult AttachStudentToGroup(AddStudentToGroupVm addStudentToGroupVm) {
      if (ModelState.IsValid) {
        _groupService.AddStudentToGroup(addStudentToGroupVm);
        return RedirectToAction("Index");
      }

      return View();
    }

    public IActionResult DetachStudentFromGroup(int studentId) {
      var student = _studentService.GetStudent(s => s.Id == studentId);
      ViewBag.StudentName = student.StudentName;

      var groups = _groupService.GetGroups(
          g => g.SubjectGroups.Any(
              sg => sg.SubjectId == student.Id));
      ViewBag.GroupsSelectList = new SelectList(groups.Select(t => new {
          Text = t.Name,
          Value = t.Id
      }), "Value", "Text");

      return View();
    }

    [HttpPost]
    public IActionResult DetachStudentFromGroup(RemoveStudentFromGroupVm removeStudentFromGroupVm) {
      if (ModelState.IsValid) {
        _groupService.RemoveStudentFromGroup(removeStudentFromGroupVm);
        return RedirectToAction("Index");
      }

      return View();
    }
  }
}
