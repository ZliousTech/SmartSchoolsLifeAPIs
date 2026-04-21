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

        public async Task<IHttpActionResult> ExecuteAsync<T>(Func<Task<BaseResponseDTO<T>>> action,
                                                             HttpStatusCode successStatusCode = HttpStatusCode.OK)
            where T : class
        {
            try
            {
                var response = await action();

                if (!response.IsSuccess)
                {
                    return Content(HttpStatusCode.BadRequest, response);
                }

                return Content(successStatusCode, response);
            }
            catch (Exception ex)
            {
                var methodName = ActionContext?.ActionDescriptor?.ActionName ?? "UnknownAction";
                var requestInfo = BuildRequestInfoAsync();

                _loggerService.Error($"Exception: {ex}{Environment.NewLine}requestInfo: {requestInfo}", methodName);

                return Content(HttpStatusCode.InternalServerError, new BaseResponseDTO<T>
                {
                    IsSuccess = false,
                    Data = default(T),
                    Message = "Please re-try again, if the error persist please contact Administrator."
                });
            }
        }

        private string BuildRequestInfoAsync()
        {
            var loggerBulider = new StringBuilder();

            loggerBulider.AppendLine($"URL       : {Request.RequestUri}");
            loggerBulider.AppendLine($"Method    : {Request.Method}");

            if (!string.IsNullOrWhiteSpace(Request.RequestUri.Query))
            {
                loggerBulider.AppendLine($"Query     : {Request.RequestUri.Query}");
            }

            // Parse query string params into key-value pairs
            var queryParams = HttpUtility.ParseQueryString(Request.RequestUri.Query);
            foreach (string key in queryParams)
            {
                loggerBulider.AppendLine($"  [{key}] = {queryParams[key]}");
            }

            // Log request body (POST/PUT params)
            if (Request.Content != null)
            {
                try
                {
                    if (Request.Properties.TryGetValue("RequestBodyBuffer", out var cachedBody) &&
                        cachedBody is string bodyStr && !string.IsNullOrWhiteSpace(bodyStr))
                    {
                        loggerBulider.AppendLine($"Body      : {bodyStr}");
                    }
                }
                catch (Exception)
                {
                    loggerBulider.AppendLine("Body      : [Could not read body - stream likely closed]");
                }
            }

            return loggerBulider.ToString();
        }
    }
}