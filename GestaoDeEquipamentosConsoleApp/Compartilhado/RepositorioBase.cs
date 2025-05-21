namespace GestaoDeEquipamentosConsoleApp.Compartilhado
{
    public class RepositorioBase<T> where T : IEntidade
    {
        private T[] registros = new T[100];
        private int contadorRegistros = 0;

        public void CadastrarRegistro(T novoRegistro)
        {
            if (contadorRegistros < registros.Length)
            {
                registros[contadorRegistros] = novoRegistro;
                contadorRegistros++;
            }
        }

        public T[] SelecionarRegistros()
        {
            return registros;
        }

        public T SelecionarRegistroPorId(int id)
        {
            for (int i = 0; i < contadorRegistros; i++)
            {
                if (registros[i] != null && registros[i].id == id)
                    return registros[i];
            }
            return default;
        }

        public bool VerificarExistenciaRegistros()
        {
            for (int i = 0; i < contadorRegistros; i++)
            {
                if (registros[i] != null)
                    return true;
            }
            return false;
        }
    }
}
