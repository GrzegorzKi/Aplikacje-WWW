using System.ComponentModel.DataAnnotations;

namespace SchoolRegister.ViewModels.VM {
  public class AddOrUpdateGroupVm {
    public int? Id { get; set; }

    [Required]
    [Display(Name = "Group Name")]
    public string Name { get; set; }
  }
}
