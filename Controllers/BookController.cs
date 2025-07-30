using Bookish_v2_DB;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using Bookish_v2.Models;


namespace Bookish_v2.Controllers;

[ApiController]
[Route("Books")]
[Produces("application/json")]
public class BookController : Controller
{
    private readonly ILogger<BookController> _logger;
    private readonly BookishContext _context;

    public BookController(ILogger<BookController> logger, BookishContext context)
    {
        _logger = logger;
        _context = context;
    }



    [HttpGet("")]
    public IActionResult ListAllBooks(string? SearchString)
    {


        var query = _context.Books.AsQueryable();

        if (!string.IsNullOrEmpty(SearchString))
        {
            query = query.Where(b =>
                (b.Author != null && b.Author.ToLower().Contains(SearchString.ToLower())) ||
                (b.Title != null && b.Title.ToLower().Contains(SearchString.ToLower())));
        }

        var books = query.OrderBy(b => b.Author).ToList();
        var members = _context.Members.ToList();
        var memberBooks = _context.MemberBooks.ToList();

        return View(new BookViewModel { Books = books, Members = members, MemberBooks = memberBooks });

    }

    [HttpGet("AddBook")]
    public IActionResult AddNewBookPage(string? SearchString)
    {
        return View("AddNewBook");
    }

    [HttpPost("AddBook")]
    [ValidateAntiForgeryToken]
    public IActionResult AddNewBookMethod([FromForm] AddBookViewModel addbook)
    {
        if (addbook.Title == null || addbook.Author == null)
        {
            return BadRequest("Enter title and author");
        }

        var existingBook = _context.Books.FirstOrDefault(b => b.Title == addbook.Title && b.Author == addbook.Author);

        if (existingBook != null)
        {
            return BadRequest("Book already exists in catalogue - please edit if needed");
        }

        var newBook = new BookViewModel
        {
            Title = addbook.Title,
            Author = addbook.Author,
            AvailableCopies = addbook.AddCopies > 0 ? addbook.AddCopies : 1,
            TotalCopies = addbook.AddCopies > 0 ? addbook.AddCopies : 1
        };

        _context.Books.Add(newBook);
        _context.SaveChanges();

        return RedirectToAction("ListAllBooks");
    }

    [HttpPost("DeleteBook/{Id}")]
    public IActionResult DeleteBook(int Id)
    {

        var book = _context.Books.FirstOrDefault(b => b.BookID == Id);
        if (book == null)
        {
            return NotFound("Book doesn't exist");
        }
        _context.Books.Remove(book);
        _context.SaveChanges();

        return RedirectToAction("ListAllBooks");
    }

    [HttpGet("EditBook/{Id}")]
    public IActionResult EditBookPage(int Id)
    {
        var book = _context.Books.FirstOrDefault(b => b.BookID == Id);
        if (book == null)
        {
            return NotFound();
        }
        return View("EditBook", book);
    }

    [Route("EditBook/{Id}")]
    [ValidateAntiForgeryToken]
    [HttpPost]
    public IActionResult EditBook([FromForm] BookViewModel book, int Id)
    {

        var existingBook = _context.Books.FirstOrDefault(b => b.BookID == Id);
        if (existingBook == null)
        {
            return NotFound();
        }

        existingBook.Title = book.Title;
        existingBook.Author = book.Author;
        existingBook.AvailableCopies = existingBook.AvailableCopies - (existingBook.TotalCopies - book.TotalCopies);
        existingBook.TotalCopies = book.TotalCopies;
        if (existingBook.AvailableCopies < 0)
        {
            return BadRequest("Cannot remove checked out books - check book in first");
        }

        if (existingBook.TotalCopies == 0 && existingBook.AvailableCopies == 0)
        {
            _context.Remove(existingBook);

        }
        _context.SaveChanges();

        // return View("ListAllBooks", new BookViewModel { Books = _context.Books.OrderBy(b => b.Author).ToList() });
        return RedirectToAction("ListAllBooks");
    }

    [HttpGet("CheckOut/{Id}")]
    public IActionResult CheckOutBookPage(int Id)
    {
        var book = _context.Books.FirstOrDefault(b => b.BookID == Id);
        if (book == null)
        {
            return NotFound();
        }
        var memberBook = new MemberBookViewModel
        {
            BookID = Id,
            Book = book,
        };

        return View("CheckOutBook", memberBook);
    }

    [Route("CheckOut/{Id}")]
    [ValidateAntiForgeryToken]
    [HttpPost]
    public IActionResult CheckOut([FromForm] MemberBookViewModel memberBook, int Id)
    {
        var book = _context.Books.FirstOrDefault(b => b.BookID == Id);
        if (book == null)
        {
            return NotFound("Book doesn't exist");
        }
        if (book.AvailableCopies <= 0)
        {
            return BadRequest("No available copies");
        }
        var member = _context.Members.FirstOrDefault(m => m.MemberID == memberBook.MemberID);
        if (member == null)
        {
            return NotFound($"Member with ID {memberBook.MemberID} doesn't exist");
        }
        book.AvailableCopies--;


        memberBook.BookID = Id;
        memberBook.DueDate = DateTime.UtcNow.AddMonths(1);


        _context.MemberBooks.Add(memberBook);
        _context.SaveChanges();

        return RedirectToAction("ListAllBooks");

    }
    
    [ValidateAntiForgeryToken]
    [HttpPost("CheckIn/{memberID}")]
    public IActionResult CheckIn( [FromForm] int bookID, int memberID)
    {

        var book = _context.Books.FirstOrDefault(b => b.BookID == bookID);
        if (book == null)
        {
            return NotFound("Book doesn't exist");
        }
        if (book.AvailableCopies == book.TotalCopies)
        {
            return BadRequest("This book is not currently checked out");
        }
        var member = _context.Members.FirstOrDefault(m => m.MemberID == memberID);
        if (member == null)
        {
            return NotFound($"Member with ID {memberID} doesn't exist");
        }
        book.AvailableCopies++;

        var memberBook = _context.MemberBooks.FirstOrDefault(mb => mb.BookID == bookID && mb.MemberID == memberID);
        if (memberBook != null)
        {
            _context.MemberBooks.Remove(memberBook);
        }
        _context.SaveChanges();

        return RedirectToAction("MemberPage","Member", new { Id = memberID });

    }

}




