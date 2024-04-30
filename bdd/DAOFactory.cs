using System;
using MySql.Data.MySqlClient;
using Mediateq_AP_SIO2;

/// <summary>
/// Espace de noms contenant les classes relatives à l'application Mediateq.
/// </summary>
namespace Mediateq_AP_SIO2
{
    /// <summary>
    /// Classe représentant une usine pour créer des objets d'accès aux données.
    /// </summary>
    class DAOFactory
    {
        private static MySqlConnection connexion;
        internal static MySqlConnection connection;

        /// <summary>
        /// Récupère une connexion à la base de données.
        /// </summary>
        /// <returns>La connexion à la base de données.</returns>
        public static MySqlConnection GetSqlConnection()
        {
            string serverIp = "localhost";
            string username = "root";
            string password = "";
            string databaseName = "mediateq";

            string dbConnectionString = string.Format("server={0};uid={1};pwd={2};database={3};", serverIp, username, password, databaseName);

            try
            {
                connexion = new MySqlConnection(dbConnectionString);
                return connexion;  // Retournez la connexion ici
            }
            catch (Exception e)
            {
                Console.WriteLine("Erreur connexion BDD", e.Message);
                return null;  // Gérez l'erreur de connexion ici si nécessaire
            }
        }


        /// <summary>
        /// Crée une connexion à la base de données.
        /// </summary>
        public static void creerConnection()
        {
            string serverIp = "localhost";
            string username = "root";
            string password = "";
            string databaseName = "mediateq";

            string dbConnectionString = string.Format("server={0};uid={1};pwd={2};database={3};", serverIp, username, password, databaseName);
           
            try
            {
                connexion = new MySqlConnection(dbConnectionString);
            }
            catch (Exception e)
            {
                Console.WriteLine("Erreur connexion BDD", e.Message);
            }
            
        }

        /// <summary>
        /// Connecte à la base de données.
        /// </summary>
        public static void connecter()
        {
            try
            {
                connexion.Open();
            }
            catch (Exception e)
            {
                throw new ExceptionSIO(2, "problème ouverture connexion BDD", e.Message);
            }
        }

        /// <summary>
        /// Déconnecte de la base de données.
        /// </summary>
        public static void deconnecter()
        {
            connexion.Close();
        }


        /// <summary>
        /// Exécute une requête de lecture et retourne un DataReader.
        /// </summary>
        /// <param name="requete">La requête SQL à exécuter.</param>
        /// <returns>Un DataReader contenant les résultats de la requête.</returns>
        public static MySqlDataReader execSQLRead(string requete)
        {
            MySqlCommand command;
            MySqlDataAdapter adapter;
            command = new MySqlCommand();
            command.CommandText = requete;
            command.Connection = connexion;

            adapter = new MySqlDataAdapter();
            adapter.SelectCommand = command;

            MySqlDataReader dataReader;
            dataReader = command.ExecuteReader();

            return dataReader;
        }

        /// <summary>
        /// Exécute une requête d'écriture (Insert ou Update).
        /// </summary>
        /// <param name="requete">La requête SQL à exécuter.</param>
        public static void execSQLWrite(string requete)
        {

            MySqlCommand command;
            command = new MySqlCommand();
            command.CommandText = requete;
            command.Connection = connexion;

            command.ExecuteNonQuery();
        }
    }
}
