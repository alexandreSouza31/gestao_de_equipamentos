using GestaoDeEquipamentosConsoleApp.Dados;
using GestaoDeEquipamentosConsoleApp.Negocio;
using GestaoDeEquipamentosConsoleApp.Utils;

namespace GestaoDeEquipamentosConsoleApp.Apresentacao
{
    public class TelaChamado
    {

        public string pagina;
        public RepositorioChamado repositorioChamado;
        public RepositorioEquipamento repositorioEquipamento;
        Direcionar direcionar = new Direcionar();

        private RepositorioFabricante repositorioFabricante;
        public TelaEquipamento telaEquipamento;
        public TelaFabricante telaFabricante;

        public TelaChamado(RepositorioChamado repositorioChamado, RepositorioEquipamento repositorioEquipamento, TelaEquipamento telaEquipamento)
        {
            this.repositorioChamado = repositorioChamado;
            this.repositorioEquipamento = repositorioEquipamento;
            this.telaEquipamento = telaEquipamento;
        }

        public char ApresentarMenu()
        {
            ExibirCabecalho("");
            Console.WriteLine();
            Console.WriteLine("1 - Cadastrar Chamado");
            Console.WriteLine("2 - Visualizar Chamado");
            Console.WriteLine("3 - Editar Chamado");
            Console.WriteLine("4 - Excluir Chamado");
            Console.WriteLine("S - Sair");
            Console.Write("\nDigite uma opção: ");
            char opcaoEscolhida = Convert.ToChar(Console.ReadLine()!.ToUpper()[0]);

            return opcaoEscolhida;
        }

        public bool ExecutarMenuChamado(TelaChamado telaChamado)
        {
            char opcaoEscolhida = telaChamado.ApresentarMenu();

            if (opcaoEscolhida == 'S') return false;

            switch (opcaoEscolhida)
            {
                case '1':
                    bool continuar = telaChamado.Cadastrar();
                    if (!continuar) return false;
                    break;
                case '2':
                    telaChamado.Visualizar(true, true);
                    break;
                case '3':
                    telaChamado.Editar();
                    break;
                case '4':
                    telaChamado.Excluir();
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
            string nomeSolucao = "Gestão de Chamados";
            Console.Clear();
            Console.WriteLine($"----- {nomeSolucao}/{pagina} -----");
            Console.WriteLine();
        }

        public bool Cadastrar()
        {
            pagina = "Cadastrar chamado";
            Chamado chamado = new Chamado();

            ExibirCabecalho(pagina);

            bool haEquipamentos = repositorioEquipamento.VerificarExistenciaEquipamentos();
            var resultado = direcionar.DirecionarParaMenu(haEquipamentos, true, "Equipamento");
            if (resultado != ResultadoDirecionamento.Continuar) return false;

            var novosdados = ObterNovosDados(chamado, false ,this);
            AtualizarChamado(chamado, novosdados, repositorioChamado);

            chamado.id = Chamado.numeroId++;
            repositorioChamado.CadastrarEquipamento(chamado);
            Console.WriteLine($"chamado {chamado.titulo} cadastrado com sucesso! id: {chamado.id}");
            DigitarEnterEContinuar.Executar();
            return true;
        }

        public bool Visualizar(bool exibirCabecalho, bool digitarEnterEContinuar, bool msgAoCadastrar = true)
        {
            pagina = "Visualizar chamado";
            if (exibirCabecalho) ExibirCabecalho(pagina);

            bool haEquipamentos = repositorioEquipamento.VerificarExistenciaEquipamentos();

            if (msgAoCadastrar==true)
            {
                bool haChamados = repositorioChamado.contadorChamados > 0;
                var resultado = direcionar.DirecionarParaMenu(haChamados, false, "Chamado");
                if (resultado != ResultadoDirecionamento.Continuar) return false;

            }

            Chamado[] chamados = repositorioChamado.SelecionarChamados();
            int encontrados = 0;

            string tamanhoCabecalhoColunas = "{0, -5} | {1, -20} | {2, -40} | {3, -15} | {4, -15}";

            for (int i = 0; i < chamados.Length; i++)
            {
                Chamado e = chamados[i];
                if (e == null) continue;

                if (encontrados == 0)
                {
                    Console.WriteLine(
                        tamanhoCabecalhoColunas,
                        "Id".ToUpper(), "Título".ToUpper(), "Descrição".ToUpper(), "Data Abertura".ToUpper(), "Equipamento".ToUpper()
                    );
                }

                Console.WriteLine(
                    tamanhoCabecalhoColunas,
                    e.id, e.titulo, e.descricao, e.dataAbertura.ToShortDateString(), e.equipamento
                );

                encontrados++;
            }

            if (encontrados == 0 && msgAoCadastrar)
                Console.WriteLine("Ainda não há chamados! Faça um cadastro!");

            if (digitarEnterEContinuar)
                DigitarEnterEContinuar.Executar();

            return encontrados > 0;
        }


        public bool Editar()
        {
            pagina = "Editar chamado";
            ExibirCabecalho(pagina);

            bool visualizarCadastrados = Visualizar(false, false, false);
            if (!visualizarCadastrados) return false;

            while (true)
            {
                Console.WriteLine();
                Console.Write("Digite o Id do chamado para editar: ");
                if (!int.TryParse(Console.ReadLine(), out int idChamado))
                {
                    Console.WriteLine("ID inválido. Tente novamente.");
                    continue;
                }

                Chamado chamadoExistente = repositorioChamado.SelecionarChamadoPorId(idChamado);

                if (chamadoExistente == null)
                {
                    Console.WriteLine("Chamado não encontrado. Tente novamente.");
                    continue;
                }

                Chamado novosDados = ObterNovosDados(chamadoExistente, true,this);
                novosDados.id = chamadoExistente.id;
                AtualizarChamado(chamadoExistente, novosDados,repositorioChamado);

                Visualizar(true, false);
                Console.WriteLine();
                Console.WriteLine($"{chamadoExistente.titulo} editado com sucesso! id: {chamadoExistente.id}");
                DigitarEnterEContinuar.Executar();
                return true;
            }
        }

        public bool Excluir()
        {
            pagina = "Excluir chamado";
            ExibirCabecalho(pagina);

            bool haChamados = repositorioChamado.VerificarExistenciaChamados();
            var resultado = direcionar.DirecionarParaMenu(haChamados, false, "Chamado");
            if (resultado != ResultadoDirecionamento.Continuar) return false;

            bool visualizarCadastrados = Visualizar(false, false,false);
            if (!visualizarCadastrados) return false;

            Chamado[] chamados = repositorioChamado.chamados;

            while (true)
            {
                Console.WriteLine();
                Console.Write("Digite o Id do chamado para excluir: ");

                bool idValido = (!int.TryParse(Console.ReadLine(), out int idEscolhido));
                var chamado = repositorioChamado.SelecionarChamadoPorId(idEscolhido);

                if (!idValido && chamado == null)
                {
                    Console.WriteLine("ID inválido. Tente novamente.");
                    continue;
                }

                for (int i = 0; i < chamados.Length; i++)
                {
                    if (chamados[i] == null) continue;

                    if (idEscolhido == chamados[i].id)
                    {
                        DesejaExcluir desejaExcluir = new DesejaExcluir();
                        var vaiExcluir = desejaExcluir.DesejaMesmoExcluir(chamado.titulo);
                        if (vaiExcluir != "S") return false;

                        Console.WriteLine();
                        Console.WriteLine($"Chamado {chamado.titulo} excluído com sucesso! id: {idEscolhido}");
                        chamados[i] = null;
                        DigitarEnterEContinuar.Executar();
                        return true;
                    }
                }
            }
        }

        public static Chamado ObterNovosDados(Chamado dadosOriginais, bool editar, TelaChamado telaChamado)
        {
            Chamado novosDados = new Chamado();

            telaChamado.Visualizar(true, false, false);

            if (editar == true)
            {
                Console.WriteLine();
                Console.WriteLine("************* Caso não queira alterar um campo, basta pressionar Enter para ignorá-lo");
            }

            while (true)
            {
                string etiquetaTitulo = editar ? $"Título ({dadosOriginais.titulo}): " : "Título: ";
                Console.Write(etiquetaTitulo);

                string inputTitulo = Console.ReadLine()!;
                novosDados.titulo = string.IsNullOrWhiteSpace(inputTitulo) ? dadosOriginais.titulo : inputTitulo;
                break;
            }

            string etiquetaDescricao = editar ? $"Descricao ({dadosOriginais.descricao}): " : "Descricao: ";
            Console.Write(etiquetaDescricao);

            string inputDescricao = Console.ReadLine()!;
            novosDados.descricao = string.IsNullOrWhiteSpace(inputDescricao) ? dadosOriginais.descricao : inputDescricao;

            DateTime inputData = DateTime.Now;
            string etiquetaDataAbertura = editar ? $"Data de Abertura ({dadosOriginais.dataAbertura.ToShortDateString()}): " : $"Data de Abertura: {inputData}";
            Console.Write(etiquetaDataAbertura);

            novosDados.dataAbertura = inputData;
            Console.WriteLine();

            Equipamento equipamentoSelecionado = null;

            while (equipamentoSelecionado == null)
            {
                telaChamado.telaEquipamento.Visualizar(true, false, false);
                Console.WriteLine();
                Console.Write("Digite o ID do equipamento que deseja associar: ");
                string etiquetaEquipamento = editar ? $"Equipamento ({dadosOriginais.equipamento.nome}): " : "";
                Console.Write(etiquetaEquipamento);

                string inputId = Console.ReadLine()!;

                if (string.IsNullOrWhiteSpace(inputId) && editar)
                {
                    equipamentoSelecionado = dadosOriginais.equipamento;
                    break;
                }

                if (int.TryParse(inputId, out int inputEquipamentoId))
                {
                    equipamentoSelecionado = telaChamado.repositorioEquipamento.SelecionarEquipamentoPorId(inputEquipamentoId);
                    if (equipamentoSelecionado == null)
                    {
                        Console.WriteLine("\nEquipamento não encontrado. Tente novamente.");
                    }
                }
                else
                {
                    Console.WriteLine("Entrada inválida. Digite um número de ID ou pressione Enter para manter o atual.");
                }
                DigitarEnterEContinuar.Executar(true);
            }

            novosDados.equipamento = equipamentoSelecionado;

            return novosDados;
       }

        public static void AtualizarChamado(Chamado dadosOriginais, Chamado novosDados, RepositorioChamado repositorioChamado)
        {
            dadosOriginais.titulo = novosDados.titulo;
            dadosOriginais.descricao = novosDados.descricao;
            dadosOriginais.dataAbertura = novosDados.dataAbertura;
            dadosOriginais.equipamento = novosDados.equipamento;

            for (int i = 0; i < repositorioChamado.chamados.Length; i++)
            {
                if (repositorioChamado.chamados[i]?.id == dadosOriginais.id)
                {
                    repositorioChamado.chamados[i] = dadosOriginais;
                    break;
                }
            }
        }
    }
}