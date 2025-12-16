using HiloGame.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace HiloGame.Infrastructure.Persistence;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    public DbSet<GameState> Games => Set<GameState>();


    // TODO: Add mapping configurations if needed
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<GameState>(entity =>
        {
            entity.HasKey(e => e.GameId);
            entity.OwnsOne(e => e.Range, range =>
            {
                range.Property(r => r.MinValue).HasColumnName("MinRange");
                range.Property(r => r.MaxValue).HasColumnName("MaxRange");
            });
        });

        base.OnModelCreating(modelBuilder);
    }
}