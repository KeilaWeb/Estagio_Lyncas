export class Venda{
    constructor({id, nome, quantidadeItens, dataVenda, dataFaturamento, valorTotalVenda, itens, clienteId}) {
        this.id = id,
        this.nome = nome,
        this.quantidadeItens = quantidadeItens,
        this.dataVenda = new Date();
        this.dataFaturamento = dataFaturamento,
        this.valorTotalVenda = valorTotalVenda,
        this.itens = itens,
        this.clienteId = clienteId
    }
}