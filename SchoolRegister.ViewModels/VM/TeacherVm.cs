using System;
using System.Collections.Generic;

namespace SchoolRegister.ViewModels.VM {
  public class TeacherVm {
    public int Id { get; set; }

    public string TeacherName { get; set; }

    public string Title { get; set; }

    public IList<SubjectVm> Subjects { get; set; }
  }
}
