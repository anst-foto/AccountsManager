using AccountsManager.Config;
using AccountsManager.DbLib;

var config = AppSettings.Init();

using var db = new DataContext(config.ConnectionString, config.Logger, config.LogLevel);

do
{
    foreach (var a in db.Accounts.ToList())
    {
        var role = a.IdRoleNavigation.RoleName;
        var user = a.User;
        Console.Write($"{a.Login} ({a.Password}) [{role}] -> {user.LastName} {user.FirstName} ({user.Email}): ");
        foreach (var p in user.ProjectsUsers)
        {
            Console.Write($"{p.IdProjectNavigation.Title}\t");
        }
        Console.WriteLine();
    }

    Console.ReadKey();
} while (true);
