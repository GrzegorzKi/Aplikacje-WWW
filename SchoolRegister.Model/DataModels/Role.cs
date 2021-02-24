using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace SchoolRegister.BLL.DataModels
{
  public class Role : IdentityRole<int>
  {
    public RoleValue RoleValue { get; set; }

    public Role()
    {
    }

    public Role(String name, RoleValue roleValue) : base(name)
    {

    }
  }
}