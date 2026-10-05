using DeskFlow.API.Data;
using DeskFlow.API.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace DeskFlow.API.Repositories;

public class InteracaoRepository
{
    private readonly DeskFlowDbContext _context;

    public InteracaoRepository(DeskFlowDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(Interacao interacao)
    {
        await _context.Interacoes.AddAsync(interacao);
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}
