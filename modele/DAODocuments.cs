using Mediateq_AP_SIO2;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;

/// <summary>
/// Espace de noms contenant les classes relatives à l'application Mediateq.
/// </summary>
namespace Mediateq_AP_SIO2
{
    /// <summary>
    /// Classe représentant un accès aux données des documents.
    /// </summary>
    class DAODocuments
    {
        /// <summary>
        /// Récupère toutes les catégories de documents.
        /// </summary>
        /// <returns>Une liste de catégories de documents.</returns>
        public static List<Categorie> getAllCategories()
        {
            List<Categorie> lesCategories = new List<Categorie>();
            string req = "Select * from public";

            DAOFactory.connecter();

            MySqlDataReader reader = DAOFactory.execSQLRead(req);

            while (reader.Read())
            {
                Categorie categorie = new Categorie(reader[0].ToString(), reader[1].ToString());
                lesCategories.Add(categorie);
            }
            DAOFactory.deconnecter();
            return lesCategories;
        }

        /// <summary>
        /// Crée un nombre spécifié d'exemplaires pour un document donné.
        /// </summary>
        /// <param name="nbEx">Le nombre d'exemplaires à créer.</param>
        /// <param name="numero">Le numéro du document.</param>
        public static void createEx(int nbEx, int numero)
        {
            DAOFactory.connecter();
            try
            {
                string sql = "INSERT INTO exemplaire (idDocument, numero, dateAchat, idRayon, idEtat, estCommande) " +
                     "SELECT '" + numero + "', IFNULL(MAX(numero), 0) + 1, NOW(), 4, 1, 0 " +
                     "FROM exemplaire;";

                // Répéter la requête pour insérer plusieurs exemplaires
                for (int i = 1; i < nbEx; i++)
                {
                    sql += "INSERT INTO exemplaire (idDocument, numero, dateAchat, idRayon, idEtat, estCommande) " +
                           "SELECT '" + numero + "', IFNULL(MAX(numero), 0) + 1, NOW(), 4, 1, 0 " +
                           "FROM exemplaire;";
                }

                DAOFactory.execSQLWrite(sql);
            }
            catch (Exception exc)
            {
                throw exc;
            }
            finally
            {
                DAOFactory.deconnecter();
            }
        }

        /// <summary>
        /// Récupère tous les descripteurs.
        /// </summary>
        /// <returns>Une liste de descripteurs.</returns>
        public static List<Descripteur> getAllDescripteurs()
        {
            List<Descripteur> lesGenres = new List<Descripteur>();
            try
            {
                string req = "Select * from descripteur";

                DAOFactory.connecter();

                MySqlDataReader reader = DAOFactory.execSQLRead(req);

                while (reader.Read())
                {
                    Descripteur genre = new Descripteur(reader[0].ToString(), reader[1].ToString());
                    lesGenres.Add(genre);
                }
                DAOFactory.deconnecter();
            }
            catch (Exception exc)
            {
                throw exc;
            }
            return lesGenres;
        }

        /// <summary>
        /// Récupère tous les livres.
        /// </summary>
        /// <returns>Une liste de livres.</returns>
        public static List<Livre> getAllLivres()
        {
            List<Livre> lesLivres = new List<Livre>();
            string req = "Select l.idDocument, l.ISBN, l.auteur, d.titre, d.image, l.collection from livre l join document d on l.idDocument=d.id";

            DAOFactory.connecter();

            MySqlDataReader reader = DAOFactory.execSQLRead(req);

            while (reader.Read())
            {
                // On ne renseigne pas le genre et la catégorie car on ne peut pas ouvrir 2 dataReader dans la même connexion
                Livre livre = new Livre(reader[0].ToString(), reader[3].ToString(), reader[1].ToString(),
                    reader[2].ToString(), reader[5].ToString(), reader[4].ToString());
                lesLivres.Add(livre);
            }
            DAOFactory.deconnecter();

            return lesLivres;
        }

        /// <summary>
        /// Récupère la catégorie associée à un livre donné.
        /// </summary>
        /// <param name="pLivre">Le livre pour lequel on souhaite obtenir la catégorie.</param>
        /// <returns>La catégorie associée au livre spécifié.</returns>
        public static Categorie getCategorieByLivre(Livre pLivre)
        {
            Categorie categorie;
            string req = "Select p.id,p.libelle from public p,document d where p.id = d.idPublic and d.id='";
            req += pLivre.IdDoc + "'";

            DAOFactory.connecter();

            MySqlDataReader reader = DAOFactory.execSQLRead(req);

            if (reader.Read())
            {
                categorie = new Categorie(reader[0].ToString(), reader[1].ToString());
            }
            else
            {
                categorie = null;
            }
            DAOFactory.deconnecter();
            return categorie;
        }

        /// <summary>
        /// Récupère tous les DVD.
        /// </summary>
        /// <returns>Une liste de DVD.</returns>
        public static List<DVD> getAllDVD()
        {
            List<DVD> lesDVD = new List<DVD>();
            string req = "Select d.idDocument, d.synopsis, d.réalisateur, doc.titre, doc.image, d.duree from dvd d join document doc on d.idDocument=doc.id";

            DAOFactory.connecter();

            MySqlDataReader reader = DAOFactory.execSQLRead(req);

            while (reader.Read())
            {
                // On ne renseigne pas le genre et la catégorie car on ne peut pas ouvrir 2 dataReader dans la même connexion
                DVD dvd = new DVD(reader[0].ToString(), reader[3].ToString(), reader[1].ToString(),
                    reader[2].ToString(), Int32.Parse(reader[5].ToString()), reader[4].ToString());
                lesDVD.Add(dvd);
            }
            DAOFactory.deconnecter();

            return lesDVD;
        }

        /// <summary>
        /// Récupère toutes les commandes avec un état donné.
        /// </summary>
        /// <param name="etat">L'état des commandes à récupérer.</param>
        /// <returns>Une liste de commandes avec l'état spécifié.</returns>
        public static List<Commande> getAllCommandes(string etat)
        {
            List<Commande> lesCommandes = new List<Commande>();
            string req = "SELECT * FROM Commande WHERE etat LIKE '" + etat + "'";

            DAOFactory.connecter();

            MySqlDataReader reader = DAOFactory.execSQLRead(req);

            while (reader.Read())
            {
                Document doc = getDocumentById(reader[4].ToString());
                Commande uneCMD = new Commande(reader[0].ToString(), Int32.Parse(reader[1].ToString()), DateTime.Parse(reader[2].ToString()), Int32.Parse(reader[3].ToString()), doc, reader[5].ToString());
                lesCommandes.Add(uneCMD);
            }

            // Déconnecter de la base de données après avoir terminé l'exécution de la requête
            DAOFactory.deconnecter();

            return lesCommandes;
        }



        /// <summary>
        /// Récupère une commande en fonction de son identifiant.
        /// </summary>
        /// <param name="id">L'identifiant de la commande à récupérer.</param>
        /// <returns>La commande correspondant à l'identifiant spécifié.</returns>
        public static Commande getCommandeById(string id)
        {
            Commande cmd = null;
            string numcmd = "";
            int nbEx = 0;
            DateTime date = DateTime.MinValue;
            int montant = 0;
            string idDoc = "";
            string etat = "";
            string req = "SELECT * FROM Commande WHERE id = " + id;
            Document doc = null;

            DAOFactory.connecter();

            using (MySqlDataReader reader = DAOFactory.execSQLRead(req))
            {
                if (reader.Read())
                {
                    numcmd = reader[0].ToString();
                    nbEx = Int32.Parse(reader[1].ToString());
                    date = reader.GetDateTime(2);
                    montant = Int32.Parse(reader[3].ToString());
                    idDoc = reader[4].ToString();
                    etat = reader[5].ToString();
                }
            }

            DAOFactory.deconnecter();

            if (idDoc.Equals("")){
                return null;
            }
            doc = getDocumentById(idDoc);

            cmd = new Commande(numcmd, nbEx, date, montant, doc, etat);

            return cmd;
        }

        /// <summary>
        /// Récupère un document en fonction de son identifiant.
        /// </summary>
        /// <param name="id">L'identifiant du document à récupérer.</param>
        /// <returns>Le document correspondant à l'identifiant spécifié.</returns>
        public static Document getDocumentById(string id)
        {
            Document doc = null;
            string req = "SELECT * FROM Document WHERE id = '" + id + "'";

            if (DAOFactory.connection == null || DAOFactory.connection.State == ConnectionState.Closed)
            {
                DAOFactory.creerConnection(); 
                DAOFactory.connecter(); 
            }

            using (MySqlDataReader reader = DAOFactory.execSQLRead(req))
            {
                if (reader.Read())
                {
                    doc = new Document(reader[0].ToString(), reader[1].ToString(), reader[2].ToString());
                }
            }

            DAOFactory.deconnecter();
            return doc;
        }

        /// <summary>
        /// Met à jour l'état d'une commande.
        /// </summary>
        /// <param name="id">L'identifiant de la commande à mettre à jour.</param>
        /// <param name="etat">Le nouvel état de la commande.</param>
        public static void updateEtatCommande(string id, string etat)
        {
            string req = "UPDATE Commande SET etat = '" + etat + "' WHERE commande.id = " + id;

            DAOFactory.connecter();

            MySqlDataReader reader = DAOFactory.execSQLRead(req);

            DAOFactory.deconnecter();
        }

        /// <summary>
        /// Supprime une commande et met à jour les exemplaires correspondants.
        /// </summary>
        /// <param name="id">L'identifiant de la commande à supprimer.</param>
        /// <param name="nbEx">Le nombre d'exemplaires associés à la commande.</param>
        public static void deleteCommande(string id, int nbEx)
        {
            // Supprimer la commande de la table Commande
            string reqDeleteCommande = $"DELETE FROM Commande WHERE commande.id = {id}";

            // Mettre à jour les exemplaires correspondants
            string reqUpdateExemplaire = $"UPDATE exemplaire SET estCommande = 0 WHERE idDocument = {id} LIMIT {nbEx}";

            try
            {
                DAOFactory.connecter();

                // Supprimer la commande
                DAOFactory.execSQLWrite(reqDeleteCommande);

                // Mettre à jour les exemplaires
                DAOFactory.execSQLWrite(reqUpdateExemplaire);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erreur lors de la suppression de la commande : " + ex.Message);
            }
            finally
            {
                DAOFactory.deconnecter();
            }
        }


        /// <summary>
        /// Récupère le nombre d'exemplaires disponibles pour un document donné.
        /// </summary>
        /// <param name="id">L'identifiant du document.</param>
        /// <returns>Le nombre d'exemplaires disponibles pour le document spécifié.</returns>
        public static int getNbExemplaireByIdDocument(string id)
        {
            int nbEx = 0;
            string req = "SELECT COUNT(*) FROM exemplaire WHERE estCommande = 0 AND idDocument = " + id;

            DAOFactory.connecter();

            MySqlDataReader reader = DAOFactory.execSQLRead(req);

            while (reader.Read())
            {
                nbEx = Int32.Parse(reader[0].ToString());
            }

            DAOFactory.deconnecter();

            return nbEx;
        }

        /// <summary>
        /// Crée une nouvelle commande pour un document donné.
        /// </summary>
        /// <param name="idDoc">L'identifiant du document.</param>
        /// <param name="nbEx">Le nombre d'exemplaires à commander.</param>
        /// <param name="prix">Le prix de chaque exemplaire.</param>
        public static void createCommande(string idDoc, int nbEx, int prix)
        {
            int maxId = 0;
            string req = "SELECT MAX(id) FROM commande";
            DAOFactory.connecter();
            MySqlDataReader reader = DAOFactory.execSQLRead(req);

            if (reader.Read())
            {
                maxId = Int32.Parse(reader[0].ToString());
            }

            DAOFactory.deconnecter();

            string dateFormatted = DateTime.Now.ToString("yyyy-MM-dd");

            // Insérer la commande
            string req1 = $"INSERT INTO commande VALUES ({maxId}+1, {nbEx}, '{dateFormatted}', {prix * nbEx}, {idDoc}, 'en cours')";
            DAOFactory.connecter();
            DAOFactory.execSQLWrite(req1);

            // Mettre à jour les exemplaires
            string req2 = $"UPDATE exemplaire SET estCommande = 1 WHERE idDocument = {idDoc} LIMIT {nbEx}";
            DAOFactory.execSQLWrite(req2);

            DAOFactory.deconnecter();
        }


        /// <summary>
        /// Récupère un document en fonction de son titre.
        /// </summary>
        /// <param name="titre">Le titre du document à récupérer.</param>
        /// <returns>Le document correspondant au titre spécifié.</returns>
        public static Document getDocumentByTitre(string titre)
        {
            Document doc = null;
            string req = "SELECT * FROM Document WHERE titre = '" + titre + "'";

            if (DAOFactory.connection == null || DAOFactory.connection.State == ConnectionState.Closed)
            {
                DAOFactory.creerConnection();
                DAOFactory.connecter();
            }

            using (MySqlDataReader reader = DAOFactory.execSQLRead(req))
            {
                if (reader.Read())
                {
                    doc = new Document(reader[0].ToString(), reader[1].ToString(), reader[2].ToString());
                }
            }

            DAOFactory.deconnecter();
            return doc;
        }
    }
}
