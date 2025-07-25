using System.ComponentModel.DataAnnotations;

namespace Bookish_v2.Models;

public class BookViewModel
{
    public BookViewModel()
    {
        MemberBooks = new List<MemberBookViewModel>();
    
    }

    [System.ComponentModel.DataAnnotations.Schema.DatabaseGenerated(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity)]
    [Key]
    public int BookID { get; set; }
    public string? Title { get; set; }
    public string? Author { get; set; }
    public int AvailableCopies { get; set; }
    public int TotalCopies { get; set; }
    public IList<BookViewModel>? Books { get; set; }
    public IList<MemberBookViewModel> MemberBooks { get; set; }


}
