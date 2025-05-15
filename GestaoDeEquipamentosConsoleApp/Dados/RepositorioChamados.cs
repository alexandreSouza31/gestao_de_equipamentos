using GestaoDeEquipamentosConsoleApp.Negocio;

namespace GestaoDeEquipamentosConsoleApp.Dados
{
    public class RepositorioChamado
    {
        private Chamado[] chamados = new Chamado[100];

        private int contadorChamados = 0;
        public void CadastrarEquipamento(Chamado chamado)
        {
            chamados[contadorChamados] = chamado;
            contadorChamados++;
        }

        public Chamado[] SelecionarChamados()
        {
            return chamados;
        }

        public Chamado SelecionarChamadoPorId(int idSelecionado)
        {
            for (int i = 0; i < chamados.Length; i++)
            {
                Chamado c = chamados[i];

                if (c == null) continue;

                if (c.id == idSelecionado) return c;
            }

            return null;
        }

        public bool VerificarExistenciaChamados()
        {
            for (int i = 0; i < chamados.Length; i++)
            {
                if (chamados[i] != null) return true;
            }
            return false;
        }
    }
}