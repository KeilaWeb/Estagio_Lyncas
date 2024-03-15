<template>
  <MenuPrincipal>
    <template #botao-slot>
      <BotaoBtn :class="classBotao" :nomeBotao='BotaoCabecalho' @click="botaoSuperior"/>
    </template>
  </MenuPrincipal>
  <div class="conteudo">
      <div class="cliente-geral">
          <h2 class="texto-H2">Lista de vendas</h2>          
          <BuscaPesquisa v-model="termoBusca" @buscarClientes="buscarVendas"/>
      </div>
    <div class="tabela-cliente">
      <table>
        <thead>
          <tr>
            <th>Cliente</th>
            <th>Qtd. itens</th>
            <th>Data da venda</th>
            <th>Data faturamento</th>
            <th>Valor total</th>
            <th>Ações</th>
          </tr>
        </thead>
      </table>
      <table>
        <tbody>
          <tr v-for="venda in vendas" :key="venda.id">
            <td class="pontaInicio">{{ venda.nome }}</td>
            <td>{{ venda.quantidadeItens }}</td>
            <td>{{ formatarData(venda.dataVenda) }}</td>
            <td>{{ formatarData(venda.dataFaturamento) }}</td>
            <td>{{ "R$ " + formatarValor(venda.valorTotalVenda) }}</td>
            <td class="pontaFinal">
              <a href="#" class="delete-cliente" @click="deletarVenda(venda.id)"> Deletar</a>
              <a href="#" class="editar-cliente" @click="editarVenda(venda.id)"> Editar  </a>
            </td>
          </tr>
        </tbody>
      </table>      
    </div>
  </div>
</template>
  
<script>
import FormularioTexto from '@/components/inputs/InputTexto.vue';
import BuscaPesquisa from '@/components/inputs/InputBusca.vue';
import MenuPrincipal from '@/layouts/MenuPrincipal.vue';
import BotaoBtn from '@/components/botoes/Botao.vue';
import vendaService from '@/service/vendaService';
import { vendas } from '@/models/vendas';

export default {
  name: 'vendas',
  components: {
    FormularioTexto,
    BuscaPesquisa,
    MenuPrincipal,
    BotaoBtn,
  },        
  data(){
    return { 
      id: null,         
      BotaoCabecalho: 'Cadastrar',
      classBotao: 'botao-verde',          
      tipoBusca: 'Buscar venda',
      rotaRedirecionamento: '',  
      vendas: [], 
              
    }
  },  
  emits: ["clientesVendas"],     
  methods: {    
    botaoSuperior() {
      this.$router.push({ name: 'formulario-vendas' });
    },
    async buscarVendas(){
      await vendaService.listar().then(listaVenda => 
      {
        this.vendas = listaVenda
      }).catch((error) => 
      {
        console.log(error)
      })
    },

    editarVenda(id){
      this.$router.push({ name: 'formulario-vendas', params: { id: id } });
    },
    
    deletarVenda(idVenda){
      if (confirm("Tem certeza que deseja deletar esta venda?")) {
        vendaService.excluir(idVenda).then(() => { 
          console.log("Id venda: ", idVenda)
          this.buscarVendas();
          console.log("Venda excluído com sucesso.");
        }).catch((error) => {
          console.error('Erro ao excluir venda:', error);
        });
      }
    },
    formatarData(valor) {
        if (!valor) return '';         
        const date = new Date(valor); 
        const year = date.getFullYear();           
        const month = String(date.getMonth() + 1).padStart(2, '0'); 
        const day = String(date.getDate()).padStart(2, '0');
        return `${year}-${month}-${day}`;
    },

    formatarValor(valor) {
      if (typeof valor !== 'number') {
        return ''; 
      }      
      // Formata o número com vírgula para separar os decimais e ponto para separar os milhares
      return valor.toLocaleString('pt-br',{style: 'currency', currency: 'BRL'});
    }
  },
  
  mounted(){
    this.buscarVendas();
  }
}

</script>

<style scoped>
.pontaInicio {
      border-end-start-radius: 6px;
      border-start-start-radius: 6px;
  }
  .pontaFinal {
      border-end-end-radius: 6px;
      border-start-end-radius: 6px;
  }
@media screen and (max-width: 900px) {
  .menu-toggle {
      display: block;
  }
  .botao-verde {
      position: absolute;
      top:40px; left: 110px;
  }
  .cabecalho {
      margin-left: 0px;
      padding: 20px ;
  }
  .form-geral{
      margin-left: 0;
  }
}
</style>