using System;
using SchoolRegister.Model.DataModels;

namespace SchoolRegister.ViewModels.VM {
  public class GradeVm {
    public DateTime DateOfIssue { get; set; }

    public string GradeValue { get; set; }

    public SubjectVm Subject { get; set; }
  }
}
