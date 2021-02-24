using Microsoft.AspNetCore.Identity;
using System;

namespace SchoolRegister.BLL.DataModels
{
  public class User : IdentityUser<int>
  {
    public String FirstName { get; set; }
    public String LastName { get; set; }
    public DateTime RegistrationDate { get; set; }
  }
}