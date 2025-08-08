using System;
using System.Collections.Generic;
using CSC.PublicApi.Interface.Models.ImportDb.Entities;
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

    public virtual DbSet<ImpLinkGrantedFundingToPublication> ImpLinkGrantedFundingToPublications { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ImpLinkGrantedFundingToPublication>(entity =>
        {
            entity.ToTable("imp_link_granted_funding_to_publication");

            entity.HasIndex(e => new { e.ClientId, e.FunderProjectNumber, e.OrganizationId }, "Index_imp_link_granted_funding_to_publication_1");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.ClientId)
                .HasMaxLength(255)
                .HasColumnName("client_id");
            entity.Property(e => e.Created)
                .HasPrecision(4)
                .HasColumnName("created");
            entity.Property(e => e.Doi)
                .HasMaxLength(255)
                .HasColumnName("doi");
            entity.Property(e => e.FunderProjectNumber)
                .HasMaxLength(255)
                .HasColumnName("funder_project_number");
            entity.Property(e => e.Modified)
                .HasPrecision(4)
                .HasColumnName("modified");
            entity.Property(e => e.OrganizationId)
                .HasMaxLength(255)
                .HasColumnName("organization_id");
            entity.Property(e => e.PublicationId)
                .HasMaxLength(255)
                .HasColumnName("publication_id");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
