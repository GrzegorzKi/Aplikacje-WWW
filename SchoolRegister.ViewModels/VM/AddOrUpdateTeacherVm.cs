using System;
using System.ComponentModel.DataAnnotations;

namespace SchoolRegister.ViewModels.VM {
  public class AddOrUpdateTeacherVm {
    public int? Id { get; set; }

    [Required]
    public string FirstName { get; set; }

    [Required]
    public string LastName { get; set; }

    public DateTime? RegistrationDate { get; set; }

    [Required]
    public string Title { get; set; }
  }
}
