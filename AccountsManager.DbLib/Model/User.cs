namespace AccountsManager.DbLib.Model;

public class User
{
    public int Id { get; set; }

    public string LastName { get; set; } = null!;

    public string FirstName { get; set; } = null!;

    public string Email { get; set; } = null!;

    public virtual Account IdNavigation { get; set; } = null!;

    public virtual ICollection<ProjectsUser> ProjectsUsers { get; set; } = new List<ProjectsUser>();
}
