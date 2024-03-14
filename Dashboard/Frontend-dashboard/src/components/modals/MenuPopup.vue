<template>
    <Transition name="menu-fade">
        <div class="menu">        
            <div class="menu-container">
                <div class="btnFechar">
                    <button @click="fecharPopup"> X </button>
                </div>                
                <div class="logo_topo">
                    <router-link class="logo" to="/">
                        <img src='@/assets/img/Logo.png' alt='Logo Lyncas'>
                    </router-link>
                </div>
                <div>
                    <router-link class="link_menu" :to="{name: 'dashboard'}">
                        <span class="material-icons">grid_view</span> Dashboard
                    </router-link>
                </div>
                <div>
                    <router-link class="link_menu" :to="{name: 'clientes'}">
                        <span class="material-icons">groups</span>Cliente
                    </router-link>
                </div>
                <div>
                    <router-link class="link_menu" :to="{name: 'vendas'}">
                        <span class="material-icons">shopping_cart</span>Vendas
                    </router-link>
                </div>
            </div>
        </div>
    </Transition>
</template>  

<script>
    export default {
        name: 'MenuPopup',
        data() {
            return {
                showMenu: false,
            };
        },
        methods: {
            fecharPopup() {
                this.$emit("fecharPopup");
            },
        }
    }
</script>
<style scoped> 
    .material-icons {
    font-size: 18px;
    margin-right: 7px;
    }
    .menu {
        z-index: 2;
        position: absolute;
        top: 0px; right: 0px;
    }    
    .logo img{
        z-index: 1;
        max-width: 100%;
        padding: 40px 20px;
    }
      
    .menu-container {
        padding: 0 30px;
        position: fixed;
        margin-top: 40px;
    }
    .btnFechar {
        margin-bottom: 80px;
        display: flex;
        align-items: right;
        justify-content: right;
    }
    .menu-fade-enter-active {
        transition: opacity 0.9s ease;
    }
    .menu-fade-leave-active{
        transition: opacity .9s ease-out;
    }
    .menu-fade-enter, .menu-fade-leave-to {
        opacity: 0;
    }
    .menu {
        animation: fadeInOut 1s ease;
    }
    @keyframes fadeInOut {
        0% {
            opacity: 0;
            transform: translateX(90px);
        }
        50% {
            opacity: 0.5;
            transform: translateX(0);
        }
    }
</style>
