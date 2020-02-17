using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Configuration;

namespace KaniTietoKantaAPI.Models
{
    public partial class KanitContext : DbContext
    {
        public static string APIKey;
        private readonly IConfiguration _configuration;

        public KanitContext(IConfiguration configuration)
        {
            _configuration = configuration;
            var conn = _configuration["ConnectionStrings:KaniDBContext"];

            //This is my solution in getting the APIKey, since I did not know other ways to do it. It works, tho.
            APIKey = _configuration["ConnectionStrings:KaniAPIKey"];
        }

        //This is my solution in getting the APIKey, since I did not know other ways to do it. It works, tho.
        public string GetAPIKey()
        {
            return APIKey;
        }

        //public KanitContext(DbContextOptions<KanitContext> options) : base(options)
        //{
        //}

        public virtual DbSet<Astutukset> Astutukset { get; set; }
        public virtual DbSet<Kani> Kani { get; set; }
        public virtual DbSet<Ostaja> Ostaja { get; set; }
        public virtual DbSet<Poikastiedot> Poikastiedot { get; set; }
        public virtual DbSet<Poikueet> Poikueet { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(_configuration["ConnectionStrings:KaniDBContext"]);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Astutukset>(entity =>
            {
                entity.HasKey(e => e.AstutusId)
                    .HasName("PK__astutuks__6F90B2A87708E2D5");

                entity.ToTable("astutukset");

                entity.Property(e => e.AstutusId).HasColumnName("astutus_id");

                entity.Property(e => e.Astutuspäivä)
                    .HasColumnName("astutuspäivä")
                    .HasColumnType("datetime");

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasColumnName("email")
                    .HasMaxLength(320);

                entity.Property(e => e.EmäId).HasColumnName("emä_id");

                entity.Property(e => e.EmäKnimi)
                    .IsRequired()
                    .HasColumnName("emä_knimi")
                    .HasMaxLength(100);

                entity.Property(e => e.EmäRotu)
                    .IsRequired()
                    .HasColumnName("emä_rotu")
                    .HasMaxLength(100);

                entity.Property(e => e.EmäSyntymäpv)
                    .HasColumnName("emä_syntymäpv")
                    .HasColumnType("date");

                entity.Property(e => e.EmäVnimi)
                    .HasColumnName("emä_vnimi")
                    .HasMaxLength(100);

                entity.Property(e => e.EmäVäri)
                    .HasColumnName("emä_väri")
                    .HasMaxLength(150);

                entity.Property(e => e.IsäId).HasColumnName("isä_id");

                entity.Property(e => e.IsäKnimi)
                    .IsRequired()
                    .HasColumnName("isä_knimi")
                    .HasMaxLength(100);

                entity.Property(e => e.IsäRotu)
                    .IsRequired()
                    .HasColumnName("isä_rotu")
                    .HasMaxLength(100);

                entity.Property(e => e.IsäSyntymäpv)
                    .HasColumnName("isä_syntymäpv")
                    .HasColumnType("date");

                entity.Property(e => e.IsäVnimi)
                    .HasColumnName("isä_vnimi")
                    .HasMaxLength(100);

                entity.Property(e => e.IsäVäri)
                    .HasColumnName("isä_väri")
                    .HasMaxLength(150);

                entity.Property(e => e.Lisätietoja)
                    .HasColumnName("lisätietoja")
                    .HasMaxLength(500);

                entity.HasOne(d => d.Emä)
                    .WithMany(p => p.AstutuksetEmä)
                    .HasForeignKey(d => d.EmäId)
                    .HasConstraintName("FK_emä_id");

                entity.HasOne(d => d.Isä)
                    .WithMany(p => p.AstutuksetIsä)
                    .HasForeignKey(d => d.IsäId)
                    .HasConstraintName("FK_isä_id");
            });

            modelBuilder.Entity<Kani>(entity =>
            {
                entity.ToTable("kani");

                entity.Property(e => e.KaniId).HasColumnName("kani_id");

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasColumnName("email")
                    .HasMaxLength(320);

                entity.Property(e => e.EmänNimi)
                   .HasColumnName("emän_nimi")
                   .HasMaxLength(150);

                entity.Property(e => e.IsänNimi)
                    .HasColumnName("isän_nimi")
                    .HasMaxLength(150);

                entity.Property(e => e.KasvattajanTiedot)
                    .HasColumnName("kasvattajan_tiedot")
                    .HasMaxLength(300);

                entity.Property(e => e.Kuolinpäivä)
                    .HasColumnName("kuolinpäivä")
                    .HasColumnType("datetime");

                entity.Property(e => e.Kuollut).HasColumnName("kuollut");

                entity.Property(e => e.Kutsumanimi)
                    .IsRequired()
                    .HasColumnName("kutsumanimi")
                    .HasMaxLength(50);

                entity.Property(e => e.Myyty).HasColumnName("myyty");

                entity.Property(e => e.Nimi)
                    .HasColumnName("nimi")
                    .HasMaxLength(100);

                entity.Property(e => e.Ok)
                    .HasColumnName("ok")
                    .HasMaxLength(10);

                entity.Property(e => e.OstajanTiedot)
               .HasColumnName("ostajan_tiedot")
               .HasMaxLength(300);

                entity.Property(e => e.PoikueenId).HasColumnName("poikueen_id");

                entity.Property(e => e.Rokotettu).HasColumnName("rokotettu");

                entity.Property(e => e.Rokotuspv)
                    .HasColumnName("rokotuspv")
                    .HasColumnType("datetime");

                entity.Property(e => e.Rotu)
                    .HasColumnName("rotu")
                    .HasMaxLength(100);

                entity.Property(e => e.Sukupuoli)
                    .HasColumnName("sukupuoli")
                    .HasMaxLength(10);

                entity.Property(e => e.Syntymäpäivä)
                    .HasColumnName("syntymäpäivä")
                    .HasColumnType("datetime");

                entity.Property(e => e.Tietoja)
                    .HasColumnName("tietoja")
                    .HasMaxLength(300);

                entity.Property(e => e.Vk)
                    .HasColumnName("vk")
                    .HasMaxLength(10);

                entity.Property(e => e.Väri)
                    .HasColumnName("väri")
                    .HasMaxLength(250);

                entity.HasOne(d => d.Poikueen)
                    .WithMany(p => p.Kani)
                    .HasForeignKey(d => d.PoikueenId)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("FK_poikueet_poikue_id");
            });

            modelBuilder.Entity<Ostaja>(entity =>
            {
                entity.ToTable("ostaja");

                entity.Property(e => e.OstajaId).HasColumnName("ostaja_id");

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasColumnName("email")
                    .HasMaxLength(320);

                entity.Property(e => e.Hinta).HasColumnName("hinta");

                entity.Property(e => e.Maksettu).HasColumnName("maksettu");

                entity.Property(e => e.Nimi)
                    .IsRequired()
                    .HasColumnName("nimi")
                    .HasMaxLength(300);

                entity.Property(e => e.OEmail)
                    .HasColumnName("o_email")
                    .HasMaxLength(320);

                entity.Property(e => e.Ostopäivä)
                    .HasColumnName("ostopäivä")
                    .HasColumnType("datetime");

                entity.Property(e => e.PoikanenId).HasColumnName("poikanen_id");

                entity.Property(e => e.Puhnro)
                    .HasColumnName("puhnro")
                    .HasMaxLength(20);

                entity.Property(e => e.Tietoja)
                    .HasColumnName("tietoja")
                    .HasMaxLength(300);
            });

            modelBuilder.Entity<Poikastiedot>(entity =>
            {
                entity.HasKey(e => e.PoikanenId)
                    .HasName("PK__poikasti__058D0CF1DBF31B32");

                entity.ToTable("poikastiedot");

                entity.Property(e => e.PoikanenId).HasColumnName("poikanen_id");

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasColumnName("email")
                    .HasMaxLength(320);

                entity.Property(e => e.KaniId).HasColumnName("kani_id");

                entity.Property(e => e.Nimi)
                    .HasColumnName("nimi")
                    .HasMaxLength(100);

                entity.Property(e => e.OstajaId).HasColumnName("ostaja_id");

                entity.Property(e => e.PoikueId).HasColumnName("poikue_id");

                entity.Property(e => e.Sukupuoli)
                    .HasColumnName("sukupuoli")
                    .HasMaxLength(10);

                entity.Property(e => e.Syntymäpv)
                    .HasColumnName("syntymäpv")
                    .HasColumnType("datetime");

                entity.Property(e => e.Tietoja)
                    .HasColumnName("tietoja")
                    .HasMaxLength(300);

                entity.Property(e => e.Väri)
                    .HasColumnName("väri")
                    .HasMaxLength(250);

                entity.HasOne(d => d.Kani)
                    .WithMany(p => p.Poikastiedot)
                    .HasForeignKey(d => d.KaniId)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("FK_kani_id");

                entity.HasOne(d => d.Poikue)
                    .WithMany(p => p.Poikastiedot)
                    .HasForeignKey(d => d.PoikueId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_poikue_id");
            });

            modelBuilder.Entity<Poikueet>(entity =>
            {
                entity.HasKey(e => e.PoikueId)
                    .HasName("PK__poikueet__3258FE655C0942DA");

                entity.ToTable("poikueet");

                entity.Property(e => e.PoikueId).HasColumnName("poikue_id");

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasColumnName("email")
                    .HasMaxLength(320);

                entity.Property(e => e.EmäId).HasColumnName("emä_id");

                entity.Property(e => e.EmäKnimi)
                    .IsRequired()
                    .HasColumnName("emä_knimi")
                    .HasMaxLength(50);

                entity.Property(e => e.EmäVk)
                    .HasColumnName("emä_vk")
                    .HasMaxLength(10);

                entity.Property(e => e.EmäVnimi)
                    .HasColumnName("emä_vnimi")
                    .HasMaxLength(100);

                entity.Property(e => e.IsäId).HasColumnName("isä_id");

                entity.Property(e => e.IsäKnimi)
                    .IsRequired()
                    .HasColumnName("isä_knimi")
                    .HasMaxLength(50);

                entity.Property(e => e.IsäVk)
                    .HasColumnName("isä_vk")
                    .HasMaxLength(10);

                entity.Property(e => e.IsäVnimi)
                    .HasColumnName("isä_vnimi")
                    .HasMaxLength(100);

                entity.Property(e => e.Poikaslkm).HasColumnName("poikaslkm");

                entity.Property(e => e.Rotu)
                    .HasColumnName("rotu")
                    .HasMaxLength(100);

                entity.Property(e => e.Syntymäpv)
                    .HasColumnName("syntymäpv")
                    .HasColumnType("datetime");

                entity.Property(e => e.Tietoja)
                   .HasColumnName("tietoja")
                   .HasMaxLength(300);

                entity.HasOne(d => d.Emä)
                    .WithMany(p => p.PoikueetEmä)
                    .HasForeignKey(d => d.EmäId)
                    .HasConstraintName("FK_kanit_emä_id");

                entity.HasOne(d => d.Isä)
                    .WithMany(p => p.PoikueetIsä)
                    .HasForeignKey(d => d.IsäId)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("FK_kanit_isä_id");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}