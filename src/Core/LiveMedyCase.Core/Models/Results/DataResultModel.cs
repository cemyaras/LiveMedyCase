namespace LiveMedyCase.Core.Models.Results
{
    public class DataResultModel<T> : DataResultModel, IDataResultModel<T>
    {
        public DataResultModel(T? data, bool success, string message) : base(success, message)
        {
            Data = data;
        }

        public DataResultModel(T? data) : this(data, true, string.Empty)
        {
        }

        public DataResultModel(bool success, string message) : this(default, success, message)
        {
        }

        public T? Data { get; set; }
    }

    public class DataResultModel : IDataResultModel
    {
        public DataResultModel(bool success, string message) : this(success)
        {
            Message = message;
        }

        public DataResultModel(bool success)
        {
            Success = success;
            Message = string.Empty;
        }
        public bool Success { get; set; }
        public string Message { get; set; }
    }
}
