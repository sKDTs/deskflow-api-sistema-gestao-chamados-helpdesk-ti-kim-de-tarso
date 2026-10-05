using DeskFlow.API.Data;
using DeskFlow.API.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace DeskFlow.API.Repositories;

public class ChamadoRepository
{
    private readonly DeskFlowDbContext _context;

    public ChamadoRepository(DeskFlowDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(Chamado chamado)
    {
        await _context.Chamados.AddAsync(chamado);
    }

    public async Task<List<Chamado>> GetAllAsync(
        StatusChamado? status,
        Prioridade? prioridade,
        int? categoriaId)
    {
        var query = _context.Chamados
            .AsNoTracking()
            .Include(c => c.Categoria)
            .AsQueryable();

        if (status.HasValue)
        {
            query = query.Where(c => c.Status == status.Value);
        }

        if (prioridade.HasValue)
        {
            query = query.Where(c => c.Prioridade == prioridade.Value);
        }

        if (categoriaId.HasValue)
        {
            query = query.Where(c => c.CategoriaId == categoriaId.Value);
        }

        return await query
            .OrderByDescending(c => c.DataAbertura)
            .ToListAsync();
    }

    public async Task<Chamado?> GetByIdAsync(int id)
    {
        return await _context.Chamados
            .AsNoTracking()
            .Include(c => c.Categoria)
            .Include(c => c.Interacoes)
            .FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task<Chamado?> GetByIdForUpdateAsync(int id)
    {
        return await _context.Chamados
            .FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task<bool> CategoriaExistsAsync(int categoriaId)
    {
        return await _context.Categorias
            .AnyAsync(c => c.Id == categoriaId);
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}
