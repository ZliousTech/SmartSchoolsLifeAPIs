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
                var requestInfo = await BuildRequestInfoAsync();

                _loggerService.Error($"Exception: {ex.ToString()}\n{requestInfo}", methodName);

                return Content(HttpStatusCode.InternalServerError, new BaseResponseDTO<T>
                {
                    IsSuccess = false,
                    Data = default(T),
                    Message = "Please re-try again, if the error persist please contact Administrator."
                });
            }
        }

        private async Task<string> BuildRequestInfoAsync()
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
                    // This is the key: Load the content into a memory buffer 
                    // so it can be read again even if the controller already read it.
                    await Request.Content.LoadIntoBufferAsync();

                    var body = await Request.Content.ReadAsStringAsync();

                    if (!string.IsNullOrWhiteSpace(body))
                    {
                        loggerBulider.AppendLine($"Body      : {body}");
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