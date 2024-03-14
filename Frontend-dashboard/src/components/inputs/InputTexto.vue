<template>
    <div class="input-container" >
        <label> {{ label }} </label>
        <input 
            :type="type"
            :placeholder="placeholder"
            :required="required"
            :value="modelValue"
            @input="enviarValorInput"
            :class="{ 'erro-input': validarPreenchimento }"  
         />         
        <div v-if="validarPreenchimento" class="erro-mensagem">{{ mensagemErro }}</div>
    </div>
</template>  
<script>
export default { 
    name: 'InputTexto',
    props: {
        id: { type: String, default: '' },
        label: { type: String, default: '' },
        placeholder: { type: String, default: '' },
        type: { type: String, default: 'text' },
        modelValue: { type: String, default: '' },
        required:{ type: Boolean, default: false },
        cpf: { type: String, default: '' },
        mensagemErro: { type: String, default: '' }
    },
    data(){
        return {
            validarPreenchimento: false,             
        }
    },
    watch: {
        modelValue: 'valido',
    },
    methods: {
        enviarValorInput(event) {
            this.$emit('update:modelValue', event.target.value);
        },
        valido() {
            const preenchimentoValido = this.required ? this.modelValue !== "" : true;
            const nomeValido = this.id === "nomeCliente" ? this.modelValue.trim().length >= 3 : true;
            const senhaValida = this.type === "password" ? this.modelValue.trim().length >= 8 : true;

            this.validarPreenchimento = !preenchimentoValido || !nomeValido || !senhaValida;

            this.$emit("update:mensagemErro", this.validarPreenchimento); // o evento está sendo usado para atualizar uma propriedade no componente pai

            return !this.validarPreenchimento;
        }
    }
};
</script>

  