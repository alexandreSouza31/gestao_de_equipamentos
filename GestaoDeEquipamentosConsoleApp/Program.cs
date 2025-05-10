using GestaoDeEquipamentosConsoleApp.Apresentacao;
using GestaoDeEquipamentosConsoleApp.Dados;
using GestaoDeEquipamentosConsoleApp.Utils;

namespace GestaoDeEquipamentosConsoleApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            RepositorioFabricante repositorioFabricante = new RepositorioFabricante();
            TelaFabricante telaFabricante = new TelaFabricante(repositorioFabricante);
            TelaEquipamento telaEquipamento = new TelaEquipamento(repositorioFabricante, telaFabricante);
            TelaChamado telaChamado = new TelaChamado();

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
                }else if (telaEscolhida == '3')
                {
                    while (true)
                    {
                        bool menuFabricante = telaFabricante.ExibirMenuFabricante(telaFabricante);
                        if (menuFabricante == false) break;
                    }
                }
            }
        }
    }
}