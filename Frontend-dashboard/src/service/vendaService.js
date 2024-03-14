import ApiService from '@/common/api'

let apiBasePath = 'vendas'

const vendaService = {
    async listar(search) {
        let { data } = await ApiService.buscar(`${apiBasePath}?search=${search}`)
        return data
    },

    async criarVenda(form) {
        let { data } = await ApiService.criar(`${apiBasePath}`, form)
        return data
    },

    async buscarPorId(id) {
        let { data } = await ApiService.buscar(`${apiBasePath}/${id}`);
        return data;
    },

    async editar(id, form) {
        let { data } = await ApiService.atualizar(`${apiBasePath}/${id}`, form)
        return data
    },

    async excluir(idVenda) {
        try {
            return await ApiService.delete(`${apiBasePath}/${idVenda}`);
        } catch (error){
            console.log("!Erro ao exluir venda:", error)
        }
    },
   
}

export default vendaService; 