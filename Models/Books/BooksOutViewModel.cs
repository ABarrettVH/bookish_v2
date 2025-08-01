using System.ComponentModel.DataAnnotations;

namespace Bookish_v2.Models;

public class BookOutViewModel
{

    public int BookID { get; set; }
    public string? Title { get; set; }
    public string? Author { get; set; }
    public DateTime DueDate { get; set; }




}
