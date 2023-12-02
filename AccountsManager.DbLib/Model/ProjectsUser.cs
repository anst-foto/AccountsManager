namespace AccountsManager.DbLib.Model;

public class ProjectsUser
{
    public int Id { get; set; }

    public int IdUser { get; set; }

    public int IdProject { get; set; }

    public virtual Project IdProjectNavigation { get; set; } = null!;

    public virtual User IdUserNavigation { get; set; } = null!;
}
