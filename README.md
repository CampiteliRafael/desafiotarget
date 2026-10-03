# Desafio Dev em C#

Aplicação de console em C# (.NET) que resolve os três exercícios do desafio:

1. Cálculo de comissão por vendedor
2. Movimentação de estoque
3. Cálculo de juros por atraso

## Requisitos

- [.NET SDK](https://dotnet.microsoft.com/download) 8 ou superior
- VS Code com a extensão **C# Dev Kit** (opcional, o programa roda por qualquer terminal)

Confira a versão instalada com `dotnet --version`.

> O projeto vem configurado para `net8.0`. Se você tiver apenas o SDK/runtime de outra versão (por exemplo, 10), abra o `Desafio.csproj` e altere a linha `<TargetFramework>net8.0</TargetFramework>` para a sua versão (`net10.0`, por exemplo).

## Como executar

Na pasta que contém o arquivo `Desafio.csproj`:

```
dotnet run
```

O menu principal será exibido:

```
===== DESAFIO =====
1 - Comissão dos vendedores
2 - Movimentação de estoque
3 - Cálculo de juros
0 - Sair
```

## Estrutura do projeto

```
Desafio/
├── Dados/
│   ├── vendas.json      # Vendas do exercício 1
│   └── estoque.json     # Produtos do exercício 2
├── Comissoes.cs         # Exercício 1
├── Estoque.cs           # Exercício 2
├── Juros.cs             # Exercício 3
├── Program.cs           # Menu principal
├── Desafio.csproj
└── README.md
```

Os arquivos JSON são copiados automaticamente para a pasta de saída do build.

## Exercício 1: Comissão dos vendedores

Lê `Dados/vendas.json`, agrupa as vendas por vendedor e calcula a comissão de **cada venda**:

| Valor da venda | Comissão |
|---|---|
| Abaixo de R$ 100,00 | Sem comissão |
| De R$ 100,00 até abaixo de R$ 500,00 | 1% |
| A partir de R$ 500,00 | 5% |

Uma venda de exatamente R$ 500,00 entra na faixa de 5%.

Resultado esperado com os dados do desafio:

| Vendedor | Total vendido | Comissão |
|---|---|---|
| João Silva | R$ 10.754,70 | R$ 495,68 |
| Maria Souza | R$ 9.874,30 | R$ 465,95 |
| Carlos Oliveira | R$ 7.928,35 | R$ 379,37 |
| Ana Lima | R$ 8.763,95 | R$ 404,98 |

## Exercício 2: Movimentação de estoque

Carrega os produtos de `Dados/estoque.json` e permite lançar movimentações pelo menu:

- **1 - Entrada**: soma a quantidade ao estoque do produto.
- **2 - Saída**: subtrai a quantidade do estoque do produto.
- **3 - Histórico**: lista as movimentações feitas na sessão.

Para cada movimentação, o programa pede o código do produto, a quantidade e uma descrição do tipo de movimentação (por exemplo, "Venda balcão" ou "Compra de fornecedor"). Cada movimentação recebe um **número identificador único** e sequencial. Ao final do lançamento, o programa mostra a **quantidade final em estoque** do produto movimentado.

Validações:

- A quantidade deve ser maior que zero.
- A descrição é obrigatória.
- O produto deve existir.
- Uma saída não pode ser maior que o estoque disponível.

> As alterações ficam apenas em memória. Ao sair do programa, o estoque volta aos valores do JSON.

## Exercício 3: Cálculo de juros

Informe o valor e a data de vencimento (`dd/MM/aaaa`). O programa calcula os dias de atraso até a data de hoje e aplica **2,5% ao dia** sobre o valor, em juros simples:

```
juros = valor × 2,5% × dias de atraso
```

Se a data de vencimento é hoje ou futura, os juros são zero.

Exemplo: R$ 1.000,00 com 5 dias de atraso resulta em R$ 125,00 de juros, com total a pagar de R$ 1.125,00.

Se o enunciado exigir juros compostos, basta alterar a fórmula no método `Juros.Calcular`.

## Problemas comuns

- **"Não foi possível localizar um projeto para executar"**: você não está na pasta que contém o `Desafio.csproj`. Entre na pasta correta com `cd`.
- **Erro de runtime não encontrado**: o `TargetFramework` do `.csproj` não corresponde ao .NET instalado. Veja `dotnet --list-runtimes` e ajuste o `.csproj`.
- **Acentos incorretos no terminal do Windows**: rode `chcp 65001` antes do `dotnet run`.
