using SchoolRegister.DAL.EF;
using SchoolRegister.Services.Interfaces;

namespace SchoolRegister.Tests.UnitTests {
  public class TeacherServiceUnitTests : BaseUnitTests {
    private readonly ITeacherService _teacherService;

    public TeacherServiceUnitTests(ApplicationDbContext dbContext, ITeacherService teacherService) : base(dbContext) {
      _teacherService = teacherService;
    }
  }
}
