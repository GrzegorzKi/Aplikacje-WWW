using System.ComponentModel.DataAnnotations;

namespace SchoolRegister.ViewModels.VM {
  public class ParentVm {
    [Display(Name = "Parent Id")]
    public int Id { get; set; }

    [Display(Name = "Parent Name")]
    public string ParentName { get; set; }
  }
}
