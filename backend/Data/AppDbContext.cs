using backend.Models;
using Microsoft.EntityFrameworkCore;

namespace backend.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Department> Departments => Set<Department>();
    public DbSet<User> Users => Set<User>();
    public DbSet<Assignment> Assignments => Set<Assignment>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // 1. Справочник подразделений (departments)
        modelBuilder.Entity<Department>(entity =>
        {
            entity.ToTable("departments");
            entity.HasKey(d => d.Id);

            entity.Property(d => d.Id)
                .HasColumnName("id")
                .UseIdentityByDefaultColumn();

            entity.Property(d => d.Code)
                .HasColumnName("code")
                .IsRequired();

            entity.Property(d => d.Name)
                .HasColumnName("name")
                .IsRequired();

            entity.HasIndex(d => d.Code).IsUnique();
            entity.HasIndex(d => d.Name).IsUnique();
        });

        // 2. Пользователи (users)
        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable("users", t =>
            {
                t.HasCheckConstraint(
                    "users_role_check",
                    "role IN ('Manager', 'Engineer', 'Dispatcher')");
            });

            entity.HasKey(u => u.Id);

            entity.Property(u => u.Id)
                .HasColumnName("id")
                .UseIdentityByDefaultColumn();

            entity.Property(u => u.FullName)
                .HasColumnName("full_name")
                .IsRequired();

            entity.Property(u => u.Email)
                .HasColumnName("email")
                .IsRequired();

            entity.Property(u => u.Role)
                .HasColumnName("role")
                .IsRequired();

            entity.HasIndex(u => u.Email).IsUnique();
        });

        // 3. Главный объект — Поручения (assignments)
        modelBuilder.Entity<Assignment>(entity =>
        {
            entity.ToTable("assignments", t =>
            {
                t.HasCheckConstraint(
                    "assignments_status_check",
                    "status IN ('New', 'InProgress', 'Closed', 'Cancelled')");
                t.HasCheckConstraint(
                    "assignments_title_len_check",
                    "char_length(btrim(title)) BETWEEN 5 AND 80");
                t.HasCheckConstraint(
                    "assignments_description_len_check",
                    "char_length(btrim(description)) BETWEEN 10 AND 500");
            });

            entity.HasKey(a => a.Id);

            entity.Property(a => a.Id)
                .HasColumnName("id")
                .UseIdentityByDefaultColumn();

            entity.Property(a => a.Number)
                .HasColumnName("number")
                .IsRequired();

            entity.Property(a => a.Title)
                .HasColumnName("title")
                .HasMaxLength(80)
                .IsRequired();

            entity.Property(a => a.Description)
                .HasColumnName("description")
                .HasMaxLength(500)
                .IsRequired();

            entity.Property(a => a.DepartmentId)
                .HasColumnName("department_id")
                .IsRequired();

            entity.Property(a => a.Status)
                .HasColumnName("status")
                .HasDefaultValue("New")
                .IsRequired();

            entity.Property(a => a.CreatedAt)
                .HasColumnName("created_at")
                .HasDefaultValueSql("now()")
                .IsRequired();

            entity.Property(a => a.CreatedByUserId)
                .HasColumnName("created_by_user_id")
                .IsRequired();

            entity.Property(a => a.AssigneeUserId)
                .HasColumnName("assignee_user_id")
                .IsRequired(false);

            entity.HasIndex(a => a.Number).IsUnique();

            // Обязательные индексы из ЛР9
            entity.HasIndex(a => a.Status)
                .HasDatabaseName("assignments_status_idx");

            entity.HasIndex(a => a.DepartmentId)
                .HasDatabaseName("assignments_department_id_idx");

            entity.HasOne(a => a.Department)
                .WithMany(d => d.Assignments)
                .HasForeignKey(a => a.DepartmentId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(a => a.CreatedByUser)
                .WithMany(u => u.CreatedAssignments)
                .HasForeignKey(a => a.CreatedByUserId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(a => a.AssigneeUser)
                .WithMany(u => u.AssignedAssignments)
                .HasForeignKey(a => a.AssigneeUserId)
                .OnDelete(DeleteBehavior.SetNull);
        });
    }
}
