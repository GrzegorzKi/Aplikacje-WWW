using System;
using System.ComponentModel.DataAnnotations;

namespace SchoolRegister.ViewModels.VM {
  public class AddGradeToStudentVm {

    [Required]
    public DateTime DateOfIssue { get; set; }

    [Required]
    public string GradeValue { get; set; }

    [Required]
    public int SubjectId { get; set; }

    [Required]
    public int StudentId { get; set; }

    [Required]
    public int TeacherId { get; set; }
  }
}
