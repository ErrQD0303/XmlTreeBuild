using XMLParser.Helpers;
using XMLParser.Models;

namespace backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class XMLController : ControllerBase
{
    private readonly ILogger<XMLController> _logger;

    public XMLController(ILogger<XMLController> logger)
    {
        _logger = logger;
    }

    [HttpPost]
    [Route("fileupload")]
    public async Task<IActionResult> UploadFile([FromForm] IFormFile file)
    {
        if (file is null || file.Length == 0)
        {
            return BadRequest("File is empty");
        }

        var uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/upload");

        if (!Directory.Exists(uploadPath))
        {
            Directory.CreateDirectory(uploadPath);
        }

        var filePath = Path.Combine(uploadPath, file.FileName);

        EfficientRead(file, filePath);

        return Ok("File uploaded successfully");
    }

    [HttpPost]
    [Route("PrologParse")]
    public async Task<IActionResult> ParseProlog([FromForm] IFormFile file)
    {
        return Ok(await XMLProlog.ParseAsync(await new StreamReader(file.OpenReadStream()).ReadToEndAsync()));
    }

    [HttpPost]
    [Route("XMLDeclarationParse")]
    public async Task<IActionResult> ParseXMLDeclaration([FromForm] IFormFile file)
    {
        return Ok(await XMLDeclaration.ParseAsync(await new StreamReader(file.OpenReadStream()).ReadToEndAsync()));
    }

    private static void EfficientRead(IFormFile readFile, string writeFilePath)
    {
        using var readStream = readFile.OpenReadStream();

        using var writeStream = new FileStream(writeFilePath, FileMode.Create, FileAccess.Write);

        int bufferSize = 1024;
        byte[] buffer = new byte[bufferSize];

        int bytesRead = readStream.Read(buffer, 0, bufferSize);
        while (bytesRead > 0)
        {
            writeStream.Write(buffer, 0, bytesRead);
            bytesRead = readStream.Read(buffer, 0, bufferSize);
        }
    }
}