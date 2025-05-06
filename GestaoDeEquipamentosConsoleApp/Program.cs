using GestaoDeEquipamentosConsoleApp.Utils;
using GestaoDeEquipamentosConsoleApp.Apresentacao;

namespace GestaoDeEquipamentosConsoleApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            TelaEquipamento telaEquipamento = new TelaEquipamento();
            TelaChamado telaChamado = new TelaChamado();
            char telaEscolhida = Convert.ToChar(telaChamado.ExibirMenuPrincipal());
            if (telaEscolhida == 'S') return;

            if (telaEscolhida == '1')
            {
                while (true)
                {
                    char opcaoEscolhida = telaEquipamento.ApresentarMenu();

                    if (opcaoEscolhida == 'S')
                    {
                        telaChamado.ExibirMenuPrincipal();
                        break;
                    }

                    switch (opcaoEscolhida)
                    {
                        case '1':
                            telaEquipamento.Cadastrar();
                            break;
                        case '2':
                            telaEquipamento.Visualizar(true, true);
                            break;
                        case '3':
                            telaEquipamento.Editar();
                            break;
                        case '4':
                            telaEquipamento.Excluir();
                            break;
                        default:
                            Console.WriteLine("Digite uma opção válida!");
                            DigitarEnterEContinuar.Executar(true);
                            break;
                    }
                }
            }
            else if(telaEscolhida == '2')
            {
                while (true)
                {
                    char opcaoEscolhida = telaChamado.ApresentarMenu();

                    if (opcaoEscolhida == 'S')
                    {
                        break;
                    }

                    switch (opcaoEscolhida)
                    {
                        case '1':
                            //telaChamado.Cadastrar();
                            break;
                        case '2':
                            //telaChamado.Visualizar(true, true);
                            break;
                        case '3':
                            //telaChamado.Editar();
                            break;
                        case '4':
                            //telaChamado.Excluir();
                            break;
                        default:
                            Console.WriteLine("Digite uma opção válida!");
                            DigitarEnterEContinuar.Executar(true);
                            break;
                    }
                }
            }
        }
    }
}