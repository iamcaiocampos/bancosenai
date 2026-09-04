using BancoSENAIAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace BancoSENAIAPI.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class AgenciaCliente : ControllerBase
    {
        private static List<Cliente> _clientes = new List<Cliente>
        {
            new Cliente { CodigoCliente = 1, NomeCliente = "Felipe Gustavo", CPF = "123.456.789-00", NumeroAgencia = 10, SaldoTotal = 0, Sexo = "M", Endereco = "Rua A, 123", Cidade = "Aracaju", Estado = "SE" },
            new Cliente { CodigoCliente = 2, NomeCliente = "Paulo Eduardo", CPF = "987.654.321-11", NumeroAgencia = 10, SaldoTotal = 0, Sexo = "F", Endereco = "Av. B, 456", Cidade = "São Paulo", Estado = "SP" },
            new Cliente { CodigoCliente = 3, NomeCliente = "Lucas vinícius", CPF = "456.789.123-22", NumeroAgencia = 10, SaldoTotal = 0, Sexo = "M", Endereco = "Rua C, 789", Cidade = "Salvador", Estado = "BA" }
        };

        [HttpGet]
        public IActionResult ListarTodas()
        {
            return Ok(_clientes);
        }

        [HttpPost]
        public IActionResult Cadastrar([FromBody] Cliente novoCliente)
        {

            if (_clientes.Any(a => a.CodigoCliente == novoCliente.CodigoCliente))
                return BadRequest(new { message = "Este número de agência já existe." });

            _clientes.Add(novoCliente);
            // Retorna Status 201 Created conforme boas práticas REST [6, 8]
            return Created("", novoCliente);
        }

        [HttpGet("{codigo}")]
        public IActionResult ConsultarPorCodigo(int codigo)
        {
            var cliente = _clientes.FirstOrDefault(a => a.CodigoCliente == codigo);

            if (cliente == null)
                return NotFound(new { message = "Cliente não encontrado." }); // Status 404 [6, 7]

            return Ok(cliente); // Status 200 OK [6, 7]
        }

        [HttpPut("{codigo}")]
        public IActionResult Alterar(int codigo, [FromBody] Cliente clienteAtualizado)
        {
            var clienteExistente = _clientes.FirstOrDefault(a => a.CodigoCliente == codigo);

            if (clienteExistente == null) return NotFound();

            clienteExistente.NomeCliente = clienteAtualizado.NomeCliente;
            clienteExistente.CPF = clienteAtualizado.CPF;
            clienteExistente.NumeroAgencia = clienteAtualizado.NumeroAgencia;
            clienteExistente.SaldoTotal = clienteAtualizado.SaldoTotal;
            clienteExistente.Sexo = clienteAtualizado.Sexo;
            clienteExistente.Endereco = clienteAtualizado.Endereco;
            clienteExistente.Cidade = clienteAtualizado.Cidade;
            clienteExistente.Estado = clienteAtualizado.Estado;

            // Retorna Status 204 No Content para atualizações bem-sucedidas [6, 9]
            return NoContent();
        }

        [HttpDelete("{codigo}")]
        public IActionResult Excluir(int codigo)
        {
            var cliente = _clientes.FirstOrDefault(a => a.CodigoCliente == codigo);

            if (cliente == null) return NotFound();

            _clientes.Remove(cliente);
            return Ok(new { message = "Agência excluída com sucesso." }); // Status 200 [6]
        }
    }
}
