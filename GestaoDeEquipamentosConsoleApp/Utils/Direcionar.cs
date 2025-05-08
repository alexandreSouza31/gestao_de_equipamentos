using GestaoDeEquipamentosConsoleApp.Apresentacao;

namespace GestaoDeEquipamentosConsoleApp.Utils
{
    public class Direcionar
    {
        public bool DirecionarParaMenu(bool haEquipamentos, bool menuPrincipal)
        {
            if (!haEquipamentos)
            {
                if (menuPrincipal == true)
                {
                    Console.WriteLine("\nNenhum equipamento cadastrado. Cadastre um equipamento antes de abrir um chamado.");
                    Console.WriteLine("Voltando ao menu principal...");
                    Thread.Sleep(7000);

                    TelaChamado tela = new TelaChamado();
                    tela.ExibirMenuPrincipal();
                    return false;
                }
                else
                {
                    Console.WriteLine("\nNenhum chamado cadastrado!");
                    Console.WriteLine("Voltando ao menu de chamados...");
                    Thread.Sleep(4000);

                    TelaChamado tela = new TelaChamado();
                    tela.ExibirMenuPrincipal();
                    return false;
                }
            }
            return true;
        }
    }
}
