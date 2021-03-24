using System.ComponentModel.DataAnnotations;
using SchoolRegister.Model.DataModels;

namespace SchoolRegister.ViewModels.VM {
  public class AddStudentToGroupVm {
    [Required]
    public int StudentId { get; set; }

    [Required]
    public int GroupId { get; set; }
  }
}
