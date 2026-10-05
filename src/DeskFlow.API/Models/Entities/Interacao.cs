using System.Text.Json.Serialization;

namespace DeskFlow.API.Models.Entities;

public class Interacao
{
    public int Id { get; set; }

    public int ChamadoId { get; set; }

    [JsonIgnore]
    public Chamado Chamado { get; set; } = null!;

    public string Autor { get; set; } = string.Empty;

    public string Mensagem { get; set; } = string.Empty;

    public DateTime DataRegistro { get; set; }
}
