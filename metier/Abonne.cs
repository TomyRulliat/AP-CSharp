using System;

/// <summary>
/// Espace de noms contenant les classes relatives à l'application Mediateq.
/// </summary>
namespace Mediateq_AP_SIO2
{
    /// <summary>
    /// Classe représentant un abonné dans le système.
    /// </summary>
    public class Abonne
    {
        private int idAbonne;
        private string nom;
        private string prenom;
        private string adresse;
        private DateTime dateNaissance;
        private string adresseCourriel;
        private string numeroTelephone;
        private string motDePasse;
        private DateTime dateFinAbonnement;
        private DateTime datePremierAbonnement;
        private string typeAbonnement;
        private int numTypeAbonnement;

        /// <summary>
        /// Initialise une nouvelle instance de la classe <see cref="Abonne"/>.
        /// </summary>
        /// <param name="idAbonne">L'identifiant de l'abonné.</param>
        /// <param name="nom">Le nom de l'abonné.</param>
        /// <param name="prenom">Le prénom de l'abonné.</param>
        /// <param name="adresse">L'adresse de l'abonné.</param>
        /// <param name="dateNaissance">La date de naissance de l'abonné.</param>
        /// <param name="adresseCourriel">L'adresse courriel de l'abonné.</param>
        /// <param name="numeroTelephone">Le numéro de téléphone de l'abonné.</param>
        /// <param name="motDePasse">Le mot de passe de l'abonné.</param>
        /// <param name="dateFinAbonnement">La date de fin d'abonnement de l'abonné.</param>
        /// <param name="datePremierAbonnement">La date du premier abonnement de l'abonné.</param>
        /// <param name="typeAbonnement">Le type d'abonnement de l'abonné.</param>
        /// <param name="numTypeAbonnement">Le numéro du type d'abonnement de l'abonné.</param>
        public Abonne(int idAbonne, string nom, string prenom, string adresse, DateTime dateNaissance,
                      string adresseCourriel, string numeroTelephone, string motDePasse,
                      DateTime dateFinAbonnement, DateTime datePremierAbonnement, string typeAbonnement, int numTypeAbonnement)
        {
            this.idAbonne = idAbonne;
            this.nom = nom;
            this.prenom = prenom;
            this.adresse = adresse;
            this.dateNaissance = dateNaissance;
            this.adresseCourriel = adresseCourriel;
            this.numeroTelephone = numeroTelephone;
            this.motDePasse = motDePasse;
            this.dateFinAbonnement = dateFinAbonnement;
            this.datePremierAbonnement = datePremierAbonnement;
            this.typeAbonnement = typeAbonnement;
            this.numTypeAbonnement = numTypeAbonnement;
        }

        /// <summary>
        /// Obtient ou définit l'identifiant de l'abonné.
        /// </summary>
        public int IdAbonne { get => idAbonne; set => idAbonne = value; }

        /// <summary>
        /// Obtient ou définit le nom de l'abonné.
        /// </summary>
        public string Nom { get => nom; set => nom = value; }

        /// <summary>
        /// Obtient ou définit le prénom de l'abonné.
        /// </summary>
        public string Prenom { get => prenom; set => prenom = value; }

        /// <summary>
        /// Obtient ou définit l'adresse de l'abonné.
        /// </summary>
        public string Adresse { get => adresse; set => adresse = value; }

        /// <summary>
        /// Obtient ou définit la date de naissance de l'abonné.
        /// </summary>
        public DateTime DateNaissance { get => dateNaissance; set => dateNaissance = value; }

        /// <summary>
        /// Obtient ou définit l'adresse courriel de l'abonné.
        /// </summary>
        public string AdresseCourriel { get => adresseCourriel; set => adresseCourriel = value; }

        /// <summary>
        /// Obtient ou définit le numéro de téléphone de l'abonné.
        /// </summary>
        public string NumeroTelephone { get => numeroTelephone; set => numeroTelephone = value; }

        /// <summary>
        /// Obtient ou définit le mot de passe de l'abonné.
        /// </summary>
        public string MotDePasse { get => motDePasse; set => motDePasse = value; }

        /// <summary>
        /// Obtient ou définit la date de fin d'abonnement de l'abonné.
        /// </summary>
        public DateTime DateFinAbonnement { get => dateFinAbonnement; set => dateFinAbonnement = value; }

        /// <summary>
        /// Obtient ou définit la date du premier abonnement de l'abonné.
        /// </summary>
        public DateTime DatePremierAbonnement { get => datePremierAbonnement; set => datePremierAbonnement = value; }

        /// <summary>
        /// Obtient ou définit le type d'abonnement de l'abonné.
        /// </summary>
        public string TypeAbonnement { get => typeAbonnement; set => typeAbonnement = value; }

        /// <summary>
        /// Obtient ou définit le numéro du type d'abonnement de l'abonné.
        /// </summary>
        public int NumTypeAbonnement { get => numTypeAbonnement; set => numTypeAbonnement = value; }
    }
}
