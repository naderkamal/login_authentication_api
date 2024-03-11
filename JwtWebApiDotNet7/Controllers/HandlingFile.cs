using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Upload_File.Models;

namespace Upload_File.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HandlingFile : ControllerBase
    {
        private dbContext _dbContext;
        public HandlingFile(dbContext dbContext)
        {
            _dbContext = dbContext;
        }
        [HttpPost("upload")]
        public async Task<IActionResult> UploadFile(IFormFile file)
        {
            try
            {
                if (file == null || file.Length == 0)
                {
                    return BadRequest("File is empty.");
                }

                using (MemoryStream memoryStream = new MemoryStream())
                {
                    await file.CopyToAsync(memoryStream);
                    var fileBytes = memoryStream.ToArray();


                    return Ok("File uploaded successfully.");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("getfile/{id}")]
        public async Task<IActionResult> GetFile(int id)
        {
            try
            {
                var fileEntity = await _dbContext.Files.FindAsync(id);

                if (fileEntity == null)
                {
                    return NotFound("File not found.");
                }

                return File(fileEntity.Content, "application/octet-stream", fileEntity.FileName);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
