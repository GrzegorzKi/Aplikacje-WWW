using System;
using System.ComponentModel.DataAnnotations;

namespace SchoolRegister.ViewModels.VM {
  public class GradeVm {
    [Display(Name = "Date of issue")]
    public DateTime DateOfIssue { get; set; }

    [Display(Name = "Grade")]
    public string GradeValue { get; set; }

    public SubjectVm Subject { get; set; }

    [Display(Name = "Student name")]
    public string StudentName { get; set; }

    [Display(Name = "Student Id")]
    public int? StudentId { get; set; }
  }
}
