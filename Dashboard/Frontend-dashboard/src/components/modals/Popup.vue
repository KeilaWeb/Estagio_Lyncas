<template>
    <Transition name="modal">
        <div v-if="showPopup" class="popup">
            <div class="area-popup">
                <div :class="tipoPopup" class="modal">
                    <span class="material-icons">{{ icone }}</span>                   
                    <p class="mensagem-popup">{{ mensagem }}</p>
                    <button @click="fecharPopup"> OK </button>
                </div>
            </div>
        </div>
    </Transition>
</template>
  
<script>
export default { 
    props: {
        showPopup: {
            type: Boolean,
            default: false,
        },
        erroFormulario: {
            type: Boolean,
            default: false,
        },
        icone: {
            type: String,
            default: "",
        },
        mensagem: {
            type: String,
            default: "",
        },
        rotaRedirecionamento: {
            type: String,
            default: "", 
        },
    },
    computed: {
        tipoPopup() {
            return this.erroFormulario ? "popup-erro" : "popup-sucesso";
        },
    },
    methods: {
        fecharPopup(confirmado) {
            this.$emit("fecharPopup", confirmado);
            if (!this.erroFormulario && this.rotaRedirecionamento) {
                this.$router.push({ name: this.rotaRedirecionamento });
            }
        }
    }
}
</script>
  
<style scoped>    
    .material-icons {
        color: white;
        font-size: 60px;
        margin-bottom: 20px;
    }
    /*Transição*/
    .modal-enter-active {
        transition: opacity 0.6s ease;
    }
    .modal-leave-active{
        transition: opacity .6s ease-out;
    }
    .modal-enter, .modal-leave-to {
        opacity: 0;
    }
    .popup {
        animation: modalInOut .6s ease;
    }
    @keyframes modalInOut {
        0% {
            opacity: 0;
            transform: translateX(40px);
        }
        50% {
            opacity: 0.5;
            transform: translateX(20);
        }
        100% {
            opacity: 1;
            transform: translateX(0);
        }
    }

</style>