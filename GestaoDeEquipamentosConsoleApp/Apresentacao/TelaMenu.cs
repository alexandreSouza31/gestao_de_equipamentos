namespace GestaoDeEquipamentosConsoleApp.Apresentacao
{
    public class TelaMenu
    {
        public char ExibirMenuPrincipal()
        {
            string nomeSolucao = "Gestão de Equipamentos";
            Console.Clear();
            Console.WriteLine($"----- {nomeSolucao} -----");
            Console.WriteLine();

            Console.WriteLine("1 - Equipamentos");
            Console.WriteLine("2 - Chamados");
            Console.WriteLine("3 - Fabricantes");
            Console.WriteLine("S - Sair");
            Console.Write("\nDigite uma opção: ");
            char telaEscolhida = Convert.ToChar(Console.ReadLine()!.ToUpper());

            return telaEscolhida;
        }
    }
}
