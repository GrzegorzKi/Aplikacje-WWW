using System;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SchoolRegister.Services.Interfaces;
using SchoolRegister.ViewModels.VM;

namespace SchoolRegister.Api.Controllers {
  [Authorize(Roles = "Admin")]
  public class GroupApiController : BaseApiController {
    private readonly IGroupService _groupService;
    private readonly ISubjectService _subjectService;
    private readonly IStudentService _studentService;

    private static readonly string ERROR_MESSAGE = "An error occurred";

    public GroupApiController(ILogger logger,
        IMapper mapper,
        IGroupService groupService,
        ISubjectService subjectService,
        IStudentService studentService) : base(logger, mapper) {
      _groupService = groupService;
      _subjectService = subjectService;
      _studentService = studentService;
    }

    public IActionResult Get() {
      return Ok(_groupService.GetGroups());
    }

    [HttpGet("{id:int:min(1)}")]
    public IActionResult Get(int id) {
      try {
        return Ok(_groupService.GetGroup(x => x.Id == id));
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
    public IActionResult Post([FromBody] AddOrUpdateGroupVm addOrUpdateGroupVm) {
      try {
        if (ModelState == null || !ModelState.IsValid) {
          return BadRequest(ModelState);
        }
        var groupVm = _groupService.AddOrUpdateGroup(addOrUpdateGroupVm);
        return Ok(groupVm);
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
    public IActionResult Put([FromBody] AddOrUpdateGroupVm addOrUpdateGroupVm) {
      try {
        if (ModelState == null || !ModelState.IsValid) {
          return BadRequest(ModelState);
        }
        var groupVm = _groupService.AddOrUpdateGroup(addOrUpdateGroupVm);
        return Ok(groupVm);
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
        var result = _groupService.DeleteGroup(new DeleteGroupVm {Id = id});
        return Ok(new {success = result});
      }
      catch (ArgumentNullException) {
        return NotFound();
      }
      catch (Exception) {
        return BadRequest(ERROR_MESSAGE);
      }
    }

    // TODO Add support for (maybe new Apis?):
    // * AttachSubjectToGroup
    // * DetachSubjectFromGroup
    // * AttachStudentToGroup
    // * DetachStudentFromGroup
  }
}
