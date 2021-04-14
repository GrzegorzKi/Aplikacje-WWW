using SchoolRegister.DAL.EF;
using SchoolRegister.Services.Interfaces;

namespace SchoolRegister.Tests.UnitTests {
  public class StudentServiceUnitTests : BaseUnitTests {
    private readonly IStudentService _studentService;

    public StudentServiceUnitTests(ApplicationDbContext dbContext, IStudentService studentService) : base(dbContext) {
      _studentService = studentService;
    }
  }
}
