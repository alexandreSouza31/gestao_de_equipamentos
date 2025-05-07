using GestaoDeEquipamentosConsoleApp.Negocio;

namespace GestaoDeEquipamentosConsoleApp.Dados
{
    public class RepositorioEquipamento
    {
        public Equipamento[] equipamentos = new Equipamento[100];

        public int contadorEquipamentos = 0;
        public void CadastrarEquipamento(Equipamento equipamento)
        {
            equipamentos[contadorEquipamentos] = equipamento;
            contadorEquipamentos++;
        }

        public Equipamento[] SelecionarEquipamentos()
        {
            return equipamentos;
        }

        public Equipamento SelecionarEquipamentoPorId(int idSelecionado)
        {
            for (int i = 0; i < equipamentos.Length; i++)
            {
                Equipamento e = equipamentos[i];

                if (e == null)
                    continue;

                if (e.id == idSelecionado)
                    return e;
            }

            return null;
        }
    }
}