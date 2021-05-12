using System.ComponentModel.DataAnnotations;

namespace SchoolRegister.ViewModels.VM {
  public class AddOrUpdateSubjectVm {
    public int? Id { get; set; }

    [Required]
    [Display(Name = "Name")]
    public string Name { get; set; }

    [Display(Name = "Description")]
    public string Description { get; set; }

    [Required]
    [Display(Name = "Teacher")]
    public int TeacherId { get; set; }
  }
}
