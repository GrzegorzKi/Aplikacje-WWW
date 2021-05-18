using System;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SchoolRegister.Model.DataModels;
using SchoolRegister.Services.Interfaces;
using SchoolRegister.ViewModels.VM;

namespace SchoolRegister.Api.Controllers {
  [Authorize(Roles = "Admin, Parent, Teacher")]
  public class StudentApiController : BaseApiController {
    private readonly IStudentService _studentService;
    private readonly UserManager<User> _userManager;

    private static readonly string ERROR_MESSAGE = "An error occurred";

    public StudentApiController(ILogger logger,
        IMapper mapper,
        IStudentService studentService,
        UserManager<User> userManager) : base(logger, mapper) {
      _studentService = studentService;
      _userManager = userManager;
    }

    [HttpGet]
    public async Task<IActionResult> Get() {
      var user = await _userManager.FindByNameAsync(User?.Identity?.Name);
      if (await _userManager.IsInRoleAsync(user, "Admin")
          || await _userManager.IsInRoleAsync(user, "Teacher")) {
        return Ok(_studentService.GetStudents());
      } else if (await _userManager.IsInRoleAsync(user, "Parent")) {
        if (user is Parent parent) {
          return Ok(_studentService.GetStudents(s => s.ParentId == parent.Id));
        } else {
          return BadRequest("Parent role is assigned to user, but user is not of Parent type");
        }
      } else {
        return BadRequest(ERROR_MESSAGE);
      }
    }

    [HttpGet("{id:int:min(1)}")]
    public async Task<IActionResult> Get(int id) {
      var user = await _userManager.FindByNameAsync(User?.Identity?.Name);
      if (await _userManager.IsInRoleAsync(user, "Admin")
          || await _userManager.IsInRoleAsync(user, "Teacher")) {
        return Ok(_studentService.GetStudent(s => s.Id == id));
      } else if (await _userManager.IsInRoleAsync(user, "Parent")) {
        if (user is Parent parent) {
          return Ok(_studentService.GetStudent(s => s.ParentId == parent.Id && s.Id == id));
        } else {
          return BadRequest("Parent role is assigned to user, but user is not of Parent type");
        }
      } else {
        return BadRequest(ERROR_MESSAGE);
      }
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public IActionResult Post([FromBody] AddOrUpdateStudentVm addOrUpdateStudentVm) {
      try {
        if (ModelState == null || !ModelState.IsValid) {
          return BadRequest(ModelState);
        }
        var studentVm = _studentService.AddOrUpdateStudent(addOrUpdateStudentVm);
        return Ok(studentVm);
      }
      catch (ArgumentNullException) {
        return NotFound();
      }
      catch (Exception) {
        return BadRequest(ERROR_MESSAGE);
      }
    }

    [HttpPut]
    [Authorize(Roles = "Admin")]
    public IActionResult Put([FromBody] AddOrUpdateStudentVm addOrUpdateStudentVm) {
      try {
        if (ModelState == null || !ModelState.IsValid) {
          return BadRequest(ModelState);
        }
        var studentVm = _studentService.AddOrUpdateStudent(addOrUpdateStudentVm);
        return Ok(studentVm);
      }
      catch (ArgumentNullException) {
        return NotFound();
      }
      catch (Exception) {
        return BadRequest(ERROR_MESSAGE);
      }
    }
  }
}
