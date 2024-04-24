///Auteur: Joël Pittet
///Date: 23.04.2024
///Lieu: Lausanne - ETML
///Description: Cette classe contient des attributs et des méthode pour gérer les site entré dans le gestionnaire
///             il y a des attributs comme l'url, le login ou le mot de passe du site
///             Il y a une methode qui permet d'encrypté le mot de passe et le login
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P_Secu
{
    internal class Password
    {
        /// <summary>
        /// Nom du site
        /// </summary>
        private string _passwordName;

        /// <summary>
        /// GETTER/SETTER
        /// Nom du site
        /// </summary>
        public string PasswordName
        {
            get
            {
                return _passwordName;
            }
            set
            {
                _passwordName = value;
            }
        }

        /// <summary>
        /// URL du site
        /// </summary>
        private string _url;

        /// <summary>
        /// Login du site
        /// </summary>
        private string _login;

        /// <summary>
        /// Mot de passe du site
        /// </summary>
        private string _password = " ";


        /// <summary>
        /// Constructeur
        /// </summary>
        /// <param name="passwordName">Nom du mot de passe pour pouvoir y accéder</param>
        /// <param name="url">URL du site</param>
        /// <param name="login">Login du site</param>
        /// <param name="password">Mot de passe du site</param>
        public Password(string passwordName, string url, string login, string password)
        {
            _passwordName = passwordName;
            _url = url;
            _login = login;
            _password = password;
        }

        /// <summary>
        /// Encode le mot de passe
        /// </summary>
        public string EncryptPassword()
        {
            //Instancie un tableau pour stocker les lettres du mot de passe
            byte[] tabSplitedPassword = Encoding.ASCII.GetBytes(_password);

            //Mot de passe final encrypté
            string encryptedPassword = "";

            //Parcourt toutes les lettres du mot de passe pour les chiffrer
            foreach (byte letterInAscii in tabSplitedPassword)
            {
                //Change le numéro ASCII de la lettre en la décalant de 2
                int letterEncryptedInAscii = letterInAscii + 2;

                //Récupère la lettre encryptée
                char letterEncrypted = Convert.ToChar(letterEncryptedInAscii);

                //Ajoute chaque lettre pour obtenir le mot de passe codé
                encryptedPassword += letterEncrypted;
            }


            return encryptedPassword;

        }

        /// <summary>
        /// Encode le login
        /// </summary>
        public string EncryptLogin()
        {
            //Instancie un tableau pour stocker les lettres du login
            byte[] tabSplitedLogin = Encoding.ASCII.GetBytes(_login);

            //Login final encrypté
            string encryptedLogin = "";

            //Parcourt toutes les lettres du mot de passe pour les chiffrer
            foreach (byte letterInAscii in tabSplitedLogin)
            {
                //Change le numéro ASCII de la lettre en la décalant de 2
                int letterEncryptedInAscii = letterInAscii + 2;

                //Récupère la lettre encryptée
                char letterEncrypted = Convert.ToChar(letterEncryptedInAscii);

                //Ajoute chaque lettre pour obtenir le mot de passe codé
                encryptedLogin += letterEncrypted;
            }


            return encryptedLogin;

        }


    }
}
