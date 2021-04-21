using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using SchoolRegister.DAL.EF;
using SchoolRegister.Model.DataModels;
using SchoolRegister.ViewModels.VM;

namespace SchoolRegister.Web.Areas.Identity.Pages.Account.Manage {
  [Authorize(Roles = "Admin")]
  public class RegisterNewUsersDataModel : PageModel {
    private readonly ApplicationDbContext _dbContext;
    private readonly IEmailSender _emailSender;
    private readonly ILogger<RegisterNewUsersDataModel> _logger;
    private readonly SignInManager<User> _signInManager;
    private readonly UserManager<User> _userManager;

    public RegisterNewUsersDataModel(UserManager<User> userManager,
      SignInManager<User> signInManager,
      ILogger<RegisterNewUsersDataModel> logger,
      IEmailSender emailSender,
      ApplicationDbContext dbContext) {
      _userManager = userManager;
      _signInManager = signInManager;
      _logger = logger;
      _emailSender = emailSender;
      _dbContext = dbContext;
    }

    [BindProperty]
    public RegisterNewUserVm Input { get; set; }

    public void OnGet() {
      ViewData["Roles"] = new SelectList(_dbContext.Roles.Select(t => new {
        Text = t.Name,
        Value = t.Id
      }), "Value", "Text", _dbContext.Roles.FirstOrDefault(x => x.RoleValue == RoleValue.Parent)?.Id);

      ViewData["Groups"] = new SelectList(_dbContext.Groups.Select(t => new {
        Text = t.Name,
        Value = t.Id
      }), "Value", "Text");

      ViewData["Parents"] = new SelectList(_dbContext.Users.OfType<Parent>().Select(t =>
        new {
          Text = $"{t.FirstName} {t.LastName}",
          Value = t.Id
        }), "Value", "Text");
    }

    public async Task<IActionResult> OnPostAsync() {
      var returnUrl = "./RegisterNewUsers";
      if (ModelState.IsValid) {
        var tupleUserRole = CreateUserBasedOnRole(Input);
        var result = await _userManager.CreateAsync(tupleUserRole.Item1, Input.Password);
        if (result.Succeeded) {
          _logger.LogInformation("User created a new account with password");
          var code = await _userManager.GenerateEmailConfirmationTokenAsync(tupleUserRole.Item1);
          var callbackUrl = Url.Page(
            "/Account/ConfirmEmail",
            null,
            new {
              userId = tupleUserRole.Item1.Id,
              code
            },
            Request.Scheme);
          await _userManager.AddToRoleAsync(tupleUserRole.Item1, tupleUserRole.Item2.Name);
          return LocalRedirect(returnUrl);
        }

        foreach (var error in result.Errors) ModelState.AddModelError(string.Empty, error.Description);
      }

      OnGet();

      // If we got this far, something failed, redisplay form
      return Page();
    }

    private Tuple<User, Role> CreateUserBasedOnRole(RegisterNewUserVm inputModel) {
      var role = _dbContext.Roles.FirstOrDefault(r => r.Id == inputModel.RoleId);
      if (role == null) throw new InvalidOperationException($"Role with id {inputModel.RoleId} does not exist");

      switch (role.RoleValue) {
        case RoleValue.Student:
          return new Tuple<User, Role>(new Student {
            UserName = inputModel.Email,
            Email = inputModel.Email,
            FirstName = inputModel.FirstName,
            LastName = inputModel.LastName,
            GroupId = inputModel.GroupId,
            ParentId = inputModel.ParentId,
            RegistrationDate = DateTime.Now
          }, role);
        case RoleValue.Parent:
          return new Tuple<User, Role>(new Parent {
            UserName = inputModel.Email,
            Email = inputModel.Email,
            FirstName = inputModel.FirstName,
            LastName = inputModel.LastName,
            RegistrationDate = DateTime.Now
          }, role);
        case RoleValue.Teacher:
          return new Tuple<User, Role>(new Teacher {
            UserName = inputModel.Email,
            Email = inputModel.Email,
            FirstName = inputModel.FirstName,
            LastName = inputModel.LastName,
            RegistrationDate = DateTime.Now,
            Title = inputModel.TeacherTitles
          }, role);
        case RoleValue.Admin:
        case RoleValue.User:
          return new Tuple<User, Role>(new User {
            UserName = inputModel.Email,
            Email = inputModel.Email,
            FirstName = inputModel.FirstName,
            LastName = inputModel.LastName,
            RegistrationDate = DateTime.Now
          }, role);
        default:
          throw new ApplicationException($"{role.RoleValue} not recognized");
      }
    }
  }
}
