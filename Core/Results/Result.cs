namespace ProjetaARQ.Core.Results
{
    public class Result
    {
        public ResultStatus Status { get; }
        public string Message { get; }

        public bool IsSuccess => Status == ResultStatus.Success;
        public bool IsFatal => Status == ResultStatus.FatalError;

        protected Result(ResultStatus status, string message)
        {
            Status = status;
            Message = message;
        }

        public static Result Success() => new Result(ResultStatus.Success, string.Empty); // 1. Sucesso Silencioso
        public static Result Success(string message) => new Result(ResultStatus.Success, message); // 2. Sucesso com TaskDialog automático
        public static Result Cancelled() => new Result(ResultStatus.Cancelled, string.Empty); // 3. Cancelamento Silencioso (O usuário clicou em Cancelar na UI)
        public static Result Warning(string warningMessage) => new Result(ResultStatus.Warning, warningMessage); // 4. Aviso de Negócio (Mostra TaskDialog amigável e sai sem tela vermelha)
        public static Result Fatal(string errorMessage) => new Result(ResultStatus.FatalError, errorMessage); // 5. Erro Fatal (Repassa pro Revit gritar)

    }

    public class Result<T> : Result
    {
        public T Value { get; }

        protected Result(T value, ResultStatus status, string message) : base(status, message)
        {
            Value = value;
        }

        public static Result<T> Success(T value) => new Result<T>(value, ResultStatus.Success, string.Empty);
        public static new Result<T> Cancelled() => new Result<T>(default, ResultStatus.Cancelled, string.Empty);
        public static new Result<T> Warning(string msg) => new Result<T>(default, ResultStatus.Warning, msg);
        public static new Result<T> Fatal(string err) => new Result<T>(default, ResultStatus.FatalError, err);
    }
}