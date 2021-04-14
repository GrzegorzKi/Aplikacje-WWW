using SchoolRegister.DAL.EF;
using SchoolRegister.Services.Interfaces;

namespace SchoolRegister.Tests.UnitTests {
  public class GroupServiceUnitTests : BaseUnitTests {
    private readonly IGroupService _groupService;

    public GroupServiceUnitTests(ApplicationDbContext dbContext, IGroupService groupService) : base(dbContext) {
      _groupService = groupService;
    }
  }
}
