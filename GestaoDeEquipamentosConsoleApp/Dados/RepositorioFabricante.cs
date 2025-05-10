using GestaoDeEquipamentosConsoleApp.Negocio;

namespace GestaoDeEquipamentosConsoleApp.Dados
{
    public class RepositorioFabricante
    {
        private Fabricante[] fabricantes = new Fabricante[100];
        public int contadorFabricantes = 0;
        private int proximoId = 1;

        public void Inserir(Fabricante fabricante)
        {
            if (contadorFabricantes < fabricantes.Length)
            {
                fabricantes[contadorFabricantes] = fabricante;
                contadorFabricantes++;
            }
        }

        public Fabricante[] ObterTodos()
        {
            Fabricante[] fabricantesAtivos = new Fabricante[contadorFabricantes];
            for (int i = 0; i < contadorFabricantes; i++)
            {
                fabricantesAtivos[i] = fabricantes[i];
            }
            return fabricantesAtivos;
        }

        public Fabricante BuscarPorId(int id)
        {
            foreach (Fabricante f in fabricantes)
            {
                if (f != null && f.id == id)
                    return f;
            }
            return null;
        }
    }
}
