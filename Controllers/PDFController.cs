
using Microsoft.AspNetCore.Mvc;
using PFDLOCFINDER.Models;
using PFDLOCFINDER.Services;

namespace PFDLOCFINDER.Controllers
{
    [ApiController]
    [Route("api/")]
    public class PDFController : ControllerBase
    {
        private readonly IPDFService _pdfService;

        public PDFController(IPDFService pdfService)
        {
            _pdfService = pdfService;
        }

        [HttpPost("getPDFCoordinate")]
        public ActionResult<Response<DocumentLocPageDto>> getPDFCoordinate(IFormFile file, string textFind)
        {
            Response<DocumentLocPageDto> response = _pdfService.getPDFLOC(file, textFind);

            if (response.response_message == "SUCCESS" && response.response_code == "200" && response.http_response == 00)
            {
                return Ok(response);
            }
            else
            {
                return NotFound(response);
            }
        }
    }
}