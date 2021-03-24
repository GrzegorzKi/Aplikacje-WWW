using System.ComponentModel.DataAnnotations;

namespace SchoolRegister.ViewModels.VM {
  public class CreateEmailVm {
    [Required]
    public int SenderId { get; set; }

    [Required]
    public int RecipientId { get; set; }

    public string Subject { get; set; }

    public string Body { get; set; }
  }
}
