using System.Text;
using System.Security.Cryptography;


namespace PlataformaCursosAPI.Helpers
{
    public static class SenhaHelper
    {
        // Gera o hash da senha
        public static string GerarHash(string senha)
        {
            using (var sha256 = SHA256.Create())
            {
                var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(senha));
                return Convert.ToBase64String(bytes);
            }
        }

        // Verifica se a senha fornecida corresponde ao hash armazenado
        public static bool VerificarHash(string senha, string senhaHash)
        {
            var senhaGerada = GerarHash(senha);
            return senhaGerada == senhaHash;
        }
    }
}
