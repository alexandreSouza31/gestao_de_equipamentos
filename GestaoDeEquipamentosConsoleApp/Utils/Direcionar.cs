using GestaoDeEquipamentosConsoleApp.Apresentacao;

namespace GestaoDeEquipamentosConsoleApp.Utils
{
    public class Direcionar
    {
        public bool DirecionarParaMenu(bool haEquipamentos, bool menuPrincipal, string contexto)
        {
            TelaMenu telaPrincipal = new TelaMenu();

            string mensagem = $"\nNenhum {contexto} cadastrado ainda!";

            if (!haEquipamentos)
            {
                if(menuPrincipal)
                {
                    Console.WriteLine(mensagem);
                    Console.WriteLine("Voltando ao menu principal...");
                    Thread.Sleep(4000);

                    telaPrincipal.ExibirMenuPrincipal();
                    return false;
                }
                else
                {
                    Console.WriteLine(mensagem);
                    Console.WriteLine($"Voltando ao menu de {contexto}...");
                    Thread.Sleep(4000);
                    return false;
                }
            }
            return true;
        }
    }
}
