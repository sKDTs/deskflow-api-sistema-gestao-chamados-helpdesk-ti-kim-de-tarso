using DeskFlow.API.Data;
using DeskFlow.API.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace DeskFlow.API.Repositories;

public class CategoriaRepository
{
    private readonly DeskFlowDbContext _context;

    public CategoriaRepository(DeskFlowDbContext context)
    {
        _context = context;
    }

    public async Task<List<Categoria>> GetAllAsync()
    {
        return await _context.Categorias
            .AsNoTracking()
            .OrderBy(c => c.Nome)
            .ToListAsync();
    }

    public async Task<Categoria?> GetByIdAsync(int id)
    {
        return await _context.Categorias
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task<Categoria?> GetByIdWithChamadosAsync(int id)
    {
        return await _context.Categorias
            .Include(c => c.Chamados)
            .FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task AddAsync(Categoria categoria)
    {
        await _context.Categorias.AddAsync(categoria);
    }

    public void Update(Categoria categoria)
    {
        _context.Categorias.Update(categoria);
    }

    public void Delete(Categoria categoria)
    {
        _context.Categorias.Remove(categoria);
    }

    public async Task<bool> ExistsAsync(int id)
    {
        return await _context.Categorias
            .AnyAsync(c => c.Id == id);
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}
