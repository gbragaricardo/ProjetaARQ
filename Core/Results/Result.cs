namespace ProjetaARQ.Core.Results
{
    public class Result
    {
        public bool IsSuccess { get; }
        public string Message { get; }
        public bool ShowMessage { get; }
        public bool IsFailure => !IsSuccess;

        protected Result(bool isSuccess, string message, bool showMessage)
        {
            IsSuccess = isSuccess;
            Message = message;
            ShowMessage = showMessage;
        }

        public static Result Success(bool showMessage, string message = "") => new Result(true, message, showMessage);
        public static Result Failure(bool showMessage, string message) => new Result(false, message, showMessage);
    }

    public class Result<T> : Result
    {
        public T Value { get; }

        protected Result(T value, bool isSuccess, string message) : base(isSuccess, message, false)
        {
            Value = value;
        }

        public static Result<T> Success(T value) => new Result<T>(value, true, string.Empty);
        public static new Result<T> Failure(string error) => new Result<T>(default, false, error);
    }
}
