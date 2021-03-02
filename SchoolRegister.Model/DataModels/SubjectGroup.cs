using System.Collections.Generic;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolRegister.Model.DataModels
{
  public class SubjectGroup
  {
    public Group Group { get; set; }
    [Key]
    [Column(Order = 1)]
    [ForeignKey("Group")]
    public int GroupId { get; set; }
    public Subject Subject { get; set; }
    [Key]
    [Column(Order = 2)]
    [ForeignKey("Subject")]
    public int SubjectId { get; set; }
  }
}