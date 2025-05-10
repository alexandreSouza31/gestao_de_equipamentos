using GestaoDeEquipamentosConsoleApp.Utils;
using GestaoDeEquipamentosConsoleApp.Dados;
using GestaoDeEquipamentosConsoleApp.Negocio;

namespace GestaoDeEquipamentosConsoleApp.Apresentacao
{
    public class TelaFabricante
    {
        public RepositorioFabricante repositorioFabricante;
        public Direcionar direcionar=new Direcionar();
        Fabricante fabricante = new Fabricante();

        public TelaFabricante()
        {
            if (repositorioFabricante == null)
                repositorioFabricante = new RepositorioFabricante();
        }

        public TelaFabricante(RepositorioFabricante repositorioFabricante)
        {
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

        public bool ExibirMenuFabricante(TelaFabricante telaFabricante)
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
                    // telaFabricante.Excluir();
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
            Console.Clear();
            Console.WriteLine("----- Fabricantes Registrados -----");

            var todos = repositorioFabricante.ObterTodos();

            if(msgAoCadastrar)
            {
                if (todos.Length == 0)
                {
                    Console.WriteLine("Nenhum fabricante registrado.");
                    Console.WriteLine();
                    Console.Write("Digite [Enter] para continuar...");
                    Console.ReadLine();
                    return false;
                }
            }

            foreach (var f in todos)
            {
                Console.WriteLine($"ID: {f.id} | Nome: {f.nome} | Email: {f.email} | Telefone: {f.telefone}");
            }

            if (digitarEnterEContinuar) DigitarEnterEContinuar.Executar();
            return true;
        }

        public bool Cadastrar()
        {
            Console.Clear();
            Console.WriteLine("----- Cadastro de Fabricante -----");

            var novosDados = ObterNovosDados(fabricante, false);
            AtualizarFabricante(fabricante, novosDados);

            fabricante.id = fabricante.numeroId++;
            repositorioFabricante.Inserir(fabricante);

            Console.WriteLine($"\nFabricante '{fabricante.nome}' cadastrado com sucesso! ID: {fabricante.id}");
            Console.Write("Digite [Enter] para continuar...");
            Console.ReadLine();
            return true;
        }

        public bool Editar()
        {
            //pagina = "Editar chamado";
            //ExibirCabecalho(pagina);

            bool visualizarCadastrados = Visualizar(false, false, false);

            var todos = repositorioFabricante.ObterTodos();
            bool haFabricantes = repositorioFabricante.contadorFabricantes > 0;
            bool continuar = direcionar.DirecionarParaMenu(haFabricantes, false, "Fabricante");
            if (!continuar) return false;

            while (true)
            {
                Console.WriteLine();
                Console.Write("Digite o Id do fabricante para editar: ");
                if (!int.TryParse(Console.ReadLine(), out int idFabricante))
                {
                    Console.WriteLine("ID inválido. Tente novamente.");
                    continue;
                }

                Fabricante fabricanteExistente = repositorioFabricante.BuscarPorId(idFabricante);

                if (fabricanteExistente == null)
                {
                    Console.WriteLine("Fabricante não encontrado. Tente novamente.");
                    continue;
                }

                Fabricante novosDados = ObterNovosDados(fabricanteExistente, true);
                novosDados.id = fabricanteExistente.id;
                AtualizarFabricante(fabricanteExistente, novosDados);

                Visualizar(true, false,false);
                Console.WriteLine();
                Console.WriteLine($"{fabricanteExistente.nome} editado com sucesso! id: {fabricanteExistente.id}");
                DigitarEnterEContinuar.Executar();
                return true;
            }
        }

        public static Fabricante ObterNovosDados(Fabricante dadosOriginais, bool editar)
        {
            Fabricante novosDados = new Fabricante();
            var tela = new TelaFabricante();

            tela.Visualizar(true, false, false);

            if (editar)
            {
                Console.WriteLine();
                Console.WriteLine("************* Caso não queira alterar um campo, pressione Enter para mantê-lo.");
            }

            Console.Write(editar ? $"Nome ({dadosOriginais.nome}): " : "Nome: ");
            string inputNome = Console.ReadLine();
            novosDados.nome = string.IsNullOrWhiteSpace(inputNome) ? dadosOriginais.nome : inputNome;

            Console.Write(editar ? $"Email ({dadosOriginais.email}): " : "Email: ");
            string inputEmail = Console.ReadLine();
            novosDados.email = string.IsNullOrWhiteSpace(inputEmail) ? dadosOriginais.email : inputEmail;

            Console.Write(editar ? $"Telefone ({dadosOriginais.telefone}): " : "Telefone: ");
            string inputTelefone = Console.ReadLine();
            novosDados.telefone = string.IsNullOrWhiteSpace(inputTelefone) ? dadosOriginais.telefone : inputTelefone;

            return novosDados;
        }

        public static void AtualizarFabricante(Fabricante original, Fabricante novosDados)
        {
            original.nome = novosDados.nome;
            original.email = novosDados.email;
            original.telefone = novosDados.telefone;
        }
    }
}
