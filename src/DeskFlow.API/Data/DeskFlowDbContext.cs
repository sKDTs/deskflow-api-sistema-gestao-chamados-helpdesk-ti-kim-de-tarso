using DeskFlow.API.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace DeskFlow.API.Data;

public class DeskFlowDbContext : DbContext
{
    public DeskFlowDbContext(DbContextOptions<DeskFlowDbContext> options)
        : base(options)
    {
    }

    public DbSet<Categoria> Categorias => Set<Categoria>();

    public DbSet<Chamado> Chamados => Set<Chamado>();

    public DbSet<Interacao> Interacoes => Set<Interacao>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Categoria>(entity =>
        {
            entity.HasKey(c => c.Id);

            entity.Property(c => c.Nome)
                .IsRequired()
                .HasMaxLength(100);
        });

        modelBuilder.Entity<Chamado>(entity =>
        {
            entity.HasKey(c => c.Id);

            entity.Property(c => c.Titulo)
                .IsRequired()
                .HasMaxLength(200);

            entity.Property(c => c.Descricao)
                .IsRequired();

            entity.Property(c => c.SolicitanteNome)
                .IsRequired()
                .HasMaxLength(150);

            entity.Property(c => c.Solucao)
                .HasMaxLength(2000);

            entity.HasOne(c => c.Categoria)
                .WithMany(c => c.Chamados)
                .HasForeignKey(c => c.CategoriaId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<Interacao>(entity =>
        {
            entity.HasKey(i => i.Id);

            entity.Property(i => i.Autor)
                .IsRequired()
                .HasMaxLength(150);

            entity.Property(i => i.Mensagem)
                .IsRequired()
                .HasMaxLength(2000);

            entity.HasOne(i => i.Chamado)
                .WithMany(c => c.Interacoes)
                .HasForeignKey(i => i.ChamadoId)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }
}
