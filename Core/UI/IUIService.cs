namespace ProjetaARQ.Core.UI
{
    public interface IUIService<TResult> where TResult : class
    {
        TResult ShowDialog();
    }
}
