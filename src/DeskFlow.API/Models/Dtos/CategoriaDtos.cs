using System.ComponentModel.DataAnnotations;

namespace DeskFlow.API.Models.Dtos;

public class CriarCategoriaDto
{
    [Required]
    [MaxLength(100)]
    public string Nome { get; set; } = string.Empty;
}

public class AtualizarCategoriaDto
{
    [Required]
    [MaxLength(100)]
    public string Nome { get; set; } = string.Empty;
}
