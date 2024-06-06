namespace HomeBankingMindHub.Models
{
    public class ResponseModel<T>: Response
    {
        public T Model { get; set; }
        public ResponseModel() {}
        public ResponseModel(int code, string msg, T model)
        {
            StatusCode = code;
            Message = msg;
            Model = model;
        }
    }
}
