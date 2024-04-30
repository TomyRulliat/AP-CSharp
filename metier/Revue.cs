using System;
/// <summary>
/// Espace de noms contenant les classes relatives à l'application Mediateq.
/// </summary>
namespace Mediateq_AP_SIO2
{
    /// <summary>
    /// Classe représentant une revue dans le système.
    /// </summary>
    public class Revue
    {
        private string id;
        private string titre;
        private char empruntable;
        private string periodicite;
        private DateTime dateFinAbonnement;
        private int delaiMiseADispo;
        private string idDescripteur;

        /// <summary>
        /// Initialise une nouvelle instance de la classe <see cref="Revue"/>.
        /// </summary>
        /// <param name="id">L'identifiant de la revue.</param>
        /// <param name="titre">Le titre de la revue.</param>
        /// <param name="empruntable">La disponibilité de la revue pour l'emprunt.</param>
        /// <param name="periodicite">La périodicité de la revue.</param>
        /// <param name="dateFinAbonnement">La date de fin d'abonnement de la revue.</param>
        /// <param name="delaiMiseADispo">Le délai de mise à disposition de la revue.</param>
        /// <param name="idDescripteur">L'identifiant du descripteur associé à la revue.</param>
        public Revue(string id, string titre, char empruntable, string periodicite, DateTime dateFinAbonnement, int delaiMiseADispo, string idDescripteur)
        {
            this.id = id;
            this.titre = titre;
            this.empruntable = empruntable;
            this.periodicite = periodicite;
            this.dateFinAbonnement = dateFinAbonnement;
            this.delaiMiseADispo = delaiMiseADispo;
            this.idDescripteur = idDescripteur;
        }

        /// <summary>
        /// Obtient ou définit l'identifiant de la revue.
        /// </summary>
        public string Id { get => id; set => id = value; }

        /// <summary>
        /// Obtient ou définit le titre de la revue.
        /// </summary>
        public string Titre { get => titre; set => titre = value; }

        /// <summary>
        /// Obtient ou définit la disponibilité de la revue pour l'emprunt.
        /// </summary>
        public char Empruntable { get => empruntable; set => empruntable = value; }

        /// <summary>
        /// Obtient ou définit la périodicité de la revue.
        /// </summary>
        public string Periodicite { get => periodicite; set => periodicite = value; }

        /// <summary>
        /// Obtient ou définit la date de fin d'abonnement de la revue.
        /// </summary>
        public DateTime DateFinAbonnement { get => dateFinAbonnement; set => dateFinAbonnement = value; }

        /// <summary>
        /// Obtient ou définit le délai de mise à disposition de la revue.
        /// </summary>
        public int DelaiMiseADispo { get => delaiMiseADispo; set => delaiMiseADispo = value; }

        /// <summary>
        /// Obtient ou définit l'identifiant du descripteur associé à la revue.
        /// </summary>
        public string IdDescripteur { get => idDescripteur; set => idDescripteur = value; }
    }
}
