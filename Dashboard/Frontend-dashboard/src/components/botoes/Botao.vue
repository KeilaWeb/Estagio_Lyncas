<template>
    <div>
        <button type="submit" :class="classeBotao" >{{ nomeBotao }}</button>
    </div>
</template>
  
<script>
export default {
    name: 'BotaoBtn',
    props: {
        nomeBotao: {
            type: String,
            default: String,
        },
        classeBotao: {
            type: String,
            default: '',
        }
    }
}
</script>
  
