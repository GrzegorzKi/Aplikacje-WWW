using System;
using System.ComponentModel.DataAnnotations;
using SchoolRegister.Model.DataModels;

namespace SchoolRegister.ViewModels.VM {
  public class AddGradeToStudentVm {

    [Required]
    [Display(Name = "Date of issue")]
    public DateTime DateOfIssue { get; set; }

    [Required]
    [Display(Name = "Grade")]
    public GradeScale GradeValue { get; set; }

    [Required]
    [Display(Name = "Subject")]
    public int SubjectId { get; set; }

    [Required]
    [Display(Name = "Student")]
    public int StudentId { get; set; }

    [Required]
    [Display(Name = "Teacher")]
    public int TeacherId { get; set; }
  }
}
