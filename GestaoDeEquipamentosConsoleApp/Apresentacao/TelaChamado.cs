using GestaoDeEquipamentosConsoleApp.Compartilhado;
using GestaoDeEquipamentosConsoleApp.Dados;
using GestaoDeEquipamentosConsoleApp.Negocio;
using GestaoDeEquipamentosConsoleApp.Utils;

namespace GestaoDeEquipamentosConsoleApp.Apresentacao
{
    public class TelaChamado : TelaBase<Chamado>
    {

        //public string pagina;
        public RepositorioChamado repositorioChamado;
        public RepositorioEquipamento repositorioEquipamento;
        Direcionar direcionar = new Direcionar();

        private RepositorioFabricante repositorioFabricante;
        public TelaEquipamento telaEquipamento;
        public TelaFabricante telaFabricante;
        public TelaChamado telaChamado;

        public TelaChamado(RepositorioChamado repositorioChamado, RepositorioEquipamento repositorioEquipamento, TelaEquipamento telaEquipamento)
            : base("Chamado", repositorioChamado ?? new RepositorioChamado())
        {
            this.repositorioChamado = repositorioChamado ?? new RepositorioChamado();
            this.repositorioEquipamento = repositorioEquipamento;
            this.telaEquipamento = telaEquipamento;
        }

        public override Chamado CriarInstanciaVazia()
        {
            return new Chamado();
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
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Digite uma opção válida!");
                    Console.ResetColor();
                    DigitarEnterEContinuar.Executar(true);
                    break;
            }
            return true;
        }

        //public bool Excluir()
        //{
        //    //pagina = "Excluir chamado";
        //    ExibirCabecalho();

        //    bool haChamados = repositorioChamado.VerificarExistenciaRegistros();
        //    var resultado = direcionar.DirecionarParaMenu(haChamados, false, "Chamado");
        //    if (resultado != ResultadoDirecionamento.Continuar) return false;

        //    bool visualizarCadastrados = Visualizar(false, false,false);
        //    if (!visualizarCadastrados) return false;

        //    Chamado[] chamados = repositorioChamado.SelecionarRegistros();

        //    while (true)
        //    {
        //        Console.WriteLine();
        //        Console.Write("Digite o Id do chamado para excluir: ");

        //        bool idValido = (!int.TryParse(Console.ReadLine(), out int idEscolhido));
        //        var chamado = repositorioChamado.SelecionarRegistroPorId(idEscolhido);

        //        if (!idValido && chamado == null)
        //        {
        //            Console.ForegroundColor = ConsoleColor.Red;
        //            Console.WriteLine("ID inválido. Tente novamente.");
        //            Console.ResetColor();
        //            continue;
        //        }

        //        for (int i = 0; i < chamados.Length; i++)
        //        {
        //            if (chamados[i] == null) continue;

        //            if (idEscolhido == chamados[i].id)
        //            {
        //                DesejaExcluir desejaExcluir = new DesejaExcluir();
        //                var vaiExcluir = desejaExcluir.DesejaMesmoExcluir(chamado.titulo);
        //                if (vaiExcluir != "S") return false;

        //                Console.WriteLine();
        //                Console.ForegroundColor = ConsoleColor.Green;
        //                Console.WriteLine($"Chamado {chamado.titulo} excluído com sucesso! id: {idEscolhido}");
        //                Console.ResetColor();
        //                chamados[i] = null;
        //                DigitarEnterEContinuar.Executar();
        //                return true;
        //            }
        //        }
        //    }
        //}

        protected override Chamado ObterNovosDados(Chamado dadosOriginais, bool editar)
        {
            //pagina = "Cadastrar chamado";
            if (editar)
            {
                //pagina = "Editar chamado";
                Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("************* Caso não queira alterar um campo, basta pressionar Enter para ignorá-lo");
                Console.ResetColor();
            }

            while (true)
            {
                ExibirCabecalho();

                string titulo = EntradaHelper.ObterEntrada("Título", dadosOriginais.titulo, editar);
                string descricao = EntradaHelper.ObterEntrada("Descrição", dadosOriginais.descricao, editar);
                DateTime dataAbertura = EntradaHelper.ObterEntrada("Data de Abertura", dadosOriginais.dataAbertura, editar);

                bool haEquipamentos = telaEquipamento.Visualizar(true, false, false);
                var resultado = direcionar.DirecionarParaMenu(haEquipamentos, true, "Equipamento");

                if (resultado != ResultadoDirecionamento.Continuar)
                    return null!;

                Console.Write(editar ? $"ID do Equipamento ({dadosOriginais.equipamento?.id}): " : "ID do Equipamento: ");
                string inputEquipamento = Console.ReadLine()!;

                Equipamento equipamento;

                if (string.IsNullOrWhiteSpace(inputEquipamento))
                {
                    equipamento = dadosOriginais.equipamento!;
                }
                else
                {
                    if (!int.TryParse(inputEquipamento, out int idEquipamento))
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("ID inválido! Pressione Enter para continuar...");
                        Console.ResetColor();
                        Console.ReadLine();
                        continue;
                    }

                    equipamento = repositorioEquipamento.SelecionarRegistroPorId(idEquipamento);

                    if (equipamento == null)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Equipamento não encontrado! Pressione Enter para continuar...");
                        Console.ResetColor();
                        Console.ReadLine();
                        continue;
                    }
                }

                string[] nomesCampos = { "titulo", "descricao", "data abertura", "equipamento" };
                string[] valoresCampos = { titulo, descricao, dataAbertura.ToString(), equipamento.ToString()! };
                string erros = ValidarCampo.ValidarCampos(nomesCampos, valoresCampos);

                if (!string.IsNullOrEmpty(erros))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("\nErros encontrados:");
                    Console.WriteLine(erros);
                    Console.ResetColor();
                    DigitarEnterEContinuar.Executar();
                    Console.Clear();
                    continue;
                }
                Chamado novosDados = new Chamado(titulo, descricao, dataAbertura, equipamento);
                return novosDados;
            }
        }

        public static void AtualizarChamado(Chamado dadosOriginais, Chamado novosDados, RepositorioChamado repositorioChamado)
        {
            dadosOriginais.titulo = novosDados.titulo;
            dadosOriginais.descricao = novosDados.descricao;
            dadosOriginais.dataAbertura = novosDados.dataAbertura;
            dadosOriginais.equipamento = novosDados.equipamento;

            for (int i = 0; i < repositorioChamado.SelecionarRegistros().Length; i++)
            {
                if (repositorioChamado.SelecionarRegistros()[i]?.id == dadosOriginais.id)
                {
                    repositorioChamado.SelecionarRegistros()[i] = dadosOriginais;
                    break;
                }
            }
        }

        protected override void ImprimirCabecalhoTabela()
        {
            Console.WriteLine("{0, -5} | {1, -20} | {2, -40} | {3, -15} | {4, -15}",
                 "Id".ToUpper(), "Título".ToUpper(), "Descrição".ToUpper(), "Data Abertura".ToUpper(), "Equipamento".ToUpper());
        }

        protected override void ImprimirRegistro(Chamado c)
        {
            Console.WriteLine("{0, -5} | {1, -20} | {2, -40} | {3, -15} | {4, -15}",
                c.id, c.titulo, c.descricao, c.dataAbertura.ToShortDateString(), c.equipamento);
        }
    }
}