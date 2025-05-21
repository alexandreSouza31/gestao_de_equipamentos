using GestaoDeEquipamentosConsoleApp.Utils;
using GestaoDeEquipamentosConsoleApp.Dados;
using GestaoDeEquipamentosConsoleApp.Negocio;
using static System.Runtime.InteropServices.JavaScript.JSType;
using GestaoDeEquipamentosConsoleApp.Compartilhado;

namespace GestaoDeEquipamentosConsoleApp.Apresentacao
{
    public class TelaFabricante
    {
        private RepositorioFabricante repositorioFabricante;
        public Direcionar direcionar=new Direcionar();
        public string pagina;

        public TelaFabricante(RepositorioFabricante repositorioFabricante)
        {
            if (repositorioFabricante == null)
                repositorioFabricante = new RepositorioFabricante();
            this.repositorioFabricante = repositorioFabricante;
        }

        private void ExibirCabecalho(string pagina)
        {
            string nomeSolucao = "Gestão de Fabricantes";
            Console.Clear();
            Console.WriteLine($"----- {nomeSolucao}/{pagina} -----");
            Console.WriteLine();
        }

        public char ApresentarMenu()
        {
            ExibirCabecalho("");
            Console.WriteLine();
            Console.WriteLine("1 - Cadastrar Fabricante");
            Console.WriteLine("2 - Visualizar Fabricante");
            Console.WriteLine("3 - Editar Fabricante");
            Console.WriteLine("4 - Excluir Fabricante");
            Console.WriteLine("S - Sair");
            Console.Write("\nDigite uma opção: ");
            char opcaoEscolhida = Convert.ToChar(Console.ReadLine()!.ToUpper()[0]);

            return opcaoEscolhida;
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
                    Console.WriteLine("Digite uma opção válida!");
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
                Console.WriteLine("Cadastro cancelado.");
                return false;
            }

            novosDados.id = Fabricante.numeroId++;
            repositorioFabricante.CadastrarRegistro(novosDados);

            Console.WriteLine($"\nFabricante '{novosDados.nome}' cadastrado com sucesso! ID: {novosDados.id}");
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
                Console.WriteLine("Nenhum fabricante cadastrado.");
                DigitarEnterEContinuar.Executar();
                return false;
            }

            Visualizar(true, false, false);

            while (true)
            {
                Console.Write("\nDigite o Id do fabricante para editar: ");
                if (!int.TryParse(Console.ReadLine(), out int idFabricante))
                {
                    Console.WriteLine("ID inválido. Tente novamente.");
                    DigitarEnterEContinuar.Executar();
                    continue;
                }

                if (!repositorioFabricante.TentarObterRegistro(idFabricante, out var fabricanteExistente))
                {
                    Console.WriteLine("Fabricante não encontrado. Tente novamente.");
                    continue;
                }

                var novosDados = ObterNovosDados(fabricanteExistente, true);
                novosDados.id = fabricanteExistente.id;

                repositorioFabricante.EditarRegistro(idFabricante, novosDados);

                Console.WriteLine($"\nFabricante '{novosDados.nome}' editado com sucesso! id: {novosDados.id}");
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
                Console.WriteLine("Nenhum fabricante cadastrado.");
                DigitarEnterEContinuar.Executar();
                return false;
            }


            while (true)
            {
                Console.Write("\nDigite o Id do fabricante para excluir: ");
                if (!int.TryParse(Console.ReadLine(), out int idFabricante))
                {
                    Console.WriteLine("ID inválido. Tente novamente.");
                    continue;
                }

                if (!repositorioFabricante.TentarObterRegistro(idFabricante, out var fabricante))
                {
                    Console.WriteLine("Fabricante não encontrado. Tente novamente.");
                    continue;
                }

                DesejaExcluir desejaExcluir = new DesejaExcluir();
                var vaiExcluir = desejaExcluir.DesejaMesmoExcluir(fabricante.nome);

                if (vaiExcluir != "S") return false;

                repositorioFabricante.ExcluirRegistro(idFabricante);

                Console.WriteLine($"\nFabricante '{fabricante.nome}' excluído com sucesso! id: {fabricante.id}");
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
                    Console.WriteLine("************* Caso não queira alterar um campo, pressione Enter para mantê-lo.");
                }

                string nome = RepositorioBase<Fabricante>.ObterEntrada("Nome", dadosOriginais.nome, editar);
                string email = RepositorioBase<Fabricante>.ObterEntrada("Email", dadosOriginais.email, editar);
                string telefone = RepositorioBase<Fabricante>.ObterEntrada("Telefone", dadosOriginais.telefone, editar);

                string[] nomesCampos = { "nome", "email", "telefone" };
                string[] valoresCampos = { nome, email, telefone };
                string erros = ValidarCampo.ValidarCampos(nomesCampos, valoresCampos);

                if (!string.IsNullOrEmpty(erros))
                {
                    Console.WriteLine("\nErros encontrados:");
                    Console.WriteLine(erros);
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
