using Bookish_v2_DB;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using Bookish_v2.Models;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;


namespace Bookish_v2.Controllers;

[Route("Login")]
public class LoginController : Controller
{
    private readonly ILogger<LoginController> _logger;
    private readonly BookishContext _context;

    public LoginController(ILogger<LoginController> logger, BookishContext context)
    {
        _logger = logger;
        _context = context;
    }

    [HttpGet("")]
    public IActionResult LoginPage()
    {

        return View("Login");
    }

    [HttpPost("CheckLogin")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CheckLogin([FromForm] CredentialsViewModel loginDetails)
    {
        if (loginDetails.Username == null || loginDetails.Password == null)
        {
            ViewBag.Error = "Please enter both username and password";
            return View("Login");
        }

        var existingUser = _context.Members.FirstOrDefault(m => m.Username == loginDetails.Username);

        if (existingUser == null)
        {
            ViewBag.Error = "Invalid login credentials";
            return View("Login");
        }
        byte[] saltByte =existingUser.Salt;
            
        var hashedPassword = Convert.ToBase64String(KeyDerivation.Pbkdf2(
        password: loginDetails.Password,
        salt: saltByte,
         prf: KeyDerivationPrf.HMACSHA256,
        iterationCount: 100000,
        numBytesRequested: 256 / 8));

            
        if (existingUser.Password != hashedPassword)
        {
            ViewBag.Error = "Invalid login credentials";
            return View("Login");
        }

        // Create claims for the user
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, existingUser.Username!),
            new Claim(ClaimTypes.NameIdentifier, existingUser.MemberID.ToString()),
            new Claim(ClaimTypes.Role, existingUser.Role.ToString()),
            new Claim("FullName", $"{existingUser.FirstName} {existingUser.LastName}")
        };

        var claimsIdentity = new ClaimsIdentity(claims, "Cookies");
        var authProperties = new AuthenticationProperties
        {
            IsPersistent = true, // Remember login
        };

        await HttpContext.SignInAsync("Cookies", new ClaimsPrincipal(claimsIdentity), authProperties);

        if (existingUser.Role == MemberViewModel.Roles.ADMIN)
        {
            return RedirectToAction("ListAllBooks", "Book");
        }
        return RedirectToAction("MemberPage", "Member", new { Id = existingUser.MemberID });
    }

    
    [HttpPost("Logout")]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync("Cookies");
        return RedirectToAction("LoginPage");
    }


}