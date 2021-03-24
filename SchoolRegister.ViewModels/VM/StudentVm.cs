using System;
using System.Collections.Generic;
using SchoolRegister.Model.DataModels;

namespace SchoolRegister.ViewModels.VM {
  public class StudentVm {
    public int Id { get; set; }

    public string StudentName { get; set; }

    public double AverageGrade { get; set; }

    public IList<GradeVm> Grades { get; set; }

    public GroupVm Group { get; set; }

    public Parent Parent { get; set; }
  }
}
