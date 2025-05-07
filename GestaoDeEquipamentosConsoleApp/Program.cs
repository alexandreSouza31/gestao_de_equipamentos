using GestaoDeEquipamentosConsoleApp.Apresentacao;
using GestaoDeEquipamentosConsoleApp.Utils;

namespace GestaoDeEquipamentosConsoleApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            TelaEquipamento telaEquipamento = new TelaEquipamento();
            TelaChamado telaChamado = new TelaChamado();

            TelaChamado.repositorioEquipamento = telaEquipamento.repositorioEquipamento;

            while (true)
            {
                char telaEscolhida = telaChamado.ExibirMenuPrincipal();

                if (telaEscolhida == 'S') break;

                if (telaEscolhida == '1')
                {
                    while (true)
                    {
                        bool menuEquipamento= telaEquipamento.ExibirMenuEquipamento(telaEquipamento);
                        if (menuEquipamento == false) break;
                        
                    }
                }
                else if (telaEscolhida == '2')
                {
                    while (true)
                    {
                        bool menuChamado = telaChamado.ExibirMenuChamado(telaChamado);
                        if (menuChamado == false) break;
                    }
                }
            }
        }
    }
}