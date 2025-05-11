namespace GestaoDeEquipamentosConsoleApp.Utils
{
    public class DesejaExcluir
    {
        public string DesejaMesmoExcluir(string item)
        {
            string opcaoExcluir = "";

            do
            {
                Console.Write($"Tem certeza que deseja excluir {item}? (S/N): ");
                opcaoExcluir = Console.ReadLine()!.Trim().ToUpper();
            } while (opcaoExcluir != "S" && opcaoExcluir != "N");

            return opcaoExcluir;
        }
    }
}
