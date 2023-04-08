using Microsoft.AspNetCore.Mvc;
using Shared.Models;
using Shared.Services.Contract;
using System.Security.Cryptography;
using System.Text;

namespace WebApi.Controllers;

[ApiController]
[Route("[controller]")]
public class EmsiSoftController : ControllerBase
{
    private readonly IHashesRepository _hashesRepository;
    private readonly IMessageBroker _messageBroker;
    private readonly IStringGenerator _stringGenerator;

    public EmsiSoftController(IHashesRepository hashesRepository, IMessageBroker messageBroker, IStringGenerator stringGenerator)
    {
        _hashesRepository = hashesRepository;
        _messageBroker = messageBroker;
        _stringGenerator = stringGenerator;
    }

    [HttpPost("hashes")]
    public IActionResult Post()
    {
        try
        {
            for (int x = 0; x <= 40_000; x++)
            {
                string source = _stringGenerator.GenerateRandomString(10, "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789");
                using (SHA1 sha1Hash = SHA1.Create())
                {
                    byte[] sourceBytes = Encoding.UTF8.GetBytes(source);
                    byte[] hashBytes = sha1Hash.ComputeHash(sourceBytes);
                    string hash = BitConverter.ToString(hashBytes).Replace("-", string.Empty);

                    _messageBroker.Publish(hash, "hashes", "hashes");
                }
            }

            return Ok();
        }
        catch (Exception ex)
        {
            return BadRequest(ex);
        }
    }

    [HttpGet("hashes")]
    public async Task<IActionResult> GetAsync()
    {
        try
        {
            return Ok(await _hashesRepository.GetHashesAsync());
        }
        catch (Exception ex)
        {
            return BadRequest(ex);
        }
    }
}