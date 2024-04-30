using System;
/// <summary>
/// Espace de noms contenant les classes relatives à l'application Mediateq.
/// </summary>
namespace Mediateq_AP_SIO2
{
    /// <summary>
    /// Classe représentant une parution dans le système.
    /// </summary>
    public class Parution
    {
        private int numero;
        private DateTime dateParution;
        private string photo;
        private string idRevue;

        /// <summary>
        /// Initialise une nouvelle instance de la classe <see cref="Parution"/>.
        /// </summary>
        /// <param name="numero">Le numéro de la parution.</param>
        /// <param name="dateParution">La date de parution de la parution.</param>
        /// <param name="photo">Le chemin de la photo de la parution.</param>
        /// <param name="idRevue">L'identifiant de la revue associée à la parution.</param>
        public Parution(int numero, DateTime dateParution, string photo, string idRevue)
        {
            this.numero = numero;
            this.dateParution = dateParution;
            this.photo = photo;
            this.idRevue = idRevue;
        }

        /// <summary>
        /// Obtient ou définit le numéro de la parution.
        /// </summary>
        public int Numero { get => numero; set => numero = value; }

        /// <summary>
        /// Obtient ou définit la date de parution de la parution.
        /// </summary>
        public DateTime DateParution { get => dateParution; set => dateParution = value; }

        /// <summary>
        /// Obtient ou définit le chemin de la photo de la parution.
        /// </summary>
        public string Photo { get => photo; set => photo = value; }

        /// <summary>
        /// Obtient ou définit l'identifiant de la revue associée à la parution.
        /// </summary>
        public string IdRevue { get => idRevue; set => idRevue = value; }
    }
}
