using System.Collections.Concurrent;
using Microsoft.AspNetCore.Mvc;

namespace TestVelvetechApi.Controllers;

[ApiController]
[Route("[controller]")]
public class BookController : ControllerBase
{
    private static readonly ConcurrentDictionary<int, string> BookIdName = new();
    private static int LastIndex => BookIdName.Keys.Count == 0 ? 0 : BookIdName.Keys.Max();
    private static readonly object LockObject = new();

    [HttpGet]
    public string Get(int id)
    {
        return BookIdName[id];
    }
    
    [HttpGet("AllBooks")]
    public IEnumerable<string> Get()
    {
        return BookIdName.Values;
    }

    [HttpPost("Create")]
    public int Create(string name)
    {
        lock (LockObject)
        {
            var last = LastIndex + 1;
            BookIdName[last] = name;
            return last;
        }
    }

    [HttpPost("Update")]
    public int Update(int id, string name)
    {
        BookIdName[id] = name;
        return id;
    }
    
    [HttpDelete]
    public string Delete(int id)
    {
        BookIdName.Remove(id, out var name);
        return name;
    }
}