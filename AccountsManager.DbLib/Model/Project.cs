namespace AccountsManager.DbLib.Model;

public class Project
{
    public int Id { get; set; }

    public string Title { get; set; } = null!;

    public string? Description { get; set; }

    public virtual ICollection<ProjectsUser> ProjectsUsers { get; set; } = new List<ProjectsUser>();
}
