using GestaoDeEquipamentosConsoleApp.Negocio;

namespace GestaoDeEquipamentosConsoleApp.Dados
{
    public class RepositorioFabricante
    {
        private Fabricante[] fabricantes = new Fabricante[100];
        private int contadorFabricantes = 0;

        public void CadastrarFabricante(Fabricante fabricante)
        {
            if (contadorFabricantes < fabricantes.Length)
            {
                fabricantes[contadorFabricantes] = fabricante;
                contadorFabricantes++;
            }
        }

        public Fabricante[] SelecionarFabricantes()
        {
            return fabricantes;
        }

        public Fabricante SelecionarFabricantePorId(int id)
        {
            for (int i = 0; i < fabricantes.Length; i++)
            {
                Fabricante f = fabricantes[i];

                if (f == null) continue;

                if (f.id == id) return f;
            }
            return null;
        }

        public bool VerificarExistenciaFabricantes()
        {
            for (int i = 0; i < fabricantes.Length; i++)
            {
                if (fabricantes[i] != null) return true;
            }
            return false;
        }
    }
}