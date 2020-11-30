using System;
using System.DirectoryServices.AccountManagement;
using System.Linq;

namespace PasswordChanger
{
    class Program
    {
        const string DOMAIN = "YOUR_DOMAIN.local";
        static public bool DoesUserExist(string userName)
        {
            using(var domainContext = new PrincipalContext(ContextType.Domain, DOMAIN))
            {
                using(var foundUser = UserPrincipal.FindByIdentity(domainContext, IdentityType.SamAccountName, userName))
                {
                    return foundUser != null;
                }
            }
        }

        static void Main(string[] args)
        {
            Console.WriteLine("Account Name to change password for: ");
            string user = Console.ReadLine();
            if (DoesUserExist(user))
            {
                Console.WriteLine("New password for " + user + ": ");
                string password = Console.ReadLine();
                if(password.Length == 0)
                {
                    Console.WriteLine("No password entered. Please press enter to restart the program.");
                    Console.ReadLine();
                    System.Diagnostics.Process.Start(Environment.GetCommandLineArgs()[0], Environment.GetCommandLineArgs().Length > 1 ? string.Join(" ", Environment.GetCommandLineArgs().Skip(1)) : null);
                }
                Console.WriteLine("Confirm new password: ");
                string confirm = Console.ReadLine();
                if (password.Equals(confirm))
                {
                    var context = new PrincipalContext(ContextType.Domain, DOMAIN);
                    var userAccount = UserPrincipal.FindByIdentity(context, IdentityType.SamAccountName, user);
                    userAccount.SetPassword(password);
                    Console.WriteLine("Password changed, press any key to close the program.");
                    Console.ReadLine();
                } else
                {
                    Console.WriteLine("Passwords do not match. Please press enter to restart the program.");
                    Console.ReadLine();
                    System.Diagnostics.Process.Start(Environment.GetCommandLineArgs()[0], Environment.GetCommandLineArgs().Length > 1 ? string.Join(" ", Environment.GetCommandLineArgs().Skip(1)) : null);
                }
            } else
            {
                Console.WriteLine("User " + user + " does not exist");
                Console.WriteLine("Press enter to restart the program");
                Console.ReadLine();
                System.Diagnostics.Process.Start(Environment.GetCommandLineArgs()[0], Environment.GetCommandLineArgs().Length > 1 ? string.Join(" ", Environment.GetCommandLineArgs().Skip(1)) : null);
            }
        }
    }
}
