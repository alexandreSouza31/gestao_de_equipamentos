using GestaoDeEquipamentosConsoleApp.Utils;
using GestaoDeEquipamentosConsoleApp.Dados;
using GestaoDeEquipamentosConsoleApp.Negocio;

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
            pagina = "Visualizar chamado";
            ExibirCabecalho(pagina);

            Console.Clear();
            if (exibirCabecalho)
                Console.WriteLine("----- Fabricantes Registrados -----");

            bool haEquipamentos = repositorioFabricante.VerificarExistenciaFabricantes();

            bool haChamados = repositorioFabricante.SelecionarFabricantes().Length > 0;
            var resultado = direcionar.DirecionarParaMenu(haChamados, false, "Chamado");
            if (resultado != ResultadoDirecionamento.Continuar) return false;

            Fabricante[] fabricantes = repositorioFabricante.SelecionarFabricantes();
            int encontrados = 0;

            string tamanhoCabecalhoColunas = "{0, -5} | {1, -20} | {2, -25} | {3, -15}";

            for (int i = 0; i < fabricantes.Length; i++)
            {
                Fabricante f = fabricantes[i];
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

            Fabricante dadosIniciais  = new Fabricante("", "", "");

            var novosDados = ObterNovosDados(dadosIniciais, false);
            AtualizarFabricante(dadosIniciais, novosDados);

            dadosIniciais.id = Fabricante.numeroId++;
            repositorioFabricante.CadastrarFabricante(dadosIniciais);

            Console.WriteLine($"\nFabricante '{dadosIniciais.nome}' cadastrado com sucesso! ID: {dadosIniciais.id}");
            Console.Write("Digite [Enter] para continuar...");
            Console.ReadLine();
            return true;
        }

        public bool Editar()
        {
            pagina = "Editar chamado";
            ExibirCabecalho(pagina);


            var todos = repositorioFabricante.SelecionarFabricantes();
            bool haFabricantes = repositorioFabricante.SelecionarFabricantes().Length > 0;
            var resultado = direcionar.DirecionarParaMenu(haFabricantes, false, "Fabricante");
            if (resultado != ResultadoDirecionamento.Continuar) return false;

            bool visualizarCadastrados = Visualizar(false, false, false);
            while (true)
            {
                Console.WriteLine();
                Console.Write("Digite o Id do fabricante para editar: ");
                if (!int.TryParse(Console.ReadLine(), out int idFabricante))
                {
                    Console.WriteLine("ID inválido. Tente novamente.");
                    continue;
                }

                Fabricante fabricanteExistente = repositorioFabricante.SelecionarFabricantePorId(idFabricante);

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

        public bool Excluir()
        {
            pagina = "Excluir chamado";
            ExibirCabecalho(pagina);

            bool haFabricantes = repositorioFabricante.VerificarExistenciaFabricantes();
            var resultado = direcionar.DirecionarParaMenu(haFabricantes, false, "Fabricante");
            if (resultado != ResultadoDirecionamento.Continuar) return false;

            bool visualizarCadastrados = Visualizar(false, false,false);
            if (!visualizarCadastrados) return false;

            Fabricante[] fabricantes = repositorioFabricante.SelecionarFabricantes();

            while (true)
            {
                Console.WriteLine();
                Console.Write("Digite o Id do Fabricante para excluir: ");

                bool idValido = (!int.TryParse(Console.ReadLine(), out int idEscolhido));
                var fabricante = repositorioFabricante.SelecionarFabricantePorId(idEscolhido);

                if (!idValido && fabricante == null)
                {
                    Console.WriteLine("ID inválido. Tente novamente.");
                    continue;
                }

                for (int i = 0; i < fabricantes.Length; i++)
                {
                    if (fabricantes[i] == null) continue;

                    if (idEscolhido == fabricantes[i].id)
                    {
                        DesejaExcluir desejaExcluir = new DesejaExcluir();
                        var vaiExcluir = desejaExcluir.DesejaMesmoExcluir(fabricante.nome);
                        if (vaiExcluir != "S") return false;

                        Console.WriteLine();
                        Console.WriteLine($"Fabricante {fabricante.nome} excluído com sucesso! id: {idEscolhido}");
                        fabricantes[i] = null;
                        DigitarEnterEContinuar.Executar();
                        return true;
                    }
                }
            }
        }

        private static Fabricante ObterNovosDados(Fabricante dadosOriginais, bool editar)
        {
            var tela = new TelaFabricante(null);

            tela.Visualizar(true, false, false);

            if (editar)
            {
                Console.WriteLine();
                Console.WriteLine("************* Caso não queira alterar um campo, pressione Enter para mantê-lo.");
            }

            Console.Write(editar ? $"Nome ({dadosOriginais.nome}): " : "Nome: ");
            string nome = Console.ReadLine();

            Console.Write(editar ? $"Email ({dadosOriginais.email}): " : "Email: ");
            string email = Console.ReadLine();

            Console.Write(editar ? $"Telefone ({dadosOriginais.telefone}): " : "Telefone: ");
            string telefone = Console.ReadLine();

            nome = string.IsNullOrWhiteSpace(nome) ? dadosOriginais.nome : nome;

            email = string.IsNullOrWhiteSpace(email) ? dadosOriginais.email : email;

            telefone = string.IsNullOrWhiteSpace(telefone) ? dadosOriginais.telefone : telefone;

            return new Fabricante(nome, email, telefone); ;
        }

        public static void AtualizarFabricante(Fabricante original, Fabricante novosDados)
        {
            original.nome = novosDados.nome;
            original.email = novosDados.email;
            original.telefone = novosDados.telefone;
        }
    }
}
