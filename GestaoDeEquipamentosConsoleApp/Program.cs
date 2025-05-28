using GestaoDeEquipamentosConsoleApp.Apresentacao;
using GestaoDeEquipamentosConsoleApp.Dados;

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
            TelaChamado telaChamado = new TelaChamado(repositorioChamado, repositorioEquipamento, telaEquipamento);
            TelaMenuPrincipal telaPrincipal = new TelaMenuPrincipal();

            while (true)
            {
                char telaEscolhida = telaPrincipal.ExibirMenuPrincipal();

                if (telaEscolhida == 'S') break;

                switch (telaEscolhida)
                {
                    case '1':
                        telaEquipamento.ExecutarMenu();
                        break;

                    case '2':
                        telaChamado.ExecutarMenu();
                        break;

                    case '3':
                        telaFabricante.ExecutarMenu();
                        break;

                    default:
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Opção inválida!");
                        Console.ResetColor();
                        break;
                }
            }
        }
    }
}