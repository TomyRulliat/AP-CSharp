using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using Mediateq_AP_SIO2;
/// <summary>
/// Espace de noms contenant les classes relatives à l'application Mediateq.
/// </summary>
namespace Mediateq_AP_SIO2
{
    /// <summary>
    /// Classe fournissant des méthodes pour interagir avec la base de données concernant la presse.
    /// </summary>
    class DAOPresse
    {

        /// <summary>
        /// Récupère toutes les revues de la base de données.
        /// </summary>
        /// <returns>Une liste de revues.</returns>
        public static List<Revue> getAllRevues()
        {
            List<Revue> lesRevues = new List<Revue>();
            string req = "Select * from revue";

            DAOFactory.connecter();

            MySqlDataReader reader = DAOFactory.execSQLRead(req);

            while (reader.Read())
            {
                Revue titre = new Revue(reader[0].ToString(), reader[1].ToString(), char.Parse(reader[2].ToString()), reader[3].ToString(), DateTime.Parse(reader[5].ToString()), int.Parse(reader[4].ToString()), reader[6].ToString());
                lesRevues.Add(titre);
            }
            DAOFactory.deconnecter();

            return lesRevues;
        }

        /// <summary>
        /// Récupère toutes les parutions d'une revue donnée.
        /// </summary>
        /// <param name="pTitre">La revue pour laquelle récupérer les parutions.</param>
        /// <returns>Une liste de parutions de la revue spécifiée.</returns>
        public static List<Parution> getParutionByTitre(Revue pTitre)
        {
            List<Parution> lesParutions = new List<Parution>();
            string req = "Select * from parution where idRevue = " + pTitre.Id;

            DAOFactory.connecter();

            MySqlDataReader reader = DAOFactory.execSQLRead(req);

            while (reader.Read())
            {
                Parution parution = new Parution(int.Parse(reader[1].ToString()), DateTime.Parse(reader[2].ToString()), reader[3].ToString(), pTitre.Id);
                lesParutions.Add(parution);
            }
            DAOFactory.deconnecter();
            return lesParutions;
        }
    }
}
