using System.Globalization;

namespace Desafio;

public static class Juros
{
    private static readonly CultureInfo PtBr = new("pt-BR");
    public const decimal TaxaDiaria = 0.025m;

    public static decimal Calcular(decimal valor, DateTime vencimento, DateTime hoje)
    {
        int diasAtraso = (hoje.Date - vencimento.Date).Days;
        return diasAtraso <= 0 ? 0m : Math.Round(valor * TaxaDiaria * diasAtraso, 2);
    }

    public static void Executar()
    {
        Console.WriteLine("=== CÁLCULO DE JUROS ===");
        Console.Write("Valor (ex.: 1500,00): ");
        if (!decimal.TryParse(Console.ReadLine(), NumberStyles.Number, PtBr, out var valor) || valor <= 0)
        { Console.WriteLine("Valor inválido."); return; }

        Console.Write("Data de vencimento (dd/MM/aaaa): ");
        if (!DateTime.TryParseExact(Console.ReadLine(), "dd/MM/yyyy", PtBr, DateTimeStyles.None, out var venc))
        { Console.WriteLine("Data inválida."); return; }

        var hoje = DateTime.Today;
        int dias = Math.Max(0, (hoje - venc.Date).Days);
        var juros = Calcular(valor, venc, hoje);

        Console.WriteLine($"\nDias em atraso: {dias}");
        Console.WriteLine($"Juros: {juros.ToString("C2", PtBr)}");
        Console.WriteLine($"Total a pagar: {(valor + juros).ToString("C2", PtBr)}");
    }
}
