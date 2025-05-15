namespace GestaoDeEquipamentosConsoleApp.Negocio
{
    public class Fabricante
    {
        public static int numeroId = 1;
        public int id;
        public string nome;
        public string email;
        public string telefone;
    public Fabricante(string nome, string email, string telefone)
        {
            this.nome = nome;
            this.email = email;
            this.telefone = telefone;
        }
    }
}
