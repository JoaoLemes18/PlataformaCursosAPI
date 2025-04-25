using System.Security.Cryptography;
using System.Text;

namespace PlataformaCursosAPI.Helpers
{
    public static class SenhaHelper
    {
        // Método para gerar o hash da senha
        public static string GerarHash(string senha)
        {
            using (var sha256 = SHA256.Create())
            {
                var senhaBytes = Encoding.UTF8.GetBytes(senha);
                var hashBytes = sha256.ComputeHash(senhaBytes);
                return Convert.ToBase64String(hashBytes); // Retorna o hash da senha em base64
            }
        }

        // Método para comparar a senha inserida com o hash armazenado
        public static bool VerificarSenha(string senha, string senhaHash)
        {
            var senhaGerada = GerarHash(senha);
            return senhaGerada == senhaHash;
        }
    }
}
