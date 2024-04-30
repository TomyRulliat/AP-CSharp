using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.Threading.Tasks;
/// <summary>
/// Espace de noms contenant les classes relatives à l'application Mediateq.
/// </summary>
namespace Mediateq_AP_SIO2
{
    /// <summary>
    /// Classe fournissant des méthodes pour générer et vérifier les hashs de mot de passe.
    /// </summary>
    public class Hash
    {
        /// <summary>
        /// Génère une chaîne de caractères de sel aléatoire.
        /// </summary>
        /// <param name="length">La longueur de la chaîne de caractères de sel (par défaut 16).</param>
        /// <returns>Une chaîne de caractères de sel générée aléatoirement.</returns>
        public static string GenerateSalt(int length = 16)
        {
            byte[] salt = new byte[length];
            using (var rng = new RNGCryptoServiceProvider())
            {
                rng.GetBytes(salt);
            }
            return Convert.ToBase64String(salt);
        }

        /// <summary>
        /// Hache le mot de passe avec le sel spécifié.
        /// </summary>
        /// <param name="password">Le mot de passe à hacher.</param>
        /// <param name="salt">Le sel utilisé pour hacher le mot de passe.</param>
        /// <returns>Le hash résultant du mot de passe haché.</returns>
        public static string HashPassword(string password, string salt)
        {
            byte[] saltBytes = Convert.FromBase64String(salt);

            using (var sha256 = new SHA256Managed())
            {
                byte[] passwordBytes = Encoding.UTF8.GetBytes(password);
                byte[] saltedPassword = new byte[passwordBytes.Length + saltBytes.Length];
                Buffer.BlockCopy(passwordBytes, 0, saltedPassword, 0, passwordBytes.Length);
                Buffer.BlockCopy(saltBytes, 0, saltedPassword, passwordBytes.Length, saltBytes.Length);

                byte[] hash = sha256.ComputeHash(saltedPassword);

                return Convert.ToBase64String(hash);
            }
        }

        /// <summary>
        /// Vérifie si le mot de passe fourni correspond au hash stocké, en utilisant le sel spécifié.
        /// </summary>
        /// <param name="hashedPassword">Le hash stocké du mot de passe.</param>
        /// <param name="password">Le mot de passe à vérifier.</param>
        /// <param name="salt">Le sel utilisé lors du hachage initial.</param>
        /// <returns>True si le mot de passe fourni correspond au hash stocké, False sinon.</returns>
        public static bool VerifyPassword(string hashedPassword, string password, string salt)
        {
            string hashedInput = HashPassword(password, salt);
            return hashedInput == hashedPassword;
        }
    }
}
