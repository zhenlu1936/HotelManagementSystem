using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class HotelStuff : IdentityUser
{
    // 添加自定义属性
    public string? stuff_name { get; set; }
    public string? stuff_number { get; set; }

    [ForeignKey("Role")]
    public int Rolerole_id { get; set; }

}
public class Labor
{
    public string StuffId { get; set; }
    public string StuffNumber { get; set; }
    public string StuffName { get; set; }
    public string StuffRole { get; set; }
}

public class Role
{
    [Key]
    public int role_id { get; set; }
    public string role_name { get; set; }
}
public class UserContext : IdentityDbContext<HotelStuff>
{
    public UserContext(DbContextOptions<UserContext> options)
        : base(options)
    {
    }
    public DbSet<Role> StuffRoles { get; set; }
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<Role>().HasData(
            new Role { role_id = 1, role_name = "经理" },
            new Role { role_id = 2, role_name = "管理员" },
            new Role { role_id = 3, role_name = "后端" },
            new Role { role_id = 4, role_name = "前端" }
        );

        builder.Entity<HotelStuff>()
            .HasIndex(p => p.stuff_number)
            .IsUnique();
    }
}

