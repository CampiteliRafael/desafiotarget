using System.Globalization;
using System.Text.Json;

namespace Desafio;

public record Venda(string Vendedor, decimal Valor);

public static class Comissoes
{
    private static readonly CultureInfo PtBr = new("pt-BR");

    public static decimal CalcularComissao(decimal valor) => valor switch
    {
        < 100m => 0m,
        < 500m => valor * 0.01m,
        _ => valor * 0.05m
    };

    public static void Executar()
    {
        var caminho = Path.Combine(AppContext.BaseDirectory, "Dados", "vendas.json");
        using var doc = JsonDocument.Parse(File.ReadAllText(caminho));

        var vendas = doc.RootElement.GetProperty("vendas")
            .EnumerateArray()
            .Select(v => new Venda(
                v.GetProperty("vendedor").GetString()!,
                v.GetProperty("valor").GetDecimal()))
            .ToList();

        var resultado = vendas
            .GroupBy(v => v.Vendedor)
            .Select(g => new
            {
                Vendedor = g.Key,
                TotalVendido = g.Sum(v => v.Valor),
                Comissao = Math.Round(g.Sum(v => CalcularComissao(v.Valor)), 2)
            });

        Console.WriteLine("=== COMISSÃO POR VENDEDOR ===");
        Console.WriteLine($"{"Vendedor",-18}{"Total vendido",15}{"Comissão",15}");
        foreach (var r in resultado)
            Console.WriteLine($"{r.Vendedor,-18}{r.TotalVendido.ToString("C2", PtBr),15}{r.Comissao.ToString("C2", PtBr),15}");
    }
}
