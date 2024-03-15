import ApiService from '@/common/api'

let apiBasePath = 'clientes'

const clienteService = {
    async listar(search) {
        let { data } = await ApiService.buscar(`${apiBasePath}?search=${search}`)
        return data
    },

    async criarCliente(form) {
        let { data } = await ApiService.criar(`${apiBasePath}`, form)
        return data
    },

    async enviarPaginado(form) {
        let { data } = await ApiService.criar(`${apiBasePath}/busca`, form)
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

    async excluir(idCliente) {
        try{
            return await ApiService.delete(`${apiBasePath}/${idCliente}`);
        }catch (error){
            console.log("!Erro ao exluir cliente:", error)
        }
    },   
}

export default clienteService; 