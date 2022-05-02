using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace AddressBook.Models;

public class User : IdentityUser
{
    [Required]
    [MaxLength(50)]
    public string FirstName { get; set; }

    [Required]
    [MaxLength(100)]
    public string LastName { get; set; }

}
