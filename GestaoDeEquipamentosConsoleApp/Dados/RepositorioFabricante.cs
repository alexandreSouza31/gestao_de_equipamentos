using GestaoDeEquipamentosConsoleApp.Negocio;

namespace GestaoDeEquipamentosConsoleApp.Dados
{
    public class RepositorioFabricante
    {
        private Fabricante[] fabricantes = new Fabricante[100];
        private int contador = 0;
        private int proximoId = 1;

        public void Inserir(Fabricante fabricante)
        {
            if (contador < fabricantes.Length)
            {
                fabricantes[contador] = fabricante;
                contador++;
            }
        }

        public Fabricante[] ObterTodos()
        {
            Fabricante[] fabricantesAtivos = new Fabricante[contador];
            for (int i = 0; i < contador; i++)
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
