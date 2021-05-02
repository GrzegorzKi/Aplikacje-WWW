using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using SchoolRegister.Model.DataModels;
using SchoolRegister.ViewModels.VM;

namespace SchoolRegister.Services.Interfaces {
  public interface IGroupService {
    GroupVm AddOrUpdateGroup(AddOrUpdateGroupVm addOrUpdateGroupVm);
    GroupVm DeleteGroup(DeleteGroupVm deleteGroupVm);
    GroupVm GetGroup(Expression<Func<Group, bool>> filterExpression);
    IEnumerable<GroupVm> GetGroups(Expression<Func<Group, bool>> filterExpression = null);
    StudentVm AddStudentToGroup(AttachStudentToGroupVm attachStudentToGroupVm);
    StudentVm RemoveStudentFromGroup(DetachStudentFromGroupVm detachStudentFromGroupVm);
    SubjectVm AttachSubjectToGroup(AttachSubjectToGroupVm attachSubjectToGroupVm);
    SubjectVm DetachSubjectFromGroup(DetachSubjectFromGroupVm detachSubjectFromGroupVm);
  }
}
