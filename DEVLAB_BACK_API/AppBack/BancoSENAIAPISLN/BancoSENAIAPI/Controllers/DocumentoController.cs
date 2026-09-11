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
    }
}
