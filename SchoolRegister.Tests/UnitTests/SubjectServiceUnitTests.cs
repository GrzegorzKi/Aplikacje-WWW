using SchoolRegister.DAL.EF;
using SchoolRegister.Services.Interfaces;

namespace SchoolRegister.Tests.UnitTests {
  public class SubjectServiceUnitTests : BaseUnitTests {
    private readonly ISubjectService _subjectService;

    public SubjectServiceUnitTests(ApplicationDbContext dbContext, ISubjectService subjectService) : base(dbContext) {
      _subjectService = subjectService;
    }
  }
}
