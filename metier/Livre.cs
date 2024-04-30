using System;
/// <summary>
/// Espace de noms contenant les classes relatives à l'application Mediateq.
/// </summary>
namespace Mediateq_AP_SIO2
{
    /// <summary>
    /// Classe représentant un livre dans le système, qui est un type de document.
    /// </summary>
    public class Livre : Document
    {
        private string ISBN;
        private string auteur;
        private string laCollection;

        /// <summary>
        /// Initialise une nouvelle instance de la classe <see cref="Livre"/>.
        /// </summary>
        /// <param name="unId">L'identifiant du livre.</param>
        /// <param name="unTitre">Le titre du livre.</param>
        /// <param name="unISBN">L'ISBN du livre.</param>
        /// <param name="unAuteur">L'auteur du livre.</param>
        /// <param name="uneCollection">La collection du livre.</param>
        /// <param name="uneImage">Le chemin vers l'image du livre.</param>
        public Livre(string unId, string unTitre, string unISBN, string unAuteur, string uneCollection, string uneImage) : base(unId, unTitre, uneImage)
        {
            ISBN = unISBN;
            auteur = unAuteur;
            laCollection = uneCollection;
        }

        /// <summary>
        /// Obtient ou définit l'ISBN du livre.
        /// </summary>
        public string ISBN1 { get => ISBN; set => ISBN = value; }

        /// <summary>
        /// Obtient ou définit l'auteur du livre.
        /// </summary>
        public string Auteur { get => auteur; set => auteur = value; }

        /// <summary>
        /// Obtient ou définit la collection du livre.
        /// </summary>
        public string LaCollection { get => laCollection; set => laCollection = value; }
    }
}
