using Bookish_v2_DB;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using Bookish_v2.Models;


namespace Bookish_v2.Controllers;

[ApiController]
[Route("Members")]
[Produces("application/json")]
public class MemberController : Controller
{
    private readonly ILogger<MemberController> _logger;
    private readonly BookishContext _context;

    public MemberController(ILogger<MemberController> logger, BookishContext context)
    {
        _logger = logger;
        _context = context;
    }

    [HttpGet("")]
    public IActionResult ListAllMembers(string? SearchString)
    {

        var query = _context.Members.AsQueryable();

        if (!string.IsNullOrEmpty(SearchString))
        {
            query = query.Where(m => 
                (m.FirstName != null && m.FirstName.ToLower().Contains(SearchString.ToLower())) ||
                (m.LastName != null && m.LastName.ToLower().Contains(SearchString.ToLower())) ||
                (m.PostCode != null && m.PostCode.ToLower().Contains(SearchString.ToLower())));
        }

        var members = query.OrderBy(m => m.LastName).ToList();

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
            return BadRequest("Member already exists - please edit exisiting member if needed, or use a middle initial");
        }

        var member = new MemberViewModel
        {
            FirstName = newMember.FirstName,
            LastName = newMember.LastName,
            Email = newMember.Email,
            PostCode = newMember.PostCode
        };

        _context.Members.Add(member);
        _context.SaveChanges();

        return RedirectToAction("ListAllMembers");
    }

    [HttpPost("DeleteMember/{Id}")]
    public IActionResult DeleteMember(int Id)
    {

        var member = _context.Members.FirstOrDefault(m => m.MemberID == Id);
        if (member == null)
        {
            return NotFound("Member doesn't exist");
        }
        _context.Members.Remove(member);
        _context.SaveChanges();

        return RedirectToAction("ListAllMembers");
    }

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


}




