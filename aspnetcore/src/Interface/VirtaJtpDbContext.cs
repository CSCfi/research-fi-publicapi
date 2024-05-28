using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using CSC.PublicApi.Interface.Models;

namespace CSC.PublicApi.Interface
{
    public partial class VirtaJtpDbContext : DbContext
    {
        public VirtaJtpDbContext()
        {
        }

        public VirtaJtpDbContext(DbContextOptions<VirtaJtpDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Duplikaatit> Duplikaatits { get; set; } = null!;
        public virtual DbSet<TableTest> TableTests { get; set; } = null!;
        public virtual DbSet<Testief> Testiefs { get; set; } = null!;
        public virtual DbSet<Tilanneraportti> Tilanneraporttis { get; set; } = null!;
        public virtual DbSet<Virheraportti> Virheraporttis { get; set; } = null!;
        public virtual DbSet<YhteisjulkaisutRistiriitaiset> YhteisjulkaisutRistiriitaisets { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
          
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Duplikaatit>(entity =>
            {
                entity.HasKey(e => e.DuplikaattiId)
                    .HasName("PK__duplikaa__916491C04443A310");

                entity.ToTable("duplikaatit", "web");

                entity.Property(e => e.DuplikaattiId).HasColumnName("duplikaattiID");

                entity.Property(e => e.Duplikaattijulkaisunnimi)
                    .HasMaxLength(4000)
                    .HasColumnName("duplikaattijulkaisunnimi");

                entity.Property(e => e.Duplikaattijulkaisunorgtunnus)
                    .HasMaxLength(100)
                    .HasColumnName("duplikaattijulkaisunorgtunnus");

                entity.Property(e => e.Ilmoitusvuosi).HasColumnName("ilmoitusvuosi");

                entity.Property(e => e.Julkaisunnimi)
                    .HasMaxLength(4000)
                    .HasColumnName("julkaisunnimi");

                entity.Property(e => e.Julkaisunorgtunnus)
                    .HasMaxLength(100)
                    .HasColumnName("julkaisunorgtunnus");

                entity.Property(e => e.Julkaisuntunnus)
                    .HasMaxLength(50)
                    .HasColumnName("julkaisuntunnus");

                entity.Property(e => e.Julkaisutyyppikoodi)
                    .HasMaxLength(2)
                    .HasColumnName("julkaisutyyppikoodi");

                entity.Property(e => e.Julkaisuvuosi).HasColumnName("julkaisuvuosi");

                entity.Property(e => e.Kuvaus)
                    .HasMaxLength(100)
                    .HasColumnName("kuvaus");

                entity.Property(e => e.Luontipaivamaara)
                    .HasColumnType("date")
                    .HasColumnName("luontipaivamaara");

                entity.Property(e => e.Organisaationimi)
                    .HasMaxLength(100)
                    .HasColumnName("organisaationimi");

                entity.Property(e => e.Organisaatiotunnus)
                    .HasMaxLength(20)
                    .HasColumnName("organisaatiotunnus");

                entity.Property(e => e.TarkistusId).HasColumnName("tarkistusID");

                entity.Property(e => e.Tila).HasColumnName("tila");
            });

            modelBuilder.Entity<TableTest>(entity =>
            {
                entity.ToTable("table_test");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Etunimi)
                    .HasMaxLength(255)
                    .HasColumnName("etunimi");

                entity.Property(e => e.Sukunimi)
                    .HasMaxLength(255)
                    .HasColumnName("sukunimi");
            });

            modelBuilder.Entity<Testief>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("testief", "web");

                entity.Property(e => e.Id)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("ID");

                entity.Property(e => e.Organisaationimi)
                    .HasMaxLength(100)
                    .HasColumnName("organisaationimi");

                entity.Property(e => e.Organisaatiotunnus)
                    .HasMaxLength(20)
                    .HasColumnName("organisaatiotunnus");
            });

            modelBuilder.Entity<Tilanneraportti>(entity =>
            {
                entity.ToTable("Tilanneraportti", "web");

                entity.Property(e => e.TilanneraporttiId).HasColumnName("tilanneraporttiID");

                entity.Property(e => e.ArtikkelityyppiKoodi).HasMaxLength(1);

                entity.Property(e => e.AvoinSaatavuusJulkaisumaksu).HasMaxLength(30);

                entity.Property(e => e.AvoinSaatavuusKytkin).HasMaxLength(1);

                entity.Property(e => e.AvsovellusTyyppiKoodi)
                    .HasMaxLength(1)
                    .HasColumnName("AVSovellusTyyppiKoodi");

                entity.Property(e => e.EmojulkaisuntyyppiKoodi).HasMaxLength(1);

                entity.Property(e => e.JufoLuokkaKoodi).HasMaxLength(1);

                entity.Property(e => e.JufoTunnus).HasMaxLength(10);

                entity.Property(e => e.JulkaisuKanavaOa)
                    .HasMaxLength(1)
                    .HasColumnName("JulkaisuKanavaOA");

                entity.Property(e => e.JulkaisuTyyppi).HasMaxLength(2);

                entity.Property(e => e.JulkaisunNimi).HasMaxLength(4000);

                entity.Property(e => e.JulkaisunTunnus).HasMaxLength(50);

                entity.Property(e => e.LisenssiKoodi).HasMaxLength(1);

                entity.Property(e => e.Luontipaivamaara)
                    .HasMaxLength(12)
                    .IsUnicode(false);

                entity.Property(e => e.MuotoKoodi).HasMaxLength(1);

                entity.Property(e => e.OpinnayteKoodi).HasMaxLength(1);

                entity.Property(e => e.OrganisaatioTunnus).HasMaxLength(100);

                entity.Property(e => e.OrganisaationJulkaisuTunnus).HasMaxLength(100);

                entity.Property(e => e.Organisaationimi).HasMaxLength(100);

                entity.Property(e => e.Paivityspaivamaara)
                    .HasMaxLength(12)
                    .IsUnicode(false)
                    .HasColumnName("paivityspaivamaara");

                entity.Property(e => e.RaporttiKytkin).HasMaxLength(1);

                entity.Property(e => e.RinnakkaistallennettuKytkin).HasMaxLength(1);

                entity.Property(e => e.TaidetyyppiKoodi).HasMaxLength(1);

                entity.Property(e => e.VertaisarvioituKytkin).HasMaxLength(1);

                entity.Property(e => e.YleisoKoodi).HasMaxLength(1);
            });

            modelBuilder.Entity<Virheraportti>(entity =>
            {
                entity.ToTable("Virheraportti", "web");

                entity.Property(e => e.VirheraporttiId).HasColumnName("virheraporttiID");

                entity.Property(e => e.JulkaisunNimi).HasMaxLength(4000);

                entity.Property(e => e.JulkaisunOrgTunnus).HasMaxLength(100);

                entity.Property(e => e.Julkaisutyyppikoodi).HasMaxLength(2);

                entity.Property(e => e.Koodi).HasMaxLength(50);

                entity.Property(e => e.Kuvaus).HasMaxLength(255);

                entity.Property(e => e.Luontipaivamaara).HasColumnType("date");

                entity.Property(e => e.Nimi)
                    .HasMaxLength(100)
                    .HasColumnName("nimi");

                entity.Property(e => e.OrganisaatioTunnus).HasMaxLength(20);

                entity.Property(e => e.TarkistusId).HasColumnName("TarkistusID");

                entity.Property(e => e.Virheviesti)
                    .HasMaxLength(200)
                    .HasColumnName("virheviesti");
            });

            modelBuilder.Entity<YhteisjulkaisutRistiriitaiset>(entity =>
            {
                entity.HasKey(e => e.RrId)
                    .HasName("PK__Yhteisju__86EFBD733567A197");

                entity.ToTable("YhteisjulkaisutRistiriitaiset", "web");

                entity.Property(e => e.RrId).HasColumnName("rrID");

                entity.Property(e => e.JulkaisunNimi).HasMaxLength(4000);

                entity.Property(e => e.JulkaisunOrgTunnus).HasMaxLength(100);

                entity.Property(e => e.JulkaisunTunnus).HasMaxLength(50);

                entity.Property(e => e.Julkaisutyyppi).HasMaxLength(4000);

                entity.Property(e => e.Koodi).HasMaxLength(50);

                entity.Property(e => e.Kuvaus).HasMaxLength(255);

                entity.Property(e => e.LiittyvaJulkaisunOrgTunnus).HasMaxLength(100);

                entity.Property(e => e.Luontipaivamaara).HasMaxLength(50);

                entity.Property(e => e.OrganisaatioTunnus).HasMaxLength(100);

                entity.Property(e => e.Organisaationimi).HasMaxLength(255);

                entity.Property(e => e.Tila).HasColumnName("tila");

                entity.Property(e => e.YhteisjulkaisunTunnus).HasMaxLength(500);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
