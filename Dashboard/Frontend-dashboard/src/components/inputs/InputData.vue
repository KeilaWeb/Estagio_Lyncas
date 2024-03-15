<template>
    <div>
        <label> {{ label }} </label>
        <input type="Date" 
          class="cliente-data" 
          :class="{ 'erro-input': validarPreenchimento }" 
          :value="formatarData(modelValue)" 
          @input='enviarValorInput' />
        <div v-if="validarPreenchimento" class="erro-mensagem">{{ mensagemErro }}</div>
    </div>
</template>  
<script>

export default { 
  name: 'InputData',
  props: {
    label: { type: String, default: '' },
    type: { type: String, default: 'text' },
    modelValue: { type: [Number, Date ,String], default: '' },
    required:{ type: Boolean, default: false },
    mensagemErro: { type: String, default: '' } 
  },

  data() {
    return {
      validarPreenchimento: false,  
    };
  },

  methods: {
    enviarValorInput(event) {
        this.$emit('update:modelValue', event.target.value);
    },

    formatarData(valor) {
        if (!valor) return '';

        const date = new Date(valor);
        const year = date.getFullYear();
        const month = String(date.getMonth() + 1).padStart(2, '0');
        const day = String(date.getDate()).padStart(2, '0');

        return `${year}-${month}-${day}`;
    },

    valido() {
      const preenchimentoValido = this.required ? this.modelValue.trim() !== "" : true;
      
      this.validarPreenchimento = !preenchimentoValido;
      this.$emit("update:mensagemErro", this.validarPreenchimento);

      return !this.validarPreenchimento;
    }
  }
}
</script>

  