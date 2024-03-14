<template>
  <div>
    <div class="flex-unido">
      <button class="botao-anterior" @click="paginaAnterior" :disabled="paginaAtual === 1"><span class="material-icons">keyboard_double_arrow_left</span></button>
      <span class="pagina-atual">{{ paginaAtual }}</span>
      <button class="botao-proximo" @click="proximaPagina" :disabled="paginaAtual === totalPaginas"> <span class="material-icons">keyboard_double_arrow_right</span></button>
    </div>
    <span>Pagina {{ paginaAtual }} de {{ totalPaginas }}</span>
  </div>
</template>

<script>
export default {
  props: {
    totalClientes: {
      type: Number,
      required: true
    },
    itensPorPagina: {
      type: Number,
      required: true
    },
    paginaAtual: {
      type: Number,
      required: true
    }
  },
  data() {
    return {
      totalPaginas: 0
    };
  },
  methods: {
    calcularTotalPaginas() {
      this.totalPaginas = Math.ceil(this.totalClientes / this.itensPorPagina);
      this.$emit('total-pagina');
    },
    paginaAnterior() {
      if (this.paginaAtual > 1) {
        this.$emit('atualizar-pagina', this.paginaAtual - 1);
      }
    },
    proximaPagina() {
      if (this.paginaAtual < this.totalPaginas) {
        this.$emit('atualizar-pagina', this.paginaAtual + 1);
      }
    }
  },
  watch: {
    totalClientes: 'calcularTotalPaginas',
    itensPorPagina: 'calcularTotalPaginas'
  },
  mounted() {
    this.calcularTotalPaginas();
  }
};
</script>

