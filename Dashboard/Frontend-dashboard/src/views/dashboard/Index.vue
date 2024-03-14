<template>
  <MenuPrincipal />
    <div class="dash-geral">
      <Transition name="entrada">
        <div class="dashboard-fundo">
          <h1>Bem vindo(a)</h1>
        </div>
    </Transition>
  </div>
</template>
  
<script>
import MenuPrincipal from '@/layouts/MenuPrincipal.vue';
  export default { 
    name: 'dashboard',
    components: {
        MenuPrincipal,
      },  
  }
</script>
  
<style scoped>
  .dashboard-fundo {
    margin-left: 250px;        
    color: #26324B; 
    height: 100vh;        
  }

  .dash-geral {
    padding: 60px 4% 30px 4%;  
    color: #26324B;
    display: flex;
    position: absolute;
    top: 40%; left: 35%;             
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