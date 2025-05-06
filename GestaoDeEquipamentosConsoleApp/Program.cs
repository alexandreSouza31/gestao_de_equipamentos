using GestaoDeEquipamentosConsoleApp.Utils;
using GestaoDeEquipamentosConsoleApp.Apresentacao;

namespace GestaoDeEquipamentosConsoleApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            TelaEquipamento telaEquipamento = new TelaEquipamento();
            while (true)
            {
                char opcaoEscolhida = telaEquipamento.ApresentarMenu();

                if (opcaoEscolhida == 'S')
                {
                    break;
                }

                switch (opcaoEscolhida)
                {
                    case '1':
                        telaEquipamento.Cadastrar();
                        break;
                    case '2':
                        telaEquipamento.Visualizar(true,true);
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
    }
}