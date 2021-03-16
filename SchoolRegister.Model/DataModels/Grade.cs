using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolRegister.Model.DataModels {
  public class Grade {
    // [Key, Column(Order = 1)]
    public DateTime DateOfIssue { get; set; }

    // [Key, Column(Order = 2)]
    [ForeignKey("Subject")]
    public int SubjectId { get; set; }

    // [Key, Column(Order = 3)]
    [ForeignKey("Student")]
    public int StudentId { get; set; }

    public virtual GradeScale GradeValue { get; set; }

    public virtual Student Student { get; set; }
    public virtual Subject Subject { get; set; }
  }
}