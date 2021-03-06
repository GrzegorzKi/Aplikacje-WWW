using System;
using SchoolRegister.DAL.EF;
using SchoolRegister.Services.Interfaces;
using SchoolRegister.ViewModels.VM;
using Xunit;

namespace SchoolRegister.Tests.UnitTests {
  public class GroupServiceUnitTests : BaseUnitTests {
    private readonly IGroupService _groupService;

    public GroupServiceUnitTests(ApplicationDbContext dbContext, IGroupService groupService) : base(dbContext) {
      _groupService = groupService;
    }

    [Fact]
    public void AddGroup() {
      var addOrUpdateGroupVm = new AddOrUpdateGroupVm {
        Name = "SK"
      };

      var group = _groupService.AddOrUpdateGroup(addOrUpdateGroupVm);

      Assert.NotNull(group);
      Assert.Equal("SK", group.Name);
    }

    [Fact]
    public void UpdateGroup() {
      var addOrUpdateGroupVm = new AddOrUpdateGroupVm {
        Id = 3,
        Name = "Erasmus"
      };

      var group = _groupService.AddOrUpdateGroup(addOrUpdateGroupVm);

      Assert.NotNull(group);
      Assert.Equal(3, group.Id);
      Assert.Equal("Erasmus", group.Name);
    }

    [Fact]
    public void UpdateGroupWithNullArgumentShouldThrowException() {
      Assert.Throws<ArgumentNullException>(() => _groupService.AddOrUpdateGroup(null));
    }

    [Fact]
    public void DeleteGroup() {
      var deleteGroupVm = new DeleteGroupVm {
        Id = 2
      };

      var group = _groupService.DeleteGroup(deleteGroupVm);

      Assert.NotNull(group);
      Assert.NotNull(group.Name);
    }

    [Fact]
    public void DeleteGroupWithNullArgumentShouldThrowException() {
      Assert.Throws<ArgumentNullException>(() => _groupService.DeleteGroup(null));
    }

    [Fact]
    public void DeleteGroupWithNonExistentArgumentShouldThrowException() {
      var deleteGroupVm = new DeleteGroupVm {
        Id = -1
      };

      Assert.Throws<InvalidOperationException>(() => _groupService.DeleteGroup(deleteGroupVm));
    }

    [Fact]
    public void GetGroup() {
      var group = _groupService.GetGroup(g => g.Id == 1);

      Assert.NotNull(group);
      Assert.Equal("IO", group.Name);
    }

    [Fact]
    public void GetGroupWithNullArgumentShouldThrowException() {
      Assert.Throws<ArgumentNullException>(() => _groupService.GetGroup(null));
    }

    [Fact]
    public void GetGroups() {
      var groups = _groupService.GetGroups(g => g.Id == 1);

      Assert.NotNull(groups);
      Assert.NotEmpty(groups);
      Assert.All(groups, vm => Assert.Equal(1, vm.Id));
    }

    [Fact]
    public void GetGroupsWithNullArgument() {
      var groups = _groupService.GetGroups();

      Assert.NotNull(groups);
      Assert.NotEmpty(groups);
    }

    [Fact]
    public void AddStudentToGroup() {
      var addStudentToGroupVm = new AttachStudentToGroupVm {
        StudentId = 10, // This Id cannot have GroupId assigned
        GroupId = 1
      };

      var student = _groupService.AddStudentToGroup(addStudentToGroupVm);

      Assert.NotNull(student);
      Assert.Equal(10, student.Id);
      Assert.Equal(1, student.Group.Id);
    }

    [Fact]
    public void AddStudentToGroupWithNullArgumentShouldThrowException() {
      Assert.Throws<ArgumentNullException>(() => _groupService.AddStudentToGroup(null));
    }

    [Fact]
    public void AddStudentToGroupWhenInAGroupShouldThrowException() {
      var addStudentToGroupVm = new AttachStudentToGroupVm {
        StudentId = 5,
        GroupId = 1
      };

      Assert.Throws<InvalidOperationException>(() => _groupService.AddStudentToGroup(addStudentToGroupVm));
    }

    [Fact]
    public void RemoveStudentFromGroup() {
      var removeStudentFromGroupVm = new DetachStudentFromGroupVm {
        StudentId = 6, // This Id must have GroupId assigned
        GroupId = 1
      };

      var student = _groupService.RemoveStudentFromGroup(removeStudentFromGroupVm);

      Assert.NotNull(student);
      Assert.Equal(6, student.Id);
      Assert.Null(student.Group);
    }

    [Fact]
    public void RemoveNonExistentStudentFromGroupShouldThrowException() {
      var removeStudentFromGroupVm = new DetachStudentFromGroupVm {
        StudentId = -1,
        GroupId = 1
      };

      Assert.Throws<InvalidOperationException>(() => _groupService.RemoveStudentFromGroup(removeStudentFromGroupVm));
    }

    [Fact]
    public void RemoveStudentFromGroupWithNullArgumentShouldThrowException() {
      Assert.Throws<ArgumentNullException>(() => _groupService.RemoveStudentFromGroup(null));
    }

    [Fact]
    public void RemoveStudentFromGroupWhenInAWrongGroupShouldThrowException() {
      var removeStudentFromGroupVm = new DetachStudentFromGroupVm {
        StudentId = 6,
        GroupId = 3
      };

      Assert.Throws<InvalidOperationException>(() => _groupService.RemoveStudentFromGroup(removeStudentFromGroupVm));
    }

    [Fact]
    public void AttachSubjectToGroup() {
      var attachSubjectToGroup = new AttachSubjectToGroupVm {
          SubjectId = 3, // This Id cannot have given GroupId assigned
          GroupId = 1
      };

      var subject = _groupService.AttachSubjectToGroup(attachSubjectToGroup);

      Assert.NotNull(subject);
      Assert.Equal(3, subject.Id);
      Assert.Contains(subject.Groups, vm => vm.Id == 1);
    }

    [Fact]
    public void AttachSubjectToGroupWithNullArgumentShouldThrowException() {
      Assert.Throws<ArgumentNullException>(() => _groupService.AttachSubjectToGroup(null));
    }

    [Fact]
    public void AttachSubjectToGroupWhenInAGroupShouldThrowException() {
      var attachSubjectToGroup = new AttachSubjectToGroupVm {
          SubjectId = 1,
          GroupId = 2
      };

      Assert.Throws<InvalidOperationException>(() => _groupService.AttachSubjectToGroup(attachSubjectToGroup));
    }

    [Fact]
    public void DetachSubjectFromGroup() {
      var detachSubjectFromGroup = new DetachSubjectFromGroupVm {
          SubjectId = 2, // This Id must have given GroupId assigned
          GroupId = 1
      };

      var subject = _groupService.DetachSubjectFromGroup(detachSubjectFromGroup);

      Assert.NotNull(subject);
      Assert.Equal(2, subject.Id);
      Assert.DoesNotContain(subject.Groups, vm => vm.Id == 1);
    }

    [Fact]
    public void DetachNonExistentSubjectFromGroupShouldThrowException() {
      var detachSubjectFromGroup = new DetachSubjectFromGroupVm {
          SubjectId = -1,
          GroupId = 1
      };

      Assert.Throws<InvalidOperationException>(() => _groupService.DetachSubjectFromGroup(detachSubjectFromGroup));
    }

    [Fact]
    public void DetachSubjectFromGroupWithNullArgumentShouldThrowException() {
      Assert.Throws<ArgumentNullException>(() => _groupService.DetachSubjectFromGroup(null));
    }

    [Fact]
    public void DetachSubjectFromGroupWhenInAWrongGroupShouldThrowException() {
      var detachSubjectFromGroup = new DetachSubjectFromGroupVm {
        SubjectId = 6,
        GroupId = 3
      };

      Assert.Throws<InvalidOperationException>(() => _groupService.DetachSubjectFromGroup(detachSubjectFromGroup));
    }
  }
}
