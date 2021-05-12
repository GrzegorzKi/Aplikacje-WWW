using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SchoolRegister.ViewModels.VM {
  public class StudentVm {
    [Display(Name = "Student Id")]
    public int Id { get; set; }

    [Display(Name = "Student Name")]
    public string StudentName { get; set; }

    [Display(Name = "Registration Date")]
    public DateTime? RegistrationDate { get; set; }

    [Display(Name = "Average Grade")]
    public double AverageGrade { get; set; }

    public IList<GradeVm> Grades { get; set; }

    public GroupVm Group { get; set; }

    public ParentVm Parent { get; set; }
  }
}
