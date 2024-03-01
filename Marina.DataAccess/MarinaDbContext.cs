using Marina.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;

namespace Marina.DataAccess;

public partial class MarinaDbContext : DbContext
{
    public MarinaDbContext()
    {

    }
    public MarinaDbContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<User> Users { get; set; }
    public DbSet<Region> Regions { get; set; }
    public DbSet<RSM> RSMs { get; set; }
    public DbSet<Distributor> Distributors { get; set; }
    public DbSet<Line> Lines { get; set; }
    public DbSet<Province> Provinces { get; set; }
    public DbSet<Supervisor> Supervisors { get; set; }
    public DbSet<NotImportedData> NotImportedDatas { get; set; }
    public DbSet<NSM> NSMs { get; set; }



    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) { }
    //=> optionsBuilder.UseSqlServer("Data Source=.;Initial Catalog=MarinaDb;integrated security=true;TrustServerCertificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<NSM>(entity =>
        {
            entity.ToTable("NSM");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).HasMaxLength(100);

            entity.HasData(new NSM { Id = 1, Name = "Farshid" }, new NSM { Id = 2, Name = "Ahmad" });
        });

        modelBuilder.Entity<NotImportedData>(entity =>
        {
            entity.ToTable("NotImportedData");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.DateTime).HasColumnType("datetime2").HasDefaultValue(DateTime.Now);
        });

        modelBuilder.Entity<Supervisor>(entity =>
        {
            entity.ToTable("Supervisor");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).HasMaxLength(50);

            entity.HasData(new Supervisor { Id = 1, Name = "Mohammadian", Email = "beni97d@gmail.com" }, new Supervisor { Id = 2, Name = "Mousavi", Email = "beni97d@gmail.com" });
        });

        modelBuilder.Entity<Province>(entity =>
        {
            entity.ToTable("Province");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).HasMaxLength(50);

            entity.HasData(new Province { Id = 1, Name = "Teshran" }, new Province { Id = 2, Name = "Shiraz" });
        });

        modelBuilder.Entity<Line>(entity =>
        {
            entity.ToTable("Line");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).HasMaxLength(50);

            entity.HasData(new Line { Id = 1, Name = "SunStar" }, new Line { Id = 2, Name = "Sunnyness" });
        });

        modelBuilder.Entity<Distributor>(entity =>
        {
            entity.ToTable("Distributor");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Code).HasMaxLength(50);

            entity.HasData(new Distributor { Id = 1, Code = "D1" }, new Distributor { Id = 2, Code = "D2" }, new Distributor { Id = 3, Code = "D3" });
        });

        modelBuilder.Entity<RSM>(entity =>
        {
            entity.ToTable("RSM");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).HasMaxLength(50);

            entity.HasData(new RSM { Id = 1, Name = "RSM1" }, new RSM { Id = 2, Name = "RSM2" }, new RSM { Id = 3, Name = "RSM3" });
        });

        modelBuilder.Entity<Region>(entity =>
        {
            entity.ToTable("Region");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).HasMaxLength(50);

            entity.HasData(new Region { Id = 1, Name = "R1" }, new Region { Id = 2, Name = "R2" }, new Region { Id = 3, Name = "R3" });
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable("User");
            entity.HasKey(e => e.Id);

            entity.Property(e => e.DName).HasMaxLength(50);
            entity.Property(e => e.UserName).HasMaxLength(50).IsUnicode();
            entity.Property(e => e.PhoneNumber).HasMaxLength(11).IsUnicode();

            entity.Property(e => e.IsDeleted).HasDefaultValue(false);
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.CreateDate).HasColumnType("datetime2").HasDefaultValue(DateTime.Now);
            entity.Property(e => e.UpdaterUserId).IsRequired(false);
            entity.Property(e => e.UpdateTime).HasColumnType("datetime2").IsRequired(false);

            entity.HasData(new User
            {
                Id = 1,
                DName = "admin",
                UserName = "admin",
                RegionId = 1,
                RSMId = 1,
                DistributorId = 1,
                LineId = 1,
                ProvinceId = 1,
                SupervisorId = 1,
                PhoneNumber = "0901",
                PasswordHash = "Gco+uHGl5M4H2AXm7UqdfBz/VrFZrLUQiXy9tU9f9d8=",
                Salt = "wJ8Ddinmsj1ZLo5+J9N0FvchZRgOeGlRLDKIIZu3KAs=",
                IsDeleted = false,
                IsActive = true,
                CreateDate = DateTime.Now,
                NsmId = 1,
            });

        });
        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);

}