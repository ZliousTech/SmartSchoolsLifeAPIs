using SmartSchool.Logger.Service;
using SmartSchoolAPI.Entities;
using System;
using System.Net;
using System.Reflection;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Results;

namespace SmartSchoolAPI.BaseService
{
    public static class Helper_Execution
    {
        private static ILoggerService _loggerService { get; } = new LoggerService();

        public static async Task<IHttpActionResult> ExecuteAsync<T>(this ApiController controller, Func<Task<BaseResponseDTO<T>>> action)
            where T : class, new()
        {
            try
            {
                var response = await action();

                if (!response.IsSuccess)
                {
                    return new NegotiatedContentResult<BaseResponseDTO<T>>(HttpStatusCode.BadRequest, response, controller);
                }

                return new OkNegotiatedContentResult<BaseResponseDTO<T>>(response, controller);
            }
            catch (Exception ex)
            {
                _loggerService.Error(ex.ToString(), MethodBase.GetCurrentMethod().Name);
                return new NegotiatedContentResult<BaseResponseDTO<T>>(HttpStatusCode.InternalServerError,
                                                           new BaseResponseDTO<T>
                                                           {
                                                               IsSuccess = false,
                                                               Data = new T(),
                                                               Message = "Please re-try again, if the error persist please contact Administrator."
                                                           }, controller);
            }
        }
    }
}