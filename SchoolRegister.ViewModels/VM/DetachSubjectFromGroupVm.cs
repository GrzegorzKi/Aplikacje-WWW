using System.ComponentModel.DataAnnotations;

namespace SchoolRegister.ViewModels.VM {
  public class DetachSubjectFromGroupVm {
    [Required]
    public int SubjectId { get; set; }

    [Required]
    [Display(Name = "Group")]
    public int GroupId { get; set; }
  }
}
