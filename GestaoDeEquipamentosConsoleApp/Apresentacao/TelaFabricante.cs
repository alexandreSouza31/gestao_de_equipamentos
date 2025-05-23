using GestaoDeEquipamentosConsoleApp.Utils;
using GestaoDeEquipamentosConsoleApp.Dados;
using GestaoDeEquipamentosConsoleApp.Negocio;
using static System.Runtime.InteropServices.JavaScript.JSType;
using GestaoDeEquipamentosConsoleApp.Compartilhado;

namespace GestaoDeEquipamentosConsoleApp.Apresentacao
{
    public class TelaFabricante : TelaBase
    {
        private RepositorioFabricante repositorioFabricante;
        public Direcionar direcionar=new Direcionar();
        public string pagina;

        public TelaFabricante(RepositorioFabricante repositorioFabricante) : base("Fabricante")
        {
            if (repositorioFabricante == null)
                repositorioFabricante = new RepositorioFabricante();
            this.repositorioFabricante = repositorioFabricante;
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

        public bool Visualizar(bool exibirCabecalho, bool digitarEnterEContinuar, bool msgAoCadastrar = true)
        {
            pagina = "Visualizar Fabricante";
            ExibirCabecalho(pagina);

            Console.Clear();
            if (exibirCabecalho)
                ExibirCabecalho(pagina);
                Console.WriteLine("----- Fabricantes Registrados -----");

            bool haEquipamentos = repositorioFabricante.VerificarExistenciaRegistros();

            bool haChamados = repositorioFabricante.SelecionarRegistros().Length > 0;
            var resultado = direcionar.DirecionarParaMenu(haChamados, false, "Chamado");
            if (resultado != ResultadoDirecionamento.Continuar) return false;

            object[] fabricantes = repositorioFabricante.SelecionarRegistros();
            int encontrados = 0;

            string tamanhoCabecalhoColunas = "{0, -5} | {1, -20} | {2, -25} | {3, -15}";

            for (int i = 0; i < fabricantes.Length; i++)
            {
                Fabricante f = (Fabricante)fabricantes[i];
                if (f == null) continue;

                if (encontrados == 0)
                {
                    Console.WriteLine();
                    Console.WriteLine(
                        tamanhoCabecalhoColunas,
                        "ID".ToUpper(), "NOME".ToUpper(), "EMAIL".ToUpper(), "TELEFONE".ToUpper()
                    );
                }

                Console.WriteLine(
                    tamanhoCabecalhoColunas,
                    f.id, f.nome, f.email, f.telefone
                );

                encontrados++;
            }

            if (encontrados == 0 && msgAoCadastrar)
                Console.WriteLine("Ainda não há fabricantes! Faça um cadastro!");

            if (digitarEnterEContinuar)
                DigitarEnterEContinuar.Executar();

            return encontrados > 0;
        }


        public bool Cadastrar()
        {
            pagina = "Cadastrar chamado";
            ExibirCabecalho(pagina);

            Console.Clear();
            Console.WriteLine("----- Cadastro de Fabricante -----");

            Fabricante dadosIniciais = new Fabricante("", "", "");

            var novosDados = ObterNovosDados(dadosIniciais, false);

            if (novosDados == null)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Cadastro cancelado.");
                Console.ResetColor();
                return false;
            }

            novosDados.id = Fabricante.numeroId++;
            repositorioFabricante.CadastrarRegistro(novosDados);

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"\nFabricante '{novosDados.nome}' cadastrado com sucesso! ID: {novosDados.id}");
            Console.ResetColor();
            Console.Write("Digite [Enter] para continuar...");
            Console.ReadLine();
            return true;
        }

        public bool Editar()
        {
            pagina = "Editar Fabricante";
            ExibirCabecalho(pagina);

            if (!repositorioFabricante.VerificarExistenciaRegistros())
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Nenhum fabricante cadastrado.");
                Console.ResetColor();
                DigitarEnterEContinuar.Executar();
                return false;
            }

            Visualizar(true, false, false);

            while (true)
            {
                Console.Write("\nDigite o Id do fabricante para editar: ");
                if (!int.TryParse(Console.ReadLine(), out int idFabricante))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("ID inválido. Tente novamente.");
                    Console.ResetColor();
                    DigitarEnterEContinuar.Executar();
                    continue;
                }

                if (!repositorioFabricante.TentarObterRegistro(idFabricante, out var fabricanteExistente))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Fabricante não encontrado. Tente novamente.");
                    Console.ResetColor();
                    continue;
                }

                var novosDados = ObterNovosDados(fabricanteExistente, true);
                novosDados.id = fabricanteExistente.id;

                repositorioFabricante.EditarRegistro(idFabricante, novosDados);

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"\nFabricante '{novosDados.nome}' editado com sucesso! id: {novosDados.id}");
                Console.ResetColor();
                DigitarEnterEContinuar.Executar();
                return true;
            }
        }

        public bool Excluir()
        {
            pagina = "Excluir chamado";
            ExibirCabecalho(pagina);

            if (!repositorioFabricante.VerificarExistenciaRegistros())
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Nenhum fabricante cadastrado.");
                Console.ResetColor();
                DigitarEnterEContinuar.Executar();
                return false;
            }


            while (true)
            {
                Console.Write("\nDigite o Id do fabricante para excluir: ");
                if (!int.TryParse(Console.ReadLine(), out int idFabricante))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("ID inválido. Tente novamente.");
                    Console.ResetColor();
                    continue;
                }

                if (!repositorioFabricante.TentarObterRegistro(idFabricante, out var fabricante))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Fabricante não encontrado. Tente novamente.");
                    Console.ResetColor();
                    continue;
                }

                DesejaExcluir desejaExcluir = new DesejaExcluir();
                var vaiExcluir = desejaExcluir.DesejaMesmoExcluir(fabricante.nome);

                if (vaiExcluir != "S") return false;

                repositorioFabricante.ExcluirRegistro(idFabricante);

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"\nFabricante '{fabricante.nome}' excluído com sucesso! id: {fabricante.id}");
                Console.ResetColor();
                DigitarEnterEContinuar.Executar();
                return true;
            }
        }

        private Fabricante ObterNovosDados(Fabricante dadosOriginais, bool editar)
        {
            var tela = new TelaFabricante(null);
            while (true)
            {
                tela.Visualizar(false, false, false);
                pagina = "Cadastrar Fabricante";
                ExibirCabecalho(pagina);

                if (editar)
                {
                    Console.WriteLine();
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("************* Caso não queira alterar um campo, pressione Enter para mantê-lo.");
                    Console.ResetColor();
                }

                string nome = RepositorioBase<Fabricante>.ObterEntrada("Nome", dadosOriginais.nome, editar);
                string email = RepositorioBase<Fabricante>.ObterEntrada("Email", dadosOriginais.email, editar);
                string telefone = RepositorioBase<Fabricante>.ObterEntrada("Telefone", dadosOriginais.telefone, editar);

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
    }
}
