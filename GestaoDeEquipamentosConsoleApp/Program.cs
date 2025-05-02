using System.Runtime.Intrinsics.X86;

namespace GestaoDeEquipamentosConsoleApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            TelaEquipamento telaEquipamento = new TelaEquipamento();
            while (true)
            {
                char opcaoEscolhida = telaEquipamento.ApresentarMenu();

                if (opcaoEscolhida == 's')
                {
                    break;
                }

                switch(opcaoEscolhida)
                {
                    case '1':
                        telaEquipamento.Cadastrar();
                        break;
                }
            }
        }
    }

    public class TelaEquipamento
    {
        public char ApresentarMenu()
        {
            Console.Clear();
            Console.WriteLine("----- Gestão de Equipamentos -----");
            Console.WriteLine(); Console.WriteLine("1 - Cadastrar Equipamento");
            Console.WriteLine("S - Sair");
            Console.Write("\nDigite uma opçpão: ");
            char opcaoEscolhida= Convert.ToChar(Console.ReadLine()!.ToUpper()[0]);
            Console.Write("Digite [Enter] para continuar ");
            Console.ReadLine();

            return opcaoEscolhida;
        }

        public void Cadastrar()
        {
            Equipamento equipamento = new Equipamento();

            Console.Clear();
            Console.WriteLine("----- Gestão de Equipamentos -----");
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("-- Cadastrar Equipamento --");
            Console.WriteLine();
            equipamento.id = equipamento.id++;

            Console.Write("Nome: ");
            equipamento.nome = Console.ReadLine()!;
            Console.Write("Preço de Aquisicao: ");
            equipamento.precoAquisicao = Convert.ToDecimal(Console.ReadLine());
            Console.Write("Numero de Série: ");
            equipamento.numeroSerie = Console.ReadLine()!;
            Console.Write("Data de Fabricação ex:[05/08/2022]: ");
            equipamento.dataFabricacao = DateTime.Parse(Console.ReadLine()!);
            Console.Write("Fabricante: ");
            equipamento.fabricante = Console.ReadLine()!;

            Console.WriteLine($"nome: {equipamento.nome} cadastrado com sucesso! id: {equipamento.id}")
            Console.ReadLine();

        }
    }

    public class Equipamento
    {
        static int numeroId = 1;
        public int id = numeroId++;
        public string nome;
        public decimal precoAquisicao;
        public string numeroSerie;
        public DateTime dataFabricacao;
        public string fabricante;
    }
}
