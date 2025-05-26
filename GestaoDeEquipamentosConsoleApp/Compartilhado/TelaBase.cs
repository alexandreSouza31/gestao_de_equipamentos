using GestaoDeEquipamentosConsoleApp.Dados;
using GestaoDeEquipamentosConsoleApp.Negocio;
using GestaoDeEquipamentosConsoleApp.Utils;
using System.Security.Cryptography.X509Certificates;

namespace GestaoDeEquipamentosConsoleApp.Compartilhado
{
    public abstract class TelaBase<T> where T : IEntidade
    {
        protected string nomeEntidade;
        protected RepositorioBase<T> repositorio;

        protected TelaBase(string nomeEntidade, RepositorioBase<T> repositorio)
        {
            this.nomeEntidade = nomeEntidade;
            this.repositorio = repositorio;
        }

        protected void ExibirCabecalho()
        {
            string nomeSolucao = "Gestão de Equipamentos";
            Console.Clear();
            Console.WriteLine($"----- {nomeSolucao}/{nomeEntidade} -----");
            Console.WriteLine();
        }

        public char ApresentarMenu()
        {
            ExibirCabecalho();
            Console.WriteLine();
            Console.WriteLine($"1 - Cadastrar {nomeEntidade}");
            Console.WriteLine($"2 - Visualizar {nomeEntidade}");
            Console.WriteLine($"3 - Editar {nomeEntidade}");
            Console.WriteLine($"4 - Excluir {nomeEntidade}");
            Console.WriteLine("S - Sair");
            Console.Write("\nDigite uma opção: ");
            char opcaoEscolhida = Convert.ToChar(Console.ReadLine()!.ToUpper()[0]);

            return opcaoEscolhida;
        }

        public bool Cadastrar()
        {
            ExibirCabecalho();

            T dadosIniciais = CriarInstanciaVazia();

            var novosDados = ObterNovosDados(dadosIniciais, false);

            if (novosDados == null)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Cadastro cancelado.");
                Console.ResetColor();
                return false;
            }

            repositorio.CadastrarRegistro(novosDados);

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"\n {nomeEntidade} cadastrado com sucesso! ID: {novosDados.id}");
            Console.ResetColor();
            Console.Write("Digite [Enter] para continuar...");
            Console.ReadLine();
            return true;
        }

        public bool Visualizar(bool exibirCabecalho, bool digitarEnterEContinuar, bool msgAoCadastrar = true)
        {
            //pagina = "Visualizar Fabricante";
            ExibirCabecalho();

            Console.Clear();
            if (exibirCabecalho)
                ExibirCabecalho();
            Console.WriteLine($"----- {nomeEntidade}s Registrados -----");

            bool haRegistros = repositorio.VerificarExistenciaRegistros();

            if (!haRegistros && msgAoCadastrar)
            {
                Console.WriteLine($"Ainda não há {nomeEntidade.ToLower()}s! Faça um cadastro!");
                if (digitarEnterEContinuar)
                    DigitarEnterEContinuar.Executar();
                return false;
            }

            T[] registros = repositorio.SelecionarRegistros();
            int encontrados = 0;

            foreach (T reg in registros)
            {
                if (reg == null) continue;

                if (encontrados == 0)
                {
                    Console.WriteLine();
                    ImprimirCabecalhoTabela();
                }

                ImprimirRegistro(reg);

                encontrados++;
            }

            if (digitarEnterEContinuar)
                DigitarEnterEContinuar.Executar();

            return encontrados > 0;
        }

        public bool Editar()
        {
            //pagina = "Editar Fabricante";
            ExibirCabecalho();

            if (!repositorio.VerificarExistenciaRegistros())
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Nenhum {nomeEntidade} cadastrado.");
                Console.ResetColor();
                DigitarEnterEContinuar.Executar();
                return false;
            }

            Visualizar(true, false, false);

            while (true)
            {
                Console.Write($"\nDigite o Id do {nomeEntidade} para editar: ");
                if (!int.TryParse(Console.ReadLine(), out int idRegistro))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("ID inválido. Tente novamente.");
                    Console.ResetColor();
                    DigitarEnterEContinuar.Executar();
                    continue;
                }

                if (!repositorio.TentarObterRegistro(idRegistro, out var registroExistente))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"{nomeEntidade} não encontrado. Tente novamente!");
                    Console.ResetColor();
                    continue;
                }

                var novosDados = ObterNovosDados(registroExistente, true);
                novosDados.id = registroExistente.id;

                repositorio.EditarRegistro(idRegistro, novosDados);

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"\n{nomeEntidade} editado com sucesso! id: {novosDados.id}");
                Console.ResetColor();
                DigitarEnterEContinuar.Executar();
                return true;
            }
        }

        public bool Excluir()
        {
            //pagina = "Excluir chamado";
            ExibirCabecalho();

            if (!repositorio.VerificarExistenciaRegistros())
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Nenhum {nomeEntidade} cadastrado.");
                Console.ResetColor();
                DigitarEnterEContinuar.Executar();
                return false;
            }

            Visualizar(true, false, false);
            while (true)
            {
                Console.Write($"\nDigite o Id do {nomeEntidade} para excluir: ");
                if (!int.TryParse(Console.ReadLine(), out int idRegistro))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("ID inválido. Tente novamente.");
                    Console.ResetColor();
                    continue;
                }

                if (!repositorio.TentarObterRegistro(idRegistro, out var registro))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"{nomeEntidade} não encontrado. Tente novamente.");
                    Console.ResetColor();
                    continue;
                }

                DesejaExcluir desejaExcluir = new DesejaExcluir();
                var vaiExcluir = desejaExcluir.DesejaMesmoExcluir($"esse {nomeEntidade}");

                if (vaiExcluir != "S") return false;

                repositorio.ExcluirRegistro(idRegistro);

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"\n{nomeEntidade} excluído com sucesso! id: {registro.id}");
                Console.ResetColor();
                DigitarEnterEContinuar.Executar();
                return true;
            }
        }

        public abstract T CriarInstanciaVazia();

        protected abstract T ObterNovosDados(T dadosIniciais, bool editar);

        protected abstract void ImprimirCabecalhoTabela();

        protected abstract void ImprimirRegistro(T entidade);
    }
}
