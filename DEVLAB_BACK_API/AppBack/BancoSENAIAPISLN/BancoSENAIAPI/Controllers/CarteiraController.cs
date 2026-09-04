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
            var carteira = _carteira.FirstOrDefault(a => a.NumeroCarteira == codigo);

            if (carteira == null)
                return NotFound(new { message = "Carteira não encontrada." }); // Status 404 [6, 7]

            return Ok(carteira); // Status 200 OK [6, 7]
        }

        [HttpPut("{codigo}")]
        public IActionResult Alterar(int codigo, [FromBody] Carteira carteiraAtualizada)
        {
            var carteiraExistente = _carteira.FirstOrDefault(a => a.NumeroCarteira == codigo);

            if (carteiraAtualizada == null) return NotFound();

            carteiraExistente.ApetiteCarteira = carteiraAtualizada.ApetiteCarteira;
            carteiraExistente.NomeCarteira = carteiraAtualizada.NomeCarteira;

            // Retorna Status 204 No Content para atualizações bem-sucedidas [6, 9]
            return NoContent();
        }

        [HttpDelete("{codigo}")]
        public IActionResult Excluir(int codigo)
        {
            var carteira = _carteira.FirstOrDefault(a => a.NumeroCarteira == codigo);

            if (carteira == null) return NotFound();

            _carteira.Remove(carteira);
            return Ok(new { message = "Carteira excluída com sucesso." }); // Status 200 [6]
        }
    }
}
