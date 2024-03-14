//seleção dos elementos (atraves do DOM)
const preverOperacao = document.querySelector("#prever-resultado"); // Visor do cálculo de previsão(superior)
const digitandoOperacao = document.querySelector("#calculando"); // Visor do cálculo sendo digitado(inferior)
const botoes = document.querySelectorAll("#gradeBotao button"); // Todos os botões da calculadora


//POO JS - Lógica da calcularora
class Calculado {
    constructor(preverOperacao, digitandoOperacao){ //valores de fluxo da operação
        this.preverOperacao = preverOperacao; //Propriedades do objeto para não trabalhar diretamente com o DOM
        this.digitandoOperacao = digitandoOperacao;
        this.digitando = ""; //Valor que o usuário está digitando
    }

   
    adicionaDigito(digito){//Verifica se o último caractere no visor é um ponto e se já existe um ponto (função da lógica do IF foreach)
        if (digito === "." && this.digitandoOperacao.innerText.includes(".")) {
            return;
        }
        this.digitando = digito; // adiciona o dígito ao visor
        this.atualizaTela()  //Atualiza o visor        
    }

    //processa as operações (função da logica do ELSE de botoes.foreach)
    processaOperacao(operacao){//Checar se o valor no visor é vazio (empty) e a operação não é "C"
        if(digitandoOperacao.innerText === "" && operacao !== "C"){//Mudança de operação
            if(this.preverOperacao.innerText !== ""){
                this.mudaOperacao(operacao);
            }
            return;
        }        

        //Pegando os valores atuais e os recorrente
        let valorDaOperacao
        const prever = +this.preverOperacao.innerText.split(" ")[0]; //metodo split(para a string do vetor ser ordenado para uma substring em um array e retorna array, soma na previsão)
        const recorrente = +this.digitandoOperacao.innerText //valor passado anterior para valor numerico

        switch(operacao){ 
            case "+":
                valorDaOperacao = prever + recorrente;
                this.atualizaTela(valorDaOperacao, operacao, recorrente, prever);
                break
            case "-":
                valorDaOperacao = prever - recorrente;
                this.atualizaTela(valorDaOperacao, operacao, recorrente, prever);
                break
            case "/":
                valorDaOperacao = prever / recorrente;
                this.atualizaTela(valorDaOperacao, operacao, recorrente, prever);
                break
            case "*":
                valorDaOperacao = prever * recorrente;
                this.atualizaTela(valorDaOperacao, operacao, recorrente, prever);
                break
            case "DEL":
                this.limpaUmDEL();
                break
            case "CE":
                this.limpaOperacaoRecorrenteCE();
                break
            case "C":
                this.limpaTodaOperacaoC();
                break
            case "=":
                this.resultadoDaOperacao();
                break
            default: //retorno para operação que não é válida
                return
        }
    }        

    atualizaTela(valorDaOperacao = null, operacao = null, recorrente = null, prever = null){ //metodo para ter acesso ao this.digitandoOperacao
        if (valorDaOperacao === null) {
            this.digitandoOperacao.innerText += this.digitando //concatena os valores
        }else { //checar se o valor é zero, se não adiciona o valor la pra cima          
            if(prever === 0){
                valorDaOperacao = recorrente;
            }//Jogando o valor da parte de baixo para cima            
            this.preverOperacao.innerText = `${valorDaOperacao} ${operacao}`; //aparece o valor em cima
            this.digitandoOperacao.innerText = ""; //limpa o valor debaixo no visor
        }
        this.resultadoInfinty()
    }

    resultadoInfinty(){
        if (preverOperacao.innerText === "Infinity /" || preverOperacao.innerText == 'NaN') {
            alert("[ERRO!] Adicione uma operação válida!")
            this.preverOperacao.innerText = "";
        }
    }

    mudaOperacao(operacao){
        const operacaoMatematica = ["*", "-", "+", "/"];
        if (!operacaoMatematica.includes(operacao)) {
            return;
        }//substitui o ultimo operador
        this.preverOperacao.innerText = this.preverOperacao.innerText.slice(0, -1) + operacao
    }

    limpaUmDEL() {
        this.digitandoOperacao.innerText = this.digitandoOperacao.innerText.slice(0, -1)
    }

    limpaOperacaoRecorrenteCE(){
        this.digitandoOperacao.innerText = ""
    }

    //Limpa a operação de cima e a de baixo
    limpaTodaOperacaoC(){
        this.preverOperacao.innerText = ""
        this.digitandoOperacao.innerText = ""
    }

    // Resultado da Operação
    resultadoDaOperacao(){        
        const operacao = preverOperacao.innerText.split(" ")[1]
        this.processaOperacao(operacao)
    }
}

const calculadora = new Calculado(preverOperacao, digitandoOperacao) //instancia para executar os métoodos de entrada da lógica (if e else)

//Iterando sobre todos os botões e adicionando um evento de clique a cada um
botoes.forEach((botao) => {
    botao.addEventListener("click", (e) => { //Quando um botão é clicado, este bloco de código é executado
        const value = e.target.innerText; //Obtendo o texto do botão clicado
        //Verificando se o valor é um número ou um ponto
        if (+value /*converte valor em numero*/ >= 0 || value === ".") { //// Se for um número ou um ponto, chame o método para adicionar o dígito à calculadora
            calculadora.adicionaDigito(value)
        }else { // Se não for um número, é reconhecido como uma operação, e chamamos o método correspondente
            calculadora.processaOperacao(value)
        }
    })
})

