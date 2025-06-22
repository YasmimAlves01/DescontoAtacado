﻿using System.Globalization;

string[,] produtosCatalogo = new string[,]
{

    { "7891024110348", "2,88", "2,51", "12"},
    {"7891048038017", "4,40", "4,37", "3"},
    {"7896066334509", "5,19", "-", "-"},
    {"7891700203142", "2,39", "2,38", "6"},
    {"7894321711263", "9,79", "-", "-"},
    {"7896001250611", "9,89", "9,10", "10"},
    {"7793306013029", "12,79", "12,35", "3"},
    {"7896004400914", "4,20", "4,05", "6"},
    {"7898080640017", "6,99", "6,89", "12"},
    {"7891025301516", "12,99", "-", "-"},
    {"7891030003115", "3,12", "3,09", "4"},
    
};

string[,] listaCompras = new string[,]
{

    { "7891048038017", "1", "4,40"},
    {"7896004400914", "4", "16,80"},
    {"7891030003115", "1", "3,12"},
    {"7891024110348", "6", "17,28"},
    {"7898080640017", "24", "167,76"},
    {"7896004400914", "8", "33,60"},
    {"7891700203142", "8", "19,12"},
    {"7891048038017", "1", "4,40"},
    {"7793306013029", "3", "38,37"},
    {"7896066334509", "2", "10,38"},
    
};


CultureInfo MoedaBrasil = new CultureInfo("pt-BR");
int totalProdutosCatalogo = produtosCatalogo.GetLength(0);
int totalItensComprado = listaCompras.GetLength(0);
int totalProdutos = 0;


string[] gtinsUnicos = new string[totalItensComprado];
int[] quantidadeGtin = new int[totalItensComprado];

for (int i = 0; i < totalItensComprado; i++)
{
    string codigoGtin = listaCompras[i, 0];
    int quantidade = int.Parse(listaCompras[i, 1]);

    int gtinPosicao = -1;
    for (int j = 0; j < totalProdutos; j++)
    {
        if (gtinsUnicos[j] == codigoGtin)
        {
            gtinPosicao = j;
            break;
        }
    }

    if (gtinPosicao == -1)
    {
        gtinsUnicos[totalProdutos] = codigoGtin;
        quantidadeGtin[totalProdutos] = quantidade;
        totalProdutos++;
    }
    else
    {
        quantidadeGtin[gtinPosicao] += quantidade;
    }
}

double valorSemDesconto = 0;
double valorTotalDescontos = 0;
double[] descontoProduto = new double[totalProdutos];

for (int i = 0; i < totalProdutos; i++)
{
    string codigoGtin = gtinsUnicos[i];
    int quantidade = quantidadeGtin[i];

    double precoUnitario = 0;
    double precoDesconto = 0;
    int minimoParaAtacado = 0;
    bool ProdutoTemAtacado = false;

    for (int j = 0; j < totalProdutosCatalogo; j++)
    {
        if (produtosCatalogo[j, 0] == codigoGtin)
        {
            precoUnitario = double.Parse(produtosCatalogo[j, 1], MoedaBrasil);

            if (produtosCatalogo[j, 2] != "-" && produtosCatalogo[j, 3] != "-")
            {
                precoDesconto = double.Parse(produtosCatalogo[j, 2], MoedaBrasil);
                minimoParaAtacado = int.Parse(produtosCatalogo[j, 3]);
                ProdutoTemAtacado = true;
            }
            break;
        }
    }
    valorSemDesconto += precoUnitario * quantidade;

    if (ProdutoTemAtacado && quantidade >= minimoParaAtacado)
    {
        double totalSemDesconto = precoUnitario * quantidade;
        double totalComDesconto = precoDesconto * quantidade;
        double descontoFinal = totalSemDesconto - totalComDesconto;

        descontoProduto[i] = descontoFinal;
        valorTotalDescontos += descontoFinal;
    }
    else
    {
        descontoProduto[i] = 0;
    }
}

Console.Clear();
Console.WriteLine("--- Desconto no Atacado ---\n");
Console.WriteLine("Descontos por produto:\n");
Console.WriteLine("GTIN            Valor do desconto");

for (int i = 0; i < totalProdutos; i++)
{
    if (descontoProduto[i] > 0)
    {
        Console.WriteLine($"{gtinsUnicos[i],-15} R$ {descontoProduto[i]:F2}");
    }
}

Console.WriteLine($"\n(+) Subtotal  =    R$ {valorSemDesconto:F2}");
Console.WriteLine($"(-) Descontos =      R$ {valorTotalDescontos:F2}");
Console.WriteLine($"(=) Total     =    R$ {(valorSemDesconto - valorTotalDescontos):F2}");