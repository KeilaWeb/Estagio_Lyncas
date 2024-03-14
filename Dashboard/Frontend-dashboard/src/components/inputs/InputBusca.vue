<template>
    <div>        
        <div class="pesquisa-tabela-conteudo">            
            <input type="text" class="pesquisa-tabela-conteudo"                  
                :placeholder="tipoBusca" :class="{ 'erro-input': !preenchimentoValido }" >
            <button>
                <span class="material-icons">
                    search
                </span>                
            </button>
        </div>
    </div>
</template>
  
<script>
    export default { 
        name: 'BuscaPesquisa',
        props: {
            tipoBusca: { type: [Number, Date ,String], default: '' },
            placeholder: { type: String, default: '' },
        },
        data() {
            return{
                preenchimentoValido: true,
            }
        }
    }
</script>
  
<style scoped>
    .material-icons {
        color: #BFBFBF;
        width: 25px;
        background: none;
        border: none;
        position: absolute;
        right: 10px;
        padding: 8px 12px 0px 0px;
        cursor: pointer;
    }      
</style>