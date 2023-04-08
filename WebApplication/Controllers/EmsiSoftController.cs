using Microsoft.AspNetCore.Mvc;
using Shared.Services.Contract;

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
            //Stopwatch stopwatch = new Stopwatch();
            //stopwatch.Start();

            int max = 40_000;
            int stringLength = 10;
            string charSet = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";

            Thread thread1 = new(() => Generate(2));
            Thread thread2 = new(() => Generate(2));

            thread1.Start();
            thread2.Start();

            void Generate(int numberOfThreads)
            {
                Stack<string> source = _stringGenerator.GenerateRandomString(max/numberOfThreads, stringLength, charSet);
                for (int x = 0; x <= max/numberOfThreads; x++)
                {
                    string hash = source.Pop();
                    _messageBroker.Publish(hash, "hashes", "hashes");
                }
            }

            //stopwatch.Stop();
            //Console.WriteLine("Elapsed Time is {0} ms", stopwatch.ElapsedMilliseconds);

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