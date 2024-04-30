using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
/// <summary>
/// Espace de noms contenant les classes relatives à l'application Mediateq.
/// </summary>
namespace Mediateq_AP_SIO2
{
    /// <summary>
    /// Classe représentant un utilisateur dans le système.
    /// </summary>
    public class Utilisateur
    {
        private string login;
        private string nom;
        private string prenom;
        private string password;
        private string service;

        /// <summary>
        /// Initialise une nouvelle instance de la classe <see cref="Utilisateur"/>.
        /// </summary>
        /// <param name="unLogin">Le login de l'utilisateur.</param>
        /// <param name="unNom">Le nom de l'utilisateur.</param>
        /// <param name="unPrenom">Le prénom de l'utilisateur.</param>
        /// <param name="unPassword">Le mot de passe de l'utilisateur.</param>
        /// <param name="unService">Le service auquel appartient l'utilisateur.</param>
        public Utilisateur(string unLogin, string unNom, string unPrenom, string unPassword, string unService)
        {
            this.login = unLogin;
            this.nom = unNom;
            this.prenom = unPrenom;
            this.password = unPassword;
            this.service = unService;
        }

        /// <summary>
        /// Obtient ou définit le login de l'utilisateur.
        /// </summary>
        public string Login { get => login; set => login = value; }

        /// <summary>
        /// Obtient ou définit le nom de l'utilisateur.
        /// </summary>
        public string Nom { get => nom; set => nom = value; }

        /// <summary>
        /// Obtient ou définit le prénom de l'utilisateur.
        /// </summary>
        public string Prenom { get => prenom; set => prenom = value; }

        /// <summary>
        /// Obtient ou définit le mot de passe de l'utilisateur.
        /// </summary>
        public string Password { get => password; set => password = value; }

        /// <summary>
        /// Obtient ou définit le service auquel appartient l'utilisateur.
        /// </summary>
        public string Service { get => service; set => service = value; }
    }
}
