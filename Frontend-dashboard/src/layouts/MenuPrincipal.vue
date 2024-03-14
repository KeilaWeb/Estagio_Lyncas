<template>
    <div class="cabecalho">
        <div class="infos-cabecalho">
            <div class="infos_usuario">
                <div class="usuario_img">
                    <img :src="usuarioInfo.foto_usuario" :alt="usuarioInfo.alt_img">
                </div>
                <div class="div-interna" >
                    <h2 class="resultado-H2" v-if="nomeCompleto"> Olá, {{ nomeCompleto }}</h2>
                    <button class="botao-sair" @click.prevent="sairDashboard()">Sair</button>
                </div>
            </div>             
            <div>
                <slot name="botao-slot"></slot>
            </div>           
            <div>                
                <div class="menu-toggle" @click="MobileMenu">
                    <button class="mobile-menu-btn">
                        <span class="material-icons">menu</span>
                    </button> 
                </div>                         
                <MenuPopup v-if="isMobile && showMobileMenu" @fecharPopup="fecharMenuPopup" />
            </div>
        </div>        
    </div>
</template>

<script>
import MenuPopup from '@/components/modals/MenuPopup.vue';
    export default {
        name: "MenuPrincipal",
        components: {
            MenuPopup,
        },
        data(){
            return {      
                isMobile: false,
                showMobileMenu: false,      
                linkCadastrar: '@/views/clientes/Index.vue',
                botaoNavega: 'Cadastrar',
                usuarioInfo: {  
                    foto_usuario: require("@/assets/img/usuario.jpg"),
                    alt_img:"foto_usuario",              
                    primeiroNome: 'Keila',
                    sobrenome: 'Barreto',
                }
            }       
        },
        methods: {
            MobileMenu() {
                this.showMobileMenu = !this.showMobileMenu;
            },
            fecharMenuPopup() {
                this.showMobileMenu = false;
            },
            checkIfMobile() {
                this.isMobile = window.innerWidth < 768; // Ajuste conforme necessário
                this.showMobileMenu = false; // Feche o menu ao redimensionar
            },
            sairDashboard(){
                this.$router.push({ name: 'login' });
            },
        },
        computed: {
            nomeCompleto(){
                return`${this.usuarioInfo.primeiroNome} ${this.usuarioInfo.sobrenome}`
            }
        },
        mounted() {
            window.addEventListener("resize", this.checkIfMobile);
            this.checkIfMobile();
        },
        beforeDestroy() {
            window.removeEventListener("resize", this.checkIfMobile);
        },
    }
</script>

<style scoped>
    .cabecalho {
        padding: 40px;
        margin-left: 250px;
        color: #26324B;
        display: flex;
        justify-content: space-between;               
    }
    .infos-cabecalho {
        width: 100%;
        padding-bottom: 30px;
        display: flex;
        justify-content: space-between;
        align-items: center;
        border-bottom: 1px solid #E5EBF1;         
    }
    .infos_usuario {
        width: 100%;
        display: flex;
        justify-content: start;
    } 
    .usuario_img img{
        margin-right: 20px;
        height: 70px;
        border-radius:10px;
    }
    .material-icons{
        color: #26324B;
    }

    .mobile-menu-btn {
        background: none;
        border: none;
        color: #fff;
        font-size: 24px;
        cursor: pointer;
    }
    .menu-toggle {
        display: none; 
        align-items: center;
        justify-content: center;
        cursor: pointer;
    }

    @media screen and (max-width: 900px) {
        .menu-toggle {
            display: block;
        }
    }
</style>