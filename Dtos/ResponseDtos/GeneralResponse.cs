namespace Shopping.Dtos.ResponseDtos
{
    public class GeneralResponse<Type> where Type : class
    {
        public Type Data { get; set; }
        public string Message { get; set; }
        public bool IsSuccess { get; set; }
        public Exception Error { get; set; }

    }
}
