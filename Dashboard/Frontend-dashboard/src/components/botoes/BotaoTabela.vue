<template>
    <div >
        <a href="#" 
            :class="classeDeletar" 
            @click="deletarCliente(index)">
            {{ botaoDeletar }}
        </a>
        <a href="#" 
            :class="classeEditar" 
            @click="editarCliente" >
            {{ botaoEditar }}
        </a>
    </div>
</template>

<script>
export default {
    name: 'BotaoTabela',
    props: {
        botaoDeletar: {
            type: String,
            default: 'Deletar',
        },
        botaoEditar: {
            type: String,
            default: 'Editar',
        },
        classeDeletar: {
            type: String,
            default: '',
        },
        botaclasseEditar:{
            type: String,
            default: '',
        }
    },
    data() {
        return{
            classeDeletar: 'delete-cliente',
            classeEditar: 'editar-cliente',
        }
    },
    methods: {
        deletarCliente(index){
            console.log(index)
        }
    }
}
</script>