using System;
/// <summary>
/// Espace de noms contenant les classes relatives à l'application Mediateq.
/// </summary>
namespace Mediateq_AP_SIO2
{
    /// <summary>
    /// Classe représentant un DVD dans le système, qui est un type de document.
    /// </summary>
    public class DVD : Document
    {
        private string synopsis;
        private string realisateur;
        private int duree;

        /// <summary>
        /// Initialise une nouvelle instance de la classe <see cref="DVD"/>.
        /// </summary>
        /// <param name="unId">L'identifiant du DVD.</param>
        /// <param name="unTitre">Le titre du DVD.</param>
        /// <param name="unSynopsis">Le synopsis du DVD.</param>
        /// <param name="unRealisateur">Le réalisateur du DVD.</param>
        /// <param name="uneDuree">La durée du DVD en minutes.</param>
        /// <param name="uneImage">Le chemin vers l'image du DVD.</param>
        public DVD(string unId, string unTitre, string unSynopsis, string unRealisateur, int uneDuree, string uneImage) : base(unId, unTitre, uneImage)
        {
            this.synopsis = unSynopsis;
            this.realisateur = unRealisateur;
            this.duree = uneDuree;
        }

        /// <summary>
        /// Obtient ou définit le synopsis du DVD.
        /// </summary>
        public string Synopsis { get => synopsis; set => synopsis = value; }

        /// <summary>
        /// Obtient ou définit le réalisateur du DVD.
        /// </summary>
        public string Realisateur { get => realisateur; set => realisateur = value; }

        /// <summary>
        /// Obtient ou définit la durée du DVD en minutes.
        /// </summary>
        public int Duree { get => duree; set => duree = value; }
    }
}
