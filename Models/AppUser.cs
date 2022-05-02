using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace AddressBook.Models;

public class AppUser : IdentityUser<int>
{
    [PersonalData, Required, StringLength(20)]
    public string FirstName { get; set; }

    [PersonalData, Required, StringLength(20)]
    public string LastName { get; set; }

    [NotMapped]
    public string FullName { get { return $"{FirstName} {LastName}"; } }
}
