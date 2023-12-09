using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SCP.Entities;

namespace SCP.Context;

public partial class MyDbContext : DbContext
{
    public MyDbContext()
    {
    }

    public MyDbContext(DbContextOptions<MyDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<article> articles { get; set; }

    public virtual DbSet<personnel> personnel { get; set; }

    public virtual DbSet<user> users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseNpgsql("Host=localhost;Database=scpFoundation;Username=postgres;Password=postgres");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<article>(entity =>
        {
            entity.HasKey(e => e.articleID).HasName("article_pkey");

            entity.ToTable("article");

            entity.Property(e => e.articleID).ValueGeneratedNever();
        });

        modelBuilder.Entity<personnel>(entity =>
        {
            entity.HasKey(e => e.personnelID).HasName("personnel_pkey");

            entity.Property(e => e.personnelID).ValueGeneratedNever();
        });

        modelBuilder.Entity<user>(entity =>
        {
            entity.HasKey(e => e.userID).HasName("user_pkey");

            entity.ToTable("user");

            entity.Property(e => e.userID).ValueGeneratedNever();
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
