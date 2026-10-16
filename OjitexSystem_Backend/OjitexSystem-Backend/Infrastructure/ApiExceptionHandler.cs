using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace OjitexSystem_Backend.Infrastructure;

public sealed class ApiExceptionHandler(IProblemDetailsService problemDetailsService) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext context, Exception exception, CancellationToken cancellationToken)
    {
        var conflict = exception is DbUpdateException { InnerException: SqlException { Number: 2601 or 2627 } };
        var message = conflict ? "Dữ liệu đã tồn tại. Vui lòng tải lại và thử lại." : "Không thể xử lý yêu cầu. Vui lòng thử lại sau.";
        context.Response.StatusCode = conflict ? StatusCodes.Status409Conflict : StatusCodes.Status500InternalServerError;
        var details = new ProblemDetails
        {
            Status = context.Response.StatusCode,
            Title = message,
            Extensions = { ["message"] = message, ["traceId"] = context.TraceIdentifier }
        };
        if (!await problemDetailsService.TryWriteAsync(new ProblemDetailsContext
        {
            HttpContext = context,
            ProblemDetails = details
        }))
        {
            await context.Response.WriteAsJsonAsync(details, cancellationToken: cancellationToken);
        }
        return true;
    }
}
