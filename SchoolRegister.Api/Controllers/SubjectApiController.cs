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

  [Authorize(Roles = "Teacher, Admin")]
  public class SubjectApiController : BaseApiController {
    private readonly ISubjectService _subjectService;
    private readonly ITeacherService _teacherService;
    private readonly IGradeService _gradeService;
    private readonly UserManager<User> _userManager;

    private static readonly string ERROR_MESSAGE = "An error occurred";

    public SubjectApiController(ILogger logger,
        IMapper mapper,
        ISubjectService subjectService,
        ITeacherService teacherService,
        IGradeService gradeService,
        UserManager<User> userManager) : base(logger, mapper) {
      _subjectService = subjectService;
      _teacherService = teacherService;
      _gradeService = gradeService;
      _userManager = userManager;
    }

    public async Task<IActionResult> Get() {
      var user = await _userManager.FindByNameAsync(User?.Identity?.Name);
      if (await _userManager.IsInRoleAsync(user, "Admin")) {
        return Ok(_subjectService.GetSubjects());
      } else if (await _userManager.IsInRoleAsync(user, "Teacher")) {
        if (user is Teacher teacher) {
          return Ok(_subjectService.GetSubjects(x => x.TeacherId == teacher.Id));
        } else {
          return BadRequest("Teacher role is assigned to user, but user is not of Teacher type");
        }
      } else {
        return BadRequest(ERROR_MESSAGE);
      }
    }

    [HttpGet("{id:int:min(1)}")]
    public async Task<IActionResult> Get(int id) {
      try {
        var user = await _userManager.FindByNameAsync(User?.Identity?.Name);
        if (await _userManager.IsInRoleAsync(user, "Admin")) {
          return Ok(_subjectService.GetSubject(s => s.Id == id));
        } else if (await _userManager.IsInRoleAsync(user, "Teacher")) {
          if (user is Teacher teacher) {
            return Ok(_subjectService.GetSubject(x => x.TeacherId == teacher.Id && x.Id == id));
          } else {
            return BadRequest("Teacher is assigned to role, but to the Teacher type.");
          }
        } else {
          return BadRequest("Error occurred");
        }
      }
      catch (ArgumentNullException) {
        return NotFound();
      }
      catch (Exception) {
        return BadRequest(ERROR_MESSAGE);
      }
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public IActionResult Post([FromBody] AddOrUpdateSubjectVm addOrUpdateSubjectVm) {
      try {
        if (ModelState == null || !ModelState.IsValid) {
          return BadRequest(ModelState);
        }
        var subjectVm = _subjectService.AddOrUpdateSubject(addOrUpdateSubjectVm);
        return Ok(subjectVm);
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
    public IActionResult Put([FromBody] AddOrUpdateSubjectVm addOrUpdateSubjectVm) {
      try {
        if (ModelState == null || !ModelState.IsValid) {
          return BadRequest(ModelState);
        }
        var subjectVm = _subjectService.AddOrUpdateSubject(addOrUpdateSubjectVm);
        return Ok(subjectVm);
      }
      catch (ArgumentNullException) {
        return NotFound();
      }
      catch (Exception) {
        return BadRequest(ERROR_MESSAGE);
      }
    }

    [HttpDelete("{id:int:min(1)}")]
    [Authorize(Roles = "Admin")]
    public IActionResult Delete(int id) {
      try {
        var result = _subjectService.RemoveSubject(s => s.Id == id);
        return Ok(new {success = result});
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
