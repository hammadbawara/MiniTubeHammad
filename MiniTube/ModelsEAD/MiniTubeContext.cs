using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace MiniTube.ModelsEAD;

public partial class MiniTubeContext : DbContext
{
    public MiniTubeContext()
    {
    }

    public MiniTubeContext(DbContextOptions<MiniTubeContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Comment> Comments { get; set; }

    public virtual DbSet<Like> Likes { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<Video> Videos { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=ANDDEV\\SQLEXPRESS;Database=MiniTube;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Comment>(entity =>
        {
            entity.HasKey(e => e.CommentId).HasName("PK__Comments__C3B4DFAA4164D369");

            entity.ToTable(tb => tb.HasTrigger("trg_UpdateCommentsCount"));

            entity.Property(e => e.CommentId).HasColumnName("CommentID");
            entity.Property(e => e.CommentDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.CommentText).HasColumnType("text");
            entity.Property(e => e.UserId).HasColumnName("User ID");
            entity.Property(e => e.VideoId).HasColumnName("VideoID");

            entity.HasOne(d => d.User).WithMany(p => p.Comments)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Comments__User I__2D27B809");

            entity.HasOne(d => d.Video).WithMany(p => p.Comments)
                .HasForeignKey(d => d.VideoId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Comments__VideoI__2E1BDC42");
        });

        modelBuilder.Entity<Like>(entity =>
        {
            entity.HasKey(e => e.LikeId).HasName("PK__Likes__A2922CF4CBDD8295");

            entity.Property(e => e.LikeId).HasColumnName("LikeID");
            entity.Property(e => e.LikedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.UserId).HasColumnName("User ID");
            entity.Property(e => e.VideoId).HasColumnName("VideoID");

            entity.HasOne(d => d.User).WithMany(p => p.Likes)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Likes__User ID__31EC6D26");

            entity.HasOne(d => d.Video).WithMany(p => p.Likes)
                .HasForeignKey(d => d.VideoId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Likes__VideoID__32E0915F");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__Users__8D49EF0C789ED1CC");

            entity.Property(e => e.UserId).HasColumnName("User ID");
            entity.Property(e => e.Email)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Password)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Role)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Username)
                .HasMaxLength(255)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Video>(entity =>
        {
            entity.HasKey(e => e.VideoId).HasName("PK__Videos__BAE5124ADC6F1BAC");

            entity.Property(e => e.VideoId).HasColumnName("VideoID");
            entity.Property(e => e.CommentsCount).HasDefaultValue(0);
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
            entity.Property(e => e.UserId).HasColumnName("User ID");

            entity.HasOne(d => d.User).WithMany(p => p.Videos)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Videos__User ID__29572725");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
