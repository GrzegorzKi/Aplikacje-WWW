using System.ComponentModel.DataAnnotations;

namespace SchoolRegister.ViewModels.VM {
  public class AddOrUpdateSubjectVm {
    public int? Id { get; set; }

    [Required]
    public string Name { get; set; }

    public string Description { get; set; }

    [Required]
    public int TeacherId { get; set; }
  }
}
