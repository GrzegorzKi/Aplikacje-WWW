using System.Collections.Generic;
using System;

namespace SchoolRegister.BLL.DataModels
{
  public class SubjectGroup
  {
    public Group Group { get; set; }
    public int GroupId { get; set; }
    public SubjectGroup Subject { get; set; }
    public int SubjectId { get; set; }
  }
}