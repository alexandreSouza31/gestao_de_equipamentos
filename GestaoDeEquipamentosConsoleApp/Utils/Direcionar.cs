using GestaoDeEquipamentosConsoleApp.Apresentacao;

namespace GestaoDeEquipamentosConsoleApp.Utils
{
    public class Direcionar
    {
        public bool DirecionarParaMenu(bool haEquipamentos, bool menuPrincipal, string contexto)
        {
            TelaChamado tela = new TelaChamado();

            string mensagem = $"\nNenhum {contexto} cadastrado ainda!";

            if (!haEquipamentos)
            {
                if(menuPrincipal)
                {
                    Console.WriteLine(mensagem);
                    Console.WriteLine("Voltando ao menu principal...");
                    Thread.Sleep(4000);

                    tela.ExibirMenuPrincipal();
                    //Console.ReadLine();
                    return false;
                }
                else
                {
                    Console.WriteLine(mensagem);
                    Console.WriteLine("Voltando ao menu de chamados...");
                    Thread.Sleep(4000);
                    //Console.ReadLine();
                    return false;
                }
            }
            return true;
        }
    }
}
