using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace AdvertAPI.Entities
{
    public partial class AdvertContext : DbContext
    {
        public AdvertContext()
        {
        }

        public AdvertContext(DbContextOptions<AdvertContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Banner> Banner { get; set; }
        public virtual DbSet<Building> Building { get; set; }
        public virtual DbSet<Campaign> Campaign { get; set; }
        public virtual DbSet<Client> Client { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Data Source=db-mssql;Initial Catalog=s18648;Integrated Security=True");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Banner>(entity =>
            {
                entity.HasKey(e => e.IdAdvertisement)
                    .HasName("Banner_pk");

                entity.Property(e => e.Area).HasColumnType("decimal(6, 2)");

                entity.Property(e => e.Price).HasColumnType("decimal(6, 2)");

                entity.HasOne(d => d.IdCampaignNavigation)
                    .WithMany(p => p.Banner)
                    .HasForeignKey(d => d.IdCampaign)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Banner_Campaign");
            });

            modelBuilder.Entity<Building>(entity =>
            {
                entity.HasKey(e => e.IdBuilding)
                    .HasName("Building_pk");

                entity.Property(e => e.City)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.Height).HasColumnType("decimal(6, 2)");

                entity.Property(e => e.Street)
                    .IsRequired()
                    .HasMaxLength(100);
            });

            modelBuilder.Entity<Campaign>(entity =>
            {
                entity.HasKey(e => e.IdCampaign)
                    .HasName("Campaign_pk");

                entity.Property(e => e.EndDate).HasColumnType("date");

                entity.Property(e => e.PricePerSquareMeter).HasColumnType("decimal(6, 2)");

                entity.Property(e => e.StartDate).HasColumnType("date");

                entity.HasOne(d => d.FromIdBuildingNavigation)
                    .WithMany(p => p.CampaignFromIdBuildingNavigation)
                    .HasForeignKey(d => d.FromIdBuilding)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Campaign_Building");

                entity.HasOne(d => d.IdClientNavigation)
                    .WithMany(p => p.Campaign)
                    .HasForeignKey(d => d.IdClient)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Campaign_Client");

                entity.HasOne(d => d.ToldBuildingNavigation)
                    .WithMany(p => p.CampaignToldBuildingNavigation)
                    .HasForeignKey(d => d.ToldBuilding)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("CampaignBuilding");
            });

            modelBuilder.Entity<Client>(entity =>
            {
                entity.HasKey(e => e.IdClient)
                    .HasName("Client_pk");

                entity.HasIndex(e => e.Login)
                    .HasName("UQ__Client__5E55825BF8EE412C")
                    .IsUnique();

                entity.HasIndex(e => e.TokenString)
                    .HasName("UQ__Client__6962D7933E422D5E")
                    .IsUnique();

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.Hash)
                    .IsRequired()
                    .HasMaxLength(150);

                entity.Property(e => e.LastName)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.Login)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.Phone)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.Salt)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.TokenString)
                    .IsRequired()
                    .HasMaxLength(150);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
