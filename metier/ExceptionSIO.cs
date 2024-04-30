using System;
/// <summary>
/// Espace de noms contenant les classes relatives à l'application Mediateq.
/// </summary>
namespace Mediateq_AP_SIO2
{
    /// <summary>
    /// Classe représentant une exception personnalisée pour le système Mediateq_AP_SIO2.
    /// </summary>
    public class ExceptionSIO : Exception
    {
        private int niveauExc;
        private string libelleExc;

        /// <summary>
        /// Initialise une nouvelle instance de la classe <see cref="ExceptionSIO"/>.
        /// </summary>
        /// <param name="pNiveau">Le niveau de l'exception.</param>
        /// <param name="pLibelle">Le libellé de l'exception.</param>
        /// <param name="pMessage">Le message d'erreur de l'exception.</param>
        public ExceptionSIO(int pNiveau, string pLibelle, string pMessage) : base(pMessage)
        {
            niveauExc = pNiveau;
            libelleExc = pLibelle;
        }

        /// <summary>
        /// Obtient ou définit le niveau de l'exception.
        /// </summary>
        public int NiveauExc { get => niveauExc; set => niveauExc = value; }

        /// <summary>
        /// Obtient ou définit le libellé de l'exception.
        /// </summary>
        public string LibelleExc { get => libelleExc; set => libelleExc = value; }
    }
}
