namespace GestaoDeEquipamentosConsoleApp.Compartilhado;

public abstract class EntidadeBase
{
    public static int numeroId = 1;

    public abstract void AtualizarRegistro(EntidadeBase registroAtualizado);
    public abstract string Validar();

}
