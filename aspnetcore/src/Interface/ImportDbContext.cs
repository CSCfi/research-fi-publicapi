using System;
using System.Collections.Generic;
using CSC.PublicApi.Interface.Models.FundersAPI.ImportDb.Entities;
using Microsoft.EntityFrameworkCore;

namespace CSC.PublicApi.Interface;

public partial class ImportDbContext : DbContext
{
    public ImportDbContext()
    {
    }

    public ImportDbContext(DbContextOptions<ImportDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<ImpPublicApiFundersGrantedFunding> ImpPublicApiFundersGrantedFundings { get; set; }

    public virtual DbSet<ImpPublicApiFundersGrantedFundingPublication> ImpPublicApiFundersGrantedFundingPublications { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ImpPublicApiFundersGrantedFunding>(entity =>
        {
            entity.ToTable("imp_public_api_funders_granted_funding");

            entity.HasIndex(e => e.ClientId, "Index_imp_public_api_funders_granted_funding_1");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.ClientId)
                .HasMaxLength(255)
                .HasColumnName("client_id");
            entity.Property(e => e.Created)
                .HasColumnType("datetime")
                .HasColumnName("created");
            entity.Property(e => e.FunderProjectNumber)
                .HasMaxLength(255)
                .HasColumnName("funder_project_number");
            entity.Property(e => e.Modified)
                .HasColumnType("datetime")
                .HasColumnName("modified");
            entity.Property(e => e.OrganizationId)
                .HasMaxLength(255)
                .HasColumnName("organization_id");
        });

        modelBuilder.Entity<ImpPublicApiFundersGrantedFundingPublication>(entity =>
        {
            entity.ToTable("imp_public_api_funders_granted_funding_publication");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.ClientId)
                .HasMaxLength(255)
                .HasColumnName("client_id");
            entity.Property(e => e.Created)
                .HasColumnType("datetime")
                .HasColumnName("created");
            entity.Property(e => e.Doi)
                .HasMaxLength(255)
                .HasColumnName("doi");
            entity.Property(e => e.FkGrantedFunding).HasColumnName("fk_granted_funding");
            entity.Property(e => e.Modified)
                .HasColumnType("datetime")
                .HasColumnName("modified");
            entity.Property(e => e.PublicationId)
                .HasMaxLength(255)
                .HasColumnName("publication_id");

            entity.HasOne(d => d.FkGrantedFundingNavigation).WithMany(p => p.ImpPublicApiFundersGrantedFundingPublications)
                .HasForeignKey(d => d.FkGrantedFunding)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_imp_public_api_funders_granted_funding_publication_imp_public_api_funders_granted_funding");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
