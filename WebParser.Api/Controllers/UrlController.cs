using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using WebParser.Domain.Dto;
using WebParser.Domain.Extensions;
using WebParser.DomainServices.Interfaces.Url;

namespace WebParser.Api.Controllers;

[ApiController]
public class UrlController : Controller
{
    private readonly IUrlService _urlService;
    private readonly IDistributedCache _distributedCache;

    public UrlController(IDistributedCache distributedCache, IUrlService urlService)
    {
        _distributedCache = distributedCache;
        _urlService = urlService;
    }

    [HttpPost]
    [Route("generate-short-url")]
    public IActionResult GenerateShortUrl(string url)
    {
        var response = _urlService.GenerateShortUrl(url);
        if (response.StatusCode == Domain.Enum.StatusCode.OK)
        {
            _distributedCache.SetObject($"ShortUrl_{response.Data.ShortPath}", response.Data, new DistributedCacheEntryOptions()
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(2)
            });
            return Ok(response);
        }
        return BadRequest(response);
    }
    
    [HttpGet]
    [Route("get-original-url")]
    public IActionResult GetOriginalUrl(string shortUrl)
    {
        var originalUrl = _distributedCache.GetObject<UrlDto>($"ShortUrl_{shortUrl}");
        if (originalUrl == null)
        {
            return BadRequest($"Оригинальный Url не найден по короткому: {shortUrl}");
        }
        return RedirectPermanent(originalUrl.OriginalPath);
    }
}