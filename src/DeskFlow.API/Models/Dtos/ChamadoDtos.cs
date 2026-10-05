using DeskFlow.API.Models.Entities;

namespace DeskFlow.API.Models.Dtos;

public class CriarChamadoDto
{
    public string Titulo { get; set; } = string.Empty;

    public string Descricao { get; set; } = string.Empty;

    public Prioridade Prioridade { get; set; }

    public string SolicitanteNome { get; set; } = string.Empty;

    public int CategoriaId { get; set; }
}
