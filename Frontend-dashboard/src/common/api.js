import axios from 'axios';
import {URLbase} from '@/common/config.js'

const ApiService = {
    init(){},

    setHeader(){
        this.setBaseURL();
        axios.defaults.headers.common["Content-Language"] = "pt-BR";
    },

    setBaseURL(){
        axios.defaults.baseURL = URLbase;
    },

     /**
     * Send the GET HTTP request
     * @param resource
     * @param params
     * @returns {*}
     */
    buscar(resource, params){
        this.setHeader();
        return axios.get(`${resource}`, {params});
    },

    /**
   * Send the GET HTTP request
   * @param resource
   * @param params
   * @param config
   * @returns {*}
   */
    criar(resource, params, config = null){
        this.setHeader();
        return axios.post(`${resource}`, params, config);
    },

    atualizar(resource, params){
        this.setHeader();
        return axios.put(`${resource}`, params);
    },

    delete(resource){
        this.setHeader();
        return axios.delete(resource);
    }
}
export default ApiService;
