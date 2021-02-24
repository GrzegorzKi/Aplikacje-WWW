using System.Collections.Generic;
using System;

namespace SchoolRegister.BLL.DataModels
{
  public class Student : User
  {
    public double AverageGrade { get; }
    public IDictionary<String, double> AverageGradePerSubject { get; }
    public IList<Grade> Grades { get; set; }
    public IDictionary<String, List<GradeScale>> GradesPerSubject { get; }
    public Group Group { get; set; }
    public int? GroupId { get; set; }
    public Parent Parent { get; set; }
    public int? ParentId { get; set; }
  }
}