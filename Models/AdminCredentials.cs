namespace AddressBook.Models;

public class AdminCredentialsOptions
{
    public const string SECTION = "AdminCredentials";

    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
}