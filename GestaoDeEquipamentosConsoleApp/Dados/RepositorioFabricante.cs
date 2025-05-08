using GestaoDeEquipamentosConsoleApp.Negocio;

public class RepositorioFabricante
{
    private string[] fabricantes = new string[100];
    private int contador = 0;

    public void RegistrarFabricante(string nome)
    {
        // Evita duplicatas
        for (int i = 0; i < contador; i++)
        {
            if (fabricantes[i] == nome)
                return;
        }

        fabricantes[contador++] = nome;
    }

    public string[] ObterTodos()
    {
        return fabricantes.Take(contador).ToArray();
    }

    public bool ExisteFabricante(string nome)
    {
        return fabricantes.Take(contador).Contains(nome);
    }
}
