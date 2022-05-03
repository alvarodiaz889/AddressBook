namespace AddressBook.Models;

public class EmailSettingsOptions
{
    public const string SECTION = "EmailSettings";
    public string Email { get; set; }
    public string Password { get; set; }
    public string Host { get; set; }
    public int Port { get; set; }

}