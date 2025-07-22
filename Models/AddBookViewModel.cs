using System.ComponentModel.DataAnnotations;

namespace BookishDB.Models;

public class AddBookViewModel
{

    [System.ComponentModel.DataAnnotations.Schema.DatabaseGenerated(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity)]
    [Key]
    public int BookID { get; set; }
    public string? Title { get; set; }
    public string? Author { get; set; }
    public int AddCopies { get; set; }
     public IList<BookViewModel>? Books { get; set; }
    // public IList<MemberBookViewModel> MemberBooks { get; set; }


}
