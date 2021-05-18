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
  [Authorize(Roles = "Student, Parent, Teacher")]
  public class GradeApiController : BaseApiController {
    private readonly IGradeService _gradeService;
    private readonly IStudentService _studentService;
    private readonly UserManager<User> _userManager;

    private static readonly string ERROR_MESSAGE = "An error occurred";

    public GradeApiController(ILogger logger,
        IMapper mapper,
        IGradeService gradeService,
        IStudentService studentService,
        UserManager<User> userManager) : base(logger, mapper) {
      _gradeService = gradeService;
      _studentService = studentService;
      _userManager = userManager;
    }

    public async Task<IActionResult> Get() {
      try {
        var user = await _userManager.FindByNameAsync(User?.Identity?.Name);
        if (await _userManager.IsInRoleAsync(user, "Student")) {
          var getGradesReportVm = new GetGradesReportVm {StudentId = user.Id, GetterUserId = user.Id};
          return Ok(_gradeService.GetGradesReportForStudent(getGradesReportVm));
        } else {
          return BadRequest("Id must be provided");
        }
      }
      catch (ArgumentNullException) {
        return NotFound();
      }
      catch (Exception) {
        return BadRequest(ERROR_MESSAGE);
      }
    }

    [HttpGet("{id:int:min(1)}")]
    public async Task<IActionResult> Get(int id) {
      try {
        var studentVm = _studentService.GetStudent(s => s.Id == id);
        if (studentVm is null) {
          return NotFound();
        }

        var user = await _userManager.FindByNameAsync(User?.Identity?.Name);
        var getGradesReportVm = new GetGradesReportVm {StudentId = id, GetterUserId = user.Id};
        return Ok(_gradeService.GetGradesReportForStudent(getGradesReportVm));
      }
      catch (ArgumentNullException) {
        return NotFound();
      }
      catch (Exception) {
        return BadRequest(ERROR_MESSAGE);
      }
    }

    [HttpPost]
    [Authorize(Roles = "Teacher")]
    public IActionResult Post([FromBody] AddGradeToStudentVm addGradeToStudentVm) {
      try {
        if (ModelState == null || !ModelState.IsValid) {
          return BadRequest(ModelState);
        }
        var gradeVm = _gradeService.AddGradeToStudent(addGradeToStudentVm);
        return Ok(gradeVm);
      }
      catch (ArgumentNullException) {
        return NotFound();
      }
      catch (Exception) {
        return BadRequest(ERROR_MESSAGE);
      }
    }

    [HttpPut]
    [Authorize(Roles = "Teacher")]
    public IActionResult Put([FromBody] AddGradeToStudentVm addGradeToStudentVm) {
      try {
        if (ModelState == null || !ModelState.IsValid) {
          return BadRequest(ModelState);
        }
        var gradeVm = _gradeService.AddGradeToStudent(addGradeToStudentVm);
        return Ok(gradeVm);
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
