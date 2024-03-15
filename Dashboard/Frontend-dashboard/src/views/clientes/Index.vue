<template>
  <MenuPrincipal>
    <template #botao-slot>
      <BotaoBtn class="botao-verde" :nomeBotao='BotaoCabecalho' @click="botaoSuperior" />
    </template>
  </MenuPrincipal>
  <div class="conteudo">
    <div class="cliente-geral">
      <h2 class="texto-H2">Lista de clientes</h2>
      <BuscaPesquisa v-model="termoBusca" @buscarClientes="buscarClientes"/>
    </div>
    <div class="tabela-cliente">
      <table id="tabelaClientes">
        <thead>
          <tr>
            <th>Nome</th>
            <th>E-mail</th>
            <th>Telefone</th>
            <th>CPF</th>
            <th>Ações</th>
          </tr>
        </thead>
        <tbody>
          <tr v-for="cliente in clientes" :key="cliente.id">
            <td>{{ cliente.nome }}</td>
            <td>{{ cliente.email }}</td>
            <td>{{ cliente.telefone }}</td>
            <td>{{ cliente.cpf }}</td>
            <td>
              <a href="#" class="delete-cliente" @click="deletarCliente(cliente.id)"> Deletar </a>
              <a href="#" class="editar-cliente" @click="editarCliente(cliente.id)"> Editar </a>
            </td>
          </tr>
        </tbody>
      </table>
      <TotalPaginas :totalClientes="totalClientes" :itensPorPagina="itensPorPagina" :paginaAtual="paginaAtual" @atualizar-pagina="atualizarPagina" />
    </div>
  </div>
</template>

<script>
import MenuPrincipal from '@/layouts/MenuPrincipal.vue';
import BuscaPesquisa from '@/components/inputs/InputBusca.vue';
import BotaoBtn from '@/components/botoes/Botao.vue';
import clienteService from '@/service/clienteService';
import TotalPaginas from '@/components/TotalPaginas.vue';

export default {
  name: 'clientes',
  components: {
    BuscaPesquisa,
    MenuPrincipal,
    BotaoBtn,
    TotalPaginas
  },
  data() {
    return {
      id: null,
      BotaoCabecalho: 'Cadastrar',
      rotaRedirecionamento: '',
      clientes: [],
      itensPorPagina: 5,
      paginaAtual: 1,
      totalClientes: 0,
      termoBusca: '',
    };
  },
  methods: {    
    botaoSuperior() {
      this.$router.push({ name: 'formulario-clientes' });
    },
    async buscarClientes() {  
      const  params = {
        paginaNumero: this.paginaAtual,
        paginaQuantidade: this.itensPorPagina,
        busca: this.termoBusca,
      }
      await clienteService.enviarPaginado(params).then(response => 
      {
        this.totalClientes = response.totalClientes;
        this.clientes = response.clientes;
      }).catch((error) => 
      {
        console.log(error)
      })
    },
    editarCliente(id) {
      this.$router.push({ name: 'formulario-clientes', params: { id: id } });
    },
    deletarCliente(idCliente) {
      if (confirm("Tem certeza que deseja deletar este usuário?")) {
        clienteService.excluir(idCliente).then(() => {           
          console.log("Id cliente: ", idCliente)
          this.buscarClientes();
          console.log("Cliente excluído com sucesso.");
        }).catch((error) => {
          console.error('Erro ao excluir cliente:', error);
        });
      }
    },
    atualizarPagina(pagina) {
      this.paginaAtual = pagina;
      this.buscarClientes();
    }
},
  mounted() {
    this.buscarClientes();
  },
}
</script>

<style scoped>
@media screen and (max-width: 900px) {
  .menu-toggle {
    display: block;
  }

  .botao-verde {
    position: absolute;
    top: 40px;
    left: 110px;
  }

  .cabecalho {
    margin-left: 0px;
    padding: 20px;
  }

  .form-geral {
    margin-left: 0;
  }
}
</style>
