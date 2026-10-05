using System.ComponentModel.DataAnnotations;
using DeskFlow.API.Models.Entities;

namespace DeskFlow.API.Models.Dtos;

public class CriarChamadoDto
{
    [Required]
    [MaxLength(200)]
    public string Titulo { get; set; } = string.Empty;

    [Required]
    public string Descricao { get; set; } = string.Empty;

    public Prioridade Prioridade { get; set; }

    [Required]
    [MaxLength(150)]
    public string SolicitanteNome { get; set; } = string.Empty;

    [Range(1, int.MaxValue)]
    public int CategoriaId { get; set; }
}

public class EncerrarChamadoDto
{
    [Required]
    [MaxLength(2000)]
    public string Solucao { get; set; } = string.Empty;
}

public class CriarInteracaoDto
{
    [Required]
    [MaxLength(150)]
    public string Autor { get; set; } = string.Empty;

    [Required]
    [MaxLength(2000)]
    public string Mensagem { get; set; } = string.Empty;
}
