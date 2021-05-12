using System.ComponentModel.DataAnnotations;

namespace SchoolRegister.ViewModels.VM {
  public class AttachStudentToGroupVm {
    [Required]
    public int StudentId { get; set; }

    [Required]
    [Display(Name = "Group")]
    public int GroupId { get; set; }
  }
}
