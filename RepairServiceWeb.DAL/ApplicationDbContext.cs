using Microsoft.EntityFrameworkCore;
using RepairServiceWeb.Domain.Entity;

namespace RepairServiceWeb.DAL
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Accessory> Accessories { get; set; }

        public DbSet<Client> Clients { get; set; }

        public DbSet<Device> Devices { get; set; }

        public DbSet<OrderAccessory> OrderAccessories { get; set; }

        public DbSet<Repair> Repairs { get; set; }

        public DbSet<Role> Roles { get; set; }

        public DbSet<Staff> Staff { get; set; }

        public DbSet<Supplier> Suppliers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Accessory>(entity =>
            {
                entity.Property(e => e.Cost).HasColumnType("money");
                entity.Property(e => e.Description).HasMaxLength(250);
                entity.Property(e => e.Manufacturer).HasMaxLength(50);
                entity.Property(e => e.Name).HasMaxLength(50);

                entity.HasOne(d => d.Supplier).WithMany(p => p.Accessories)
                    .HasForeignKey(d => d.SupplierId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Accessories_Suppliers");
            });

            modelBuilder.Entity<Client>(entity =>
            {
                entity.Property(e => e.Address).HasMaxLength(50);
                entity.Property(e => e.Email).HasMaxLength(50);
                entity.Property(e => e.Login).HasMaxLength(50);
                entity.Property(e => e.Name).HasMaxLength(50);
                entity.Property(e => e.Password).HasMaxLength(50);
                entity.Property(e => e.Patronymic).HasMaxLength(50);
                entity.Property(e => e.PhoneNumber)
                    .HasMaxLength(50)
                    .HasColumnName("Phone_number");
                entity.Property(e => e.Surname).HasMaxLength(50);

                entity.HasOne(d => d.Role).WithMany(p => p.Clients)
                    .HasForeignKey(d => d.RoleId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Clients_Roles");
            });

            modelBuilder.Entity<Device>(entity =>
            {
                entity.Property(e => e.Manufacturer).HasMaxLength(50);
                entity.Property(e => e.Model).HasMaxLength(50);
                entity.Property(e => e.Photo).HasMaxLength(50);
                entity.Property(e => e.SerialNumber).HasColumnName("Serial_number");
                entity.Property(e => e.Type).HasMaxLength(50);
                entity.Property(e => e.YearOfRelease).HasColumnName("Year_of_release");

                entity.HasOne(d => d.Client).WithMany(p => p.Devices)
                    .HasForeignKey(d => d.ClientId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Devices_Clients");
            });

            modelBuilder.Entity<OrderAccessory>(entity =>
            {
                entity.Property(e => e.Cost).HasColumnType("money");
                entity.Property(e => e.Count).HasMaxLength(50);
                entity.Property(e => e.DateOrder).HasColumnName("Date_order");
                entity.Property(e => e.StatusOrder)
                    .HasMaxLength(50)
                    .HasColumnName("Status_order");

                entity.HasOne(d => d.Accessories).WithMany(p => p.OrderAccessories)
                    .HasForeignKey(d => d.AccessoriesId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_OrderAccessories_Accessories");

                entity.HasOne(d => d.Client).WithMany(p => p.OrderAccessories)
                    .HasForeignKey(d => d.ClientId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_OrderAccessories_Clients");
            });

            modelBuilder.Entity<Repair>(entity =>
            {
                entity.Property(e => e.Cost).HasColumnType("money");
                entity.Property(e => e.DateOfAdmission).HasColumnName("Date_of_admission");
                entity.Property(e => e.DescriprionOfWorkDone)
                    .HasMaxLength(500)
                    .HasColumnName("Descriprion_of_work_done");
                entity.Property(e => e.DescriptionOfProblem)
                    .HasMaxLength(500)
                    .HasColumnName("Description_of_problem");
                entity.Property(e => e.EndDate).HasColumnName("End_date");

                entity.HasOne(d => d.Device).WithMany(p => p.Repairs)
                    .HasForeignKey(d => d.DeviceId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Repairs_Devices");

                entity.HasOne(d => d.Staff).WithMany(p => p.Repairs)
                    .HasForeignKey(d => d.StaffId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Repairs_Staff");
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.Property(e => e.Role1)
                    .HasMaxLength(50)
                    .HasColumnName("Role");
            });

            modelBuilder.Entity<Staff>(entity =>
            {
                entity.Property(e => e.DateOfEmployment).HasColumnName("Date_of_employment");
                entity.Property(e => e.Login).HasMaxLength(50);
                entity.Property(e => e.Name).HasMaxLength(50);
                entity.Property(e => e.Password).HasMaxLength(50);
                entity.Property(e => e.Patronymic).HasMaxLength(50);
                entity.Property(e => e.Photo).HasMaxLength(50);
                entity.Property(e => e.Post).HasMaxLength(50);
                entity.Property(e => e.Salary).HasColumnType("money");
                entity.Property(e => e.Surname).HasMaxLength(50);

                entity.HasOne(d => d.Role).WithMany(p => p.Staff)
                    .HasForeignKey(d => d.RoleId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Staff_Roles");
            });

            modelBuilder.Entity<Supplier>(entity =>
            {
                entity.Property(e => e.Address).HasMaxLength(50);
                entity.Property(e => e.CompanyName)
                    .HasMaxLength(50)
                    .HasColumnName("Company_name");
                entity.Property(e => e.ContactPerson)
                    .HasMaxLength(50)
                    .HasColumnName("Contact_person");
                entity.Property(e => e.Email).HasMaxLength(50);
                entity.Property(e => e.PhoneNumber)
                    .HasMaxLength(50)
                    .HasColumnName("Phone_number");
            });
        }
    }
}
