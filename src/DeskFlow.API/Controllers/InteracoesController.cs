using DeskFlow.API.Models.Dtos;
using DeskFlow.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace DeskFlow.API.Controllers;

[ApiController]
[Route("api/chamados/{chamadoId:int}/interacoes")]
public class InteracoesController : ControllerBase
{
    private readonly InteracaoService _service;

    public InteracoesController(InteracaoService service)
    {
        _service = service;
    }

    [HttpPost]
    public async Task<IActionResult> Create(
        int chamadoId,
        CriarInteracaoDto dto)
    {
        var interacao = await _service.CreateAsync(
            chamadoId,
            dto);

        return Created(
            $"/api/chamados/{chamadoId}/interacoes/{interacao.Id}",
            interacao);
    }
}
