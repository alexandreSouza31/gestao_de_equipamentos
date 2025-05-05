//using System.Runtime.Intrinsics.X86;
using GestaoDeEquipamentosConsoleApp.Utils;
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

                if (opcaoEscolhida == 'S')
                {
                    break;
                }

                switch (opcaoEscolhida)
                {
                    case '1':
                        telaEquipamento.Cadastrar();
                        break;
                    case '2':
                        telaEquipamento.Visualizar(true);
                        break;
                    case '3':
                        telaEquipamento.Editar();
                        break;
                    case '4':
                        telaEquipamento.Excluir();
                        break;
                    default:
                        Console.WriteLine("Digite uma opção válida!");
                        break;
                }
            }
        }
    }

    #region apresentação
    public class TelaEquipamento
    {
        public RepositorioEquipamento repositorioEquipamnto = new RepositorioEquipamento();
        public char ApresentarMenu()
        {
            ExibirCabecalho();
            Console.WriteLine();
            Console.WriteLine("1 - Cadastrar Equipamento");
            Console.WriteLine("2 - Visualizar Equipamentos");
            Console.WriteLine("3 - Editar Equipamento");
            Console.WriteLine("4 - Excluir Equipamento");
            Console.WriteLine("S - Sair");
            Console.Write("\nDigite uma opção: ");
            char opcaoEscolhida = Convert.ToChar(Console.ReadLine()!.ToUpper()[0]);
            DigitarEnterEContinuar.Executar(false);

            return opcaoEscolhida;
        }

        public void Cadastrar()
        {
            Equipamento equipamento = new Equipamento();

            ExibirCabecalho();
            Console.WriteLine("-- Cadastrar Equipamento --");
            Console.WriteLine();
            equipamento.id = equipamento.id++;

            while (true)
            {
                Console.Write("Nome: ");
                equipamento.nome = Console.ReadLine()!;

                bool validarNome = Validar.ValidarQtdCaracteres(equipamento.nome);

                if (validarNome == true) continue;
                else break;
            }
            Console.Write("Preço de Aquisicao: ");
            equipamento.precoAquisicao = Convert.ToDecimal(Console.ReadLine());
            Console.Write("Numero de Série: ");
            equipamento.numeroSerie = Console.ReadLine()!;
            Console.Write("Data de Fabricação ex:[05/08/2022]: ");
            equipamento.dataFabricacao = DateTime.Parse(Console.ReadLine()!);
            Console.Write("Fabricante: ");
            equipamento.fabricante = Console.ReadLine()!;

            repositorioEquipamnto.CadastrarEquipamento(equipamento);
            Console.WriteLine($"nome: {equipamento.nome} cadastrado com sucesso! id: {equipamento.id}");
            DigitarEnterEContinuar.Executar();
        }

        public bool Visualizar(bool exibirCabecalho)
        {
            if (exibirCabecalho)
                ExibirCabecalho();

            Console.WriteLine("-- Visualizar Equipamentos --");
            Console.WriteLine();

            Equipamento[] equipamentos = repositorioEquipamnto.SelecionarEquipamentos();
            int encontrados = 0;

            string tamanhoCabecalhoColunas = "{0, -5} | {1, -30} | {2, -15} | {3, -15} | {4, -15} | {5, -10}";

            for (int i = 0; i < equipamentos.Length; i++)
            {
                Equipamento e = equipamentos[i];
                if (e == null) continue;

                if (encontrados == 0)
                {
                    Console.WriteLine(
                        tamanhoCabecalhoColunas,
                        "Id".ToUpper(), "Nome".ToUpper(), "Preço Aquisicao".ToUpper(), "Numero Série".ToUpper(), "Data Fabricação".ToUpper(), "Fabricante".ToUpper()
                    );
                }

                Console.WriteLine(
                    tamanhoCabecalhoColunas,
                    e.id, e.nome, e.precoAquisicao.ToString("C2"), e.numeroSerie, e.dataFabricacao.ToShortDateString(), e.fabricante
                );

                encontrados++;
            }

            if (encontrados == 0) Console.WriteLine("Ainda não há equipamentos! Faça um cadastro!");

            DigitarEnterEContinuar.Executar();
            return encontrados > 0;
        }


        public void Editar()
        {
            ExibirCabecalho();
            Console.WriteLine("-- Editar Equipamento --");
            Console.WriteLine();

            bool visualizarCadastrados = Visualizar(false);
            if (visualizarCadastrados == false) return;

            Equipamento[] equipamentos = repositorioEquipamnto.equipamentos;

            while (true)
            {
                Console.WriteLine();
                Console.Write("Digite o Id do equipamento para editar: ");
                int idEscolhido = Convert.ToInt32(Console.ReadLine()!);
                
                Equipamento equipamentoSelecionado = null;

                for (int i = 0; i < equipamentos.Length; i++)
                {
                    Equipamento e = equipamentos[i];
                    if (e == null) continue;

                    if (idEscolhido == e.id)
                    {
                        equipamentoSelecionado = e;
                        break;
                    }
                }

                if (equipamentoSelecionado != null)
                {
                    ObterDados(equipamentoSelecionado);
                    Console.WriteLine();
                    Console.WriteLine($"nome: {equipamentoSelecionado.nome} editado com sucesso! id: {equipamentoSelecionado.id}");
                    DigitarEnterEContinuar.Executar();
                    return;
                }
                else
                {
                    Console.WriteLine();
                    Console.WriteLine("ID inválido. Tente novamente.");
                }
            }
        }

        private static void ObterDados(Equipamento e)
        {
            while (true)
            {
                Console.Write($"Nome ({e.nome}):");
                e.nome = Console.ReadLine()!;

                bool validarNome = Validar.ValidarQtdCaracteres(e.nome);

                if (validarNome == true) continue;
                else break;
            }
            Console.Write($"Preço de Aquisicao ({e.precoAquisicao}):");
            e.precoAquisicao = Convert.ToDecimal(Console.ReadLine());
            Console.Write($"Numero de Série ({e.numeroSerie}):");
            e.numeroSerie = Console.ReadLine()!;
            Console.Write($"Data de Fabricação ex:05/08/2022 ({e.dataFabricacao}):");
            e.dataFabricacao = DateTime.Parse(Console.ReadLine()!);
            Console.Write($"Fabricante ({e.fabricante}):");
            e.fabricante = Console.ReadLine()!;
        }

        private void ExibirCabecalho()
        {
            string nomeSolucao = "Gestão de Equipamentos";
            Console.Clear();
            Console.WriteLine($"----- {nomeSolucao} -----");
            Console.WriteLine();
        }

        internal void Excluir()
        {
            ExibirCabecalho();
            Console.WriteLine("-- Excluir Equipamento --");
            Console.WriteLine();

            bool visualizarCadastrados = Visualizar(false);
            if (visualizarCadastrados == false) return;

            Equipamento[] equipamentos = repositorioEquipamnto.equipamentos;

            while (true)
            {
                Console.WriteLine();
                Console.Write("Digite o Id do equipamento para excluir: ");
                int idEscolhido = Convert.ToInt32(Console.ReadLine()!);

                for (int i = 0; i < equipamentos.Length; i++)
                {
                    if (equipamentos[i] == null) continue;

                    if (idEscolhido == equipamentos[i].id)
                    {
                        equipamentos[i] = null;
                        Console.WriteLine();
                        Console.WriteLine($"Equipamento excluído com sucesso! id: {idEscolhido}");
                        DigitarEnterEContinuar.Executar();
                        return;
                    }
                    else
                    {
                        Console.WriteLine();
                        Console.WriteLine("ID inválido. Tente novamente.");
                    }
                }
            }
        }
    }
    #endregion


    #region dados
    public class RepositorioEquipamento
    {
        public Equipamento[] equipamentos = new Equipamento[100];

        public int contadorEquipamentos = 0;
        public void CadastrarEquipamento(Equipamento equipamento)
        {
            equipamentos[contadorEquipamentos] = equipamento;
            contadorEquipamentos++;
        }

        public Equipamento[] SelecionarEquipamentos()
        {
            return equipamentos;
        }
    }
    #endregion

    #region regra de negócio
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
    #endregion
}
