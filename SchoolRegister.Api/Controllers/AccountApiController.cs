using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SchoolRegister.Model.DataModels;
using SchoolRegister.ViewModels.VM;

namespace SchoolRegister.Api.Controllers {
  public class AccountApiController : BaseApiController {
    private readonly SignInManager<User> _signInManager;
    private readonly UserManager<User> _userManager;
    private readonly JwtOptionsVm _jwtOptions;

    public AccountApiController(ILogger logger,
        IMapper mapper,
        IOptions<JwtOptionsVm> jwtOptions,
        SignInManager<User> signInManager,
        UserManager<User> userManager) : base(logger, mapper) {
      _jwtOptions = jwtOptions?.Value;
      _signInManager = signInManager;
      _userManager = userManager;
    }

    [AllowAnonymous]
    [HttpPost("[action]")]
    public async Task<IActionResult> Login([FromForm] LoginUserVm applicationUser) {
      if (string.IsNullOrWhiteSpace(applicationUser.Login) || string.IsNullOrWhiteSpace(applicationUser.Password)) {
        Logger.LogInformation("Login or password are empty");
        return BadRequest("Invalid credentials");
      }

      var result = await _signInManager.PasswordSignInAsync(applicationUser.Login, applicationUser.Password, true, false);
      if (!result.Succeeded) {
        Logger.LogInformation($"Invalid username ({applicationUser.Login}) or password ({applicationUser.Password})");
        return BadRequest("Invalid credentials");
      }

      var token = await GenerateJwtSecurityToken(applicationUser);
      var encodedJwt = new JwtSecurityTokenHandler().WriteToken(token);

      return Json(new {
        access_token = encodedJwt,
        expires_in = token.ValidTo.ToString("yyyy-MM-ddTHH:mm:ss")
      });
    }

    private async Task<JwtSecurityToken> GenerateJwtSecurityToken(LoginUserVm applicationUser) {
      var user = await _userManager.FindByEmailAsync(applicationUser.Login);
      var userRoles = await _userManager.GetRolesAsync(user);
      var claims = new List<Claim> {
          new (ClaimTypes.Name, applicationUser.Login),
          new (JwtRegisteredClaimNames.Nbf,
              new DateTimeOffset(DateTime.Now).ToUnixTimeSeconds().ToString()),
          new (JwtRegisteredClaimNames.Exp,
              ((long) (DateTime.Now.AddMinutes(_jwtOptions.TokenExpirationMinutes)
                       - new DateTime(1970, 1, 1, 0, 0, 0)).TotalSeconds).ToString())
      };

      claims.AddRange(userRoles.Select(ur => new Claim(ClaimTypes.Role, ur)));
      var token = new JwtSecurityToken(
          new JwtHeader(new SigningCredentials(
              new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.SecretKey)),
              SecurityAlgorithms.HmacSha256)),
          new JwtPayload(claims));
      return token;
    }
  }
}
