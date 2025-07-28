using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bookish_v2.Models;

public class MemberViewModel
{

    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int MemberID { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Email { get; set; }
    public string? PostCode { get; set; }


}