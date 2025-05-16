using System.Net.Mail;
namespace GestaoDeEquipamentosConsoleApp.Utils
{
    public class ValidarCampo
    {
        public static string ValidarCampos(string[] nomesCampos, string[] valoresCampos)
        {
            string erros = "";

            for (int i = 0; i < nomesCampos.Length; i++)
            {
                string nomeCampo = nomesCampos[i];
                string valorCampo = valoresCampos[i];

                if (string.IsNullOrWhiteSpace(valorCampo))
                {
                    erros += $"O campo '{nomeCampo}' é obrigatório!\n";
                    continue;
                }

                if (nomeCampo.ToLower() == "email")
                {
                    if (!MailAddress.TryCreate(valorCampo, out _))
                    {
                        erros += $"O campo '{nomeCampo}' deve conter um e-mail válido (ex: nome@provedor.com).\n";
                    }
                }
                if (nomeCampo.ToLower() == "nome")
                {
                    int numeroMinimoCaracteres = 6;

                    if (valorCampo.ToLower().Length < numeroMinimoCaracteres)
                    {
                        erros += $"Nome deve ter mais de {numeroMinimoCaracteres} caracteres!\n";
                    }
                }
            }
            return erros;
        }
    }
}