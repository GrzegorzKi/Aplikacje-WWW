namespace SchoolRegister.ViewModels.VM {
  public class EmailVm {
    public string SenderName { get; set; }

    public string SenderEmail { get; set; }

    public string RecipientName { get; set; }

    public string RecipientEmail { get; set; }

    public string Subject { get; set; }

    public string Body { get; set; }

    // Pomijamy w viewmodel-u załączniki
  }
}
