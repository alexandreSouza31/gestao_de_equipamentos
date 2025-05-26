using GestaoDeEquipamentosConsoleApp.Compartilhado;

namespace GestaoDeEquipamentosConsoleApp.Negocio
{
    public class Fabricante : IEntidade
    {
        private static int numeroId = 1;

        public int id { get; set; }
        public string nome { get; set; }
        public string email { get; set; }
        public string telefone { get; set; }

        public Fabricante(string nome, string email, string telefone)
        {
            this.id = numeroId++;
            this.nome = nome;
            this.email = email;
            this.telefone = telefone;
        }

        public Fabricante() {}

        public override string ToString()
        {
            return nome;
        }
    }
}