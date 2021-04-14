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
    public void UpdateStudentWithNullArgumentShouldThrowException() {

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
      var group = _groupService.GetGroups(g => g.Id == 1);

      Assert.NotNull(group);
      Assert.NotEmpty(group);
      Assert.All(group, vm => Assert.Equal(1, vm.Id));
    }

    [Fact]
    public void GetGroupsWithNullArgument() {
      var group = _groupService.GetGroups(null);

      Assert.NotNull(group);
      Assert.NotEmpty(group);
    }

    [Fact]
    public void AddStudentToGroup() {
      var addStudentToGroupVm = new AddStudentToGroupVm {
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
      var addStudentToGroupVm = new AddStudentToGroupVm {
        StudentId = 5,
        GroupId = 1
      };

      Assert.Throws<InvalidOperationException>(() => _groupService.AddStudentToGroup(addStudentToGroupVm));
    }

    [Fact]
    public void RemoveStudentFromGroup() {
      var removeStudentFromGroupVm = new RemoveStudentFromGroupVm {
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
      var removeStudentFromGroupVm = new RemoveStudentFromGroupVm {
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
      var removeStudentFromGroupVm = new RemoveStudentFromGroupVm {
        StudentId = 6,
        GroupId = 3
      };

      Assert.Throws<InvalidOperationException>(() => _groupService.RemoveStudentFromGroup(removeStudentFromGroupVm));
    }
  }
}
