using SchoolRegister.DAL.EF;

namespace SchoolRegister.Tests.UnitTests {
  public abstract class BaseUnitTests {
    protected readonly ApplicationDbContext DbContext;

    protected BaseUnitTests (ApplicationDbContext dbContext) {
      DbContext = dbContext;
    }
  }
}
