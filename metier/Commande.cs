using System;

/// <summary>
/// Espace de noms contenant les classes relatives à l'application Mediateq.
/// </summary>
namespace Mediateq_AP_SIO2
{
    /// <summary>
    /// Classe représentant une commande dans le système.
    /// </summary>
    public class Commande
    {
        private string id;
        private int nbExemplaire;
        private DateTime dateCommande;
        private int montant;
        private Document document;
        private string etat;

        /// <summary>
        /// Initialise une nouvelle instance de la classe <see cref="Commande"/>.
        /// </summary>
        /// <param name="id">L'identifiant de la commande.</param>
        /// <param name="nbExemplaire">Le nombre d'exemplaires commandés.</param>
        /// <param name="dateCommande">La date de la commande.</param>
        /// <param name="montant">Le montant de la commande.</param>
        /// <param name="document">Le document commandé.</param>
        /// <param name="etat">L'état de la commande.</param>
        public Commande(string id, int nbExemplaire, DateTime dateCommande, int montant, Document document, string etat)
        {
            this.id = id;
            this.nbExemplaire = nbExemplaire;
            this.dateCommande = dateCommande;
            this.montant = montant;
            this.document = document;
            this.etat = etat;
        }

        /// <summary>
        /// Obtient ou définit l'identifiant de la commande.
        /// </summary>
        public string Id
        {
            get => id;
            set => id = value;
        }

        /// <summary>
        /// Obtient ou définit l'état de la commande.
        /// </summary>
        public string Etat
        {
            get => etat;
            set => etat = value;
        }

        /// <summary>
        /// Obtient ou définit le nombre d'exemplaires commandés.
        /// </summary>
        public int NbExemplaire
        {
            get => nbExemplaire;
            set => nbExemplaire = value;
        }

        /// <summary>
        /// Obtient ou définit la date de la commande.
        /// </summary>
        public DateTime DateCommande
        {
            get => dateCommande;
            set => dateCommande = value;
        }

        /// <summary>
        /// Obtient ou définit le montant de la commande.
        /// </summary>
        public int Montant
        {
            get => montant;
            set => montant = value;
        }

        /// <summary>
        /// Obtient ou définit le document commandé.
        /// </summary>
        public Document Document
        {
            get => document;
            set => document = value;
        }
    }
}
