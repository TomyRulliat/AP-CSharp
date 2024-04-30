using System;
/// <summary>
/// Espace de noms contenant les classes relatives à l'application Mediateq.
/// </summary>
namespace Mediateq_AP_SIO2
{
    /// <summary>
    /// Classe représentant un descripteur dans le système.
    /// </summary>
    public class Descripteur
    {
        private string id;
        private string libelle;

        /// <summary>
        /// Initialise une nouvelle instance de la classe <see cref="Descripteur"/>.
        /// </summary>
        /// <param name="id">L'identifiant du descripteur.</param>
        /// <param name="libelle">Le libellé du descripteur.</param>
        public Descripteur(string id, string libelle)
        {
            this.id = id;
            this.libelle = libelle;
        }

        /// <summary>
        /// Obtient ou définit l'identifiant du descripteur.
        /// </summary>
        public string Id { get => id; set => id = value; }

        /// <summary>
        /// Obtient ou définit le libellé du descripteur.
        /// </summary>
        public string Libelle { get => libelle; set => libelle = value; }
    }
}
