<template>
  <div>
    <div>
      <Navegacao :logo="logo_src" :alt="logo_nome" />      
    </div>
    <transition name="customizar-transacao" mode="out-in"> 
    <div>        
        <router-view/>     
      </div>
    </transition> 
  </div>
  
</template>

<script>
  import Navegacao from '@/layouts/Navegacao.vue';

  export default {
    components: {
      Navegacao,
    },
    data(){
      return {
        logo_src: require('@/assets/img/Logo.png'),
        logo_nome: 'Logo Lyncas',
      }
      
    }
  }
</script>
<style scoped>
  /*Transição de paginas*/
  .customizar-transacao-enter, .customizar-transacao-leave-to {
    opacity: 0;
  }

  .customizar-transacao-enter-active, .customizar-transacao-leave-active {
    transition: opacity 0.9s easy-in-out;
  }
</style>