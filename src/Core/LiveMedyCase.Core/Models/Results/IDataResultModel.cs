namespace LiveMedyCase.Core.Models.Results
{
    public interface IDataResultModel<T> : IDataResultModel
    {
        T? Data { get; set; }
    }

    public interface IDataResultModel
    {
        bool Success { get; }
        string Message { get; }
    }

}
