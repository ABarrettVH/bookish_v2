using System.ComponentModel.DataAnnotations;

namespace BookishDB.Models;

public class MemberBookViewModel
{
    [System.ComponentModel.DataAnnotations.Schema.DatabaseGenerated(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity)]   
    [Key]
    public int MemberBookID { get; set; }
    public int MemberID { get; set; }
    public int BookID { get; set; }
    public BookViewModel? Book { get; set; }
    public MemberViewModel? Member { get; set; }

}