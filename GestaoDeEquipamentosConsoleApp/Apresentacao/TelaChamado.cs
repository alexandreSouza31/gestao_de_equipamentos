using GestaoDeEquipamentosConsoleApp.Dados;
using GestaoDeEquipamentosConsoleApp.Negocio;
using GestaoDeEquipamentosConsoleApp.Utils;

namespace GestaoDeEquipamentosConsoleApp.Apresentacao
{
    public class TelaChamado
    {
        public string pagina;
        public RepositorioChamado repositorioChamado = new RepositorioChamado();
        public static RepositorioEquipamento repositorioEquipamento;
        public char ApresentarMenu()
        {
            ExibirCabecalho("");
            Console.WriteLine();
            Console.WriteLine("1 - Cadastrar Chamado");
            Console.WriteLine("2 - Visualizar Chamado");
            Console.WriteLine("3 - Editar Chamado");
            Console.WriteLine("4 - Excluir Chamado");
            Console.WriteLine("S - Sair");
            Console.Write("\nDigite uma op��o: ");
            char opcaoEscolhida = Convert.ToChar(Console.ReadLine()!.ToUpper()[0]);

            return opcaoEscolhida;
        }

        public bool ExibirMenuChamado(TelaChamado telaChamado)
        {
            char opcaoEscolhida = telaChamado.ApresentarMenu();

            if (opcaoEscolhida == 'S') return false;

            switch (opcaoEscolhida)
            {
                case '1':
                    telaChamado.Cadastrar();
                    break;
                case '2':
                    telaChamado.Visualizar(true, true);
                    break;
                case '3':
                    telaChamado.Editar();
                    break;
                default:
                    Console.WriteLine("Digite uma op��o v�lida!");
                    DigitarEnterEContinuar.Executar(true);
                    break;
            }
            return true;
        }

        private void ExibirCabecalho(string pagina)
        {
            string nomeSolucao = "Gest�o de Chamados";
            Console.Clear();
            Console.WriteLine($"----- {nomeSolucao}/{pagina} -----");
            Console.WriteLine();
        }
        public char ExibirMenuPrincipal()
        {
            string nomeSolucao = "Gest�o de Equipamentos";
            Console.Clear();
            Console.WriteLine($"----- {nomeSolucao} -----");
            Console.WriteLine();

            char telaEscolhida;
            Console.WriteLine("1 - Equipamentos");
            Console.WriteLine("2 - Chamados");
            Console.WriteLine("S - Sair");
            telaEscolhida = Convert.ToChar(Console.ReadLine()!.ToUpper());
            return telaEscolhida;
        }

        public bool Cadastrar()
        {
            pagina = "Cadastrar chamado";
            Chamado chamado = new Chamado();
            Equipamento[] equipamentos = repositorioEquipamento.equipamentos;

            ExibirCabecalho(pagina);
            chamado.id = Equipamento.numeroId++;

            bool haEquipamentos = VerificarExistenciaEquipamentos();

            if (!haEquipamentos)
            {
                Console.WriteLine("\nNenhum equipamento cadastrado. Cadastre um equipamento antes de abrir um chamado.");
                Console.WriteLine("Voltando ao menu principal...");
                Thread.Sleep(7000);
                ExibirMenuPrincipal();
                return false;
            }

            var novosdados = ObterNovosDados(chamado, false);
            AtualizarChamado(chamado, novosdados);

            repositorioChamado.CadastrarEquipamento(chamado);
            Console.WriteLine($"chamado {chamado.titulo} cadastrado com sucesso! id: {chamado.id}");
            DigitarEnterEContinuar.Executar();
            return true;
        }

        public bool Visualizar(bool exibirCabecalho, bool digitarEnterEContinuar, bool msgAoCadastrar =true)
        {
            pagina = "Visualizar chamado";
            if (exibirCabecalho) ExibirCabecalho(pagina);

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
                        "Id".ToUpper(), "T�tulo".ToUpper(), "Descri��o".ToUpper(), "Data Abertura".ToUpper(), "Equipamento".ToUpper()
                    );
                }

                Console.WriteLine(
                    tamanhoCabecalhoColunas,
                    e.id, e.titulo, e.descricao, e.dataAbertura.ToShortDateString(), e.equipamento
                );

                encontrados++;
            }

            if (encontrados == 0)
            {
                if(msgAoCadastrar)
                Console.WriteLine("Ainda n�o h� chamados! Fa�a um cadastro!");
            }

            if (digitarEnterEContinuar) DigitarEnterEContinuar.Executar();
            return encontrados > 0;
        }


        public bool Editar()
        {
            pagina = "Editar chamado";
            ExibirCabecalho(pagina);

            bool haChamados = Visualizar(false, false, false);
            if (!haChamados)
            {
                Console.WriteLine("\nNenhum chamado cadastrado ainda.");
                Console.WriteLine("Voltando ao menu de chamados...");
                Thread.Sleep(4000);

                return false;
            }

            bool haEquipamentos = VerificarExistenciaEquipamentos();

            if (!haEquipamentos)
            {
                Console.WriteLine("\nNenhum equipamento cadastrado. Cadastre um equipamento antes de abrir um chamado.");
                Console.WriteLine("Voltando ao menu principal...");
                Thread.Sleep(7000);
                
                ExibirMenuPrincipal();
                return false;
            }

            while (true)
            {
                Console.WriteLine();
                Console.Write("Digite o Id do chamado para editar: ");
                if (!int.TryParse(Console.ReadLine(), out int idChamado))
                {
                    Console.WriteLine("ID inv�lido. Tente novamente.");
                    continue;
                }

                Chamado chamadoExistente = repositorioChamado.SelecionarChamadoPorId(idChamado);

                if (chamadoExistente == null)
                {
                    Console.WriteLine("Chamado n�o encontrado. Tente novamente.");
                    continue;
                }
                
                Chamado novosDados = ObterNovosDados(chamadoExistente, true);
                novosDados.id = chamadoExistente.id;
                AtualizarChamado(chamadoExistente, novosDados);

                Visualizar(true, false);
                Console.WriteLine();
                Console.WriteLine($"{chamadoExistente.titulo} editado com sucesso! id: {chamadoExistente.id}");
                DigitarEnterEContinuar.Executar();
                return true;     
            }
        }

        //internal void Excluir()
        //{
        //    pagina = "Excluir chamado";
        //    ExibirCabecalho(pagina);

        //    bool visualizarCadastrados = Visualizar(false, true);
        //    if (visualizarCadastrados == false) return;

        //    Equipamento[] equipamentos = repositorioChamado.equipamentos;
        //    bool equipamentoExcluido = false;

        //    while (true)
        //    {
        //        Console.WriteLine();
        //        Console.Write("Digite o Id do equipamento para excluir: ");
        //        int idEscolhido = Convert.ToInt32(Console.ReadLine()!);

        //        for (int i = 0; i < equipamentos.Length; i++)
        //        {
        //            if (equipamentos[i] == null) continue;

        //            if (idEscolhido == equipamentos[i].id)
        //            {
        //                equipamentos[i] = null;
        //                Console.WriteLine();
        //                Console.WriteLine($"Equipamento exclu�do com sucesso! id: {idEscolhido}");
        //                DigitarEnterEContinuar.Executar();
        //                equipamentoExcluido |= true;
        //                return;
        //            }
        //        }
        //        if (equipamentoExcluido == false)
        //        {
        //            Console.WriteLine();
        //            Console.WriteLine("ID inv�lido. Tente novamente.");
        //        }
        //    }
        //}

        public static Chamado ObterNovosDados(Chamado dadosOriginais, bool editar)
        {
            Chamado novosDados = new Chamado();
            var tela = new TelaChamado();

            tela.Visualizar(true, false, false);

            if (editar == true)
            {
                Console.WriteLine();
                Console.WriteLine("************* Caso n�o queira alterar um campo, basta pressionar Enter para ignor�-lo");
            }

            while (true)
            {
                string etiquetaTitulo = editar ? $"T�tulo ({dadosOriginais.titulo}): " : "T�tulo: ";
                Console.Write(etiquetaTitulo);

                string inputTitulo = Console.ReadLine()!;
                novosDados.titulo = string.IsNullOrWhiteSpace(inputTitulo) ? dadosOriginais.titulo : inputTitulo;
                break;
            }

            string etiquetaDescricao = editar ? $"Descricao ({dadosOriginais.descricao}): " : "Descricao: ";
            Console.Write(etiquetaDescricao);

            string inputDescricao = Console.ReadLine()!;
            novosDados.descricao = string.IsNullOrWhiteSpace(inputDescricao) ? dadosOriginais.descricao : inputDescricao;

            DateTime inputData = DateTime.Now;
            string etiquetaDataAbertura = editar ? $"Data de Abertura ({dadosOriginais.dataAbertura.ToShortDateString()}): " : $"Data de Abertura: {inputData}";
            Console.Write(etiquetaDataAbertura);

            novosDados.dataAbertura = inputData;
            Console.WriteLine();

            Equipamento equipamentoSelecionado = null;

            while (equipamentoSelecionado == null)
            {
                Console.Write("Digite o ID do equipamento que deseja associar: ");
                string etiquetaEquipamento = editar ? $"Equipamento ({dadosOriginais.equipamento.nome}): " : "";
                Console.Write(etiquetaEquipamento);

                string inputId = Console.ReadLine()!;

                if (string.IsNullOrWhiteSpace(inputId) && editar)
                {
                    equipamentoSelecionado = dadosOriginais.equipamento;
                    break;
                }

                if (int.TryParse(inputId, out int inputEquipamentoId))
                {
                    equipamentoSelecionado = repositorioEquipamento.SelecionarEquipamentoPorId(inputEquipamentoId);
                    if (equipamentoSelecionado == null)
                    {
                        Console.WriteLine("\nEquipamento n�o encontrado. Tente novamente.");
                    }
                }
                else
                {
                    Console.WriteLine("Entrada inv�lida. Digite um n�mero de ID ou pressione Enter para manter o atual.");
                }
            }

            novosDados.equipamento = equipamentoSelecionado;

            return novosDados;
       }

        private static bool VerificarExistenciaEquipamentos()
        {
            Equipamento[] equipamentos = repositorioEquipamento.equipamentos;

            bool haEquipamentos = false;
            for (int i = 0; i < equipamentos.Length; i++)
            {
                if (equipamentos[i] != null)
                {
                    haEquipamentos = true;
                    break;
                }
            }

            return haEquipamentos;
        }

        public static void AtualizarChamado(Chamado dadosOriginais, Chamado novosDados)
        {
            dadosOriginais.titulo = novosDados.titulo;
            dadosOriginais.descricao = novosDados.descricao;
            dadosOriginais.dataAbertura = novosDados.dataAbertura;
            dadosOriginais.equipamento = novosDados.equipamento;
        }
    }
}