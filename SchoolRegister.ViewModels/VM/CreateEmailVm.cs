using System.ComponentModel.DataAnnotations;

namespace SchoolRegister.ViewModels.VM {
  public class CreateEmailVm {
    [Required]
    [Display(Name = "Sender")]
    public int SenderId { get; set; }

    [Required]
    [Display(Name = "Recipient")]
    public int RecipientId { get; set; }

    [Display(Name = "Subject")]
    public string Subject { get; set; }

    [Display(Name = "Body")]
    public string Body { get; set; }
  }
}
