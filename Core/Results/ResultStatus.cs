namespace ProjetaARQ.Core.Results
{
    public enum ResultStatus
    {
        Success,       // Operação concluída
        Cancelled,     // O usuário abortou ativamente (fechou a janela)
        Warning,       // Falha de regra de negócio (ex: não há portas na fase) -> Vai gerar um TaskDialog amigável
        FatalError     // Falha de infraestrutura (ex: banco caiu) -> Vai gerar a tela vermelha do Revit
    }
}