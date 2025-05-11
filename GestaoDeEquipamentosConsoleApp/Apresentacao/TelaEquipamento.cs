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
            Equipamento equipamento = new Equipamento();

            ExibirCabecalho(pagina);

            bool haFabricante = repositorioFabricante.VerificarExistenciaFabricantes();
            var resultado = direcionar.DirecionarParaMenu(haFabricante, true, "Fabricante");
            if (resultado != ResultadoDirecionamento.Continuar) return false;

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

            Equipamento[] equipamentos = repositorioEquipamento.equipamentos;

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
            bool haEquipamentos = repositorioEquipamento.contadorEquipamentos > 0;
            var resultado = direcionar.DirecionarParaMenu(haEquipamentos, false, "Equipamento");
            if (resultado != ResultadoDirecionamento.Continuar) return false;

            bool visualizarCadastrados = Visualizar(false, false, false);
            if (!visualizarCadastrados) return false;

            Equipamento[] equipamentos = repositorioEquipamento.equipamentos;
            bool equipamentoExcluido = false;

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
                        equipamentoExcluido |= true;
                        return true;
                    }
                }
            }
        }

        public Equipamento ObterNovosDados(Equipamento dadosOriginais, bool editar)
        {
            Equipamento novosDados = new Equipamento();

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

            bool haFabricantes = telaFabricante.Visualizar(true,false,false);

            var resultado = direcionar.DirecionarParaMenu(haFabricantes, true, "Fabricante");
            if (resultado != ResultadoDirecionamento.Continuar) return null;

            string etiquetaFabricante = editar ? $"ID do Fabricante ({dadosOriginais.fabricante?.id}): " : "ID do Fabricante: ";
            Console.Write(etiquetaFabricante);

            string inputIdFabricante = Console.ReadLine()!;

            if (string.IsNullOrWhiteSpace(inputIdFabricante))
            {
                novosDados.fabricante = dadosOriginais.fabricante;
            }
            else
            {
                int idFabricante = int.Parse(inputIdFabricante);

                Fabricante fabricanteEncontrado = repositorioFabricante.SelecionarFabricantePorId(idFabricante);

                if (fabricanteEncontrado == null)
                {
                    Console.WriteLine("Fabricante não encontrado! Pressione Enter para continuar...");
                    Console.ReadLine();
                    return null;
                }

                novosDados.fabricante = fabricanteEncontrado;
            }

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