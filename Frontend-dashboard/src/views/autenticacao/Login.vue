<template>
  <div class="sessao-azul">
    <div class="logo_topo">
          <router-link class="logo" to="/">
              <img :src="logo" :alt="alt">
          </router-link>
          <h2 class="texto-H2 subtitulo-h2">Bem-vindo ao Lyncas Sales, uma aplicação simples para gerenciar vendas e clientes.</h2>
      </div>
  </div>
  <div class="sessao-branca">
    <h1 class="texto-H1 login-h1">Entrar</h1> 
    <form @submit.prevent="preenchimentoFormulario">
      <div class="coluna-formulario">
        <InputMask type="email" placeholder="E-mail" ref="clienteEmail" v-model="clienteEmail"  required :mensagemErro="'Preencha um e-mail válido'"></InputMask>
      </div>
      <div class="coluna-formulario">
        <InputTexto type="password" placeholder="Senha" ref="clienteSenha"  v-model="clienteSenha" required :mensagemErro="'Preencha com uma senha válido'" />
      </div>
      
      <div class="formulario-btn">
        <div class="lembra-me"> 
          <input type="checkbox" id="remember"><label for="remember"> Lembrar de mim </label> 
        </div>
          <BotaoBtn class="botao-verde" nomeBotao='Entrar' @click.prevent="entrarLogin" />                 
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
</template>
  
<script>
import InputTexto from '@/components/inputs/InputTexto.vue';
import BotaoBtn from '@/components/botoes/Botao.vue';
import InputMask from '@/components/inputs/InputMask.vue';
import Popup from '@/components/modals/Popup.vue';
  export default {
    name: 'login',
    components:{
      InputTexto,
      InputMask,
      BotaoBtn,
      Popup
    },
    props: {
        logo: String,
        alt: String,
    },
    data() {
      return {
          clienteEmail: '',
          clienteSenha: '',
          showPopup: false,
          mensagemPopup: '',
          erroFormulario: false,
          rotaRedirecionamento: ''                
      }; 
    },     
    methods: {
      entrarLogin() {
        const emailValido = this.$refs.clienteEmail.valido();
        const senhaValido = this.$refs.clienteSenha.valido();

        const camposValidos = emailValido && senhaValido;
        console.log(camposValidos)

        this.erroFormulario = !camposValidos;
        this.showPopup = true;
        this.mensagemPopup = camposValidos ? "Formulário enviado com sucesso" : "Existem campos inválidos. Por favor, corrija-os para logar.";
        this.rotaRedirecionamento = camposValidos ? 'dashboard' : '';
      }    
    }
  }
</script>

<style scoped> 
  .sessao-azul {
    background-color: #274A9D;       
    height: 100%;
    width: 50vw;
    position: fixed;
    display: flex;
    text-align: center;
    padding: 0px 220px;
  }  
  .logo img{
    max-width: 100%;
    padding-top: 30vh;
    padding-bottom: 40px;     
  }  
  .subtitulo-h2{
    color: #D9D9D9;
    text-align: center;    
  }
  .login-h1{
    margin-bottom: 20px;
    color: #26324B;
  }

  .sessao-branca {
    background-color: #F8F8F8;       
    height: 100%;
    margin-left: 50vw;
    width: 50vw;
    margin-bottom: 20px;
    justify-content: center;
    text-align: center;
    padding: 30vh 200px;
  }  

  .formulario-btn {
    display: flex;
    justify-content: space-between;
    margin-right: 15px;
  }
  .lembra-me {
    margin-left: 20px;
    display: flex;
    align-items: center;
    color: #7790cc;
  }
  #remember {
    width: 14px;
    margin-right: 6px;
    margin-bottom: 0px;
  }

  @media screen and (max-width: 900px) {
    .sessao-azul {       
      height: 30%;
      width: 100%;      
      padding: 0px 120px;
    } 
    .logo img{
      max-width: 50%;
      padding-top: 40px;
      padding-bottom: 0px;     
    } 
    .sessao-branca { 
      padding-top: 50%; 
      padding-left: 50px; 
      padding-right: 50px;
      padding-bottom: 20px;
      height: 100%;
      width: 100%;
      margin-left: 0vw;
      width: 100%;
      margin-bottom: 20px;
    }  
  }
</style>

