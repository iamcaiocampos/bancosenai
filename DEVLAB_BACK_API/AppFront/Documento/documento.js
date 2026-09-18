
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
                    <button class="btn-editar" onclick="download(${a.id}, '${a.name}', '${a.extensao}')">Editar</button>
                    <button class="btn-excluir" onclick="excluirAgencia(${a.id}, '${a.name}', '${codigoCliente}')">Excluir</button>
                </td>
            </tr>`;
        });

    } else {
        const erro = await lista.text();
        alert("Erro: " + (erro) || "Falha ao enviar o documento.")
    }

    
}

async function download(identification, nome, extension) {

}

async function excluirAgencia(idee, nomee, codcliente) {

    if (confirm(`Deseja realmente excluir o arquivo ${nomee}?`)) {
        const response = await fetch(`${URL_API}/cliente/${codcliente}/excluir/${idee}`, { method: 'DELETE' });
        
    }
}
