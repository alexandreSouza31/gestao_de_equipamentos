using GestaoDeEquipamentosConsoleApp.Compartilhado;
using GestaoDeEquipamentosConsoleApp.Dados;
using GestaoDeEquipamentosConsoleApp.Negocio;
using GestaoDeEquipamentosConsoleApp.Utils;

namespace GestaoDeEquipamentosConsoleApp.Apresentacao
{
    public class TelaChamado : TelaBase<Chamado>
    {
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

        public void ExecutarMenu()
        {
            var menuFabricante = new TelaMenuEntidadeBase<Chamado>(this);

            bool continuar = true;
            while (continuar)
            {
                continuar = menuFabricante.ExecutarMenuEntidade();
            }
        }


        public override Chamado CriarInstanciaVazia()
        {
            return new Chamado();
        }

        protected override Chamado ObterNovosDados(Chamado dadosOriginais, bool editar)
        {
            if (editar)
            {
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