using System.ComponentModel.DataAnnotations;

namespace SchoolRegister.ViewModels.VM {
  public class AttachSubjectToGroupVm {
    [Required]
    public int SubjectId { get; set; }

    [Required]
    public int GroupId { get; set; }
  }
}
