using DeskFlow.API.Models.Dtos;
using DeskFlow.API.Models.Entities;
using DeskFlow.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace DeskFlow.API.Controllers;

[ApiController]
[Route("api/chamados")]
public class ChamadosController : ControllerBase
{
    private readonly ChamadoService _service;

    public ChamadosController(ChamadoService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(
        [FromQuery] StatusChamado? status,
        [FromQuery] Prioridade? prioridade,
        [FromQuery] int? categoriaId)
    {
        var chamados = await _service.GetAllAsync(
            status,
            prioridade,
            categoriaId);

        return Ok(chamados);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var chamado = await _service.GetByIdAsync(id);

        if (chamado is null)
        {
            return NotFound(new
            {
                mensagem = "Chamado não encontrado."
            });
        }

        return Ok(chamado);
    }

    [HttpPost]
    public async Task<IActionResult> Create(CriarChamadoDto dto)
    {
        var chamado = await _service.CreateAsync(dto);

        return CreatedAtAction(
            nameof(GetById),
            new { id = chamado.Id },
            chamado);
    }

    [HttpPost("{id:int}/iniciar")]
    public async Task<IActionResult> Iniciar(int id)
    {
        await _service.IniciarAsync(id);

        return NoContent();
    }

    [HttpPost("{id:int}/encerrar")]
    public async Task<IActionResult> Encerrar(
        int id,
        EncerrarChamadoDto dto)
    {
        await _service.EncerrarAsync(id, dto);

        return NoContent();
    }
}
