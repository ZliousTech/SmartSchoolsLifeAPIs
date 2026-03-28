using SmartSchool.Logger.Service;
using SmartSchoolAPI.Entities;
using System;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace SmartSchoolAPI.BaseService
{
    public class BaseController : ApiController
    {
        private ILoggerService _loggerService { get; } = new LoggerService();

        public async Task<IHttpActionResult> ExecuteAsync<T>(Func<Task<BaseResponseDTO<T>>> action)
            where T : class, new()
        {
            try
            {
                var response = await action();

                if (!response.IsSuccess)
                {
                    return Content(HttpStatusCode.BadRequest, response);
                }

                return Ok(response);
            }
            catch (Exception ex)
            {
                var methodName = $"{ex.TargetSite?.Name}.{ex.TargetSite?.DeclaringType?.FullName}";
                var requestInfo = await BuildRequestInfoAsync();

                _loggerService.Error($"Exception: {ex.ToString()}\n{requestInfo}", methodName);

                return Content(HttpStatusCode.InternalServerError, new BaseResponseDTO<T>
                {
                    IsSuccess = false,
                    Data = new T(),
                    Message = "Please re-try again, if the error persist please contact Administrator."
                });
            }
        }

        private async Task<string> BuildRequestInfoAsync()
        {
            var loggerBulider = new StringBuilder();

            loggerBulider.AppendLine($"URL       : {Request.RequestUri}");
            loggerBulider.AppendLine($"Method    : {Request.Method}");
            loggerBulider.AppendLine($"Query     : {Request.RequestUri.Query}");

            // Parse query string params into key-value pairs
            var queryParams = HttpUtility.ParseQueryString(Request.RequestUri.Query);
            foreach (string key in queryParams)
            {
                loggerBulider.AppendLine($"  [{key}] = {queryParams[key]}");
            }

            // Log request body (POST/PUT params)
            if (Request.Content != null)
            {
                var body = await Request.Content.ReadAsStringAsync();
                if (!string.IsNullOrWhiteSpace(body))
                {
                    loggerBulider.AppendLine($"Body      : {body}");
                }
            }

            return loggerBulider.ToString();
        }
    }
}