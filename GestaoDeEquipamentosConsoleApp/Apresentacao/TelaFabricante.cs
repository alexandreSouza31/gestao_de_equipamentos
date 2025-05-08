using GestaoDeEquipamentosConsoleApp.Utils;
using GestaoDeEquipamentosConsoleApp.Dados;

namespace GestaoDeEquipamentosConsoleApp.Apresentacao
{
    public class TelaFabricante
    {
        private RepositorioFabricante repositorioFabricante;

        public TelaFabricante(RepositorioFabricante repositorio)
        {
            this.repositorioFabricante = repositorio;
        }

        public bool Visualizar()
        {
            Console.Clear();
            Console.WriteLine("----- Fabricantes Registrados -----");

            var todos = repositorioFabricante.ObterTodos();
            if (todos.Length == 0)
            {
                Console.WriteLine("Nenhum fabricante registrado.");
                Console.WriteLine();
                Console.Write("Digite [Enter] para continuar...");
                Console.ReadLine();
                return false;
            }
            else
            {
                for (int i = 0; i < todos.Length; i++)
                {
                    Console.WriteLine($"{i + 1}. {todos[i]}");
                }
            }

            Console.WriteLine();
            Console.Write("Digite [Enter] para continuar...");
            Console.ReadLine();
            return false;
        }
    }
}
