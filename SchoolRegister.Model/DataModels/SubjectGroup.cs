using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolRegister.Model.DataModels {
  public class SubjectGroup {
    public int GroupId { get; set; }
    public int SubjectId { get; set; }

    public virtual Group Group { get; set; }
    public virtual Subject Subject { get; set; }
  }
}
