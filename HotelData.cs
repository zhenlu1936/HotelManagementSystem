using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

public class Bill
{
    [Key]
    public int bill_id { get; set; }
    public int bill_price { get; set; }
    [Range(1, 10, ErrorMessage = "人数必须在1到10之间")]
    public int bill_people { get; set; }
    public DateTime bill_bookTime { get; set; }
    public DateTime bill_checkInTime { get; set; } = DateTime.Today;
    public DateTime bill_checkOutTime { get; set; } = DateTime.Today.AddDays(1);
    public DateTime? bill_trueCheckInTime { get; set; }
    public DateTime? bill_trueCheckOutTime { get; set; }
    public DateTime? bill_payTime { get; set; }
    public ICollection<Client> clients { get; set; } = new List<Client>();
    public ICollection<Room> rooms { get; set; } = new List<Room>();
}
public class BillSearchInputModel
{
    public int? RoomTrueId { get; set; }
    public DateTime? CheckInTime { get; set; }
    public DateTime? CheckOutTime { get; set; }
    public bool? IfPaid { get; set; }
    public bool? IfOut { get; set; }
    public bool? IfChecked { get; set; }
}
public class Client
{
    [Key]
    public int client_id { get; set; }
    [Required(ErrorMessage = "身份证号不得为空")]
    public string client_trueId { get; set; }
    [Required(ErrorMessage = "姓名不得为空")]
    public string client_name { get; set; }
    public string? client_tel { get; set; }

    [ForeignKey("Bill")]
    public int Billbill_id { get; set; }

}

public class ClientSearchInputModel
{
    public string? ClientName { get; set; }
    public string? ClientTel { get; set; }
    public string? ClientTrueId { get; set; }
}

public class Room
{
    [Key]
    public int room_id { get; set; }
    [Range(1, 10, ErrorMessage = "楼层必须在1到10之间")]
    public int room_floor { get; set; } = 1;
    [Range(1, 10, ErrorMessage = "层内序号必须在1到10之间")]
    public int room_number { get; set; } = 1;
    public int room_trueId => room_floor * 100 + room_number;
    public bool room_ifStayIn { get; set; } = false;

    [ForeignKey("Class")]
    public int Classclass_id { get; set; }
    public Class? roomclass { get; set; }
    public ICollection<Bill>? bills { get; set; } = new List<Bill>();
}
public class Class
{
    [Key]
    public int class_id { get; set; }
    [Required(ErrorMessage = "房间种类名不得为空")]
    public string class_name { get; set; }
    [Range(1, 4, ErrorMessage = "最大人数必须在1到4之间")]
    public int class_capacity { get; set; } = 1;
    [Range(10, 10000, ErrorMessage = "价格必须在10到10000之间")]
    public int class_price { get; set; } = 100;
    public List<Room> rooms { get; set; } = new List<Room>();

}
public class HotelManagementContext : DbContext
{
    public HotelManagementContext(DbContextOptions<HotelManagementContext> options)
        : base(options)
    {
    }
    public DbSet<Bill> Bills { get; set; }
    public DbSet<Client> Clients { get; set; }
    public DbSet<Room> Rooms { get; set; }
    public DbSet<Class> Classes { get; set; }

}