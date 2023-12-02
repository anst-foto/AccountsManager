using AccountsManager.DbLib.Model;
using AccountsManager.Logger;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ILogger = AccountsManager.Logger.ILogger;

namespace AccountsManager.DbLib;

public class DataContext : DbContext
{
    private readonly string _connectionString;
    private readonly ILogger _logger;
    private readonly LogLevel _logLevel;

    public DataContext(string connectionString, ILogger? logger = null, LogLevel? logLevel = null)
    {
        _connectionString = connectionString;

        _logger = logger ?? new LogToConsole();

        _logLevel = logLevel ?? LogLevel.Debug;
    }

    public virtual DbSet<Account> Accounts { get; set; }

    public virtual DbSet<Project> Projects { get; set; }

    public virtual DbSet<ProjectsUser> ProjectsUsers { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder
            .UseNpgsql(_connectionString)
            .UseLazyLoadingProxies()
            .LogTo(_logger.Info, _logLevel);

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasPostgresExtension("pg_catalog", "adminpack");

        modelBuilder.Entity<Account>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("table_accounts_pkey");

            entity.ToTable("table_accounts");

            entity.HasIndex(e => e.Login, "constraint_unique_login").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.IdRole).HasColumnName("id_role");
            entity.Property(e => e.Login).HasColumnName("login");
            entity.Property(e => e.Password).HasColumnName("password");

            entity.HasOne(d => d.IdRoleNavigation).WithMany(p => p.Accounts)
                .HasForeignKey(d => d.IdRole)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("constraint_foreign_key_id_role");
        });

        modelBuilder.Entity<Project>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("table_projects_pkey");

            entity.ToTable("table_projects");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.Title).HasColumnName("title");
        });

        modelBuilder.Entity<ProjectsUser>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("table_projects_users_pkey");

            entity.ToTable("table_projects_users");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.IdProject).HasColumnName("id_project");
            entity.Property(e => e.IdUser).HasColumnName("id_user");

            entity.HasOne(d => d.IdProjectNavigation).WithMany(p => p.ProjectsUsers)
                .HasForeignKey(d => d.IdProject)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("constraint_foreign_key_id_project");

            entity.HasOne(d => d.IdUserNavigation).WithMany(p => p.ProjectsUsers)
                .HasForeignKey(d => d.IdUser)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("constraint_foreign_key_id_user");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("table_roles_pkey");

            entity.ToTable("table_roles");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.RoleName).HasColumnName("role");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("table_users_pkey");

            entity.ToTable("table_users");

            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnName("id");
            entity.Property(e => e.Email).HasColumnName("email");
            entity.Property(e => e.FirstName).HasColumnName("first_name");
            entity.Property(e => e.LastName).HasColumnName("last_name");

            entity.HasOne(d => d.IdNavigation).WithOne(p => p.User)
                .HasForeignKey<User>(d => d.Id)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("constraint_foreign_key_id");
        });
    }
}
