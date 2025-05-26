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
        public Direcionar direcionar = new Direcionar();
        //public string pagina;

        public TelaFabricante(RepositorioFabricante? repositorioFabricante = null)
            : base("Fabricante", repositorioFabricante ?? new RepositorioFabricante())
        {
            this.repositorioFabricante = repositorioFabricante ?? new RepositorioFabricante();
        }

        public override Fabricante CriarInstanciaVazia()
        {
            return new Fabricante();
        }

        public bool ExecutarMenuFabricante(TelaFabricante telaFabricante)
        {
            char opcaoEscolhida = telaFabricante.ApresentarMenu();

            if (opcaoEscolhida == 'S') return false;

            switch (opcaoEscolhida)
            {
                case '1':
                    telaFabricante.Cadastrar();
                    break;
                case '2':
                    telaFabricante.Visualizar(true,true,false);
                    break;
                case '3':
                    telaFabricante.Editar();
                    break;
                case '4':
                    telaFabricante.Excluir();
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

        //    if (!repositorioFabricante.VerificarExistenciaRegistros())
        //    {
        //        Console.ForegroundColor = ConsoleColor.Red;
        //        Console.WriteLine("Nenhum fabricante cadastrado.");
        //        Console.ResetColor();
        //        DigitarEnterEContinuar.Executar();
        //        return false;
        //    }


        //    while (true)
        //    {
        //        Console.Write("\nDigite o Id do fabricante para excluir: ");
        //        if (!int.TryParse(Console.ReadLine(), out int idFabricante))
        //        {
        //            Console.ForegroundColor = ConsoleColor.Red;
        //            Console.WriteLine("ID inválido. Tente novamente.");
        //            Console.ResetColor();
        //            continue;
        //        }

        //        if (!repositorioFabricante.TentarObterRegistro(idFabricante, out var fabricante))
        //        {
        //            Console.ForegroundColor = ConsoleColor.Red;
        //            Console.WriteLine("Fabricante não encontrado. Tente novamente.");
        //            Console.ResetColor();
        //            continue;
        //        }

        //        DesejaExcluir desejaExcluir = new DesejaExcluir();
        //        var vaiExcluir = desejaExcluir.DesejaMesmoExcluir(fabricante.nome);

        //        if (vaiExcluir != "S") return false;

        //        repositorioFabricante.ExcluirRegistro(idFabricante);

        //        Console.ForegroundColor = ConsoleColor.Green;
        //        Console.WriteLine($"\nFabricante '{fabricante.nome}' excluído com sucesso! id: {fabricante.id}");
        //        Console.ResetColor();
        //        DigitarEnterEContinuar.Executar();
        //        return true;
        //    }
        //}

        protected override Fabricante ObterNovosDados(Fabricante dadosOriginais, bool editar)
        {
            var tela = new TelaFabricante(null);
            while (true)
            {
                tela.Visualizar(false, false, false);
                //pagina = "Cadastrar Fabricante";
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
