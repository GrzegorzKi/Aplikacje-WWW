using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolRegister.Model.DataModels {
  public class Student : User {
    [ForeignKey("Group")]
    public int? GroupId { get; set; }

    [ForeignKey("Parent")]
    public int? ParentId { get; set; }

    public double AverageGrade { get; }

    public virtual IList<Grade> Grades { get; set; }
    public IDictionary<string, List<GradeScale>> GradesPerSubject { get; }
    public IDictionary<string, double> AverageGradePerSubject { get; }
    public virtual Parent Parent { get; set; }
    public virtual Group Group { get; set; }
  }
}