<template>
    <div class="input-container">
        <label> {{ label }} </label>
        <input 
            :type="type"
            :placeholder="placeholder"
            :required="required"
            :value="modelValue"
            autocomplete="true"
            @input="enviarValorInput"
            v-maska :data-maska="[mascarado]"
            :class="{ 'erro-input': validarPreenchimento }"
        />              
        <div v-if="validarPreenchimento" class="erro-mensagem">{{ mensagemErro }}</div>
    </div>
</template>  
<script>
import helpers from '@/common/helpers';
import { vMaska } from "maska"

    export default { 
        name: 'InputMask',
        directives: { maska: vMaska },
        props: {
            label: { type: String, default: '' },
            placeholder: { type: String, default: '' },
            type: { type: String, default: '' },
            modelValue: { type: [Number, String], default: '' },
            required:{ type: Boolean, default: false },
            cpf: { type: String, default: '' },
            mascarado: { type: String, default: '' },
            mensagemErro: { type: String, default: '' }        
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
                const preenchimentoValido = this.required && this.modelValue.trim() !== "";

                const telefoneValido = this.type === "tel" ? helpers.telefoneValido(this.modelValue) : true;
                const emailValido = this.type === "email" ? helpers.emailValido(this.modelValue) : true;
                const cpfValido = this.cpf ? helpers.cpfValido(this.modelValue) : true;

                this.validarPreenchimento = !preenchimentoValido || !telefoneValido || !emailValido || !cpfValido;

                this.$emit("update:mensagemErro", this.validarPreenchimento);

                return !this.validarPreenchimento;
            }
        }
    };
</script>
