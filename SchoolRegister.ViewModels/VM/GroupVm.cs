using System.ComponentModel.DataAnnotations;

namespace SchoolRegister.ViewModels.VM {
  public class GroupVm {
    [Display(Name = "Group Id")]
    public int Id { get; set; }

    [Display(Name = "Group Name")]
    public string Name { get; set; }
  }
}
