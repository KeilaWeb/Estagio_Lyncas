<template>
<MenuPrincipal>
    <template #botao-slot >
        <BotaoBtn class="botao-voltar" nomeBotao='Voltar' @click="botaoSuperior" />
    </template>
</MenuPrincipal>
<div class="form-geral">
    <div class="caixa-form">
        <h2 class="texto-H2"> Adicionar cliente</h2>
        <form @submit.prevent="form">
            <div class="linha-formulario">
                <div class="coluna-formulario">
                    <InputTexto type="text" id="nome" label="Nome" ref="nome" v-model="cliente.nome" required :mensagemErro="'Preencha um nome válido'" />            
                </div>
                <div class="coluna-formulario">
                    <InputMask type="email" label="E-mail" ref="email" v-model="cliente.email" required :mensagemErro="'Preencha um e-mail válido'"/> 
                </div>
            </div>
            <div class="linha-formulario">
                <div class="coluna-formulario">
                    <InputMask type="tel" label="Telefone" ref="telefone" v-model="cliente.telefone" mascarado="(##) #####-####" required :mensagemErro="'Preencha um telefone válido'" /> 
                </div>
                <div class="coluna-formulario">                
                    <InputMask cpf label="CPF" ref="cpf" v-model="cliente.cpf" mascarado='###.###.###-##' required :mensagemErro="'Preencha um CPF válido'"/> 
                </div>   
            </div>
            <div class="formulario-btn">
                <BotaoBtn v-if="$route.params.id" class="botao-verde" nomeBotao="Atualizar" @click.prevent="atualizarCliente" />    
                <BotaoBtn v-else class="botao-verde" nomeBotao="Salvar" @click.prevent="criarCliente" />             
            </div> 
            <Popup 
                :showPopup="showPopup" 
                :mensagem="mensagemPopup" 
                :erroFormulario="erroFormulario"
                :icone="erroFormulario ? 'block' : 'check_circle'" 
                :rotaRedirecionamento="rotaRedirecionamento"
                @fecharPopup="showPopup = false" 
            />
        </form>
    </div>
</div>    
</template>

<script>
import MenuPrincipal from '@/layouts/MenuPrincipal.vue';
import InputTexto from '@/components/inputs/InputTexto.vue';
import InputMask from '@/components/inputs/InputMask.vue';
import BotaoBtn from '@/components/botoes/Botao.vue';
import Popup from '@/components/modals/Popup.vue';
import clienteService from '@/service/clienteService';
import { Cliente } from '@/models/clientes';

export default {
    components: {
        InputMask,
        InputTexto,
        BotaoBtn,
        MenuPrincipal,
        Popup,
    },
    data() {
        return {
            id: null,
            cliente: new Cliente({}),
            valido: false,
            showPopup: false,
            mensagemPopup: "",
            erroFormulario: false,
            rotaRedirecionamento: "",
        };      
    },
    emits: ["modelValue, cliente"], 
    mounted() {
        // Acessando o ID do cliente a partir dos parâmetros da rota
        if(this.$route.params.id){
            this.id = this.$route.params.id;
            this.buscarCliente();
        }
    },       
    methods: {
        botaoSuperior() {
            this.$router.push({ name: 'clientes' });
        },
        async criarCliente(){
            try {
                const camposValidos = this.validarTodosCampos() 
                if (!camposValidos) {
                    this.erroFormulario = true;
                    this.showPopup = true;
                    this.mensagemPopup = "Cliente criado com sucesso";
                    return;
                }
                await clienteService.criarCliente(this.cliente)
                .catch(error => {
                    throw error;
                });
                this.showPopup = true;
                this.mensagemPopup = "Cliente criado com sucesso!";
                this.rotaRedirecionamento = ('clientes' );
            } catch (error) {
                console.error("Erro ao criar cliente:", error);
                this.erroFormulario = true;
                this.showPopup = true;
                this.mensagemPopup = "Erro ao criar cliente. Por favor, tente novamente mais tarde.";
                this.rotaRedirecionamento = "";
            }
        },
        async buscarCliente() {
            try {
                await clienteService.buscarPorId(this.id).then(resposta => {
                    this.cliente = resposta;
                })
            } catch (error) {
                console.error("Erro ao buscar cliente:", error);
            }
        },
        validarTodosCampos(){   
            try {
                const nomeValido = this.$refs.nome.valido();
                const emailValido = this.$refs.email.valido();
                const telefoneValido = this.$refs.telefone.valido();
                const cpfValido = this.$refs.cpf.valido(); 
                return nomeValido && emailValido && telefoneValido && cpfValido
            } catch (error) {
                console.error("Erro durante a validação dos campos:", error);
                return false; 
            }      
        },

        async atualizarCliente() {
            try {
                const camposValidos = this.validarTodosCampos()                  
                if (!camposValidos) {
                    this.erroFormulario = true;
                    this.showPopup = true;
                    this.mensagemPopup = "Existem campos inválidos. Por favor, corrija-os antes de enviar.";
                    return;
                }
                await clienteService.editar(this.id, this.cliente)
                .catch(error => {
                    throw error;
                })
                this.showPopup = true;
                this.mensagemPopup = "Cliente atualizado com sucesso!";
                this.$router.push({ name: 'clientes' });
            } catch (error) {
                console.error("Erro ao editar cliente:", error);
                this.erroFormulario = true;
                this.showPopup = true;
                this.mensagemPopup = "Erro ao atualizar cliente. Por favor, tente novamente mais tarde.";
            }
        }
    }
};
</script>

<style scoped>
    .form-geral{
        background-color: #F8F8F8;
        height: 100%;
    }

    .formulario-btn {
        display: flex;
        justify-content: end;
    }

    #textoH2 {
        margin-bottom: 20px;
        padding-top: 14px;
    }
    @media screen and (max-width: 900px) {      
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
