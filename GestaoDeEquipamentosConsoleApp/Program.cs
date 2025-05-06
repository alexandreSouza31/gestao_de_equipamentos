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
                        telaEquipamento.Visualizar(true,true);
                        break;
                    case '3':
                        telaEquipamento.Editar();
                        break;
                    case '4':
                        telaEquipamento.Excluir();
                        break;
                    default:
                        Console.WriteLine("Digite uma opção válida!");
                        DigitarEnterEContinuar.Executar(true);
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

            return opcaoEscolhida;
        }

        public void Cadastrar()
        {
            Equipamento equipamento = new Equipamento();

            ExibirCabecalho();
            Console.WriteLine("-- Cadastrar Equipamento --");
            Console.WriteLine();
            equipamento.id = Equipamento.numeroId++;

            var novosDados = ObterNovosDados(equipamento,false);
            AtualizarEquipamento(equipamento, novosDados);

            repositorioEquipamnto.CadastrarEquipamento(equipamento);
            Console.WriteLine($"nome: {equipamento.nome} cadastrado com sucesso! id: {equipamento.id}");
            DigitarEnterEContinuar.Executar();
        }

        public bool Visualizar(bool exibirCabecalho, bool digitarEnterEContinuar)
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

            if(digitarEnterEContinuar) DigitarEnterEContinuar.Executar();
            return encontrados > 0;
        }


        public void Editar()
        {
            ExibirCabecalho();
            Console.WriteLine("-- Editar Equipamento --");
            Console.WriteLine();

            bool visualizarCadastrados = Visualizar(false, true);
            if (!visualizarCadastrados) return;

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
                    var novosDados = ObterNovosDados(equipamentoSelecionado,true);
                    AtualizarEquipamento(equipamentoSelecionado, novosDados);

                    Visualizar(true, false);
                    Console.WriteLine();
                    Console.WriteLine($"{equipamentoSelecionado.nome} editado com sucesso! id: {equipamentoSelecionado.id}");
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


        public static Equipamento ObterNovosDados(Equipamento dadosOriginais,bool editar)
        {
            Equipamento novosDados=new Equipamento();

            if (editar == true)
            {
                Console.WriteLine();
                Console.WriteLine("************* Caso não queira alterar um campo, basta pressionar Enter para ignorá-lo");
            }

            while (true)
            {
                string etiquetaNome = editar ? $"Nome ({dadosOriginais.nome}): " : "Nome: ";
                Console.Write(etiquetaNome);

                string inputNome = Console.ReadLine()!;
                novosDados.nome = string.IsNullOrWhiteSpace(inputNome) ? dadosOriginais.nome : inputNome;
                break;
            }

            string etiquetaPrecoAquisicao = editar ? $"Preço de Aquisição ({dadosOriginais.precoAquisicao}): " : "Preço de Aquisição: ";
            Console.Write(etiquetaPrecoAquisicao);

            string inputPrecoAquisicao = Console.ReadLine()!;
            novosDados.precoAquisicao = string.IsNullOrWhiteSpace(inputPrecoAquisicao) ? dadosOriginais.precoAquisicao : Convert.ToDecimal(inputPrecoAquisicao);

            string etiquetaNumeroSerie = editar ? $"Número de Série ({dadosOriginais.numeroSerie}): " : "Número de Série: ";
            Console.Write(etiquetaNumeroSerie);

            string inputNumeroSerie = Console.ReadLine()!;
            novosDados.numeroSerie = string.IsNullOrWhiteSpace(inputNumeroSerie) ? dadosOriginais.numeroSerie : inputNumeroSerie;

            string etiquetaDataFabricacao = editar ? $"Data de Fabricação ({dadosOriginais.dataFabricacao.ToShortDateString()}): " : "Data de Fabricação: ";
            Console.Write(etiquetaDataFabricacao);

            string inputData = Console.ReadLine()!;
            novosDados.dataFabricacao = string.IsNullOrWhiteSpace(inputData) ? dadosOriginais.dataFabricacao : DateTime.Parse(inputData);

            string etiquetaFabricante = editar ? $"Fabricante ({dadosOriginais.fabricante}): " : "Fabricante: ";
            Console.Write(etiquetaFabricante);

            string inputFabricante = Console.ReadLine()!;
            novosDados.fabricante = string.IsNullOrWhiteSpace(inputFabricante) ? dadosOriginais.fabricante : inputFabricante;

            return novosDados;
        }

        public static void AtualizarEquipamento(Equipamento dadosOriginais, Equipamento novosDados)
        {
            dadosOriginais.nome = novosDados.nome;
            dadosOriginais.precoAquisicao = novosDados.precoAquisicao;
            dadosOriginais.numeroSerie = novosDados.numeroSerie;
            dadosOriginais.dataFabricacao = novosDados.dataFabricacao;
            dadosOriginais.fabricante = novosDados.fabricante;
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

            bool visualizarCadastrados = Visualizar(false, true);
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
        public static int numeroId = 1;
        public int id;
        public string nome;
        public decimal precoAquisicao;
        public string numeroSerie;
        public DateTime dataFabricacao;
        public string fabricante;
    }
    #endregion
}
