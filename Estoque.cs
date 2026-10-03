using System.Text.Json;

namespace Desafio;

public enum TipoMovimentacao { Entrada = 1, Saida = 2 }

public class Produto
{
    public int CodigoProduto { get; set; }
    public string DescricaoProduto { get; set; } = "";
    public int Quantidade { get; set; }
}

public record Movimentacao(
    int Id, int CodigoProduto, TipoMovimentacao Tipo,
    string Descricao, int Quantidade, DateTime DataHora);

public class ControleEstoque
{
    private readonly List<Produto> _produtos;
    private readonly List<Movimentacao> _historico = new();
    private int _proximoId = 1;

    public ControleEstoque(IEnumerable<Produto> produtos) => _produtos = produtos.ToList();

    public IReadOnlyList<Produto> Produtos => _produtos;
    public IReadOnlyList<Movimentacao> Historico => _historico;

    public static ControleEstoque CarregarDoJson(string caminho)
    {
        using var doc = JsonDocument.Parse(File.ReadAllText(caminho));
        var produtos = doc.RootElement.GetProperty("estoque").EnumerateArray()
            .Select(p => new Produto
            {
                CodigoProduto = p.GetProperty("codigoProduto").GetInt32(),
                DescricaoProduto = p.GetProperty("descricaoProduto").GetString()!,
                Quantidade = p.GetProperty("estoque").GetInt32()
            });
        return new ControleEstoque(produtos);
    }

    public (Movimentacao Mov, int EstoqueFinal) Movimentar(
        int codigoProduto, TipoMovimentacao tipo, int quantidade, string descricao)
    {
        if (quantidade <= 0)
            throw new ArgumentException("A quantidade deve ser maior que zero.");
        if (string.IsNullOrWhiteSpace(descricao))
            throw new ArgumentException("Informe a descrição da movimentação.");

        var produto = _produtos.FirstOrDefault(p => p.CodigoProduto == codigoProduto)
            ?? throw new InvalidOperationException($"Produto {codigoProduto} não encontrado.");

        if (tipo == TipoMovimentacao.Saida && produto.Quantidade < quantidade)
            throw new InvalidOperationException(
                $"Estoque insuficiente. Disponível: {produto.Quantidade}, solicitado: {quantidade}.");

        produto.Quantidade += tipo == TipoMovimentacao.Entrada ? quantidade : -quantidade;

        var mov = new Movimentacao(_proximoId++, codigoProduto, tipo, descricao.Trim(), quantidade, DateTime.Now);
        _historico.Add(mov);
        return (mov, produto.Quantidade);
    }
}

public static class EstoqueMenu
{
    public static void Executar()
    {
        var caminho = Path.Combine(AppContext.BaseDirectory, "Dados", "estoque.json");
        var controle = ControleEstoque.CarregarDoJson(caminho);

        while (true)
        {
            Console.WriteLine("\n=== ESTOQUE ===");
            foreach (var p in controle.Produtos)
                Console.WriteLine($"  {p.CodigoProduto} - {p.DescricaoProduto,-28} Qtde: {p.Quantidade}");
            Console.WriteLine("\n1 - Entrada | 2 - Saída | 3 - Histórico | 0 - Voltar");
            Console.Write("Opção: ");

            switch (Console.ReadLine()?.Trim())
            {
                case "1": Lancar(controle, TipoMovimentacao.Entrada); break;
                case "2": Lancar(controle, TipoMovimentacao.Saida); break;
                case "3":
                    foreach (var m in controle.Historico)
                        Console.WriteLine($"#{m.Id} | {m.DataHora:dd/MM/yyyy HH:mm} | {m.Tipo} | Produto {m.CodigoProduto} | Qtde {m.Quantidade} | {m.Descricao}");
                    if (controle.Historico.Count == 0) Console.WriteLine("Nenhuma movimentação ainda.");
                    break;
                case "0": return;
                default: Console.WriteLine("Opção inválida."); break;
            }
        }
    }

    private static void Lancar(ControleEstoque controle, TipoMovimentacao tipo)
    {
        try
        {
            Console.Write("Código do produto: ");
            var codigo = int.Parse(Console.ReadLine() ?? "");
            Console.Write("Quantidade: ");
            var qtde = int.Parse(Console.ReadLine() ?? "");
            Console.Write("Descrição da movimentação (ex.: Compra de fornecedor, Venda, Ajuste): ");
            var desc = Console.ReadLine() ?? "";

            var (mov, final) = controle.Movimentar(codigo, tipo, qtde, desc);
            Console.WriteLine($"\nMovimentação #{mov.Id} registrada ({mov.Tipo}: {mov.Descricao}).");
            Console.WriteLine($"Estoque final do produto {codigo}: {final}");
        }
        catch (FormatException) { Console.WriteLine("Valor numérico inválido."); }
        catch (Exception ex) when (ex is ArgumentException or InvalidOperationException)
        { Console.WriteLine($"Erro: {ex.Message}"); }
    }
}
