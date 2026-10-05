using DeskFlow.API.Models.Dtos;
using DeskFlow.API.Models.Entities;
using DeskFlow.API.Repositories;

namespace DeskFlow.API.Services;

public class InteracaoService
{
    private readonly InteracaoRepository _repository;
    private readonly ChamadoRepository _chamadoRepository;

    public InteracaoService(
        InteracaoRepository repository,
        ChamadoRepository chamadoRepository)
    {
        _repository = repository;
        _chamadoRepository = chamadoRepository;
    }

    public async Task<Interacao> CreateAsync(
        int chamadoId,
        CriarInteracaoDto dto)
    {
        ValidarDados(dto);

        var chamado = await _chamadoRepository
            .GetByIdForUpdateAsync(chamadoId);

        if (chamado is null)
        {
            throw new KeyNotFoundException(
                "Chamado não encontrado.");
        }

        if (chamado.Status == StatusChamado.Fechado)
        {
            throw new InvalidOperationException(
                "Não é possível adicionar interações a um chamado fechado.");
        }

        var interacao = new Interacao
        {
            ChamadoId = chamadoId,
            Autor = dto.Autor.Trim(),
            Mensagem = dto.Mensagem.Trim(),
            DataRegistro = DateTime.Now
        };

        await _repository.AddAsync(interacao);
        await _repository.SaveChangesAsync();

        return interacao;
    }

    private static void ValidarDados(CriarInteracaoDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Autor))
        {
            throw new ArgumentException(
                "O autor da interação é obrigatório.");
        }

        if (dto.Autor.Trim().Length > 150)
        {
            throw new ArgumentException(
                "O autor deve possuir no máximo 150 caracteres.");
        }

        if (string.IsNullOrWhiteSpace(dto.Mensagem))
        {
            throw new ArgumentException(
                "A mensagem da interação é obrigatória.");
        }

        if (dto.Mensagem.Trim().Length > 2000)
        {
            throw new ArgumentException(
                "A mensagem deve possuir no máximo 2000 caracteres.");
        }
    }
}
