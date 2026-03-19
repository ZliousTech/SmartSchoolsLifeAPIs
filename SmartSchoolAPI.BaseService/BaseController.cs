using SmartSchool.Logger.Service;
using SmartSchoolAPI.Entities;
using System;
using System.Net;
using System.Reflection;
using System.Threading.Tasks;
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
                _loggerService.Error(ex.ToString(), MethodBase.GetCurrentMethod().Name);
                return Content(HttpStatusCode.InternalServerError, new BaseResponseDTO<T>
                {
                    IsSuccess = false,
                    Data = new T(),
                    Message = "Please re-try again, if the error persist please contact Administrator."
                });
            }
        }
    }
}