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
        /// Nombre de caractère dans la table ASCII étendue
        /// </summary>
        private const int _NB_CHAR_IN_ANSI = 256;

        /// <summary>
        /// Mise à zéro du compteur
        /// </summary>
        private const int _COUNT_LIMIT = 0;

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
        public string EncryptPassword(string masterPassword)
        {
            //Transforme le mot de passe avec la clef
            string passwordTransformed = TransformPasswordOrLoginWithKey(_password, masterPassword);

            //Instancie un tableau pour stocker chaque code ASCII de chaque lettre du mot de passe
            byte[] tabSplitedPassword = Encoding.ASCII.GetBytes(_password);

            //Encrypte le mot de passe avec Vigenere et récupère le résultat
            string finalPasswordEncryptedWithVigenere = EncryptPasswordorLoginVigenere(passwordTransformed, tabSplitedPassword);

            //Retourne le mot de passe final encrypté
            return finalPasswordEncryptedWithVigenere;

        }

        /// <summary>
        /// Encode le Login
        /// </summary>
        /// <param name="masterPassword"></param>
        /// <returns></returns>
        public string EncryptLogin(string masterPassword)
        {
            //Transforme le login avec la clef
            string loginTransformed = TransformPasswordOrLoginWithKey(_login, masterPassword);

            //Instancie un tableau pour stocker chaque code ASCII de chaque lettre du login
            byte[] tabSplitedLogin = Encoding.ASCII.GetBytes(_login);

            //Encrypte le login avec Vigenere et récupère le résultat
            string finalLoginEncryptedWithVigenere = EncryptPasswordorLoginVigenere(loginTransformed, tabSplitedLogin);

            //Retourne le login final encrypté
            return finalLoginEncryptedWithVigenere;
        }

        /// <summary>
        /// Transforme le mot de passe avec les lettres de la clef
        /// </summary>
        /// <param name="passwordOrLogin"></param>
        /// <param name="masterPassword"></param>
        /// <returns></returns>
        public string TransformPasswordOrLoginWithKey(string passwordOrLogin, string masterPassword)
        {
            //Mot de passe transformé en lettre
            string passwordOrLoginWithKey = "";

            //Compteur au cas ou la clef est plus petite que le mot de passe
            //Pour que le compte reprenne à zero
            int keyCountToZero = 0;

            //Parcourt le mot de passe et remplace une a une les lettres
            for (int i = 0; i < passwordOrLogin.Length; i++)
            {
                //Si le compteur pour la clef dépasse le dernier index du tableau
                //Remet l'index du compteur à zero, pour recommencer la transformation depuis le début du mot
                if (keyCountToZero == masterPassword.Length)
                {
                    keyCountToZero = _COUNT_LIMIT;
                }

                //Ajoute la lettre transformée par la clef dans le mot de passe transformé par la clef
                passwordOrLoginWithKey += masterPassword[keyCountToZero];

                //Incrémente le compteur pour la clef
                keyCountToZero++;
            }

            return passwordOrLoginWithKey;
        }

        /// <summary>
        /// Encrypte le mot de passe
        /// </summary>
        /// <param name="passwordOrLoginTransformed"></param>
        /// <param name="tabSplitedPasswordOrLogin"></param>
        /// <returns></returns>
        public string EncryptPasswordorLoginVigenere(string passwordOrLoginTransformed, byte[] tabSplitedPasswordOrLogin)
        {
            //Traduit le mot de passe ou le Login transformé en code ASCII
            byte[] passwordOrLoginTransformedInAscii = Encoding.ASCII.GetBytes(passwordOrLoginTransformed);

            //Mot de passe ou login final encrypté
            string finalEncryptedPasswordOrLogin = "";

            //Encrypte chaque lettre avec le code de Vigenère
            for (int i = 0; i < passwordOrLoginTransformed.Length; i++)
            {
                //Additionne le code ascii de la lettre du mot de passe ou celle du login et
                //du mot de passe ou login transformé de chaque lettre une a une
                int letterEncryptedInAscii = (passwordOrLoginTransformedInAscii[i] + tabSplitedPasswordOrLogin[i]) % _NB_CHAR_IN_ANSI;

                //Converti le code ASCII en lettre et l'ajoute au mot de passe ou login final encrypté
                finalEncryptedPasswordOrLogin += Convert.ToChar(letterEncryptedInAscii);
            }

            return finalEncryptedPasswordOrLogin;
        }


        

    }
}
