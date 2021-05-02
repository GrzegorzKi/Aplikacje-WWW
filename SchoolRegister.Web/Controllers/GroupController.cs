using System;
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
  [Authorize(Roles = "Admin")]
  [AutoValidateAntiforgeryToken]
  public class GroupController : BaseController {
    private readonly IGroupService _groupService;
    private readonly ISubjectService _subjectService;
    private readonly IStudentService _studentService;

    public GroupController(
        IGroupService groupService,
        ISubjectService subjectService,
        IStudentService studentService,
        ILogger logger,
        IMapper mapper,
        IStringLocalizer localizer) : base(logger, mapper, localizer) {
      _groupService = groupService;
      _subjectService = subjectService;
      _studentService = studentService;
    }

    public IActionResult Index() {
      return View(_groupService.GetGroups());
    }

    public IActionResult Details(int id) {
      var groupVm = _groupService.GetGroup(x => x.Id == id);
      if (groupVm is null) {
        return new NotFoundResult();
      }
      return View(groupVm);
    }

    public IActionResult AddOrEditGroup(int? id = null) {
      if (id.HasValue) {
        var groupVm = _groupService.GetGroup(x => x.Id == id);
        if (groupVm is null) {
          return new NotFoundResult();
        }
        ViewBag.ActionType = "Edit";
        return View(Mapper.Map<AddOrUpdateGroupVm>(groupVm));
      }

      ViewBag.ActionType = "Add";
      return View();
    }

    [HttpPost]
    public IActionResult AddOrEditGroup(AddOrUpdateGroupVm addOrUpdateGroupVm) {
      if (ModelState.IsValid) {
        _groupService.AddOrUpdateGroup(addOrUpdateGroupVm);
        return RedirectToAction("Index");
      }

      return View();
    }

    public IActionResult DeleteGroup(int id) {
      var groupVm = _groupService.GetGroup(x => x.Id == id);
      if (groupVm is null) {
        return new NotFoundResult();
      }
      ViewBag.GroupName = groupVm.Name;
      return View(Mapper.Map<DeleteGroupVm>(groupVm));
    }

    [HttpPost]
    public IActionResult DeleteGroup(DeleteGroupVm deleteGroupVm) {
      if (ModelState.IsValid) {
        _groupService.DeleteGroup(deleteGroupVm);
        return RedirectToAction("Index");
      }

      return View();
    }

    public IActionResult AttachSubjectToGroup(int subjectId) {
      var subjectVm = _subjectService.GetSubject(s => s.Id == subjectId);
      if (subjectVm is null) {
        return new NotFoundResult();
      }
      ViewBag.SubjectName = subjectVm.Name;

      var groups = _groupService.GetGroups(
          g => g.SubjectGroups.All(sg => sg.SubjectId != subjectVm.Id));

      ViewBag.GroupsSelectList = new SelectList(groups.Select(t => new {
          Text = t.Name,
          Value = t.Id
      }), "Value", "Text");

      var modelVm = new AttachSubjectToGroupVm {SubjectId = subjectId};

      return View(modelVm);
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
      var subjectVm = _subjectService.GetSubject(s => s.Id == subjectId);
      if (subjectVm is null) {
        return new NotFoundResult();
      }
      ViewBag.SubjectName = subjectVm.Name;

      var groups = _groupService.GetGroups(
          g => g.SubjectGroups.Any(sg => sg.SubjectId == subjectVm.Id));

      ViewBag.GroupsSelectList = new SelectList(groups.Select(t => new {
          Text = t.Name,
          Value = t.Id
      }), "Value", "Text");

      var modelVm = new DetachSubjectFromGroupVm() {SubjectId = subjectId};

      return View(modelVm);
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
      var studentVm = _studentService.GetStudent(s => s.Id == studentId);
      if (studentVm is null) {
        return new NotFoundResult();
      }
      if (studentVm.Group is not null) {
        return new BadRequestResult();
      }
      ViewBag.SubjectName = studentVm.StudentName;

      var groups = _groupService.GetGroups(
          g => g.Students.All(s => s.Id != studentVm.Id));

      ViewBag.GroupsSelectList = new SelectList(groups.Select(t => new {
          Text = t.Name,
          Value = t.Id
      }), "Value", "Text");

      var modelVm = new AttachStudentToGroupVm {StudentId = studentId};

      return View(modelVm);
    }

    [HttpPost]
    public IActionResult AttachStudentToGroup(AttachStudentToGroupVm attachStudentToGroupVm) {
      if (ModelState.IsValid) {
        try {
          _groupService.AddStudentToGroup(attachStudentToGroupVm);
        }
        catch (InvalidOperationException e) {
          Logger.LogError(e.Message);
          return new BadRequestResult();
        }
        return RedirectToAction("Index");
      }

      return View();
    }

    public IActionResult DetachStudentFromGroup(int studentId) {
      var studentVm = _studentService.GetStudent(s => s.Id == studentId);
      if (studentVm is null) {
        return new NotFoundResult();
      }
      ViewBag.StudentName = studentVm.StudentName;
      ViewBag.GroupVm = studentVm.Group;

      Logger.LogInformation("{Id}", studentVm.Id);
      Logger.LogInformation("{Id}", studentVm.Group.Id);

      return View(Mapper.Map<DetachStudentFromGroupVm>(studentVm));
    }

    [HttpPost]
    public IActionResult DetachStudentFromGroup(DetachStudentFromGroupVm detachStudentFromGroupVm) {
      Logger.LogInformation("{StudentId}", detachStudentFromGroupVm.StudentId);
      Logger.LogInformation("{GroupId}", detachStudentFromGroupVm.GroupId);

      if (ModelState.IsValid) {
        _groupService.RemoveStudentFromGroup(detachStudentFromGroupVm);
        return RedirectToAction("Index");
      }

      return View();
    }
  }
}
