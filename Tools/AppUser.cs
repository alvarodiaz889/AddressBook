using AddressBook.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace AddressBook.Tools
{
    public class AppUserTools
    {

        public static AppUser CreateUser()
        {
            try
            {
                return Activator.CreateInstance<AppUser>();
            }
            catch
            {
                throw new InvalidOperationException($"Can't create an instance of '{nameof(AppUser)}'. " +
                    $"Ensure that '{nameof(AppUser)}' is not an abstract class and has a parameterless constructor, or alternatively " +
                    $"override the register page in /Areas/Identity/Pages/Account/Register.cshtml");
            }
        }

        public static UserViewModel GetUserViewModel(AppUser appUser)
        {
            return new UserViewModel
            {
                Id = appUser.Id,
                FirstName = appUser.FirstName,
                LastName = appUser.LastName,
                Email = appUser.Email,
            };
        }

        public static async void CreateInitialIdentity(UserManager<AppUser> um,
            RoleManager<AppRole> rm,
            AdminCredentialsOptions op)
        {
            try
            {
                var user = um.Users.FirstOrDefault(u => u.Email == op.Email);
                if (user == null)
                {
                    user = new AppUser
                    {
                        FirstName = op.FirstName,
                        LastName = op.LastName,
                        Email = op.Email,
                        UserName = op.Email
                    };
                    var result = await um.CreateAsync(user, op.Password);
                    if (result.Succeeded == false) return;
                }

                foreach (var r in op.Roles)
                {
                    AppRole ar = new AppRole { Name = r };

                    if (rm.Roles.Any(role => role.Name == r) == false)
                    {
                        var result = await rm.CreateAsync(ar);
                        if (result.Succeeded == false) return;
                    }
                    var isInRole = await um.IsInRoleAsync(user, r);
                    if (!isInRole)
                    {
                        var added = await um.AddToRoleAsync(user, r);
                        if (added.Succeeded == false) return;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
    }
}