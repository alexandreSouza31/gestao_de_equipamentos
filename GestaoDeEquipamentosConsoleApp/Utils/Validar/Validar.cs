namespace GestaoDeEquipamentosConsoleApp.Utils
{
    public class Validar
    {
        public static bool ValidarQtdCaracteres(string nome)
        {
            int numeroMinimoCaracteres = 6;

            if (nome.Length < numeroMinimoCaracteres)
            {
                Console.WriteLine($"Nome deve ter mais de {numeroMinimoCaracteres} caracteres!");
                Console.Write("Digite [Enter] para continuar ");
                Console.ReadLine();
                return true;
            }

            return false;
        }
    }
}