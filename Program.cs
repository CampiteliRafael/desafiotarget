using System.Text;
using Desafio;

Console.OutputEncoding = Encoding.UTF8;

while (true)
{
    Console.WriteLine("\n===== DESAFIO =====");
    Console.WriteLine("1 - Comissão dos vendedores");
    Console.WriteLine("2 - Movimentação de estoque");
    Console.WriteLine("3 - Cálculo de juros");
    Console.WriteLine("0 - Sair");
    Console.Write("Opção: ");

    switch (Console.ReadLine()?.Trim())
    {
        case "1": Comissoes.Executar(); break;
        case "2": EstoqueMenu.Executar(); break;
        case "3": Juros.Executar(); break;
        case "0": return;
        default: Console.WriteLine("Opção inválida."); break;
    }
}
