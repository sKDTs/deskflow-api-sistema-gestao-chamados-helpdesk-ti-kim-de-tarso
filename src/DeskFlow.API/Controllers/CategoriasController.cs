using DeskFlow.API.Models.Dtos;
using DeskFlow.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace DeskFlow.API.Controllers;

[ApiController]
[Route("api/categorias")]
public class CategoriasController : ControllerBase
{
    private readonly CategoriaService _service;

    public CategoriasController(CategoriaService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var categorias = await _service.GetAllAsync();

        return Ok(categorias);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var categoria = await _service.GetByIdAsync(id);

        if (categoria is null)
        {
            return NotFound(new
            {
                mensagem = "Categoria não encontrada."
            });
        }

        return Ok(categoria);
    }

    [HttpPost]
    public async Task<IActionResult> Create(CriarCategoriaDto dto)
    {
        var categoria = await _service.CreateAsync(dto);

        return CreatedAtAction(
            nameof(GetById),
            new { id = categoria.Id },
            categoria);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(
        int id,
        AtualizarCategoriaDto dto)
    {
        var atualizado = await _service.UpdateAsync(id, dto);

        if (!atualizado)
        {
            return NotFound(new
            {
                mensagem = "Categoria não encontrada."
            });
        }

        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _service.DeleteAsync(id);

        if (!deleted)
        {
            return NotFound(new
            {
                mensagem = "Categoria não encontrada."
            });
        }

        return NoContent();
    }
}
