using DeskFlow.API.Models.Dtos;
using DeskFlow.API.Models.Entities;
using DeskFlow.API.Repositories;

namespace DeskFlow.API.Services;

public class CategoriaService
{
    private readonly CategoriaRepository _repository;

    public CategoriaService(CategoriaRepository repository)
    {
        _repository = repository;
    }

    public async Task<List<Categoria>> GetAllAsync()
    {
        return await _repository.GetAllAsync();
    }

    public async Task<Categoria?> GetByIdAsync(int id)
    {
        return await _repository.GetByIdAsync(id);
    }

    public async Task<Categoria> CreateAsync(CriarCategoriaDto dto)
    {
        ValidarNome(dto.Nome);

        var categoria = new Categoria
        {
            Nome = dto.Nome.Trim()
        };

        await _repository.AddAsync(categoria);
        await _repository.SaveChangesAsync();

        return categoria;
    }

    public async Task<bool> UpdateAsync(int id, AtualizarCategoriaDto dto)
    {
        ValidarNome(dto.Nome);

        var categoria = await _repository.GetByIdAsync(id);

        if (categoria is null)
        {
            return false;
        }

        categoria.Nome = dto.Nome.Trim();

        _repository.Update(categoria);
        await _repository.SaveChangesAsync();

        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var categoria = await _repository.GetByIdWithChamadosAsync(id);

        if (categoria is null)
        {
            return false;
        }

        if (categoria.Chamados.Count > 0)
        {
            throw new InvalidOperationException(
                "Não é possível excluir uma categoria que possui chamados associados.");
        }

        _repository.Delete(categoria);
        await _repository.SaveChangesAsync();

        return true;
    }

    private static void ValidarNome(string nome)
    {
        if (string.IsNullOrWhiteSpace(nome))
        {
            throw new ArgumentException("O nome da categoria é obrigatório.");
        }

        if (nome.Trim().Length > 100)
        {
            throw new ArgumentException(
                "O nome da categoria deve possuir no máximo 100 caracteres.");
        }
    }
}
