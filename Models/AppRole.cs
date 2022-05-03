using Microsoft.AspNetCore.Identity;

namespace AddressBook.Models;

public class AppRole : IdentityRole<int>
{
    public AppRole() { }

    public AppRole(string name)
    {
        Name = name;
    }
}
