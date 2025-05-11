using GestaoDeEquipamentosConsoleApp.Apresentacao;

namespace GestaoDeEquipamentosConsoleApp.Utils
{
    public class Direcionar
    {
        public ResultadoDirecionamento DirecionarParaMenu(bool haItens, bool menuPrincipal, string contexto)
        {
            if (haItens)
                return ResultadoDirecionamento.Continuar;

            Console.WriteLine($"\nNenhum {contexto} cadastrado ainda!");

            if (menuPrincipal)
            {
                Console.WriteLine("Voltando ao menu principal...");
                Thread.Sleep(3000);
                return ResultadoDirecionamento.VoltarMenuPrincipal;
            }
            else
            {
                Console.WriteLine($"Voltando ao menu de {contexto}s...");
                Thread.Sleep(3000);
                return ResultadoDirecionamento.VoltarMenuContexto;
            }
        }
    }
}
