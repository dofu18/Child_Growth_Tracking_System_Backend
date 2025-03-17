using ApplicationLayer.DTOs.Supabase;
using ApplicationLayer.Middlewares;
using ApplicationLayer.Service;
using Microsoft.AspNetCore.Mvc;

namespace ControllerLayer.Controllers
{
    [ApiController]
    [Route("api/v1/")]
    public class FileUploadController : ControllerBase
    {
        private readonly ILogger<FileUploadController> _logger;
        private readonly ISupabaseStorageService _storageService;

        public FileUploadController(ISupabaseStorageService storageService, ILogger<FileUploadController> logger)
        {
            _storageService = storageService;
            _logger = logger;
        }

        [Protected]
        [HttpPost("upload")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> UploadFile([FromForm] UploadFileRequest request)
        {
            _logger.LogInformation("upload file");
            var url = await _storageService.UploadFileAsync(request.File);
            return Ok(new { url });
        }
    }
}
