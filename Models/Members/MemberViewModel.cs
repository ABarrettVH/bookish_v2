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
    [DataType(DataType.EmailAddress)]
    public string? Email { get; set; }

    public string? PostCode { get; set; }
    // [Required]
    public string? Username { get; set; }
    // [Required]
    [DataType(DataType.Password)]
    public string? Password { get; set; }
    public byte[]? Salt { get; set; }
    public float Fine { get; set; } = 0;
    public Roles Role { get; set; } = Roles.MEMBER;

    public enum Roles
{
    ADMIN,
    MEMBER
}


}