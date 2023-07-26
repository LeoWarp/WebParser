using WebParser.Domain.Enum;

namespace WebParser.Domain.Response;

public class BaseResponse<T> : IBaseResponse<T>
{
    public BaseResponse(string description, StatusCode statusCode, T data)
    {
        Description = description;
        StatusCode = statusCode;
        Data = data;
    }
    
    public BaseResponse(string description, StatusCode statusCode)
    {
        Description = description;
        StatusCode = statusCode;
    }
    
    public BaseResponse(T data, StatusCode statusCode)
    {
        StatusCode = statusCode;
        Data = data;
    }

    public BaseResponse() {  }
    
    public string Description { get; set; }        

    public StatusCode StatusCode { get; set; }
        
    public T Data { get; set; }
}

public interface IBaseResponse<T>
{
    string Description { get; }
    StatusCode StatusCode { get; }
    T Data { get; }
}