namespace SmartSchoolAPI.Entities
{
    public class BaseResponseDTO<T> where T : class
    {
        public bool IsSuccess { get; set; }

        public T Data { get; set; }

        public string Message { get; set; }

        public BaseResponseDTO()
        {
        }

        public BaseResponseDTO(bool isSucess, T data, string message)
        {
            IsSuccess = isSucess;
            Data = data;
            Message = message;
        }
    }
}