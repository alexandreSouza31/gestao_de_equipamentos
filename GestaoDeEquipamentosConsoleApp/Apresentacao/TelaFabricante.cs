using GestaoDeEquipamentosConsoleApp.Utils;
using GestaoDeEquipamentosConsoleApp.Dados;
using GestaoDeEquipamentosConsoleApp.Negocio;
using static System.Runtime.InteropServices.JavaScript.JSType;
using GestaoDeEquipamentosConsoleApp.Compartilhado;

namespace GestaoDeEquipamentosConsoleApp.Apresentacao
{
    public class TelaFabricante : TelaBase<Fabricante>
    {
        private RepositorioFabricante repositorioFabricante;

        public TelaFabricante(RepositorioFabricante? repositorioFabricante = null)
            : base("Fabricante", repositorioFabricante ?? new RepositorioFabricante())
        {
            this.repositorioFabricante = repositorioFabricante ?? new RepositorioFabricante();
        }

        public void ExecutarMenu()
        {
            var menuFabricante = new TelaMenuEntidadeBase<Fabricante>(this);

            bool continuar = true;
            while (continuar)
            {
                continuar = menuFabricante.ExecutarMenuEntidade();
            }
        }

        public override Fabricante CriarInstanciaVazia()
        {
            return new Fabricante();
        }

        protected override Fabricante ObterNovosDados(Fabricante dadosOriginais, bool editar)
        {
            var tela = new TelaFabricante(null);
            while (true)
            {
                tela.Visualizar(false, false, false);
                ExibirCabecalho();

                if (editar)
                {
                    Console.WriteLine();
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("************* Caso não queira alterar um campo, pressione Enter para mantê-lo.");
                    Console.ResetColor();
                }

                string nome = EntradaHelper.ObterEntrada("Nome", dadosOriginais.nome, editar);
                string email = EntradaHelper.ObterEntrada("Email", dadosOriginais.email, editar);
                string telefone = EntradaHelper.ObterEntrada("Telefone", dadosOriginais.telefone, editar);

                string[] nomesCampos = { "nome", "email", "telefone" };
                string[] valoresCampos = { nome, email, telefone };
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
                return new Fabricante(nome, email, telefone);
            }
        }

        public static void AtualizarFabricante(Fabricante original, Fabricante novosDados)
        {
            original.nome = novosDados.nome;
            original.email = novosDados.email;
            original.telefone = novosDados.telefone;
        }

        protected override void ImprimirCabecalhoTabela()
        {
            Console.WriteLine("{0, -5} | {1, -20} | {2, -25} | {3, -15}",
                "ID".ToUpper(), "NOME".ToUpper(), "EMAIL".ToUpper(), "TELEFONE".ToUpper());
        }

        protected override void ImprimirRegistro(Fabricante f)
        {
            Console.WriteLine("{0, -5} | {1, -20} | {2, -25} | {3, -15}",
                f.id, f.nome, f.email, f.telefone);
        }
    }
}
