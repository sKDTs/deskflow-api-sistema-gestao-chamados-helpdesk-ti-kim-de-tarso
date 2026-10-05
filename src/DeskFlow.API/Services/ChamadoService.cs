using DeskFlow.API.Models.Dtos;
using DeskFlow.API.Models.Entities;
using DeskFlow.API.Repositories;

namespace DeskFlow.API.Services;

public class ChamadoService
{
    private readonly ChamadoRepository _repository;

    public ChamadoService(ChamadoRepository repository)
    {
        _repository = repository;
    }

    public async Task<List<Chamado>> GetAllAsync()
    {
        return await _repository.GetAllAsync();
    }

    public async Task<Chamado?> GetByIdAsync(int id)
    {
        return await _repository.GetByIdAsync(id);
    }

    public async Task<Chamado> CreateAsync(CriarChamadoDto dto)
    {
        ValidarDados(dto);

        var categoriaExiste =
            await _repository.CategoriaExistsAsync(dto.CategoriaId);

        if (!categoriaExiste)
        {
            throw new KeyNotFoundException(
                "A categoria informada não existe.");
        }

        var chamado = new Chamado
        {
            Titulo = dto.Titulo.Trim(),
            Descricao = dto.Descricao.Trim(),
            Prioridade = dto.Prioridade,
            Status = StatusChamado.Aberto,
            SolicitanteNome = dto.SolicitanteNome.Trim(),
            DataAbertura = DateTime.Now,
            CategoriaId = dto.CategoriaId
        };

        await _repository.AddAsync(chamado);
        await _repository.SaveChangesAsync();

        return chamado;
    }

    private static void ValidarDados(CriarChamadoDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Titulo))
        {
            throw new ArgumentException("O título do chamado é obrigatório.");
        }

        if (dto.Titulo.Trim().Length > 200)
        {
            throw new ArgumentException(
                "O título deve possuir no máximo 200 caracteres.");
        }

        if (string.IsNullOrWhiteSpace(dto.Descricao))
        {
            throw new ArgumentException(
                "A descrição do chamado é obrigatória.");
        }

        if (string.IsNullOrWhiteSpace(dto.SolicitanteNome))
        {
            throw new ArgumentException(
                "O nome do solicitante é obrigatório.");
        }

        if (dto.SolicitanteNome.Trim().Length > 150)
        {
            throw new ArgumentException(
                "O nome do solicitante deve possuir no máximo 150 caracteres.");
        }

        if (!Enum.IsDefined(dto.Prioridade))
        {
            throw new ArgumentException(
                "A prioridade informada é inválida.");
        }
    }
}
