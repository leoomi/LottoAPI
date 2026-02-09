namespace LottoAPI.Infrastructure;

using LottoAPI.Domain.Entities;
using Microsoft.EntityFrameworkCore;

public class LottoDbContext : DbContext
{
    public DbSet<Lotto> Lottos { get; set; }
    public DbSet<Result> Results { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Lotto>().HasKey(l => l.Id);

        modelBuilder.Entity<Result>().HasKey(r => new { r.Id, r.LottoId });

        modelBuilder
            .Entity<Result>()
            .HasOne(r => r.Lotto)
            .WithMany()
            .HasForeignKey(r => r.LottoId)
            .IsRequired();

        modelBuilder
            .Entity<Lotto>()
            .HasData(
                new Lotto { Id = "megasena", Name = "Mega Sena" },
                new Lotto { Id = "lotofacil", Name = "Loto Fácil" },
                new Lotto { Id = "quina", Name = "Quina" }
            );
    }

    public LottoDbContext(DbContextOptions<LottoDbContext> options)
        : base(options) { }
}
