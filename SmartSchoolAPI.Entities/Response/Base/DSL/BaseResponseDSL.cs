namespace SmartSchoolAPI.Entities
{
    public static class BaseResponseDSL<T> where T : class, new()
    {
        public static BaseResponseDTO<T>
            CreateGenericResponse(bool isSucess, T date, string message)
        {
            return new BaseResponseDTO<T>
            {
                IsSuccess = isSucess,
                Data = date ?? new T(),
                Message = message
            };
        }
    }
}