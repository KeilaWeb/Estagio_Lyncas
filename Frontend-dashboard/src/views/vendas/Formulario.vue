<template>
    <MenuPrincipal>
        <template #botao-slot>
            <BotaoBtn class="botao-voltar" nomeBotao='Voltar' @click="botaoSuperior" />
        </template>
    </MenuPrincipal>
    <div class="form-geral">
        <div class="caixa-form">            
            <form @submit.prevent="preenchimentoFormulario">
                <h2 class="texto-H2"> Adicionar venda </h2>
                <div class="linha-formulario">
                    <div class="coluna-formulario">
                        <InputSelect label="Cliente" ref="clienteSelecionado" :clienteSelecao="clientes" v-model="vendas.clienteId" :required="true" :mensagem-erro="'Selecione'"/>
                    </div>
                    <div class="coluna-formulario">
                        <InputData label="Data de faturamento" ref="dataFaturamento" v-model="vendas.dataFaturamento" required :mensagemErro="'Selecione uma data válida'" />
                    </div> 
                </div>
                <div>     
                    <h2 class="Linha texto-H2 adiciona-itens"> Adicionar Itens </h2>
                </div> 
                <div class="linha-formulario">
                    <div class="coluna-formulario">         
                        <InputTexto label="Descrição do item" ref="descricaoItem" v-model="item.descricaoItem" required :mensagemErro="'Adicione uma descrição do item'" />
                    </div>   
                    <div class="coluna-formulario">                  
                        <InputMoney label="Valor unitário" ref="precoUnitario" v-model="item.precoUnitario" required :mensagemErro="'Digite o valor do produto'" />
                    </div>
                </div>    
                <div class="linha-formulario">
                    <div class="coluna-formulario"> 
                        <InputMask type="number" ref="quantidade" label="Quantidade" v-model="item.quantidade" required :mensagemErro="'Você precisa adicionar uma quantidade válida'" />
                    </div>
                    <div class="coluna-formulario">             
                        <InputMoney id="valorItens" ref="valorItens" label="Valor total" v-model="item.valorTotal" />
                    </div>
                </div>
                <div class="botaoAzul">
                    <button type="submit" class="botao-azul" @click.prevent="adicionarItens"> Adicionar Itens </button>
                </div>   
                <p>{{ vendas }}</p>
                <div class="tabela-cliente" v-if="mostrarTabelaItens ||  vendas.itens.length > 0 ">
                    <h3  class="texto-H2 ">Resumo do Pedido</h3>
                    <table id="tabelaClientes">            
                        <thead>
                            <tr>
                                <th>Descrição do Item</th>
                                <th>Valor Unitário</th>
                                <th>Qtd</th>
                                <th>Valor Total</th>
                                <th>Ações</th>
                            </tr>
                        </thead>
                        <tbody>
                            <tr v-for="item in vendas.itens" :key="vendas.id">
                                <td class="pontaInicio">{{ item.descricaoItem}}</td>
                                <td>{{ "R$" + formatarValor(item.precoUnitario) }}</td>
                                <td>{{ item.quantidade }}</td>
                                <td>{{ "R$" + formatarValor(item.valorTotal) }}</td>
                                <td class="pontaFinal">
                                    <a href="#" class="delete-cliente" @click.prevent="deletarItem(item)"> Deletar </a>
                                    <a href="#" class="editar-cliente" @click.prevent="editarItem(item)"> Editar </a>
                                </td>
                            </tr>
                        </tbody>
                    </table>
                </div>                           
            </form>      
            <div class="Linha resultado-form">
                <h2 class="resultado-H2">{{ vendas.valorTotalVenda ? "R$" + formatarValor(vendas.valorTotalVenda) : 'R$0,00' }}</h2>
                <BotaoBtn class="botao-verde" nomeBotao='Salvar' @click.prevent="criarVenda"  />     
            </div>   
            <Popup 
                :showPopup="showPopup" 
                :mensagem="mensagemPopup" 
                :erroFormulario="erroFormulario"
                :icone="erroFormulario ? 'block' : 'check_circle'" 
                :rotaRedirecionamento="rotaRedirecionamento"
                @fecharPopup="showPopup = false" 
            />
        </div>
    </div>
    </template>
    
<script>
import MenuPrincipal from '@/layouts/MenuPrincipal.vue';
import InputSelect from '@/components/inputs/InputSelect.vue';
import InputData from '@/components/inputs/InputData.vue';
import InputTexto from '@/components/inputs/InputTexto.vue';
import BotaoBtn from '@/components/botoes/Botao.vue';
import InputMoney from '@/components/inputs/InputMoney.vue';
import InputMask from '@/components/inputs/InputMask.vue';
import Popup from '@/components/modals/Popup.vue';    
import clienteService from '@/service/clienteService';
import vendaService from '@/service/vendaService';
import { Venda } from '@/models/vendas';
import { Item } from '@/models/item';
    
export default {
    components: {
        MenuPrincipal,
        InputSelect,
        InputData,
        InputTexto,
        BotaoBtn,
        InputMoney,
        InputMask,
        Popup
    },
    data(){
        return {  
            id: null,
            clientes: [],
            vendas: new Venda({          
                quantidadeItens: 0,
                itens: []
            }),
            item: new Item({}),
            mostrarTabelaItens: false,
            valido: false,
            showPopup: false, 
            mensagemPopup: "",
            erroFormulario: false,
            rotaRedirecionamento: "",
        }
    },
    emits: ["modelValue, vendas"], 
    mounted(){
        this.clienteSelect(); 
        //acessando o ID da venda
        if(this.$route.params.id){
            this.id = this.$route.params.id;
            this.buscarVenda();
        }
    },
    methods: {
        botaoSuperior() {
            this.$router.push({ name: 'vendas' });
        },

        async clienteSelect(){
            await clienteService.listar().then( listaClientes => 
            {
                this.clientes = listaClientes
            }).catch((error) => 
            {
                console.log(error)
            })
        },  
        
        async criarVenda() {
            const validarCampos = this.validarTodosCampos();
            if(validarCampos){
                try{  
                    this.conversaoStrings();     
                    await vendaService.criarVenda(this.vendas);
                    this.erroFormulario = false;
                    this.showPopup = true;
                    this.mensagemPopup = "Venda criada com sucesso" ;
                    this.rotaRedirecionamento = ( 'vendas' );
                }catch (error){
                    this.erroFormulario = true;
                    this.showPopup = true;
                    this.mensagemPopup = "Existem campos inválidos. Por favor, corrija-os antes de enviar.";
                    this.rotaRedirecionamento = '';
                } 
            }
        }, 

        conversaoStrings() {     
            this.vendas.valorTotalVenda = Number(this.vendas.valorTotalVenda);
            this.vendas.clienteId = parseInt(this.vendas.clienteId);
        },

        adicionarItens() { 
            const validarItens = this.validarItens();
            if (!validarItens) {
                this.erroFormulario = true;
                this.showPopup = true;
                this.mensagemPopup = "Existem campos inválidos. Por favor, corrija-os antes de enviar.";
                this.mostrarTabelaItens = false;
            } else {
                this.listarTabelaItens();
            }
        },

        listarTabelaItens() {         
            this.item.quantidade = parseInt(this.item.quantidade);
            this.item.precoUnitario = Number(this.item.precoUnitario);

            this.vendas.itens.push(this.item);
            this.mostrarTabelaItens = true;

            this.calculaTotalVenda();
            this.limparCampos();
        },
        limparCampos() {            
            this.item = {
                descricaoItem: null,
                quantidade: null,
                precoUnitario:null,
                valorTotal: null,
            }
        },        

        validarItens() {
            try{
                const descricaoItem = this.$refs.descricaoItem.valido();
                const precoUnitario = this.$refs.precoUnitario.valido();
                const quantidade = this.$refs.quantidade.valido();
                return descricaoItem && precoUnitario && quantidade;
            } catch (error) {
                console.error("Erro durante a validação dos campos:", error);
                return false; 
            }            
        },
        validarTodosCampos() {
            try {
                const clienteSelecionadoValido = this.$refs.clienteSelecionado.valido();
                const dataFaturaValida = this.$refs.dataFaturamento.valido();   
                return clienteSelecionadoValido && dataFaturaValida;
            } catch (error) {
                console.error("Erro durante a validação dos campos:", error);
                return false; 
            }
        },     

        calculaTotalVenda() {
            let total = 0;
            this.vendas.itens.forEach(item => {
                total += Number(item.valorTotal);
            });
            this.vendas.valorTotalVenda = Number(total.toFixed(2));
            this.vendas.quantidadeItens = this.vendas.itens.length
        }, 

        calculaTotalItens() {
            if (this.item.precoUnitario && this.item.quantidade) {
                const precoUnitario = Number(this.item.precoUnitario);
                const quantidade = parseInt(this.item.quantidade);
                if (!isNaN(precoUnitario) && !isNaN(quantidade)) {
                    const total = precoUnitario * quantidade;
                    return total.toFixed(2);
                }
            }
            return 0;
        },
        
        async buscarVenda() {
            try {
                await vendaService.buscarPorId(this.id).then(resposta => {
                    console.log(resposta);
                    this.vendas = resposta;
                    this.vendas.dataFaturamento = resposta.dataFaturamento;
                })
            } catch (error) {
                console.error("Erro ao buscar venda:", error);
            }
        },  

        formatarValor(valor) {
            if (typeof valor !== 'number') {
                return ''; 
            }      
            return valor.toLocaleString('pt-br',{style: 'currency', currency: 'BRL'});
        },

        formatarData(valor) {
            if (!valor) return '';         
            const date = new Date(valor); 
            const year = date.getFullYear();           
            const month = String(date.getMonth() + 1).padStart(2, '0'); 
            const day = String(date.getDate()).padStart(2, '0');
            return `${year}-${month}-${day}`;
        },

        deletarItem(item){
            if (confirm("Tem certeza que deseja deletar este item?")) {
                const index = this.vendas.itens.indexOf(item);
                this.vendas.itens.splice(index, 1);
            if (this.vendas.itens.length == 0) {
                this.mostrarTabelaItens = false;
            }
            this.item = {};
            this.precoUnitario = null
            this.quantidade =  null
            }
        },

        editarItem(item){
            this.item.descricaoItem = item.descricaoItem,
            this.item.precoUnitario = item.precoUnitario,
            this.item.quantidade = item.quantidade,
            this.item.valorTotal = item.valorTotal
            if(this.item){
                this.vendas.itens.splice(item, 1);
            }
        }            
    },      
    watch: {
        'item.precoUnitario'() {
                this.item.valorTotal = this.calculaTotalItens();
        },
        'item.quantidade'() {
                this.item.valorTotal = this.calculaTotalItens();
        }        
    },
}
</script>
    
<style scoped>
    .Linha {
        border-top: 1px dashed #E5EBF1;
        padding: 20px 0px;
    }  

    .resultado-form {
        margin: 20px 0px 10px 0px;        
        display: flex;
        justify-content: space-between;
    }

    .botaoAzul {        
        display: flex;
        justify-content: end;
        padding: 20px 0px 10px 0px;
    }

    .adiciona-itens {
        margin-top: 20px 0px;
    }
    .tabela-cliente {
        padding: 20px;
        background-color: #5b8ac70e;
        border: 1px solid #5b8ac723;
        border-radius: 5px;
    }
    .pontaInicio {
        border-end-start-radius: 6px;
        border-start-start-radius: 6px;
    }
    .pontaFinal {
        border-end-end-radius: 6px;
        border-start-end-radius: 6px;
    }
    @media screen and (max-width: 900px) {d
        .menu-toggle {
            display: block;
        }
        .botao-voltar {
            position: absolute;
            top:53px; left: 110px;
        }
        .cabecalho {
            margin-left: 0px;
            padding: 20px ;
        }
        .infos-cabecalho {
            padding-bottom: 20px;      
        }
        .form-geral{
            margin-left: 0;
        }
    }
    
</style>
    
    