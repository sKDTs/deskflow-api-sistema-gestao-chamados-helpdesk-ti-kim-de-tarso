using System.Text.Json.Serialization;

namespace DeskFlow.API.Models.Entities;

public class Categoria
{
    public int Id { get; set; }

    public string Nome { get; set; } = string.Empty;

    [JsonIgnore]
    public ICollection<Chamado> Chamados { get; set; } = new List<Chamado>();
}
