using System;
/// <summary>
/// Espace de noms contenant les classes relatives à l'application Mediateq.
/// </summary>
namespace Mediateq_AP_SIO2
{
    /// <summary>
    /// Classe représentant un document dans le système.
    /// </summary>
    public class Document
    {
        private string idDoc;
        private string titre;
        private string image;
        private string laCategorie;

        /// <summary>
        /// Initialise une nouvelle instance de la classe <see cref="Document"/>.
        /// </summary>
        /// <param name="unId">L'identifiant du document.</param>
        /// <param name="unTitre">Le titre du document.</param>
        /// <param name="uneImage">Le chemin vers l'image du document.</param>
        public Document(string unId, string unTitre, string uneImage)
        {
            idDoc = unId;
            titre = unTitre;
            image = uneImage;
        }

        /// <summary>
        /// Obtient ou définit l'identifiant du document.
        /// </summary>
        public string IdDoc { get => idDoc; set => idDoc = value; }

        /// <summary>
        /// Obtient ou définit le titre du document.
        /// </summary>
        public string Titre { get => titre; set => titre = value; }

        /// <summary>
        /// Obtient ou définit le chemin vers l'image du document.
        /// </summary>
        public string Image { get => image; set => image = value; }

        /// <summary>
        /// Obtient ou définit la catégorie du document.
        /// </summary>
        public string LaCategorie { get => laCategorie; set => laCategorie = value; }
    }
}
