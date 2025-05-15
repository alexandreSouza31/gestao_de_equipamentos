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
            ExibirCabecalho(pagina);

            bool haEquipamentos = repositorioEquipamento.VerificarExistenciaEquipamentos();
            var resultado = direcionar.DirecionarParaMenu(haEquipamentos, true, "Equipamento");
            if (resultado != ResultadoDirecionamento.Continuar) return false;

            Chamado chamado = new Chamado("", "", new DateTime(1975, 1, 1),null);

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
                bool haChamados = repositorioChamado.SelecionarChamados().Length > 0;
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

            Chamado[] chamados = repositorioChamado.SelecionarChamados();

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

        private static Chamado ObterNovosDados(Chamado dadosOriginais, bool editar, TelaChamado telaChamado)
        {
            if (editar)
            {
                Console.WriteLine();
                Console.WriteLine("************* Caso não queira alterar um campo, basta pressionar Enter para ignorá-lo");
            }

            string titulo;

            while (true)
            {
                string etiquetaTitulo = editar ? $"Título ({dadosOriginais.titulo}): " : "Título: ";
                Console.Write(etiquetaTitulo);

                titulo = Console.ReadLine();
                titulo = string.IsNullOrWhiteSpace(titulo) ? dadosOriginais.titulo : titulo;
                break;
            }

            Console.Write(editar ? $"Descrição ({dadosOriginais.descricao}): " : "Descrição: ");
            string inputDescricao = Console.ReadLine();
            string descricao = string.IsNullOrWhiteSpace(inputDescricao) ? dadosOriginais.descricao : inputDescricao;

            Console.Write(editar ? $"Data de Abertura ({dadosOriginais.dataAbertura.ToShortDateString()}): " : "Data de Abertura: ");
            string inputData = Console.ReadLine();
            DateTime dataAbertura = string.IsNullOrWhiteSpace(inputData) ? dadosOriginais.dataAbertura : DateTime.Parse(inputData);

            bool haEquipamentos = telaChamado.telaEquipamento.Visualizar(true, false, false);
            var resultado = telaChamado.direcionar.DirecionarParaMenu(haEquipamentos, true, "Equipamento");

            if (resultado != ResultadoDirecionamento.Continuar)
                return null;

            Console.Write(editar ? $"ID do Equipamento ({dadosOriginais.equipamento?.id}): " : "ID do Equipamento: ");
            string inputEquipamento = Console.ReadLine();

            Equipamento equipamento;

            if (string.IsNullOrWhiteSpace(inputEquipamento))
            {
                equipamento = dadosOriginais.equipamento;
            }
            else
            {
                int idEquipamento = int.Parse(inputEquipamento);
                equipamento = telaChamado.repositorioEquipamento.SelecionarEquipamentoPorId(idEquipamento);

                if (equipamento == null)
                {
                    Console.WriteLine("Equipamento não encontrado! Pressione Enter para continuar...");
                    Console.ReadLine();
                    return null;
                }
            }

            Chamado novosDados = new Chamado(titulo, descricao, dataAbertura,equipamento);
            return novosDados;
        }

        public static void AtualizarChamado(Chamado dadosOriginais, Chamado novosDados, RepositorioChamado repositorioChamado)
        {
            dadosOriginais.titulo = novosDados.titulo;
            dadosOriginais.descricao = novosDados.descricao;
            dadosOriginais.dataAbertura = novosDados.dataAbertura;
            dadosOriginais.equipamento = novosDados.equipamento;

            for (int i = 0; i < repositorioChamado.SelecionarChamados().Length; i++)
            {
                if (repositorioChamado.SelecionarChamados()[i]?.id == dadosOriginais.id)
                {
                    repositorioChamado.SelecionarChamados()[i] = dadosOriginais;
                    break;
                }
            }
        }
    }
}