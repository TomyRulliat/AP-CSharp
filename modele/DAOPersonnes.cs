using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mediateq_AP_SIO2;
using MySql.Data.MySqlClient;
/// <summary>
/// Espace de noms contenant les classes relatives à l'application Mediateq.
/// </summary>
namespace Mediateq_AP_SIO2
{
    /// <summary>
    /// Classe contenant les méthodes pour interagir avec les abonnés et les utilisateurs dans la base de données.
    /// </summary>
    class DAOPersonnes
    {

        /// <summary>
        /// Récupère tous les abonnés depuis la base de données.
        /// </summary>
        /// <returns>Une liste d'objets Abonne représentant les abonnés.</returns>
        public static List<Abonne> getAllAbonnes()
        {
            List<Abonne> lesAbonnes = new List<Abonne>();
            string req = "SELECT IdAbonne, Nom, Prenom, Adresse, DateNaissance, AdresseCourriel, NumeroTelephone, MotDePasse, DatePremierAbonnement, DateFinAbonnement, type_abonnement.Libelle,IdTypeAbonnement FROM abonne NATURAL JOIN type_abonnement";

            DAOFactory.connecter();

            MySqlDataReader reader = DAOFactory.execSQLRead(req);

            while (reader.Read())
            {
                int id;
                int idTypeAbo;
                DateTime dateNaissance;
                DateTime datePremAbo;
                DateTime dateFinAbo;

                if (int.TryParse(reader[0].ToString(), out id))
                {
                    if (DateTime.TryParse(reader[4].ToString(), out dateNaissance))
                    {
                        if (DateTime.TryParse(reader[9].ToString(), out dateFinAbo))
                        {
                            if (DateTime.TryParse(reader[8].ToString(), out datePremAbo))
                            {
                                if (int.TryParse(reader[11].ToString(), out idTypeAbo))
                                {
                                    Abonne abo = new Abonne(id, reader[1].ToString(), reader[2].ToString(), reader[3].ToString(), dateNaissance, reader[5].ToString(), reader[6].ToString(), reader[7].ToString(), dateFinAbo, datePremAbo, reader[10].ToString(), idTypeAbo - 1);
                                    lesAbonnes.Add(abo);
                                }
                            }
                        }
                    }
                }

            }
            DAOFactory.deconnecter();

            return lesAbonnes;
        }

        /// <summary>
        /// Récupère un abonné à partir de son adresse courriel.
        /// </summary>
        /// <param name="adresse">L'adresse courriel de l'abonné à récupérer.</param>
        /// <returns>L'objet Abonne correspondant à l'adresse courriel spécifiée, ou null si aucun abonné trouvé.</returns>
        public static Abonne getAbonneByAdresse(string adresse)
        {
            Abonne abo = null;
            string req = "SELECT IdAbonne, Nom, Prenom, Adresse, DateNaissance, AdresseCourriel, NumeroTelephone, MotDePasse, DatePremierAbonnement, DateFinAbonnement, type_abonnement.Libelle,IdTypeAbonnement FROM abonne NATURAL JOIN type_abonnement WHERE AdresseCourriel='" + adresse + "'";

            DAOFactory.connecter();

            MySqlDataReader reader = DAOFactory.execSQLRead(req);

            while (reader.Read())
            {
                int id;
                int idTypeAbo;
                DateTime dateNaissance;
                DateTime datePremAbo;
                DateTime dateFinAbo;

                if (int.TryParse(reader[0].ToString(), out id))
                {
                    if (DateTime.TryParse(reader[4].ToString(), out dateNaissance))
                    {
                        if (DateTime.TryParse(reader[9].ToString(), out dateFinAbo))
                        {
                            if (DateTime.TryParse(reader[8].ToString(), out datePremAbo))
                            {
                                if (int.TryParse(reader[11].ToString(), out idTypeAbo))
                                {
                                    abo = new Abonne(id, reader[1].ToString(), reader[2].ToString(), reader[3].ToString(), dateNaissance, reader[5].ToString(), reader[6].ToString(), reader[7].ToString(), dateFinAbo, datePremAbo, reader[10].ToString(), idTypeAbo - 1);
                                }
                            }
                        }
                    }
                }

            }
            DAOFactory.deconnecter();

            return abo;
        }

        /// <summary>
        /// Récupère le type d'abonnement d'un abonné à partir de son adresse courriel.
        /// </summary>
        /// <param name="AdresseCourriel">L'adresse courriel de l'abonné.</param>
        /// <returns>Le type d'abonnement de l'abonné.</returns>
        public static String GetTypeAbonnement(String AdresseCourriel)
        {
            string typeAbo = "";
            string req = "SELECT type_abonnement.Libelle FROM abonne NATURAL JOIN type_abonnement WHERE AdresseCourriel = '";
            req += AdresseCourriel + "'";

            DAOFactory.connecter();

            MySqlDataReader reader = DAOFactory.execSQLRead(req);

            while (reader.Read())
            {
                typeAbo = reader[0].ToString();

            }
            DAOFactory.deconnecter();

            return typeAbo;
        }

        /// <summary>
        /// Crée un nouvel abonné dans la base de données avec les informations spécifiées.
        /// </summary>
        /// <param name="nom">Le nom de l'abonné.</param>
        /// <param name="prenom">Le prénom de l'abonné.</param>
        /// <param name="adresse">L'adresse de l'abonné.</param>
        /// <param name="dateNaissance">La date de naissance de l'abonné.</param>
        /// <param name="adresseCourriel">L'adresse courriel de l'abonné.</param>
        /// <param name="numeroTelephone">Le numéro de téléphone de l'abonné.</param>
        /// <param name="mdp">Le mot de passe de l'abonné.</param>
        /// <param name="dateFinAbonnement">La date de fin d'abonnement de l'abonné.</param>
        /// <param name="datePremierAbonnement">La date de début d'abonnement de l'abonné.</param>
        /// <param name="idTypeAbonnement">L'identifiant du type d'abonnement de l'abonné.</param>
        /// <returns>L'identifiant de l'abonné créé, ou -1 en cas d'erreur.</returns>
        public static int CreateAbonne(string nom, string prenom, string adresse, DateTime dateNaissance, string adresseCourriel, string numeroTelephone, string mdp, DateTime dateFinAbonnement, DateTime datePremierAbonnement, int idTypeAbonnement)
        {
            int ID = 0;
            try
            {
                string req = "SELECT MAX(IdAbonne) FROM abonne";
                DAOFactory.connecter();
                MySqlDataReader reader = DAOFactory.execSQLRead(req);

                if (reader.Read())
                {
                    object maxIdObj = reader[0];
                    if (maxIdObj != null && maxIdObj != DBNull.Value)
                    {
                        int maxId = Convert.ToInt32(maxIdObj);
                        ID = maxId + 1;
                    }
                }

                DAOFactory.deconnecter();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erreur lors de la création de l'abonné : " + ex.Message);
            }
            finally
            {
                DAOFactory.deconnecter();
            }


            try
            {


                string dateNaissanceString = dateNaissance.ToString("yyyy-MM-dd");
                string dateFinAbonnementString = dateFinAbonnement.ToString("yyyy-MM-dd");
                string datePremAbonnementString = datePremierAbonnement.ToString("yyyy-MM-dd");


                DAOFactory.connecter();

                // Insérer les informations du nouvel abonné dans la base de données
                string insertReq = "INSERT INTO `abonne`(`IdAbonne`, `Nom`, `Prenom`, `Adresse`, `DateNaissance`, `AdresseCourriel`, `NumeroTelephone`, `MotDePasse`, `DateFinAbonnement`, `DatePremierAbonnement`, `IdTypeAbonnement`) " +
                           "VALUES("+@ID+",'"+ @nom +"','"+ @prenom +"','"+ @adresse+"','"+ dateNaissanceString + "','"+ @adresseCourriel+"','"+ @numeroTelephone+"', '" + @mdp + "','" +dateFinAbonnementString + "' , '" + datePremAbonnementString + "'," + @idTypeAbonnement+")";
                DAOFactory.execSQLWrite(insertReq);

                return ID;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erreur lors de la création de l'abonné : " + ex.Message);
            }
            finally
            {
                DAOFactory.deconnecter();
            }

            return -1;
        }

        /// <summary>
        /// Supprime un abonné de la base de données en fonction de son adresse courriel.
        /// </summary>
        /// <param name="adresseCourriel">L'adresse courriel de l'abonné à supprimer.</param>
        /// <returns>True si l'abonné a été supprimé avec succès, sinon False.</returns>
        public static bool DeleteAbonneByAdresseCourriel(string adresseCourriel)
        {
            try
            {
                DAOFactory.connecter();

                // Supprimer l'abonné en fonction de l'adresse courriel
                string deleteReq = "DELETE FROM `abonne` WHERE `AdresseCourriel`='" + adresseCourriel + "'";
                DAOFactory.execSQLWrite(deleteReq);

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erreur lors de la suppression de l'abonné : " + ex.Message);
                return false;
            }
            finally
            {
                DAOFactory.deconnecter();
            }
        }

        /// <summary>
        /// Met à jour les informations d'un abonné dans la base de données.
        /// </summary>
        /// <param name="nom">Le nouveau nom de l'abonné.</param>
        /// <param name="prenom">Le nouveau prénom de l'abonné.</param>
        /// <param name="adresse">La nouvelle adresse de l'abonné.</param>
        /// <param name="dateNaissance">La nouvelle date de naissance de l'abonné.</param>
        /// <param name="adresseCourriel">La nouvelle adresse courriel de l'abonné.</param>
        /// <param name="numeroTelephone">Le nouveau numéro de téléphone de l'abonné.</param>
        /// <param name="dateFinAbonnement">La nouvelle date de fin d'abonnement de l'abonné.</param>
        /// <param name="idTypeAbonnement">Le nouvel identifiant du type d'abonnement de l'abonné.</param>
        /// <returns>True si la mise à jour a été effectuée avec succès, sinon False.</returns>
        public static bool UpdateAbonne(string nom, string prenom, string adresse, DateTime dateNaissance, string adresseCourriel, string numeroTelephone, DateTime dateFinAbonnement, int idTypeAbonnement)
        {
            try
            {
                DAOFactory.connecter();

                string dateNaissanceString = dateNaissance.ToString("yyyy-MM-dd");
                string dateFinAbonnementString = dateFinAbonnement.ToString("yyyy-MM-dd");
                int typeAbo = idTypeAbonnement + 1;

                // Mettre à jour les informations de l'abonné dans la base de données
                string updateReq = "UPDATE `abonne` SET `Nom`='" + nom + "', `Prenom`='" + prenom + "', `Adresse`='" + adresse + "', `DateNaissance`='" + dateNaissanceString + "', `NumeroTelephone`='" + numeroTelephone + "', `DateFinAbonnement`='" + dateFinAbonnementString + "', `IdTypeAbonnement`=" + typeAbo + " WHERE `AdresseCourriel`='" + adresseCourriel + "'";

                DAOFactory.execSQLWrite(updateReq);

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erreur lors de la modification de l'abonné : " + ex.Message);
                return false;
            }
            finally
            {
                DAOFactory.deconnecter();
            }
        }

        /// <summary>
        /// Met à jour les informations d'un abonné dans la base de données (version surchargée sans date de fin d'abonnement).
        /// </summary>
        /// <param name="nom">Le nouveau nom de l'abonné.</param>
        /// <param name="prenom">Le nouveau prénom de l'abonné.</param>
        /// <param name="adresse">La nouvelle adresse de l'abonné.</param>
        /// <param name="dateNaissance">La nouvelle date de naissance de l'abonné.</param>
        /// <param name="adresseCourriel">La nouvelle adresse courriel de l'abonné.</param>
        /// <param name="numeroTelephone">Le nouveau numéro de téléphone de l'abonné.</param>
        /// <returns>True si la mise à jour a été effectuée avec succès, sinon False.</returns>
        public static bool UpdateAbonne(string nom, string prenom, string adresse, DateTime dateNaissance, string adresseCourriel, string numeroTelephone)
        {
            try
            {
                DAOFactory.connecter();

                string dateNaissanceString = dateNaissance.ToString("yyyy-MM-dd");

                // Mettre à jour les informations de l'abonné dans la base de données
                string updateReq = "UPDATE `abonne` SET `Nom`='" + nom + "', `Prenom`='" + prenom + "', `Adresse`='" + adresse + "', `DateNaissance`='" + dateNaissanceString + "', `NumeroTelephone`='" + numeroTelephone + " WHERE `AdresseCourriel`=" + adresseCourriel;

                DAOFactory.execSQLWrite(updateReq);

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erreur lors de la modification de l'abonné : " + ex.Message);
                return false;
            }
            finally
            {
                DAOFactory.deconnecter();
            }
        }

        /// <summary>
        /// Récupère tous les utilisateurs depuis la base de données.
        /// </summary>
        /// <returns>Une liste d'objets Utilisateur représentant les utilisateurs.</returns>
        public static List<Utilisateur> getAllUtilisateurs()
        {
            List<Utilisateur> lesUtilisateurs = new List<Utilisateur>();
            string req = "SELECT login, nom, prenom, password, service.libelle FROM utilisateurs JOIN service ON utilisateurs.service = service.id";

            DAOFactory.connecter();

            MySqlDataReader reader = DAOFactory.execSQLRead(req);

            while (reader.Read())
            {
                // On ne renseigne pas le genre et la catégorie car on ne peut pas ouvrir 2 dataReader dans la même connexion
                Utilisateur util = new Utilisateur(reader[0].ToString(), reader[1].ToString(), reader[2].ToString(),
                    reader[3].ToString(), reader[4].ToString());
                lesUtilisateurs.Add(util);
            }
            DAOFactory.deconnecter();

            return lesUtilisateurs;
        }

        /// <summary>
        /// Crée un nouvel utilisateur dans la base de données avec les informations spécifiées.
        /// </summary>
        /// <param name="username">Le nom d'utilisateur du nouvel utilisateur.</param>
        /// <param name="password">Le mot de passe du nouvel utilisateur.</param>
        /// <param name="nom">Le nom du nouvel utilisateur.</param>
        /// <param name="prenom">Le prénom du nouvel utilisateur.</param>
        /// <param name="serviceLibelle">Le libellé du service auquel appartient le nouvel utilisateur.</param>
        /// <returns>True si l'utilisateur a été créé avec succès, sinon False.</returns>
        public static bool CreateUtilisateur(string username, string password, string nom, string prenom, string serviceLibelle)
        {
            try
            {
                if (!(string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(nom) || string.IsNullOrEmpty(prenom) || string.IsNullOrEmpty(serviceLibelle)))
                {
                    // Générer un sel pour le nouveau compte
                    string salt = Hash.GenerateSalt();

                    // Hasher le mot de passe avec le sel
                    string hashedPassword = Hash.HashPassword(password, salt);

                    DAOFactory.connecter();

                    // Insérer les informations du nouvel utilisateur dans la base de données
                    string insertReq = "INSERT INTO utilisateurs (login, nom, prenom, password, salt, service) " +
                                       "VALUES ('" + @username + "','" + @nom + "','" + @prenom + "','" + @hashedPassword + "','" + @salt + "', (SELECT id FROM service WHERE libelle = '" + @serviceLibelle + "'))";

                    DAOFactory.execSQLWrite(insertReq);

                    return true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erreur lors de la création de l'utilisateur : " + ex.Message);
            }
            finally
            {
                DAOFactory.deconnecter();
            }

            return false;
        }

        /// <summary>
        /// Supprime un utilisateur de la base de données en fonction de son nom d'utilisateur.
        /// </summary>
        /// <param name="login">Le nom d'utilisateur de l'utilisateur à supprimer.</param>
        /// <returns>True si l'utilisateur a été supprimé avec succès, sinon False.</returns>
        public static bool DeleteUtilisateurByLogin(string login)
        {
            try
            {
                if (string.IsNullOrEmpty(login))
                {
                    DAOFactory.connecter();

                    // Récupérer l'utilisateur par son nom d'utilisateur
                    string testFonctionne = "SELECT * FROM utilisateurs WHERE login = '" + login + "'";

                    MySqlDataReader reader = DAOFactory.execSQLRead(testFonctionne);

                    if (reader.Read())
                    {
                        DAOFactory.deconnecter();
                        DAOFactory.connecter();

                        // Supprimer l'utilisateur par son nom d'utilisateur
                        string deleteReq = "DELETE FROM utilisateurs WHERE login = '" + login + "'";

                        DAOFactory.execSQLWrite(deleteReq);
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erreur lors de la suppression de l'utilisateur : " + ex.Message);
            }
            finally
            {
                DAOFactory.deconnecter();
            }
            return false;
        }

        /// <summary>
        /// Récupère un utilisateur à partir de son nom d'utilisateur.
        /// </summary>
        /// <param name="login">Le nom d'utilisateur de l'utilisateur à récupérer.</param>
        /// <returns>L'objet Utilisateur correspondant au nom d'utilisateur spécifié, ou null si aucun utilisateur trouvé.</returns>
        public static Utilisateur getUtilByLogin(string login)
        {
            try
            {
                DAOFactory.connecter();

                // Sélectionner l'utilisateur par son nom d'utilisateur
                string selectReq = "SELECT login, nom, prenom, password, service.libelle FROM utilisateurs " +
                                   "JOIN service ON utilisateurs.service = service.id " +
                                   "WHERE login = '" + login;

                MySqlDataReader reader = DAOFactory.execSQLRead(selectReq);

                if (reader.Read())
                {
                    Utilisateur utilisateur = new Utilisateur(
                        reader["login"].ToString(),
                        reader["nom"].ToString(),
                        reader["prenom"].ToString(),
                        reader["password"].ToString(),
                        reader["libelle"].ToString()
                    );

                    return utilisateur;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erreur lors de la récupération de l'utilisateur : " + ex.Message);
            }
            finally
            {
                DAOFactory.deconnecter();
            }

            return null; // Retourner null si l'utilisateur n'est pas trouvé ou en cas d'erreur
        }

        /// <summary>
        /// Récupère le mot de passe d'un utilisateur à partir de son nom d'utilisateur.
        /// </summary>
        /// <param name="login">Le nom d'utilisateur de l'utilisateur.</param>
        /// <returns>Le mot de passe de l'utilisateur correspondant au nom d'utilisateur spécifié.</returns>
        public static string getMDP(string login)
        {
            try
            {
                DAOFactory.connecter();

                // Sélectionner l'utilisateur par son nom d'utilisateur
                string selectReq = "SELECT password from utilisateurs " +
                                   "WHERE login = '" + login;

                MySqlDataReader reader = DAOFactory.execSQLRead(selectReq);

                return reader["password"].ToString();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erreur lors de la récupération de l'utilisateur : " + ex.Message);
            }
            finally
            {
                DAOFactory.deconnecter();
            }

            return null; // Retourner null si l'utilisateur n'est pas trouvé ou en cas d'erreur
        }
    }
}
