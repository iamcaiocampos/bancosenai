
const URL_API = 'http://localhost:5139/api/v1/Documento'
async function enviarDocumento() {
    const codigoCliente = document.getElementById("CodigoCliente").value;
    const inputarquivo = document.getElementById("arquivo");
    const arquivo = inputarquivo.files[0];

    if (!codigoCliente || !arquivo) {
        alert("Informe o código do cliente e selecione um arquivo.")
        return;
    }

    const dadosArquivo = new FormData();
    dadosArquivo.append("arquivo", arquivo);

    const resposta = await fetch(`${URL_API}/upload/${codigoCliente}`, {
        method: "POST",
        body: dadosArquivo
    });

    if (resposta.ok) {
        alert("Documento enviado com sucesso!");
        document.getElementById("codigoCliente").value = "";
        document.getElementById("arquivo").value = "";
    } else {
        const erro = await response.json();
        alert("Erro: " + (erro.message) || "Falha ao enviar o documento.")
    }
}

async function ListarDocumentos() {
    const codigoCliente = document.getElementById("CodigoCliente").value;

    if (!codigoCliente) {
        alert("Informe o código do cliente")
        return;
    }

    const lista = await fetch(`${URL_API}/listagem/${codigoCliente}`, {
        method: "GET",
        
    });

    if (lista.ok) {
        alert("Listado com sucesso!");
        const corpo = document.getElementById("corpoTabela");
        corpo.innerHTML = '';
        const listalistavel = await lista.json();

        listalistavel.forEach(a => {
            corpo.innerHTML += `
            <tr>
                <td>${a.id}</td>
                <td>${a.name}</td>
                <td>${a.extensao}</td>
                <td>
                    <button class="btn-download" onclick="download(${a.id}, '${a.name}', '${codigoCliente}')">Download</button>
                    <button class="btn-excluir" onclick="excluirAgencia(${a.id}, '${a.name}', '${codigoCliente}')">Excluir</button>
                </td>
            </tr>`;
        });

    } else {
        const erro = await lista.text();
        alert("Erro: " + (erro) || "Falha ao enviar o documento.")
    }

    
}

function download(identification, nome, codcliente) {
    window.location.href =
        `${URL_API}/cliente/${codcliente}/download/${identification}`;
}

//^^ esse código funciona bem fácil, pelo o que eu entendi, ele diz para o seu navegador navegar até o
//`${URL_API}/cliente/${codcliente}/download/${identification}`;
//e o seu navegador faz uma request HTTP para o backend
//o back end responde e o browser sabe que é para fazer o download
//bem legal e simples em?
//mas não tem como pegar o objeto resposta, então se der algum 4xx não tem como fazer uma mensagem de erro
// então é uma faca de dois gomes, gumes? sei lá :P

async function excluirAgencia(idee, nomee, codcliente) {

    if (confirm(`Deseja realmente excluir o arquivo ${nomee}?`)) {
        const response = await fetch(`${URL_API}/cliente/${codcliente}/excluir/${idee}`, { method: 'DELETE' });
        
    }
}
