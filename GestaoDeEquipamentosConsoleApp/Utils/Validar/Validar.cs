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
                return true;
            }

            return false;
        }
    }
}