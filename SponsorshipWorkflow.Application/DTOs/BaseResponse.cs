namespace SponsorshipWorkflow.Application.DTOs;

public class BaseResponse<T>
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
    public T? Data { get; set; }

    public static BaseResponse<T> SuccessResult(T data, string message = "Success")
    {
        return new BaseResponse<T>
        {
            Success = true,
            Message = message,
            Data = data
        };
    }

    public static BaseResponse<T> ErrorResult(string message)
    {
        return new BaseResponse<T>
        {
            Success = false,
            Message = message,
            Data = default
        };
    }
}

public class BaseResponse
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;

    public static BaseResponse SuccessResult(string message = "Success")
    {
        return new BaseResponse
        {
            Success = true,
            Message = message
        };
    }

    public static BaseResponse ErrorResult(string message)
    {
        return new BaseResponse
        {
            Success = false,
            Message = message
        };
    }
}