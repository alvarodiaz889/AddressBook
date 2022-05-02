using System.ComponentModel.DataAnnotations;

namespace AddressBook.Models;

public class Contact
{
    [Key]
    public string Id { get; set; }

    [Required]
    [MaxLength(50)]
    public string FirstName { get; set; }

    [Required]
    [MaxLength(100)]
    public string LastName { get; set; }

    [MaxLength(10)]
    public string PhoneNumber { get; set; }

    [MaxLength(100)]
    public string StreetName { get; set; }

    [MaxLength(50)]
    public string City { get; set; }

    [MaxLength(20)]
    public string Province { get; set; }

    [MaxLength(7)]
    public string PostalCode { get; set; }

    [MaxLength(50)]
    public string Country { get; set; }

}
