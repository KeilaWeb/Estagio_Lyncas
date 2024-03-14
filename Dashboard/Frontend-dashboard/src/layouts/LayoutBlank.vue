<template>
  <div>
    <div>
      <Login :logo="logo_src" :alt="logo_nome" />      
    </div>

  </div>
  
</template>

<script>
import Login from '@/views/autenticacao/Login.vue';

  export default {
    components: {
      Login
    },
    data(){
      return {
        logo_src: require('@/assets/img/Logo.png'),
        logo_nome: 'Logo Lyncas',
      }      
    }
  }
</script>
