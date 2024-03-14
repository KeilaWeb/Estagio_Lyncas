<template>
    <div class="input-container">
        <label> {{ label }} </label>
        <input :type="type" :placeholder="placeholder" :required="required" :class="{ 'erro-input': validarPreenchimento }"
        :value="formatarValor(modelValue)" v-money3="config" @input='enviarValorInput' 
        />
        <div v-if="validarPreenchimento" class="erro-mensagem">{{ mensagemErro }}</div>           
    </div>
</template>  
<script>
import { Money3Directive, unformat} from 'v-money3';
    export default { 
        name: 'InputMoney',
        directives: { money3: Money3Directive },
        props: {
            label: { type: String, default: '' },
            placeholder: { type: String, default: '' },
            type: { type: String, default: 'text' },
            modelValue: { type: [String, Number] },
            required:{ type: Boolean, default: false },
            mensagemErro: { type: String, default: '' } 
        },
        data(){
            return {
                validarPreenchimento: false, 
                config: {
                    prefix: 'R$',
                    suffix: '',
                    thousands: '.',
                    decimal: ',',
                    precision: 2,
                    disableNegative: true,
                    disabled: false,
                    min: null,
                    max: null,
                    allowBlank: false,
                    shouldRound: true,
                    focusOnRight: false
                },
            }
        },    
        methods: {
            enviarValorInput(event) { //evento de entrada do input
                const unformattedValue = unformat(event.target.value);
                this.$emit('update:modelValue', unformattedValue);
            },
            valido() {
                const preenchimentoValido = this.required && this.modelValue && this.modelValue.trim() !== "0.00";
                const valorNumerico = this.label === "Valor unitário" ? this.valorValido() : true;
                this.validarPreenchimento = !preenchimentoValido || !valorNumerico;
                this.$emit("update:mensagemErro", this.validarPreenchimento);
                return !this.validarPreenchimento;
            },

            valorValido() {         
                const valorNumerico = parseFloat(this.modelValue);
                return (this.modelValue === null || this.modelValue.trim() === '') || (!isNaN(valorNumerico) && valorNumerico !== 0);
            },
            formatarValor(valor) {
                console.log("valor",valor)
                if (valor === 0) {
                    return valor.toLocaleString('pt-br',{style: 'currency', currency: 'BRL'});
                }
                return (valor);
            },
            
        },
}
</script>
