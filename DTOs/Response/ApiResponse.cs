namespace SchoolManagement.DTOs.Response;

/// <summary>
/// Standard API response wrapper for all endpoints
/// </summary>
public class ApiResponse<T>
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
    public T? Data { get; set; }

    public static ApiResponse<T> SuccessResponse(T? data, string message = "Success")
    {
        return new ApiResponse<T>
        {
            Success = true,
            Message = message,
            Data = data
        };
    }

    public static ApiResponse<T> ErrorResponse(string message)
    {
        return new ApiResponse<T>
        {
            Success = false,
            Message = message,
            Data = default
        };
    }
}

/// <summary>
/// Paginated response wrapper
/// </summary>
public class PagedResponse<T>
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
    public T? Data { get; set; }
    public int Total { get; set; }
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public int TotalPages { get; set; }

    public static PagedResponse<T> SuccessResponse(T? data, int total, int pageNumber, int pageSize, string message = "Success")
    {
        var totalPages = (int)Math.Ceiling(total / (double)pageSize);
        return new PagedResponse<T>
        {
            Success = true,
            Message = message,
            Data = data,
            Total = total,
            PageNumber = pageNumber,
            PageSize = pageSize,
            TotalPages = totalPages
        };
    }

    public static PagedResponse<T> ErrorResponse(string message)
    {
        return new PagedResponse<T>
        {
            Success = false,
            Message = message,
            Data = default
        };
    }
}
