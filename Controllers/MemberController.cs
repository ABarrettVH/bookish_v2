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

       // var members = _context.Members.ToList();

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




}




