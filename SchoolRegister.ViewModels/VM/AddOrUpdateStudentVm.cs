using System;
using System.ComponentModel.DataAnnotations;

namespace SchoolRegister.ViewModels.VM {
  public class AddOrUpdateStudentVm {
    public int? Id { get; set; }

    [Required]
    [Display(Name = "First Name")]
    public string FirstName { get; set; }

    [Required]
    [Display(Name = "Last Name")]
    public string LastName { get; set; }

    [Display(Name = "Registration Date")]
    public DateTime? RegistrationDate { get; set; }

    [Required]
    [Display(Name = "Group")]
    public int GroupId { get; set; }

    [Required]
    [Display(Name = "Parent")]
    public int ParentId { get; set; }
  }
}
