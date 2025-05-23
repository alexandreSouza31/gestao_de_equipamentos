using GestaoDeEquipamentosConsoleApp.Apresentacao;
using GestaoDeEquipamentosConsoleApp.Dados;
using GestaoDeEquipamentosConsoleApp.Utils;

namespace GestaoDeEquipamentosConsoleApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            RepositorioEquipamento repositorioEquipamento = new RepositorioEquipamento();
            RepositorioFabricante repositorioFabricante = new RepositorioFabricante();
            RepositorioChamado repositorioChamado = new RepositorioChamado();

            TelaFabricante telaFabricante = new TelaFabricante(repositorioFabricante);
            TelaEquipamento telaEquipamento = new TelaEquipamento(repositorioEquipamento, repositorioFabricante, telaFabricante);
            TelaChamado telaChamado = new TelaChamado(repositorioChamado, repositorioEquipamento,telaEquipamento);
            TelaMenuPrincipal telaPrincipal = new TelaMenuPrincipal();

            while (true)
            {
                char telaEscolhida = telaPrincipal.ExibirMenuPrincipal();

                if (telaEscolhida == 'S') break;

                if (telaEscolhida == '1')
                {
                    while (true)
                    {
                        bool menuEquipamento= telaEquipamento.ExecutarMenuEquipamento(telaEquipamento);
                        if (menuEquipamento == false) break;
                        
                    }
                }
                else if (telaEscolhida == '2')
                {
                    while (true)
                    {
                        bool menuChamado = telaChamado.ExecutarMenuChamado(telaChamado);
                        if (menuChamado == false) break;
                    }
                }else if (telaEscolhida == '3')
                {
                    while (true)
                    {
                        bool menuFabricante = telaFabricante.ExecutarMenuFabricante(telaFabricante);
                        if (menuFabricante == false) break;
                    }
                }
            }
        }
    }
}