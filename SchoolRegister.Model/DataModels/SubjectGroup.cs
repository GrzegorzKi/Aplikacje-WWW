using System.Collections.Generic;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolRegister.Model.DataModels
{
  public class SubjectGroup
  {
    // [Key, Column(Order = 1)]
    [ForeignKey("Group")]
    public int GroupId { get; set; }
    // [Key, Column(Order = 2)]
    [ForeignKey("Subject")]
    public int SubjectId { get; set; }

    public virtual Group Group { get; set; }
    public virtual Subject Subject { get; set; }
  }
}