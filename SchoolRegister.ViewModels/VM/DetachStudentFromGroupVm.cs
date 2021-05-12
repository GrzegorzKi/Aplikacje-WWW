using System.ComponentModel.DataAnnotations;

namespace SchoolRegister.ViewModels.VM {
  public class DetachStudentFromGroupVm {
    [Required]
    [Display(Name = "Student Id")]
    public int StudentId { get; set; }
    [Required]
    [Display(Name = "Group Id")]
    public int GroupId { get; set; }
  }
}
