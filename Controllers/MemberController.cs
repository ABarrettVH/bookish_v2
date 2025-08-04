using Bookish_v2_DB;
using Microsoft.AspNetCore.Mvc;
using Bookish_v2.Models;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Authorization;


namespace Bookish_v2.Controllers;

[Route("Members")]
public class MemberController : Controller
{
    private readonly ILogger<MemberController> _logger;
    private readonly BookishContext _context;

    public MemberController(ILogger<MemberController> logger, BookishContext context)
    {
        _logger = logger;
        _context = context;
    }

    [Authorize(Roles = "ADMIN")]
    [HttpGet("")]
    public IActionResult ListAllMembers(string? SearchString,string? SearchStringID)
    {

        var query = _context.Members.AsQueryable();

        if (!string.IsNullOrEmpty(SearchString))
        {
            query = query.Where(m =>
                (m.FirstName != null && m.FirstName.ToLower().Contains(SearchString.ToLower())) ||
                (m.LastName != null && m.LastName.ToLower().Contains(SearchString.ToLower())) ||
                (m.PostCode != null && m.PostCode.ToLower().Contains(SearchString.ToLower())));
        }

        if (!string.IsNullOrEmpty(SearchStringID) && int.TryParse(SearchStringID, out int memberId))
    {
        query = query.Where(m => m.MemberID == memberId);
    }

        var members = query.OrderBy(m => m.LastName).Where(m => m.Role == MemberViewModel.Roles.MEMBER).ToList();
        

        return View(members);

    }

    [HttpGet("AddMember")]
    public IActionResult AddNewMemberPage(string? SearchString)
    {
        return View("AddNewMember");
    }

    [HttpPost("AddMember")]
    [ValidateAntiForgeryToken]
    public IActionResult AddNewMemberMethod([FromForm] MemberViewModel newMember)
    {
        if (newMember.FirstName == null || newMember.LastName == null || newMember.PostCode == null)
        {
            return BadRequest("Enter member first and last name and postcode");
        }
        var existingMember = _context.Members.FirstOrDefault(m => m.FirstName == newMember.FirstName && m.LastName == newMember.LastName && m.PostCode == newMember.PostCode);

        if (existingMember != null)
        {
            ModelState.AddModelError(string.Empty, "Member already exists - please edit existing member if needed, or use a middle initial");
            return View("AddNewMember", newMember);
        }

        var usernameTaken = _context.Members.Any(m => m.Username == newMember.Username);
        if (usernameTaken)
        {
            ModelState.AddModelError(nameof(newMember.Username), "Username is taken - choose another");
            return View("AddNewMember", newMember);
        }

        byte[] salt = new byte[128 / 8];
            using (var rngCsp = new RNGCryptoServiceProvider())
            {
                rngCsp.GetNonZeroBytes(salt);

            }
        
        var hashedPassword = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: newMember.Password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 100000,
                numBytesRequested: 256 / 8));

        var member = new MemberViewModel
        {
            FirstName = newMember.FirstName,
            LastName = newMember.LastName,
            Email = newMember.Email,
            PostCode = newMember.PostCode,
            Username = newMember.Username,
            Password = hashedPassword,
            Salt = salt
        };

        _context.Members.Add(member);
        _context.SaveChanges();

        return RedirectToAction("ListAllMembers");
    }

    [Authorize(Roles = "ADMIN")]
    [HttpPost("DeleteMember/{Id}")]
    public IActionResult DeleteMember(int Id)
    {

        var member = _context.Members.FirstOrDefault(m => m.MemberID == Id);
        if (member == null)
        {
            return NotFound("Member doesn't exist");
        }

        if (_context.MemberBooks.Any(m => m.MemberID == member.MemberID))
        {
            return BadRequest("Cannot delete a member with checked out books. Check book back in first.");
        }
        _context.Members.Remove(member);
        _context.SaveChanges();

        return RedirectToAction("ListAllMembers");
    }

    [Authorize(Roles = "ADMIN")]
    [HttpGet("EditMember/{Id}")]
    public IActionResult EditMemberPage(int Id)
    {
        var member = _context.Members.FirstOrDefault(m => m.MemberID == Id);
        if (member == null)
        {
            return NotFound();
        }
        return View("EditMember", member);
    }

    [Authorize(Roles = "ADMIN")]
    [Route("EditMember/{Id}")]
    [ValidateAntiForgeryToken]
    [HttpPost]
    public IActionResult EditMember([FromForm] MemberViewModel member, int Id)
    {

        var existingMember = _context.Members.FirstOrDefault(m => m.MemberID == Id);
        if (existingMember == null)
        {
            return NotFound();
        }

        existingMember.FirstName = member.FirstName;
        existingMember.LastName = member.LastName;
        existingMember.PostCode = member.PostCode;
        existingMember.Email = member.Email;

        _context.SaveChanges();

        return RedirectToAction("ListAllMembers");
    }

    [HttpGet("EditMemberbyUser/{Id}")]
    public IActionResult EditMemberbyUserPage(int Id)
    {
        var member = _context.Members.FirstOrDefault(m => m.MemberID == Id);
        if (member == null)
        {
            return NotFound();
        }
        return View("EditMemberbyUser", member);
    }

    [Route("EditMemberbyUser/{Id}")]
    [ValidateAntiForgeryToken]
    [HttpPost]
    public IActionResult EditMemberbyUser([FromForm] MemberViewModel member, int Id)
    {

        var existingMember = _context.Members.FirstOrDefault(m => m.MemberID == Id);
        if (existingMember == null)
        {
            return NotFound();
        }

        var usernameTaken = _context.Members.Any(m => m.Username == member.Username && m.MemberID != Id);
        if (usernameTaken)
        {
        ModelState.AddModelError(nameof(member.Username), "Username is taken - choose another");
        return View("EditMemberbyUser", existingMember);
        }

        existingMember.FirstName = member.FirstName;
        existingMember.LastName = member.LastName;
        existingMember.PostCode = member.PostCode;
        existingMember.Email = member.Email;
        existingMember.Username = member.Username;

        _context.SaveChanges();

        var booksOut = _context.MemberBooks
                            .Where(m => m.MemberID == member.MemberID)
                            .Select(mb => new BookOutViewModel
                            {
                                BookID = mb.BookID,
                                Title = mb.Book.Title,
                                Author = mb.Book.Author,
                                DueDate = mb.DueDate
                            }) 
                            .ToList();
        
        var memberBooks = new MemberPageViewModel
        {
            Member = existingMember,
            Books = booksOut,

        };

        foreach (var book in booksOut)
        {
            if (book.DueDate < DateTime.UtcNow)
            {
                var overdueDays = Math.Ceiling((DateTime.UtcNow - book.DueDate).TotalDays);
                member.Fine += 0.25f * (float)overdueDays;
            }
        }
        

        return View("MemberPage", memberBooks);

    }



    [HttpGet("Member/{Id}")]
    public IActionResult MemberPage(int Id)
    {
        var member = _context.Members.FirstOrDefault(m => m.MemberID == Id);
        if (member == null)
        {
            return NotFound();
        }

        var booksOut = _context.MemberBooks
                            .Where(m => m.MemberID == member.MemberID)
                            .Select(mb => new BookOutViewModel
                            {
                                BookID = mb.BookID,
                                Title = mb.Book.Title,
                                Author = mb.Book.Author,
                                DueDate = mb.DueDate
                            }) 
                            .ToList();
        
        var memberBooks = new MemberPageViewModel
        {
            Member = member,
            Books = booksOut,

        };

        foreach (var book in booksOut)
        {
            if (book.DueDate < DateTime.UtcNow)
            {
                var overdueDays = Math.Ceiling((DateTime.UtcNow - book.DueDate).TotalDays);
                member.Fine += 0.25f * (float)overdueDays;
            }
        }
        

        return View("MemberPage", memberBooks);
    }

}




