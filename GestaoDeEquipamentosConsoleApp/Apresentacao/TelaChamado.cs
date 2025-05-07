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
            Console.Write("\nDigite uma opção: ");
            char opcaoEscolhida = Convert.ToChar(Console.ReadLine()!.ToUpper()[0]);

            return opcaoEscolhida;
        }
        private void ExibirCabecalho(string pagina)
        {
            string nomeSolucao = "Gestão de Chamados";
            Console.Clear();
            Console.WriteLine($"----- {nomeSolucao}/{pagina} -----");
            Console.WriteLine();
        }
        public char ExibirMenuPrincipal()
        {
            string nomeSolucao = "Gestão de Equipamentos";
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
                        "Id".ToUpper(), "Título".ToUpper(), "Descrição".ToUpper(), "Data Abertura".ToUpper(), "Equipamento".ToUpper()
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
                Console.WriteLine("Ainda não há chamados! Faça um cadastro!");
                //msgAoCadastrar = false;
            }

            if (digitarEnterEContinuar) DigitarEnterEContinuar.Executar();
            return encontrados > 0;
        }


        //public void Editar()
        //{
        //    pagina = "Editar chamado";
        //    ExibirCabecalho(pagina);

        //    bool visualizarCadastrados = Visualizar(false, true);
        //    if (!visualizarCadastrados) return;

        //    Equipamento[] equipamentos = repositorioChamado.equipamentos;

        //    while (true)
        //    {
        //        Console.WriteLine();
        //        Console.Write("Digite o Id do equipamento para editar: ");
        //        int idEscolhido = Convert.ToInt32(Console.ReadLine()!);

        //        Equipamento equipamentoSelecionado = null;

        //        for (int i = 0; i < equipamentos.Length; i++)
        //        {
        //            Equipamento e = equipamentos[i];
        //            if (e == null) continue;

        //            if (idEscolhido == e.id)
        //            {
        //                equipamentoSelecionado = e;
        //                break;
        //            }
        //        }

        //        if (equipamentoSelecionado != null)
        //        {
        //            var novosDados = ObterNovosDados(equipamentoSelecionado, true);
        //            AtualizarEquipamento(equipamentoSelecionado, novosDados);

        //            Visualizar(true, false);
        //            Console.WriteLine();
        //            Console.WriteLine($"{equipamentoSelecionado.nome} editado com sucesso! id: {equipamentoSelecionado.id}");
        //            DigitarEnterEContinuar.Executar();
        //            return;
        //        }
        //        else
        //        {
        //            Console.WriteLine();
        //            Console.WriteLine("ID inválido. Tente novamente.");
        //        }
        //    }
        //}

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
        //                Console.WriteLine($"Equipamento excluído com sucesso! id: {idEscolhido}");
        //                DigitarEnterEContinuar.Executar();
        //                equipamentoExcluido |= true;
        //                return;
        //            }
        //        }
        //        if (equipamentoExcluido == false)
        //        {
        //            Console.WriteLine();
        //            Console.WriteLine("ID inválido. Tente novamente.");
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
                Console.WriteLine("************* Caso não queira alterar um campo, basta pressionar Enter para ignorá-lo");
            }

            while (true)
            {
                string etiquetaTitulo = editar ? $"Título ({dadosOriginais.titulo}): " : "Título: ";
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
            //
            //Equipamento[] equipamentos = repositorioEquipamento.equipamentos;
            Equipamento equipamentoSelecionado = null;

            //bool haEquipamentos = VerificarExistenciaEquipamentos();

            //if (!haEquipamentos)
            //{
            //    Console.WriteLine("\nNenhum equipamento cadastrado. Cadastre um equipamento antes de abrir um chamado.");
            //    Console.WriteLine("Voltando ao menu principal...");
            //    Thread.Sleep(2000); // pausa de 2 segundos para leitura da mensagem
            //    return null!;
            //}


            while (equipamentoSelecionado == null)
            {
                Console.Write("Digite o ID do equipamento que deseja associar: ");
                string etiquetaEquipamento = editar ? $"Equipamento ({dadosOriginais.equipamento.nome}): " : "";
                Console.Write(etiquetaEquipamento);

                int inputId = Convert.ToInt32(Console.ReadLine()!);
                //Equipamento equipamentoSelecionado = repositorioEquipamento.SelecionarEquipamentoPorId(inputEquipamentoId);

                //Equipamento equipamentoSelecionado = null;
                if (string.IsNullOrWhiteSpace(Convert.ToString(inputId)) && editar)
                {
                    equipamentoSelecionado = dadosOriginais.equipamento;
                    break;
                }

                if (int.TryParse(Convert.ToString(inputId), out int inputEquipamentoId))
                {
                    equipamentoSelecionado = repositorioEquipamento.SelecionarEquipamentoPorId(inputEquipamentoId);
                    if (equipamentoSelecionado == null)
                    {
                        Console.WriteLine("\nEquipamento não encontrado. Tente novamente.");
                    }
                }
                else
                {
                    Console.WriteLine("Entrada inválida. Digite um número de ID ou pressione Enter para manter o atual.");
                }
            }

            novosDados.equipamento = equipamentoSelecionado;

            return novosDados;

            //    for (int i = 0; i < equipamentos.Length; i++)
            //    {
            //        Equipamento e = equipamentos[i];
            //        if (e == null) continue;

            //        if (inputEquipamentoId == e.id)
            //        {
            //            equipamentoSelecionado = e;
            //            break;
            //        }
            //    }

            //    if (equipamentoSelecionado != null)
            //    {
            //        if (equipamentoSelecionado) novosDados.equipamento = dadosOriginais.equipamento;
            //        else equipamentoSelecionado = novosDados.equipamento=equipamentoSelecionado;
            //         //novosDados.equipamento = equipamentoSelecionado ? dadosOriginais.equipamento : equipamentoSelecionado;
            //    }
            //    else
            //    {
            //        Console.WriteLine();
            //        Console.WriteLine("ID inválido. Tente novamente.");
            //    }
            //}
            //return novosDados;

            //    int inputEquipamentoId = Convert.ToInt32(Console.ReadLine()!);
            //Equipamento equipamentoSelecionado = repositorioEquipamento.SelecionarEquipamentoPorId(inputEquipamentoId);
            //novosDados.equipamento = equipamentoSelecionado;
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