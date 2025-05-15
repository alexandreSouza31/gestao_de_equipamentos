using GestaoDeEquipamentosConsoleApp.Dados;
using GestaoDeEquipamentosConsoleApp.Negocio;
using GestaoDeEquipamentosConsoleApp.Utils;

namespace GestaoDeEquipamentosConsoleApp.Apresentacao
{
    public class TelaEquipamento
    {
        public string pagina;
        private RepositorioEquipamento repositorioEquipamento;
        private RepositorioFabricante repositorioFabricante;
        public TelaFabricante telaFabricante;
        Direcionar direcionar = new Direcionar();

        public TelaEquipamento(RepositorioEquipamento repositorioEquipamento, RepositorioFabricante repositorioFabricante, TelaFabricante telaFabricante)
        {
            this.repositorioEquipamento = repositorioEquipamento;
            this.repositorioFabricante = repositorioFabricante;
            this.telaFabricante = telaFabricante;
        }

        public char ApresentarMenu()
        {
            ExibirCabecalho("");
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

        public bool ExecutarMenuEquipamento(TelaEquipamento telaEquipamento)
        {
            char opcaoEscolhida = telaEquipamento.ApresentarMenu();

            if (opcaoEscolhida == 'S') return false;

            switch (opcaoEscolhida)
            {
                case '1':
                    bool continuar = telaEquipamento.Cadastrar();
                    if (!continuar) return false;
                    break;
                case '2':
                    telaEquipamento.Visualizar(true, true);
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
            return true;
        }

        private void ExibirCabecalho(string pagina)
        {
            string nomeSolucao = "Gestão de Equipamentos";
            Console.Clear();
            Console.WriteLine($"----- {nomeSolucao}/{pagina} -----");
            Console.WriteLine();
        }

        public bool Cadastrar()
        {
            pagina = "Cadastrar";
            ExibirCabecalho(pagina);

            bool haFabricante = repositorioFabricante.VerificarExistenciaFabricantes();
            var resultado = direcionar.DirecionarParaMenu(haFabricante, true, "Fabricante");
            if (resultado != ResultadoDirecionamento.Continuar) return false;

            Equipamento equipamento = new Equipamento("", 0, "", new DateTime(1975, 1, 1), null);
            var novosDados = ObterNovosDados(equipamento, false);
            if (novosDados == null) return false;

            AtualizarEquipamento(equipamento, novosDados);

            equipamento.id = Equipamento.numeroId++;
            repositorioEquipamento.CadastrarEquipamento(equipamento);

            Console.WriteLine($"nome: {equipamento.nome} cadastrado com sucesso! id: {equipamento.id}");
            DigitarEnterEContinuar.Executar();
            return true;
        }

        public bool Visualizar(bool exibirCabecalho, bool digitarEnterEContinuar, bool msgAoCadastrar = true)
        {
            pagina = "Visualizar";
            if (exibirCabecalho) ExibirCabecalho(pagina);

            Equipamento[] equipamentos = repositorioEquipamento.SelecionarEquipamentos();
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
                    e.id, e.nome, e.precoAquisicao.ToString("C2"), e.numeroSerie, e.dataFabricacao.ToShortDateString(), e.fabricante.nome
                );

                encontrados++;
            }

            if (encontrados == 0 && msgAoCadastrar) Console.WriteLine("Ainda não há equipamentos! Faça um cadastro!");

            if (digitarEnterEContinuar) DigitarEnterEContinuar.Executar();
            return encontrados > 0;
        }


        public bool Editar()
        {
            pagina = "Editar";
            ExibirCabecalho(pagina);

            bool visualizarCadastrados = Visualizar(false, false, false);
            bool haEquipamentos = repositorioEquipamento.VerificarExistenciaEquipamentos();
            var resultado = direcionar.DirecionarParaMenu(haEquipamentos, false, "Equipamento");
            if (resultado != ResultadoDirecionamento.Continuar) return false;

            Equipamento[] equipamentos = repositorioEquipamento.SelecionarEquipamentos();

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
                    var novosDados = ObterNovosDados(equipamentoSelecionado, true);
                    AtualizarEquipamento(equipamentoSelecionado, novosDados);

                    Visualizar(true, false);
                    Console.WriteLine();
                    Console.WriteLine($"{equipamentoSelecionado.nome} editado com sucesso! id: {equipamentoSelecionado.id}");
                    DigitarEnterEContinuar.Executar();
                    return true;
                }
                else
                {
                    Console.WriteLine();
                    Console.WriteLine("ID inválido. Tente novamente.");
                }
            }
        }

        internal bool Excluir()
        {
            pagina = "Excluir";
            ExibirCabecalho(pagina);

            var todos = repositorioEquipamento.SelecionarEquipamentos();
            bool haEquipamentos = repositorioEquipamento.SelecionarEquipamentos().Length > 0;
            var resultado = direcionar.DirecionarParaMenu(haEquipamentos, false, "Equipamento");
            if (resultado != ResultadoDirecionamento.Continuar) return false;

            bool visualizarCadastrados = Visualizar(false, false, false);
            if (!visualizarCadastrados) return false;

            Equipamento[] equipamentos = repositorioEquipamento.SelecionarEquipamentos();

            while (true)
            {
                Console.WriteLine();
                Console.Write("Digite o Id do equipamento para excluir: ");

                bool idValido = (!int.TryParse(Console.ReadLine(), out int idEscolhido));
                var equipamento = repositorioEquipamento.SelecionarEquipamentoPorId(idEscolhido);

                if (!idValido && equipamento == null)
                {
                    Console.WriteLine("ID inválido. Tente novamente.");
                    continue;
                }

                for (int i = 0; i < equipamentos.Length; i++)
                {
                    if (equipamentos[i] == null) continue;

                    if (idEscolhido == equipamentos[i].id)
                    {
                        DesejaExcluir desejaExcluir = new DesejaExcluir();
                        var vaiExcluir = desejaExcluir.DesejaMesmoExcluir(equipamento.nome);
                        if (vaiExcluir != "S") return false;

                        Console.WriteLine();
                        Console.WriteLine($"Equipamento {equipamento.nome} excluído com sucesso! id: {idEscolhido}");
                        equipamentos[i] = null;
                        DigitarEnterEContinuar.Executar();
                        return true;
                    }
                }
            }
        }

        private Equipamento ObterNovosDados(Equipamento dadosOriginais, bool editar)
        {
            if (editar == true)
            {
                Console.WriteLine();
                Console.WriteLine("************* Caso não queira alterar um campo, basta pressionar Enter para ignorá-lo");
            }

            string nome;

            while (true)
            {
                string etiquetaNome = editar ? $"Nome ({dadosOriginais.nome}): " : "Nome: ";
                Console.Write(etiquetaNome);

                nome = Console.ReadLine()!;
                nome = string.IsNullOrWhiteSpace(nome) ? dadosOriginais.nome : nome;
                break;
            }

            Console.Write(editar ? $"Preço de Aquisição ({dadosOriginais.precoAquisicao}): " : "Preço de Aquisição: ");
            string inputPreco = Console.ReadLine();
            decimal precoAquisicao = string.IsNullOrWhiteSpace(inputPreco) ? dadosOriginais.precoAquisicao : Convert.ToDecimal(inputPreco);

            Console.Write(editar ? $"Número de Série ({dadosOriginais.numeroSerie}): " : "Número de Série: ");
            string inputNumeroSerie = Console.ReadLine();
            string numeroSerie = string.IsNullOrWhiteSpace(inputNumeroSerie) ? dadosOriginais.numeroSerie : inputNumeroSerie;

            Console.Write(editar ? $"Data de Fabricação ({dadosOriginais.dataFabricacao.ToShortDateString()}): " : "Data de Fabricação: ");
            string inputData = Console.ReadLine();
            DateTime dataFabricacao = string.IsNullOrWhiteSpace(inputData) ? dadosOriginais.dataFabricacao : DateTime.Parse(inputData);

            bool haFabricantes = telaFabricante.Visualizar(true, false, false);
            var resultado = direcionar.DirecionarParaMenu(haFabricantes, true, "Fabricante");
            if (resultado != ResultadoDirecionamento.Continuar)
                return null;

            Console.Write(editar ? $"ID do Fabricante ({dadosOriginais.fabricante?.id}): " : "ID do Fabricante: ");
            string inputFabricante = Console.ReadLine();

            Fabricante fabricante;

            if (string.IsNullOrWhiteSpace(inputFabricante))
            {
                fabricante = dadosOriginais.fabricante;
            }
            else
            {
                int idFabricante = int.Parse(inputFabricante);
                fabricante = repositorioFabricante.SelecionarFabricantePorId(idFabricante);

                if (fabricante == null)
                {
                    Console.WriteLine("Fabricante não encontrado! Pressione Enter para continuar...");
                    Console.ReadLine();
                    return null;
                }
            }

            Equipamento novosDados = new Equipamento(nome, precoAquisicao, numeroSerie, dataFabricacao, fabricante);
            return novosDados;
        }

        public  void AtualizarEquipamento(Equipamento dadosOriginais, Equipamento novosDados)
        {
            dadosOriginais.nome = novosDados.nome;
            dadosOriginais.precoAquisicao = novosDados.precoAquisicao;
            dadosOriginais.numeroSerie = novosDados.numeroSerie;
            dadosOriginais.dataFabricacao = novosDados.dataFabricacao;
            dadosOriginais.fabricante = novosDados.fabricante;
        }
    }
}