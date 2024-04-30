using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

/// <summary>
/// Espace de noms contenant les classes relatives à l'application Mediateq.
/// </summary>
namespace Mediateq_AP_SIO2
{
    /// <summary>
    /// Formulaire principal de l'application Mediateq.
    /// </summary>
    public partial class FrmMediateq : Form
    {
        #region Variables globales

        static List<Categorie> lesCategories;
        static List<Descripteur> lesDescripteurs;
        static List<Revue> lesRevues;
        static List<Livre> lesLivres;
        static List<DVD> lesDVD;
        static List<Utilisateur> lesUtilisateurs;
        static List<Abonne> lesAbonnes;

        #endregion

        #region Procédures évènementielles

        /// <summary>
        /// Initialise une nouvelle instance de la classe <see cref="FrmMediateq"/>.
        /// </summary>
        public FrmMediateq()
        {
            InitializeComponent();
        }


        private void FrmMediateq_Load(object sender, EventArgs e)
        {
            try
            {
                // Création de la connexion avec la base de données
                DAOFactory.creerConnection();

                // Chargement des objets en mémoire
                lesDescripteurs = DAODocuments.getAllDescripteurs();
                lesRevues = DAOPresse.getAllRevues();
            }
            catch (ExceptionSIO exc)
            {
                MessageBox.Show(exc.NiveauExc + " - " + exc.LibelleExc + " - " + exc.Message);
            }

            // Désactivez tous les onglets et leurs boutons d'accès sauf l'onglet d'authentification.
            foreach (TabPage tab in tabAbonnes.TabPages)
            {
                tabAbonnes.TabPages.Remove(tab);
            }
            tabAbonnes.TabPages.Add(tabAuth);

            btnLogout.Enabled = false;
        }

        #endregion


        #region Parutions
        //-----------------------------------------------------------
        // ONGLET "PARUTIONS"
        //------------------------------------------------------------
        private void tabParutions_Enter(object sender, EventArgs e)
        {
            cbxTitres.DataSource = lesRevues;
            cbxTitres.DisplayMember = "titre";
        }

        private void cbxTitres_SelectedIndexChanged(object sender, EventArgs e)
        {
            List<Parution> lesParutions;

            Revue titreSelectionne = (Revue)cbxTitres.SelectedItem;
            lesParutions = DAOPresse.getParutionByTitre(titreSelectionne);

            // ré-initialisation du dataGridView
            dgvParutions.Rows.Clear();

            // Parcours de la collection des titres et alimentation du datagridview
            foreach (Parution parution in lesParutions)
            {
                dgvParutions.Rows.Add(parution.Numero, parution.DateParution, parution.Photo);
            }

        }
        #endregion


        #region Revues
        //-----------------------------------------------------------
        // ONGLET "TITRES"
        //------------------------------------------------------------
        private void tabTitres_Enter(object sender, EventArgs e)
        {
            cbxDomaines.DataSource = lesDescripteurs;
            cbxDomaines.DisplayMember = "libelle";
        }

        private void cbxDomaines_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Objet Domaine sélectionné dans la comboBox
            Descripteur domaineSelectionne = (Descripteur)cbxDomaines.SelectedItem;

            // ré-initialisation du dataGridView
            dgvTitres.Rows.Clear();

            // Parcours de la collection des titres et alimentation du datagridview
            foreach (Revue revue in lesRevues)
            {
                if (revue.IdDescripteur == domaineSelectionne.Id)
                {
                    dgvTitres.Rows.Add(revue.Id, revue.Titre, revue.Empruntable, revue.DateFinAbonnement, revue.DelaiMiseADispo);
                }
            }
        }
        #endregion


        #region Livres
        //-----------------------------------------------------------
        // ONGLET "LIVRES"
        //-----------------------------------------------------------

        private void tabLivres_Enter(object sender, EventArgs e)
        {
            // Chargement des objets en mémoire
            lesCategories = DAODocuments.getAllCategories();
            lesDescripteurs = DAODocuments.getAllDescripteurs();
            lesLivres = DAODocuments.getAllLivres();
            lesRevues = DAOPresse.getAllRevues();

            // Effacer les éléments existants de la ComboBox
            cBoxDocCMDLivres.Items.Clear();

            // Ajouter les titres de tous les livres à la ComboBox
            foreach (Livre livre in lesLivres)
            {
                cBoxDocCMDLivres.Items.Add(livre.Titre);
            }
            BtnCommander.Enabled = false;
            numCreaCMD.Enabled = false;
        }

        private void btnRechercher_Click(object sender, EventArgs e)
        {
            // On réinitialise les labels
            lblNumero.Text = "";
            lblTitre.Text = "";
            lblAuteur.Text = "";
            lblCollection.Text = "";
            lblISBN.Text = "";
            lblImage.Text = "";

            // On recherche le livre correspondant au numéro de document saisi.
            // S'il n'existe pas: on affiche un popup message d'erreur
            bool trouve = false;
            foreach (Livre livre in lesLivres)
            {
                if (livre.IdDoc == txbNumDoc.Text)
                {
                    lblNumero.Text = livre.IdDoc;
                    lblTitre.Text = livre.Titre;
                    lblAuteur.Text = livre.Auteur;
                    lblCollection.Text = livre.LaCollection;
                    lblISBN.Text = livre.ISBN1;
                    lblImage.Text = livre.Image;
                    trouve = true;
                }
            }
            if (!trouve)
                MessageBox.Show("Document non trouvé dans les livres");
        }


        private void btnRechercheRevue_Click(object sender, EventArgs e)
        {
            // On réinitialise les labels
            lblNumeroRevue.Text = "";
            lblTitreRevue.Text = "";

            // On recherche le livre correspondant au numéro de document saisi.
            // S'il n'existe pas: on affiche un popup message d'erreur
            bool trouve = false;
            foreach (Revue revue in lesRevues)
            {
                if (revue.Id == txtboxNumDocRechercheRevue.Text)
                {
                    lblNumeroRevue.Text = revue.Id;
                    lblTitreRevue.Text = revue.Titre;
                    trouve = true;
                }
            }
            if (!trouve)
                MessageBox.Show("Document non trouvé dans les livres");
        }

        private void txbTitre_TextChanged(object sender, EventArgs e)
        {
            dgvLivres.Rows.Clear();

            // On parcourt tous les livres. Si le titre matche avec la saisie, on l'affiche dans le datagrid.
            foreach (Livre livre in lesLivres)
            {
                // on passe le champ de saisie et le titre en minuscules car la méthode Contains
                // tient compte de la casse.
                string saisieMinuscules;
                saisieMinuscules = txbTitre.Text.ToLower();
                string titreMinuscules;
                titreMinuscules = livre.Titre.ToLower();

                //on teste si le titre du livre contient ce qui a été saisi
                if (titreMinuscules.Contains(saisieMinuscules))
                {
                    dgvLivres.Rows.Add(livre.IdDoc, livre.Titre, livre.Auteur, livre.ISBN1, livre.LaCollection);
                }
            }
        }



        private void txtboxRechercheRevue_TextChanged(object sender, EventArgs e)
        {
            DGVRechercheRevues.Rows.Clear();

            // On parcourt tous les livres. Si le titre matche avec la saisie, on l'affiche dans le datagrid.
            foreach (Revue revue in lesRevues)
            {
                // on passe le champ de saisie et le titre en minuscules car la méthode Contains
                // tient compte de la casse.
                string saisieMinuscules;
                saisieMinuscules = txtboxRechercheRevue.Text.ToLower();
                string titreMinuscules;
                titreMinuscules = revue.Titre.ToLower();

                //on teste si le titre du livre contient ce qui a été saisi
                if (titreMinuscules.Contains(saisieMinuscules))
                {
                    DGVRechercheRevues.Rows.Add(revue.Id, revue.Titre);
                }
            }
        }
        #endregion


        #region DVD
        //-----------------------------------------------------------
        // ONGLET "DVD"
        //-----------------------------------------------------------

        private void tabDVD_Enter(object sender, EventArgs e)
        {
            // Chargement des objets en mémoire
            lesCategories = DAODocuments.getAllCategories();
            lesDescripteurs = DAODocuments.getAllDescripteurs();
            lesDVD = DAODocuments.getAllDVD();


            // Effacer les éléments existants de la ComboBox
            cBoxDocCMDDVD.Items.Clear();

            // Ajouter les titres de tous les livres à la ComboBox
            foreach (DVD dvd in lesDVD)
            {
                cBoxDocCMDDVD.Items.Add(dvd.Titre);
            }
            BtnCommanderDVD.Enabled = false;
            numCreaCMDDVD.Enabled = false;

        }

        private void btnRechercherDVD_Click(object sender, EventArgs e)
        {
            // On réinitialise les labels
            lblNumeroDVD.Text = "";
            lblTitreDVD.Text = "";
            lblRealisateurDVD.Text = "";
            lblDureeDVD.Text = "";
            lblSynopsisDVD.Text = "";
            lblImageDVD.Text = "";

            // On recherche le livre correspondant au numéro de document saisi.
            // S'il n'existe pas: on affiche un popup message d'erreur
            bool trouve = false;
            foreach (DVD dvd in lesDVD)
            {
                if (dvd.IdDoc == txbNumDocDVD.Text)
                {
                    lblNumeroDVD.Text = dvd.IdDoc;
                    lblTitreDVD.Text = dvd.Titre;
                    lblRealisateurDVD.Text = dvd.Realisateur;
                    lblDureeDVD.Text = dvd.Duree.ToString();
                    lblSynopsisDVD.Text = dvd.Synopsis;
                    lblImageDVD.Text = dvd.Image;
                    trouve = true;
                }
            }
            if (!trouve)
                MessageBox.Show("Document non trouvé dans les dvds");
        }

        private void txbTitreDVD_TextChanged(object sender, EventArgs e)
        {
            dgvDVD.Rows.Clear();

            // On parcourt tous les livres. Si le titre matche avec la saisie, on l'affiche dans le datagrid.
            foreach (DVD dvd in lesDVD)
            {
                // on passe le champ de saisie et le titre en minuscules car la méthode Contains
                // tient compte de la casse.
                string saisieMinuscules;
                saisieMinuscules = txbTitreDVD.Text.ToLower();
                string titreMinuscules;
                titreMinuscules = dvd.Titre.ToLower();

                //on teste si le titre du livre contient ce qui a été saisi
                if (titreMinuscules.Contains(saisieMinuscules))
                {
                    dgvDVD.Rows.Add(dvd.IdDoc, dvd.Titre, dvd.Realisateur, dvd.Synopsis, dvd.Duree);
                }
            }
        }
        #endregion

        private void groupBox3_Enter(object sender, EventArgs e)
        {

        }

        private void lbl_utilisateur_Click(object sender, EventArgs e)
        {

        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            string enteredUsername = Utilisateur.Text;
            string enteredPassword = MotDePasse.Text;

            Utilisateur user = DAOAuthentification.IsValidUser(enteredUsername, enteredPassword);
            // Vérifiez les informations d'authentification dans la base de données ou un autre stockage sécurisé.
            if (user != null)
            {
                // Les informations d'authentification sont correctes, activez le TabControl.
                tabAbonnes.Enabled = true;
                lbl_mdp.Text = enteredPassword;
                lbl_utilisateur.Text = enteredUsername;
                lbl_service.Text = user.Service;
                Utilisateur.Text = "";
                MotDePasse.Text = "";
                Utilisateur.Enabled = false;
                MotDePasse.Enabled = false;
                btnLogin.Enabled = false;
                btnLogout.Enabled = true;

                if (user.Service.Equals("Administrateur"))
                {
                    tabAbonnes.TabPages.Add(tabAbonne);
                    tabAbonnes.TabPages.Add(tabUtilisateurs);
                    tabAbonnes.TabPages.Add(tabAbonnements);
                    tabAbonnes.TabPages.Add(tabCmd);
                    tabAbonnes.TabPages.Add(tabExempDoc);
                    tabAbonnes.TabPages.Add(tabLivres);
                    tabAbonnes.TabPages.Add(tabParutions);
                    tabAbonnes.TabPages.Add(tabTitres);
                    tabAbonnes.TabPages.Add(tabDVD);
                    tabAbonnes.TabPages.Add(tabRevues);
                    tabAbonnes.TabPages.Add(tabSuiviEtatDoc);
                }
                if (user.Service.Equals("Prêt"))
                {
                    tabAbonnes.TabPages.Add(tabLivres);
                    tabAbonnes.TabPages.Add(tabParutions);
                    tabAbonnes.TabPages.Add(tabTitres);
                    tabAbonnes.TabPages.Add(tabDVD);
                    tabAbonnes.TabPages.Add(tabRevues);
                    tabAbonnes.TabPages.Add(tabCmd);
                    tabAbonnes.TabPages.Add(tabSuiviEtatDoc);
                }
                if (user.Service.Equals("Culture"))
                {
                }
                if (user.Service.Equals("Administratif"))
                {
                    tabAbonnes.TabPages.Add(tabAbonne);
                    tabAbonnes.TabPages.Add(tabAbonnements);
                    tabAbonnes.TabPages.Add(tabLivres);
                    tabAbonnes.TabPages.Add(tabParutions);
                    tabAbonnes.TabPages.Add(tabTitres);
                    tabAbonnes.TabPages.Add(tabDVD);
                    tabAbonnes.TabPages.Add(tabRevues);
                    tabAbonnes.TabPages.Add(tabExempDoc);
                }

                MessageBox.Show("Vous êtes bien connecté !");
            }
            else
            {
                MessageBox.Show("Nom d'utilisateur ou mot de passe incorrect.");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            // Désactivez tous les onglets et leurs boutons d'accès sauf l'onglet d'authentification.
            foreach (TabPage tab in tabAbonnes.TabPages)
            {
                tabAbonnes.TabPages.Remove(tab);
            }
            tabAbonnes.TabPages.Add(tabAuth);
            lbl_mdp.Text = "";
            lbl_utilisateur.Text = "";
            lbl_service.Text = "";
            Utilisateur.Enabled = true;
            MotDePasse.Enabled = true;
            btnLogin.Enabled = true;
            btnLogout.Enabled = false;
        }

        private void btnTestCreateUser_Click(object sender, EventArgs e)
        {
            try
            {
                DAOAuthentification.CreateUser();

                // Affichez des informations sur le nouvel utilisateur créé
                MessageBox.Show($"Nouvel utilisateur créé ");
            }
            catch (Exception ex)
            {
                // Gérez les erreurs de manière appropriée
                MessageBox.Show("Erreur lors de la création de l'utilisateur : " + ex.Message);
            }
        }

        private void tabUtilisateurs_Enter(object sender, EventArgs e)
        {
            // Chargement des objets en mémoire
            lesUtilisateurs = DAOPersonnes.getAllUtilisateurs();
            dgvUtil.Rows.Clear();
            txbUtil.Text = "";
        }

        private void txbUtil_TextChanged(object sender, EventArgs e)
        {
            dgvUtil.Rows.Clear();

            // On parcourt tous les livres. Si le titre matche avec la saisie, on l'affiche dans le datagrid.
            foreach (Utilisateur util in lesUtilisateurs)
            {
                // on passe le champ de saisie et le titre en minuscules car la méthode Contains
                // tient compte de la casse.
                string saisieMinuscules;
                saisieMinuscules = txbUtil.Text.ToLower();
                string loginMinuscules;
                loginMinuscules = util.Login.ToLower();

                //on teste si le titre du livre contient ce qui a été saisi
                if (loginMinuscules.Contains(saisieMinuscules))
                {
                    dgvUtil.Rows.Add(util.Login, util.Nom, util.Prenom, util.Password, util.Service);
                }
            }
        }

        private void btnCREATEutil_Click(object sender, EventArgs e)
        {
            string login = txbLOGINcreate.Text;
            string prenom = txbPRENOMcreate.Text;
            string nom = txbNOMcreate.Text;
            string password = txbMDPcreate.Text;
            int numService = cBoxService.SelectedIndex;
            string service = cBoxService.Text;

            try
            {
                if (!String.IsNullOrEmpty(login) &&
                    !String.IsNullOrEmpty(prenom) &&
                    !String.IsNullOrEmpty(nom) &&
                    !String.IsNullOrEmpty(password) &&
                    numService != -1)
                {
                    // Utilisez la méthode CreateUtilisateur pour créer un nouvel utilisateur
                    bool trouve = DAOPersonnes.CreateUtilisateur(login, password, nom, prenom, service);
                    if (trouve)
                    {
                        string HashedPwd = DAOPersonnes.getMDP(login);
                        Utilisateur newUtil = new Utilisateur(login, nom, prenom, HashedPwd, service);

                        lesUtilisateurs.Add(newUtil);

                        MessageBox.Show("Utilisateur ajouté avec succès !");
                    }
                    else
                    {
                        MessageBox.Show("L'utilisateur n'a pas pu être ajouté !");
                    }
                }
                else
                {
                    MessageBox.Show("Veuillez remplir tous les champs !");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Exception : " + ex.Message);
            }
            dgvUtil.Rows.Clear();
            txbUtil.Text = "";
            txbLOGINcreate.Text = "";
            txbPRENOMcreate.Text = "";
            txbNOMcreate.Text = "";
            txbMDPcreate.Text = "";
            cBoxService.SelectedIndex = -1;
        }


        private void btnSUPPRutil_Click(object sender, EventArgs e)
        {
            string login = txbLoginSupression.Text;
            try
            {
                if (String.IsNullOrEmpty(login))
                {
                    // Utilisez la méthode CreateUtilisateur pour créer un nouvel utilisateur
                    bool fonctionne = DAOPersonnes.DeleteUtilisateurByLogin(login);

                    // Chargement des objets en mémoire
                    lesUtilisateurs = DAOPersonnes.getAllUtilisateurs();

                    dgvUtil.Rows.Clear();
                    txbUtil.Text = "";

                    if (fonctionne)
                    {
                        MessageBox.Show("Utilisateur supprimé avec succès !");
                    }
                    else
                    {
                        MessageBox.Show("L'utilisateur n'a pas pu être supprimé !");
                    }

                    txbLoginSupression.Text = "";
                }
                else
                {
                    MessageBox.Show("Veuillez remplir le champ !");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Exception : " + ex.Message);
            }
        }

        private void tabAuth_Click(object sender, EventArgs e)
        {

        }

        private void txbRechercheAbonne_TextChanged(object sender, EventArgs e)
        {
            dgvAbo.Rows.Clear();

            // On parcourt tous les livres. Si le titre matche avec la saisie, on l'affiche dans le datagrid.
            foreach (Abonne abo in lesAbonnes)
            {
                // on passe le champ de saisie et le titre en minuscules car la méthode Contains
                // tient compte de la casse.
                string saisieMinuscules;
                saisieMinuscules = txbRechercheAbonne.Text.ToLower();
                string adresseMinuscules;
                adresseMinuscules = abo.AdresseCourriel.ToLower();

                //on teste si le titre du livre contient ce qui a été saisi
                if (adresseMinuscules.Contains(saisieMinuscules))
                {
                    dgvAbo.Rows.Add(abo.AdresseCourriel, abo.MotDePasse, abo.Prenom, abo.Nom, abo.Adresse, abo.NumeroTelephone, abo.DatePremierAbonnement, abo.DateFinAbonnement, abo.DateNaissance, abo.TypeAbonnement);
                }
            }
        }

        private void tabAbonne_Enter(object sender, EventArgs e)
        {
            // Chargement des objets en mémoire
            lesAbonnes = DAOPersonnes.getAllAbonnes();
            dgvAbo.Rows.Clear();
            txbRechercheAbonne.Text = "";
        }

        private void btnAboCrea_Click(object sender, EventArgs e)
        {
            string adresseCourriel = txbAboCreaAdresseCourriel.Text;
            string mdp = txbAboCreaMDP.Text;
            string prenom = txbAboCreaPrenom.Text;
            string nom = txbAboCreaNom.Text;
            string adresse = txbAboCreaAdresse.Text;
            string telephone = txbAboCreaTelephone.Text;
            DateTime dateNaissance = dtpAboCreaDate.Value;
            string typeAbo = cBoxAboCreaTypeAbo.Text;
            int numTypeAbo = cBoxAboCreaTypeAbo.SelectedIndex;
            DateTime datePremierAbo = DateTime.Now;
            DateTime dateFinAbo = DateTime.Now;
            if (numTypeAbo != 3)
            {
                dateFinAbo = datePremierAbo.AddDays(21);
            }
            else
            {
                dateFinAbo = datePremierAbo.AddDays(42);
            }


            try
            {
                if (!String.IsNullOrEmpty(adresseCourriel) &&
                    !String.IsNullOrEmpty(mdp) &&
                    !String.IsNullOrEmpty(prenom) &&
                    !String.IsNullOrEmpty(nom) &&
                    !String.IsNullOrEmpty(adresse) &&
                    !String.IsNullOrEmpty(telephone) &&
                    numTypeAbo != -1)
                {
                    // Vérification de l'adresse e-mail
                    if (IsValidEmail(adresseCourriel))
                    {
                        // Vérification du numéro de téléphone
                        if (IsNumeric(telephone))
                        {
                            // Utilisez la méthode CreateAbonne pour créer un nouvel abonné
                            int newID = DAOPersonnes.CreateAbonne(nom, prenom, adresse, dateNaissance, adresseCourriel, telephone, mdp, dateFinAbo, datePremierAbo, numTypeAbo);
                            if (newID != -1)
                            {
                                Abonne newAbo = new Abonne(newID, nom, prenom, adresse, dateNaissance, adresseCourriel, telephone, mdp, dateFinAbo, datePremierAbo, typeAbo, numTypeAbo);
                                lesAbonnes.Add(newAbo);
                                MessageBox.Show("Abonné ajouté avec succès !");
                            }
                        }
                        else
                        {
                            MessageBox.Show("Le numéro de téléphone ne doit contenir que des chiffres !");
                        }
                    }
                    else
                    {
                        MessageBox.Show("Adresse e-mail invalide !");
                    }
                }
                else
                {
                    MessageBox.Show("Veuillez remplir tous les champs !");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Exception : " + ex.Message);
            }
            dgvAbo.Rows.Clear();
            txbAboCreaAdresseCourriel.Text = "";
            txbAboCreaMDP.Text = "";
            txbAboCreaPrenom.Text = "";
            txbAboCreaNom.Text = "";
            txbAboCreaAdresse.Text = "";
            txbAboCreaTelephone.Text = "";
            cBoxAboCreaTypeAbo.SelectedIndex = -1;
        }

        private bool IsNumeric(string input)
        {
            return int.TryParse(input, out _);
        }


        private bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }


        private void btnSelectAboModif_Click(object sender, EventArgs e)
        {
            bool trouve = false;
            string adresseCourriel = txbModifAdresseCourriel.Text;

            foreach (Abonne unAbo in lesAbonnes)
            {
                if (unAbo.AdresseCourriel.Equals(adresseCourriel))
                {
                    trouve = true;
                    txbModifAdresseCourriel.Enabled = false;
                    txbAboModifAdresse.Text = unAbo.Adresse;
                    txbAboModifNom.Text = unAbo.Nom;
                    txbAboModifPrenom.Text = unAbo.Prenom;
                    txbAboModifTelephone.Text = unAbo.NumeroTelephone;
                    dtpAboModifDateNaissance.Value = unAbo.DateNaissance;
                    cBoxAboModifTypeAbo.SelectedIndex = unAbo.NumTypeAbonnement;

                    // Check if the subscription expiration date is within 30 days or has already passed
                    DateTime currentDate = DateTime.Now;
                    DateTime expirationDate = unAbo.DateFinAbonnement;
                    int remainingDays = (expirationDate - currentDate).Days;

                    if (expirationDate <= currentDate)
                    {
                        MessageBox.Show("Attention : La date d'expiration de l'abonnement a déjà expiré !");
                    }
                    else if (remainingDays <= 30)
                    {
                        MessageBox.Show($"Attention : La date d'expiration de l'abonnement est dans {remainingDays} jours.");
                    }
                    else
                    {
                        cbRenAbo.Visible = false;
                    }
                }
            }

            if (!trouve)
            {
                MessageBox.Show("Abonné non trouvé !");
            }
        }



        private void btnAboModif_Click(object sender, EventArgs e)
        {
            string AdresseCourriel = txbModifAdresseCourriel.Text;
            string Adresse = txbAboModifAdresse.Text;
            string Nom = txbAboModifNom.Text;
            string Prenom = txbAboModifPrenom.Text;
            string Telephone = txbAboModifTelephone.Text;
            int NumTypeAbonnement = cBoxAboModifTypeAbo.SelectedIndex;
            string typeAbonnement = cBoxAboModifTypeAbo.SelectedItem?.ToString();
            DateTime DateNaissance = dtpAboModifDateNaissance.Value;
            DateTime DateFinAbo = DAOPersonnes.getAbonneByAdresse(txbModifAdresseCourriel.Text).DateFinAbonnement;
            bool renouveler = cbRenAbo.Checked;

            try
            {
                if (!String.IsNullOrEmpty(AdresseCourriel) &&
                    !String.IsNullOrEmpty(Adresse) &&
                    !String.IsNullOrEmpty(Nom) &&
                    !String.IsNullOrEmpty(Prenom) &&
                    !String.IsNullOrEmpty(Telephone) &&
                    NumTypeAbonnement != -1 &&
                    typeAbonnement != null)
                {
                    string ancienAbonnement = DAOPersonnes.GetTypeAbonnement(AdresseCourriel);
                    if (renouveler)
                    {
                        if (NumTypeAbonnement != 3)
                        {
                            DateFinAbo = DateFinAbo.AddDays(21);
                        }
                        else
                        {
                            DateFinAbo = DateFinAbo.AddDays(42);
                        }
                        DAOPersonnes.UpdateAbonne(Nom, Prenom, Adresse, DateNaissance, AdresseCourriel, Telephone, DateFinAbo, NumTypeAbonnement);
                    }
                    else
                    {
                        DAOPersonnes.UpdateAbonne(Nom, Prenom, Adresse, DateNaissance, AdresseCourriel, Telephone);
                    }


                    foreach (Abonne unAbo in lesAbonnes)
                    {
                        if (unAbo.AdresseCourriel.Equals(AdresseCourriel))
                        {
                            unAbo.Nom = Nom;
                            unAbo.Prenom = Prenom;
                            unAbo.NumeroTelephone = Telephone;
                            unAbo.NumTypeAbonnement = NumTypeAbonnement;
                            unAbo.TypeAbonnement = typeAbonnement;
                            unAbo.Adresse = Adresse;
                            unAbo.DateFinAbonnement = DateFinAbo;
                            unAbo.DateNaissance = DateNaissance;
                        }
                    }

                    MessageBox.Show("Abonné modifié avec succès!");

                    txbModifAdresseCourriel.Enabled = true;
                    txbAboModifAdresse.Text = "";
                    txbAboModifNom.Text = "";
                    txbAboModifPrenom.Text = "";
                    txbAboModifTelephone.Text = "";
                    cBoxAboModifTypeAbo.SelectedIndex = -1;
                    dgvAbo.Rows.Clear();
                }
                else
                {
                    MessageBox.Show("Veuillez remplir tous les champs !");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Exception : " + ex.Message);
            }
        }


        private void btnAboSuppr_Click(object sender, EventArgs e)
        {
            string AdresseASupprimer = txbModifAdresseCourriel.Text;

            try
            {
                if (!String.IsNullOrEmpty(AdresseASupprimer))
                {
                    // Supprimer l'abonné de la base de données
                    DAOPersonnes.DeleteAbonneByAdresseCourriel(AdresseASupprimer);

                    // Rechercher l'abonné dans la liste et le supprimer
                    Abonne abonneASupprimer = lesAbonnes.FirstOrDefault(a => a.AdresseCourriel.Equals(AdresseASupprimer));

                    if (abonneASupprimer != null)
                    {
                        // Supprimer l'abonné de la liste
                        lesAbonnes.Remove(abonneASupprimer);
                        MessageBox.Show("L'abonné a bien été supprimé !");

                        txbModifAdresseCourriel.Enabled = true;
                        txbAboModifAdresse.Text = "";
                        txbAboModifNom.Text = "";
                        txbAboModifPrenom.Text = "";
                        txbAboModifTelephone.Text = "";
                        txbModifAdresseCourriel.Text = "";
                        cBoxAboModifTypeAbo.SelectedIndex = 0;
                        dgvAbo.Rows.Clear();
                    }
                    else
                    {
                        MessageBox.Show("L'abonné avec l'adresse courriel spécifiée n'a pas pu être supprimé !");
                    }
                }
                else
                {
                    MessageBox.Show("Veuillez spécifier une adresse courriel !");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Exception : " + ex.Message);
            }
        }

        private void btnDeselectAboModif_Click(object sender, EventArgs e)
        {
            txbModifAdresseCourriel.Enabled = true;
            txbModifAdresseCourriel.Text = "";
            txbAboModifAdresse.Text = "";
            txbAboModifNom.Text = "";
            txbAboModifPrenom.Text = "";
            txbAboModifTelephone.Text = "";
            dtpAboModifDateNaissance.Value = dtpAboModifDateNaissance.MinDate;
            cBoxAboModifTypeAbo.SelectedIndex = -1;

        }

        private void cBoxAboModifTypeAbo_SelectedIndexChanged(object sender, EventArgs e)
        {
            cbRenAbo.Checked = true;
        }

        private void tabAbonne_Click(object sender, EventArgs e)
        {

        }

        private void txbAboCreaAdresseCourriel_TextChanged(object sender, EventArgs e)
        {

        }

        private void BtnCommander_Click(object sender, EventArgs e)
        {
            if (numCreaCMD.Value > 0)
            {
                string idDoc = DAODocuments.getDocumentByTitre(cBoxDocCMDLivres.Text).IdDoc;
                DAODocuments.createCommande(idDoc, Convert.ToInt32(numCreaCMD.Value), Int32.Parse(lblPrix.Text));
                MessageBox.Show("Commande créée avec succès !");
                numCreaCMD.Enabled = false;
                numCreaCMD.Value = 0;
                BtnCommander.Enabled = false;
                lblExDispo.Text = "";
                lblPrix.Text = "";
                btnCreaCMDSelect.Enabled = true;
                cBoxDocCMDLivres.Enabled = true;
                cBoxDocCMDLivres.SelectedIndex = -1;
            }
            else
            {
                MessageBox.Show("Nombre d'exemplaire non conforme !");
            }
        }

        private void label51_Click(object sender, EventArgs e)
        {

        }

        private void label60_Click(object sender, EventArgs e)
        {

        }


        private void tabCmd_Enter(object sender, EventArgs e)
        {
            BtnChangeCMD.Enabled = false;
            btnLivrerCMD.Enabled = false;
            btnSupprCMD.Enabled = false;
            btnReglerCMD.Enabled = false;
            btnCloturerCMD.Enabled = false;
            btnRelancerCMD.Enabled = false;
        }

        private void BtnSelectCMD_Click(object sender, EventArgs e)
        {
            Commande cmd = DAODocuments.getCommandeById(txtBoxSaisieNumCMD.Text);
            if (cmd == null)
            {
                MessageBox.Show("Aucun document n'a été trouvé !");
            }
            else
            {
                txtBoxSaisieNumCMD.Enabled = false;
                BtnSelectCMD.Enabled = false;
                BtnChangeCMD.Enabled = true;
                lblCMDNum.Text = cmd.Id;
                lblCMDNbEx.Text = cmd.NbExemplaire.ToString();
                lblCMDDate.Text = cmd.DateCommande.ToString();
                lblCMDMontant.Text = cmd.Montant.ToString();
                lblCMDDocument.Text = cmd.Document.Titre;
                lblCMDEtat.Text = cmd.Etat;

                switch (cmd.Etat)
                {
                    case "en cours":
                        btnLivrerCMD.Enabled = true;
                        btnSupprCMD.Enabled = true;
                        btnRelancerCMD.Enabled = true;
                        btnReglerCMD.Enabled = false;
                        btnCloturerCMD.Enabled = false;
                        break;
                    case "livrée":
                        btnLivrerCMD.Enabled = false;
                        btnSupprCMD.Enabled = false;
                        btnReglerCMD.Enabled = true;
                        btnCloturerCMD.Enabled = false;
                        break;
                    case "relancée":
                        btnLivrerCMD.Enabled = true;
                        btnSupprCMD.Enabled = true;
                        btnRelancerCMD.Enabled = false;
                        btnReglerCMD.Enabled = false;
                        btnCloturerCMD.Enabled = false;
                        break;
                    case "réglée":
                        btnLivrerCMD.Enabled = false;
                        btnSupprCMD.Enabled = false;
                        btnRelancerCMD.Enabled = false;
                        btnReglerCMD.Enabled = false;
                        btnCloturerCMD.Enabled = true;
                        break;
                    case "clôturée":
                        btnLivrerCMD.Enabled = false;
                        btnSupprCMD.Enabled = false;
                        btnRelancerCMD.Enabled = false;
                        btnReglerCMD.Enabled = false;
                        btnCloturerCMD.Enabled = false;
                        break;
                    default:
                        break;
                }
            }

        }

        private void BtnChangeCMD_Click(object sender, EventArgs e)
        {
            txtBoxSaisieNumCMD.Text = "";
            txtBoxSaisieNumCMD.Enabled = true;
            BtnSelectCMD.Enabled = true;
            BtnChangeCMD.Enabled = false;
            btnLivrerCMD.Enabled = false;
            btnSupprCMD.Enabled = false;
            btnRelancerCMD.Enabled = false;
            btnReglerCMD.Enabled = false;
            btnCloturerCMD.Enabled = false;
            lblCMDNum.Text = "";
            lblCMDNbEx.Text = "";
            lblCMDDate.Text = "";
            lblCMDMontant.Text = "";
            lblCMDDocument.Text = "";
            lblCMDEtat.Text = "";
        }

        private void btnLivrerCMD_Click(object sender, EventArgs e)
        {
            Document doc = DAODocuments.getDocumentByTitre(lblCMDDocument.Text);
            DAODocuments.createEx(Int32.Parse(lblCMDNbEx.Text), Int32.Parse(doc.IdDoc));
            DAODocuments.updateEtatCommande(txtBoxSaisieNumCMD.Text, "livrée");
            lblCMDEtat.Text = "livrée";
            btnLivrerCMD.Enabled = false;
            btnSupprCMD.Enabled = false;
            btnRelancerCMD.Enabled = false;
            btnReglerCMD.Enabled = true;
            btnCloturerCMD.Enabled = false;
            MessageBox.Show("Commande passée en état 'livrée' !");
            dgvCMD.Rows.Clear();
            cBoxRechercheCMD.SelectedIndex = -1;
        }

        private void btnSupprCMD_Click(object sender, EventArgs e)
        {
            DAODocuments.deleteCommande(txtBoxSaisieNumCMD.Text, Int32.Parse(lblCMDNbEx.Text));
            MessageBox.Show("Commande supprimée avec succès !");
            txtBoxSaisieNumCMD.Text = "";
            txtBoxSaisieNumCMD.Enabled = true;
            BtnSelectCMD.Enabled = true;
            BtnChangeCMD.Enabled = false;
            btnLivrerCMD.Enabled = false;
            btnSupprCMD.Enabled = false;
            btnRelancerCMD.Enabled = false;
            btnReglerCMD.Enabled = false;
            btnCloturerCMD.Enabled = false;
            lblCMDNum.Text = "";
            lblCMDNbEx.Text = "";
            lblCMDDate.Text = "";
            lblCMDMontant.Text = "";
            lblCMDDocument.Text = "";
            lblCMDEtat.Text = "";
            dgvCMD.Rows.Clear();
            cBoxRechercheCMD.SelectedIndex = -1;
        }

        private void btnReglerCMD_Click(object sender, EventArgs e)
        {
            DAODocuments.updateEtatCommande(txtBoxSaisieNumCMD.Text, "réglée");
            lblCMDEtat.Text = "réglée";
            btnLivrerCMD.Enabled = false;
            btnSupprCMD.Enabled = false;
            btnRelancerCMD.Enabled = false;
            btnReglerCMD.Enabled = false;
            btnCloturerCMD.Enabled = true;
            MessageBox.Show("Commande passée en état 'réglée' !");
            dgvCMD.Rows.Clear();
            cBoxRechercheCMD.SelectedIndex = -1;
        }

        private void btnCloturerCMD_Click(object sender, EventArgs e)
        {
            DAODocuments.updateEtatCommande(txtBoxSaisieNumCMD.Text, "clôturée");
            lblCMDEtat.Text = "clôturée";
            btnLivrerCMD.Enabled = false;
            btnSupprCMD.Enabled = false;
            btnRelancerCMD.Enabled = false;
            btnReglerCMD.Enabled = false;
            btnCloturerCMD.Enabled = false;
            MessageBox.Show("Commande passée en état 'clôturée' !");
            dgvCMD.Rows.Clear();
            cBoxRechercheCMD.SelectedIndex = -1;
        }

        private void tabCmd_Click(object sender, EventArgs e)
        {

        }

        private void tabTitres_Click(object sender, EventArgs e)
        {

        }

        private void btnRelancerCMD_Click(object sender, EventArgs e)
        {
            DAODocuments.updateEtatCommande(txtBoxSaisieNumCMD.Text, "relancée");
            lblCMDEtat.Text = "relancée";
            btnLivrerCMD.Enabled = true;
            btnSupprCMD.Enabled = true;
            btnRelancerCMD.Enabled = false;
            btnReglerCMD.Enabled = false;
            btnCloturerCMD.Enabled = false;
            MessageBox.Show("Commande passée en état 'relancée' !");
            dgvCMD.Rows.Clear();
            cBoxRechercheCMD.SelectedIndex = -1;
        }

        private void cBoxRechercheCMD_SelectedIndexChanged(object sender, EventArgs e)
        {
            dgvCMD.Rows.Clear();
            List<Commande> lesCMD = null;
            switch (cBoxRechercheCMD.Text)
            {
                case "en cours":
                    lesCMD = DAODocuments.getAllCommandes("en cours");
                    break;
                case "livrée":
                    lesCMD = DAODocuments.getAllCommandes("livrée");
                    break;
                case "relancée":
                    lesCMD = DAODocuments.getAllCommandes("relancée");
                    break;
                case "réglée":
                    lesCMD = DAODocuments.getAllCommandes("réglée");
                    break;
                case "clôturée":
                    lesCMD = DAODocuments.getAllCommandes("clôturée");
                    break;
                default:
                    break;
            }

            if (lesCMD != null)
            {
                foreach (Commande cmd in lesCMD)
                {
                    dgvCMD.Rows.Add(cmd.Id, cmd.NbExemplaire, cmd.DateCommande, cmd.Montant, cmd.Document.Titre, cmd.Etat);
                }
            }
        }

        private void label48_Click(object sender, EventArgs e)
        {

        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {

        }

        private void btnCreaCMDSelect_Click(object sender, EventArgs e)
        {

            int nbExemplaires = DAODocuments.getNbExemplaireByIdDocument(DAODocuments.getDocumentByTitre(cBoxDocCMDLivres.Text).IdDoc);
            if (nbExemplaires > 0)
            {
                int prix = 1;

                lblExDispo.Text = nbExemplaires.ToString();
                lblPrix.Text = prix.ToString();

                numCreaCMD.Enabled = true;
                BtnCommander.Enabled = true;
                btnCreaCMDSelect.Enabled = false;
                cBoxDocCMDLivres.Enabled = false;
            }
            else
            {
                MessageBox.Show("Aucun exemplaire disponible trouvé pour ce document !");
            }

        }


        private void btnCreaCMDDVDSelect_Click(object sender, EventArgs e)
        {

            int nbExemplaires = DAODocuments.getNbExemplaireByIdDocument(DAODocuments.getDocumentByTitre(cBoxDocCMDDVD.Text).IdDoc);
            if (nbExemplaires > 0)
            {
                int prix = 1;

                lblExDispoDVD.Text = nbExemplaires.ToString();
                lblPrixDVD.Text = prix.ToString();

                numCreaCMDDVD.Enabled = true;
                BtnCommanderDVD.Enabled = true;
                btnCreaCMDDVDSelect.Enabled = false;
                cBoxDocCMDDVD.Enabled = false;
            }
            else
            {
                MessageBox.Show("Aucun exemplaire disponible trouvé pour ce document !");
            }

        }


        private void btnCreaCMDRevueSelect_Click(object sender, EventArgs e)
        {
            Document doc = DAODocuments.getDocumentByTitre(cBoxDocCMDRevue.Text);
            if (doc != null)
            {
                string idDoc = doc.IdDoc;

                int nbExemplaires = DAODocuments.getNbExemplaireByIdDocument(idDoc);
                int prix = 1;

                lblExDispoRevue.Text = nbExemplaires.ToString();
                lblPrixRevue.Text = prix.ToString();

                numCreaCMDRevue.Enabled = true;
                BtnCommanderRevue.Enabled = true;
                btnCreaCMDRevueSelect.Enabled = false;
                cBoxDocCMDRevue.Enabled = false;
            }
            else
            {
                MessageBox.Show("Erreur dans l'application !");
            }
        }

        private void tabRevues_Enter(object sender, EventArgs e)
        {

            // Chargement des objets en mémoire
            lesCategories = DAODocuments.getAllCategories();
            lesDescripteurs = DAODocuments.getAllDescripteurs();
            lesRevues = DAOPresse.getAllRevues();


            // Effacer les éléments existants de la ComboBox
            cBoxDocCMDRevue.Items.Clear();

            // Ajouter les titres de tous les livres à la ComboBox
            foreach (Revue revue in lesRevues)
            {
                cBoxDocCMDRevue.Items.Add(revue.Titre);
            }
            BtnCommanderRevue.Enabled = false;
            numCreaCMDRevue.Enabled = false;

        }

        private void BtnCommanderDVD_Click(object sender, EventArgs e)
        {
            if (numCreaCMDDVD.Value > 0 )
            {
                string idDoc = DAODocuments.getDocumentByTitre(cBoxDocCMDDVD.Text).IdDoc;
                DAODocuments.createCommande(idDoc, Convert.ToInt32(numCreaCMDDVD.Value), Int32.Parse(lblPrixDVD.Text));
                MessageBox.Show("Commande créée avec succès !");
                numCreaCMDDVD.Enabled = false;
                numCreaCMDDVD.Value = 0;
                BtnCommanderDVD.Enabled = false;
                lblExDispoDVD.Text = "";
                lblPrixDVD.Text = "";
                btnCreaCMDDVDSelect.Enabled = true;
                cBoxDocCMDDVD.Enabled = true;
                cBoxDocCMDDVD.SelectedIndex = -1;
            }
            else
            {
                MessageBox.Show("Nombre d'exemplaire non conforme !");
            }
        }

        private void BtnCommanderRevue_Click(object sender, EventArgs e)
        {
            if (numCreaCMDRevue.Value > 0)
            {
                string idDoc = DAODocuments.getDocumentByTitre(cBoxDocCMDRevue.Text).IdDoc;
                DAODocuments.createCommande(idDoc, Convert.ToInt32(numCreaCMDRevue.Value), Int32.Parse(lblPrixRevue.Text));
                MessageBox.Show("Commande créée avec succès !");
                numCreaCMDRevue.Enabled = false;
                numCreaCMDRevue.Value = 0;
                BtnCommanderRevue.Enabled = false;
                lblExDispoRevue.Text = "";
                lblPrixRevue.Text = "";
                btnCreaCMDRevueSelect.Enabled = true;
                cBoxDocCMDRevue.Enabled = true;
                cBoxDocCMDRevue.SelectedIndex = -1;
            }
            else
            {
                MessageBox.Show("Nombre d'exemplaire non conforme !");
            }
        }

        private void label82_Click(object sender, EventArgs e)
        {

        }
    }
}
