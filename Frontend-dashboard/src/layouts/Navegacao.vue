<template>
    <div class="menu">
        <div class="logo_topo">
            <router-link class="logo" to="/">
                <img :src="logo" :alt="alt">
            </router-link>
        </div>
        <div class="menu-container">
            <div>
                <router-link class="link_menu" :to="{name: 'dashboard'}">
                    <span class="material-icons">grid_view</span> Dashboard
                </router-link>
            </div>
            <div>
                <router-link class="link_menu" :to="{name: 'clientes'}">
                    <span class="material-icons">groups</span>Cliente</router-link>
            </div>
            <div>
                <router-link class="link_menu" :to="{name: 'vendas'}">
                    <span class="material-icons">shopping_cart</span>Vendas</router-link>
            </div>
        </div>
    </div>
</template>  

<script>
export default {
    props: {
        logo: String,
        alt: String,
    },
}
</script>
<style scoped>  
    .material-icons {
        font-size: 18px;
        margin-right: 7px;
    }
    .logo img{
        z-index: 1;
        max-width: 100%;
        padding: 40px;
        display: block;
    }
      
    .menu-container {
        padding: 0 20px;
        position: fixed;
        margin-top: 300px;
    }
    @media screen and (max-width: 900px) {
        .menu{
            display: block;
            display: none; 
        }
    }
</style>
