using System.Text.RegularExpressions;
using Microsoft.Extensions.Logging;
using WebParser.Domain.Dto;
using WebParser.Domain.Enum;
using WebParser.Domain.Response;
using WebParser.DomainServices.Interfaces.Url;

namespace WebParser.DomainServices.Implementation.Url;

public class UrlService : IUrlService
{
    private readonly ILogger<UrlService> _logger;
    
    private static readonly Random Random = new Random();
    private const string PatternRegex = @"(http[s]?:\/\/)?([^\/\s]+\/)(.*)";
    private readonly Regex _pathRegex = new Regex(PatternRegex, RegexOptions.None,TimeSpan.FromMilliseconds(1));

    public UrlService(ILogger<UrlService> logger)
    {
        _logger = logger;
    }

    public IBaseResponse<UrlDto> GenerateShortUrl(string url)
    {
        try
        {
            var randomString = RandomString(5);
            if (_pathRegex.IsMatch(url))
            {
                var uri = new Uri(url);
                var aLeftPart = uri.GetLeftPart(UriPartial.Authority);

                var shortPath = $"{aLeftPart}/{randomString}";
                var urlDto = new UrlDto()
                {
                    OriginalPath = url,
                    ShortPath = shortPath
                };
                return new BaseResponse<UrlDto>()
                {
                    StatusCode = StatusCode.OK,
                    Data = urlDto
                };
            }
            return new BaseResponse<UrlDto>()
            {
                Description = "Укажите корректный url",
                StatusCode = StatusCode.IncorrectUrl
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"[UrlService:GenerateShortUrl] - {ex.Message}");
            return new BaseResponse<UrlDto>()
            {
                StatusCode = StatusCode.InternalServerError,
                Description = "Внутренняя ошибка"
            };
        }
    }

    private static string RandomString(int length)
    {
        var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
        return new string(Enumerable.Repeat(chars, length)
            .Select(s => s[Random.Next(s.Length)]).ToArray());
    }
}