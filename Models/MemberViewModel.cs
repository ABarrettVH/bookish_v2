using System.ComponentModel.DataAnnotations;

namespace Bookish_v2.Models;

public class MemberViewModel
{
    public MemberViewModel()
    {
        MemberBooks = new List<MemberBookViewModel>();
    }

    [System.ComponentModel.DataAnnotations.Schema.DatabaseGenerated(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity)]
    [Key]
    public int MemberID { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Email { get; set; }
    public IList<MemberBookViewModel> MemberBooks { get; set; }

}