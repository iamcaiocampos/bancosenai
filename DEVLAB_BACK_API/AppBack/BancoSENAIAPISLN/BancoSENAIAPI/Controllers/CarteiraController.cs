using BancoSENAIAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace BancoSENAIAPI.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class CarteiraController : ControllerBase
    {
        private static List<Carteira> _carteira = new List<Carteira>
        {
            new Carteira { NumeroCarteira = 1, NomeCarteira = "Alpha", ApetiteCarteira = 1000000 },
            new Carteira { NumeroCarteira = 2, NomeCarteira = "Foxtrot", ApetiteCarteira = 1000000 },
            new Carteira { NumeroCarteira = 3, NomeCarteira = "Amber", ApetiteCarteira = 1000000 }
        };

        [HttpGet]
        public IActionResult ListarTodas()
        {
            return Ok(_carteira);
        }

        [HttpPost]
        public IActionResult Cadastrar([FromBody] Carteira novaCarteira)
        {

            if (_carteira.Any(a => a.NumeroCarteira == novaCarteira.NumeroCarteira))
                return BadRequest(new { message = "Este número de carteira já existe." });

            _carteira.Add(novaCarteira);
            // Retorna Status 201 Created conforme boas práticas REST [6, 8]
            return Created("", novaCarteira);
        }

        [HttpGet("{codigo}")]
        public IActionResult ConsultarPorCodigo(int codigo)
        {
            var agencia = _carteira.FirstOrDefault(a => a.NumeroAgencia == codigo);

            if (agencia == null)
                return NotFound(new { message = "Agência não encontrada." }); // Status 404 [6, 7]

            return Ok(agencia); // Status 200 OK [6, 7]
        }

        [HttpPut("{codigo}")]
        public IActionResult Alterar(int codigo, [FromBody] Agencia agenciaAtualizada)
        {
            var agenciaExistente = _carteira.FirstOrDefault(a => a.NumeroAgencia == codigo);

            if (agenciaExistente == null) return NotFound();

            agenciaExistente.Cidade = agenciaAtualizada.Cidade;
            agenciaExistente.SiglaEstado = agenciaAtualizada.SiglaEstado;

            // Retorna Status 204 No Content para atualizações bem-sucedidas [6, 9]
            return NoContent();
        }

        [HttpDelete("{codigo}")]
        public IActionResult Excluir(int codigo)
        {
            var agencia = _carteira.FirstOrDefault(a => a.NumeroAgencia == codigo);

            if (agencia == null) return NotFound();

            _carteira.Remove(agencia);
            return Ok(new { message = "Agência excluída com sucesso." }); // Status 200 [6]
        }
    }
}
