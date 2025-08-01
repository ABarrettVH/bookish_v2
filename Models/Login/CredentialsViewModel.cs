using System.ComponentModel.DataAnnotations;

namespace Bookish_v2.Models;

public class CredentialsViewModel
{
    [System.ComponentModel.DataAnnotations.Schema.DatabaseGenerated(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity)]
    [Key]

    [Required]
    public string Username { get; set; }
    [Required]
    [DataType(DataType.Password)]
    public string Password { get; set; }

}