using WebParser.Domain.Dto;
using WebParser.Domain.Response;

namespace WebParser.DomainServices.Interfaces.Url;

public interface IUrlService
{
    IBaseResponse<UrlDto> GenerateShortUrl(string url);
}