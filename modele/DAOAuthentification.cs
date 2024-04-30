using System;
using Mediateq_AP_SIO2;
using MySql.Data.MySqlClient;
/// <summary>
/// Espace de noms contenant les classes relatives à l'application Mediateq.
/// </summary>
namespace Mediateq_AP_SIO2
{
    /// <summary>
    /// Classe fournissant des méthodes pour l'authentification des utilisateurs.
    /// </summary>
    public class DAOAuthentification
    {
        /// <summary>
        /// Vérifie si un utilisateur est valide en fonction de son nom d'utilisateur et de son mot de passe.
        /// </summary>
        /// <param name="username">Le nom d'utilisateur de l'utilisateur.</param>
        /// <param name="password">Le mot de passe de l'utilisateur.</param>
        /// <returns>L'utilisateur si les informations d'identification sont valides, sinon null.</returns>
        public static Utilisateur IsValidUser(string username, string password)
        {
            try
            {
                DAOFactory.connecter();

                string req = "SELECT login, nom, prenom, password, salt, service.libelle FROM utilisateurs " +
                             "JOIN service ON utilisateurs.service = service.id " +
                             "WHERE login = '" + username + "'";

                using (MySqlDataReader reader = DAOFactory.execSQLRead(req))
                {
                    if (reader.Read())
                    {
                        string storedPassword = reader["password"].ToString();
                        string salt = reader["salt"].ToString();

                        // Comparer les mots de passe hachés
                        if (Hash.VerifyPassword(storedPassword, password, salt))
                        {
                            Utilisateur user = new Utilisateur(
                                reader["login"].ToString(),
                                reader["nom"].ToString(),
                                reader["prenom"].ToString(),
                                reader["password"].ToString(), // Attention, il peut être plus sûr de ne pas stocker le mot de passe en clair.
                                reader["libelle"].ToString()
                            );

                            return user;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erreur de connexion à la base de données : " + ex.Message);
            }
            finally
            {
                DAOFactory.deconnecter();
            }

            return null; // Les informations d'authentification sont incorrectes ou une erreur s'est produite.
        }

        /// <summary>
        /// Méthode de test pour créer un utilisateur.
        /// </summary>
        public static void CreateUser()
        {
            try
            {
                // Générer un sel pour le nouveau compte
                string salt = Hash.GenerateSalt();
                string password = "CultureTest";
                // Hasher le mot de passe avec le sel
                string hashedPassword = Hash.HashPassword(password, salt);

                DAOFactory.connecter();

                // Insérer les informations du nouvel utilisateur dans la base de données
                string insertReq = "INSERT INTO utilisateurs (login, nom, prenom, password, service, salt) " +
                                   "VALUES ('CultureTest', 'Culture', 'Test', '" + hashedPassword + "', 1, '" + salt + "')";

                DAOFactory.execSQLWrite(insertReq);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erreur lors de la création de l'utilisateur : " + ex.Message);
            }
            finally
            {
                DAOFactory.deconnecter();
            }
            ;
        }

    }
}
