using System;
namespace SchoolRegister.ViewModels.VM {
  public class GradeVm {
    public DateTime DateOfIssue { get; set; }

    public string GradeValue { get; set; }

    public SubjectVm Subject { get; set; }

    public string StudentName { get; set; }

    public int? StudentId { get; set; }
  }
}
