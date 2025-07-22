using System.Diagnostics;
using BookishDB;
using Microsoft.AspNetCore.Mvc;
using BookishDB.Models;

namespace BookishDB.Controllers;
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

        List<BookViewModel> books;


        if (!string.IsNullOrEmpty(SearchString))
        {
            books = _context.Books.Where(b => b.Author.ToLower().Contains(SearchString.ToLower())
                                   || b.Title.ToLower().Contains(SearchString.ToLower())).ToList();


            books = books.OrderBy(b => b.Author).ToList();

            return View(new BookViewModel { Books = books });
        }
        else
        {
            books = _context.Books.ToList();
            books = books.OrderBy(b => b.Author).ToList();
            return View(new BookViewModel { Books = books });
        }
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

        return View("ListAllBooks", new BookViewModel { Books = _context.Books.OrderBy(b => b.Author).ToList() });
    }
    [HttpGet("EditBook")]
    public IActionResult EditBookPage(BookViewModel book)
    {
        return View("EditBook");
    }
    
    [Route("/editBook")]
    [ValidateAntiForgeryToken]
    [HttpPatch]
    public IActionResult EditBook([FromForm] BookViewModel book)
    {


        BookViewModel Book = new BookViewModel();

        var existingBook = _context.Books.FirstOrDefault(b => b.Title == book.Title && b.Author == book.Author);

        if (existingBook != null)
        {
            existingBook.Title = book.Title;
            existingBook.Author = book.Author;
            existingBook.AvailableCopies = existingBook.AvailableCopies-(existingBook.TotalCopies - book.TotalCopies);
            existingBook.TotalCopies = book.TotalCopies;
        if (existingBook.AvailableCopies < 0)
            {
                return BadRequest("Cannot remove checked out books - check book in first");
            }

        if (existingBook.TotalCopies == 0 && existingBook.AvailableCopies == 0)
            {
                _context.Remove(existingBook);
            }
        }
        _context.SaveChanges();

        return View();
        
    }
}




