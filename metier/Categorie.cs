/// <summary>
/// Espace de noms contenant les classes relatives à l'application Mediateq.
/// </summary>
namespace Mediateq_AP_SIO2
{
    /// <summary>
    /// Classe représentant une catégorie dans le système.
    /// </summary>
    public class Categorie
    {
        private string id;
        private string libelle;

        /// <summary>
        /// Initialise une nouvelle instance de la classe <see cref="Categorie"/>.
        /// </summary>
        /// <param name="id">L'identifiant de la catégorie.</param>
        /// <param name="libelle">Le libellé de la catégorie.</param>
        public Categorie(string id, string libelle)
        {
            this.id = id;
            this.libelle = libelle;
        }

        /// <summary>
        /// Obtient ou définit l'identifiant de la catégorie.
        /// </summary>
        public string Id { get => id; set => id = value; }

        /// <summary>
        /// Obtient ou définit le libellé de la catégorie.
        /// </summary>
        public string Libelle { get => libelle; set => libelle = value; }
    }
}
