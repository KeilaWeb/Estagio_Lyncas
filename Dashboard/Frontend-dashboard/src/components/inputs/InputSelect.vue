<template>
    <div>
        <label> {{ label }} </label>
        <select class="cliente-select" @change="enviarValorInput" :class="{ 'erro-input': validarPreenchimento }" :value="modelValue"> 
            <option selected  disabled > Selecione um cliente </option>           
            <option v-for="clientes in clienteSelecao" :key="clientes.id" :value="clientes.id"> {{ clientes.nome }} </option>
        </select> clienteSelecao
        <div v-if="validarPreenchimento" class="erro-mensagem">{{ mensagemErro }}</div>                   
    </div>
</template>
  
<script>
export default { 
    name: 'InputSelect',
    props: {
        label: { type: String, default: '' },
        type: { type: String, default: '' },
        modelValue: { type: [String, Number], default: null },
        required: { type: Boolean, default: false },
        mensagemErro: { type: String, default: '' },
        clienteSelecao: {type: Array, default: ''}
    },
    data(){
        return {
            validarPreenchimento: false,
        }
    },
    methods: {
        enviarValorInput(event) {
            this.$emit('update:modelValue', event.target.value);
        },
        valido() {
            const preenchimentoValido = this.required && this.modelValue !== null && this.modelValue !== undefined && this.modelValue !== '';

            this.validarPreenchimento = !preenchimentoValido 

            this.$emit("update:mensagemErro", this.validarPreenchimento); 
            return !this.validarPreenchimento;
        },
    }
};
</script>
