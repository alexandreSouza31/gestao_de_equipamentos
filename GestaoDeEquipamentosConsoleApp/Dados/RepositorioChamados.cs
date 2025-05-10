using GestaoDeEquipamentosConsoleApp.Negocio;

namespace GestaoDeEquipamentosConsoleApp.Dados
{
    public class RepositorioChamado
    {
        public Chamado[] chamados = new Chamado[100];

        public int contadorChamados = 0;
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
    }
}