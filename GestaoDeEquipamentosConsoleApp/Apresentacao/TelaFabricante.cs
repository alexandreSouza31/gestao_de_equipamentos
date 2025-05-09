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
                    telaFabricante.Visualizar();
                    break;
                case '3':
                    //telaFabricante.Editar();
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

        public bool Visualizar()
        {
            Console.Clear();
            Console.WriteLine("----- Fabricantes Registrados -----");

            var todos = repositorioFabricante.ObterTodos();

            if (todos.Length == 0)
            {
                Console.WriteLine("Nenhum fabricante registrado.");
                Console.WriteLine();
                Console.Write("Digite [Enter] para continuar...");
                Console.ReadLine();
                return false;
            }

            foreach (var f in todos)
            {
                Console.WriteLine($"ID: {f.id} | Nome: {f.nome} | Email: {f.email} | Telefone: {f.telefone}");
            }

            Console.WriteLine();
            Console.Write("Digite [Enter] para continuar...");
            Console.ReadLine();
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


        public static Fabricante ObterNovosDados(Fabricante dadosOriginais, bool editar)
        {
            Fabricante novosDados = new Fabricante();

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
