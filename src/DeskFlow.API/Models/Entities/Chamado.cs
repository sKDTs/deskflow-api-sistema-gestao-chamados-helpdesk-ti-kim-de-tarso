namespace DeskFlow.API.Models.Entities;

public class Chamado
{
    public int Id { get; set; }

    public string Titulo { get; set; } = string.Empty;

    public string Descricao { get; set; } = string.Empty;

    public Prioridade Prioridade { get; set; }

    public StatusChamado Status { get; set; }

    public string SolicitanteNome { get; set; } = string.Empty;

    public DateTime DataAbertura { get; set; }

    public DateTime? DataFechamento { get; set; }

    public string? Solucao { get; set; }

    public int CategoriaId { get; set; }

    public Categoria Categoria { get; set; } = null!;

    public ICollection<Interacao> Interacoes { get; set; } = new List<Interacao>();
}

public enum Prioridade
{
    Baixa,
    Media,
    Alta
}

public enum StatusChamado
{
    Aberto,
    EmAndamento,
    Fechado
}
