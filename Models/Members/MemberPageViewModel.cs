using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bookish_v2.Models;

public class MemberPageViewModel
{

    public MemberViewModel? Member { get; set; }
    public IList<BookOutViewModel>? Books { get; set; }


}