using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace MiniTube.ModelsEAD;

public partial class DbMiniTubeContext : DbContext
{
    public DbMiniTubeContext()
    {
    }

    public DbMiniTubeContext(DbContextOptions<DbMiniTubeContext> options)
        : base(options)
    {
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Server=DESKTOP-I6UBJ5U\\SQLEXPRESS;Database=MiniTube;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
