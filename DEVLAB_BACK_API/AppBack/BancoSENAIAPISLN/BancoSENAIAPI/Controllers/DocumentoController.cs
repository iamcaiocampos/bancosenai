using BancoSENAIAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace BancoSENAIAPI.Controllers
    
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class DocumentoController : Controller
    {
        private readonly string _caminhoraiz = Path.Combine(Directory.GetCurrentDirectory() , "ClienteArquivos");

        private static List<Models.documentometadados> _documentometadados = new List<Models.documentometadados>();

        private static int _nextId = 1;

        [HttpPost("upload/{codigoCliente}")]
        public async Task<IActionResult> AnexarArquivo(int codigoCliente, IFormFile arquivo)
        {
            if (arquivo == null || arquivo.Length == 0)
            {
                return BadRequest("Nenhum arquivo foi enviado");
            }

            long tamanhoembytes = arquivo.Length;

            long tamanhomax = 2 * 1024 * 1024;

            if(tamanhoembytes > tamanhomax)
            {
                return BadRequest("Arquivo muito grande! coloque um arquivo menor que 2mb");
            }
            

            string pastaCliente = Path.Combine(_caminhoraiz, codigoCliente.ToString());

            if (!Directory.Exists(pastaCliente))
            {
                Directory.CreateDirectory(pastaCliente);
            }

            string extensao = Path.GetExtension(arquivo.FileName);
            string nomeOriginal = Path.GetFileName(arquivo.FileName);
            string novoNome = $"{codigoCliente}_{nomeOriginal}_{Guid.NewGuid()}{extensao}";
            string caminhoFinal = Path.Combine(pastaCliente, novoNome);

            if (extensao != ".pdf" && extensao != ".jpg" && extensao != ".png")
            {

                return BadRequest("Tipo de arquivo não aceito! use .pdf .jpg ou .png");
            }

            using (var stream = new FileStream(caminhoFinal, FileMode.Create))
            {
                await arquivo.CopyToAsync(stream);
            }

            var documentometadados = new Models.documentometadados()
            {
                Id = _nextId++,
                Name = nomeOriginal,
                Extensao = extensao,
                Caminho = caminhoFinal,
                CodigoCliente = codigoCliente,
            };

            _documentometadados.Add(documentometadados);

            return Ok(new { mensagem = "documento anexado com sucesso", arquivoSalvo = novoNome });

            
        }

        [HttpGet("listagem/{codigoCliente}")]
        public async Task<IActionResult> ListarDocumentos([FromRoute] int codigoCliente)
        {
            var documentos = _documentometadados.Where(d => d.CodigoCliente == codigoCliente).ToList();

            if (!documentos.Any())
            {
                return NotFound("Nenhum documento encontrado para este cliente.");
            }

            return Ok(documentos);
        }
        [HttpGet("cliente/{codigoCliente}/download/{id}")]
        public async Task<IActionResult> DownloadArquivo(
         [FromRoute] int codigoCliente, [FromRoute] int id)
        {
            var documento = _documentometadados
                .FirstOrDefault(d => d.Id == id && d.CodigoCliente == codigoCliente);

            if (documento == null)
            {
                return NotFound("Arquivo não encontrado ou você não tem acesso a ele.");
            }

            if (!System.IO.File.Exists(documento.Caminho))
            {
                return NotFound("O arquivo não foi encontrado no servidor.");
            }

            var arquivo = await System.IO.File.ReadAllBytesAsync(documento.Caminho);

            return File(arquivo, "application/octet-stream", documento.Name);
        }

        [HttpDelete("cliente/{codigoCliente}/excluir/{id}")]
        public async Task<IActionResult> ExcluirDocumento(
        [FromRoute] int codigoCliente,
        [FromRoute] int id)
        {
            var documento = _documentometadados
                .FirstOrDefault(d => d.Id == id && d.CodigoCliente == codigoCliente);

            if (documento == null)
            {
                return NotFound("Não encontramos o arquivo ou você não tem acesso.");
            }

            _documentometadados.Remove(documento);

            if (System.IO.File.Exists(documento.Caminho))
            {
                System.IO.File.Delete(documento.Caminho);
            }

            return NoContent();
        }


    }
}
