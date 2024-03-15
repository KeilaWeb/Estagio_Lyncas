<template>       
    <div class="pesquisa-tabela-conteudo">         
        <input 
            @keyup.enter="buscarClientes"
            v-model="termoBusca" 
            type="text" 
            class="pesquisa-tabela-conteudo"                  
            placeholder="Pesquisar..."
            @input="emitValor"
            :class="{ 'erro-input': !preenchimentoValido }" 
        >
        <button 
            @click="buscarClientes">                
            <span class="material-icons">
                search
            </span>                
        </button>
    </div>
</template>
  
<script>
export default { 
    name: 'BuscaPesquisa',
    props: {
        modelValue: { type: [Number, Date ,String], default: '' },
    },
    data() {
        return{
            preenchimentoValido: true,
            termoBusca: '',
        }
    },
    methods: {
        buscarClientes() {
            this.$emit('buscarClientes');
        },
        emitValor(event) {
            this.$emit('update:modelValue', event.target.value);
        }
    },
    watch: {
        modelValue: 'buscarClientes',
    },
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