using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SchoolRegister.ViewModels.VM {
  public class SubjectVm {
    [Display(Name = "Subject Id")]
    public int Id { get; set; }

    [Display(Name = "Name")]
    public string Name { get; set; }

    [Display(Name = "Description")]
    public string Description { get; set; }

    public IList<GroupVm> Groups { get; set; }

    [Display(Name = "Teacher Name")]
    public string TeacherName { get; set; }

    [Display(Name = "Teacher Id")]
    public int? TeacherId { get; set; }
  }
}
