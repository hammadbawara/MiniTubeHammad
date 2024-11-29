using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using MiniTube.ModelsEAD;

namespace MiniTube.Context;

public partial class MiniTubeContext : DbContext
{
    public MiniTubeContext()
    {
    }

    public MiniTubeContext(DbContextOptions<MiniTubeContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Video> Videos { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=(localdb)\\MSSQLLocalDB;Initial Catalog=MiniTube;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Video>(entity =>
        {
            entity.HasKey(e => e.VideoId).HasName("PK__Videos__BAE5124A1C50788C");

            entity.Property(e => e.VideoId).HasColumnName("VideoID");
            entity.Property(e => e.Description).HasColumnType("text");
            entity.Property(e => e.Keyword1)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Keyword2)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Keyword3)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.LikesCount).HasDefaultValue(0);
            entity.Property(e => e.Title)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.UploadDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.UserId).HasColumnName("UserID");
            entity.Property(e => e.ViewsCount).HasDefaultValue(0);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
