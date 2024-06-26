﻿///Auteur: Joël Pittet
///Date: 31.05.2024
///Lieu: Lausanne - ETML
///Description: Ce programme permet de stocker des informations sur des mots de passe comme l'url, le nom du site, le login et le mot de passe. 
///             Le mot de passe et le login sont stocké dans un fichier texte.
///             Il est aussi possible de consulter ses informations et de les mettre à jour
///             Cela fonctionne comme un gestionnaire de mot de passe
///
using System;
using System.Collections.Generic;
using System.IO;

namespace P_Secu
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //Total des caractère dans la table ASCII étendue
            const int _TOTAL_CHAR_ANSI = 256;

            //Chemin du fichier texte qui stocke les mots de passes
            string path = @"..\Password.txt";

            //test si le fichier des mots de passe existe ou non
            bool doesFileExists = File.Exists(path);

            if (doesFileExists)
            {
                
            }
            else
            {
                //Crée un fichier avec le nom du site.txt dans le dossier Temp de la machine
                using (FileStream fs = File.Create(path))
                {

                }

            }

            //Demande la clef pour décrypter les mots de passe par la suite
            Console.Write("Entrez votre clef : ");

            //récupère la clef
            string masterPassword = Console.ReadLine();

            //Liste pour stocker les mots de passes à écrire dans le fichier texte
            List<Password> passwordList = new List<Password>();

            //Liste pour stocker les options du menu
            List<string> optionsMainMenu = new List<string>();

            //Ajoute les options du menu principal dans la liste
            optionsMainMenu.Add("Consulter un mot de passe");
            optionsMainMenu.Add("Ajouter un mot de passe");
            optionsMainMenu.Add("Mettre à jour un mot de passe");
            //optionsMainMenu.Add("Supprimer un mot de passe");
            optionsMainMenu.Add("Quitter le programme");

            //Crée le menu principal
            Menu mainMenu = new Menu(title: "Selectionner un option:", optionList: optionsMainMenu);

            //Stocke le choix de l'utilisateur
            int selectedOption = 0;

            //Affiche le menu et ses options
            DrawMenu();

            //Affiche le menu principal et ses options
            void DrawMenu()
            {
                do
                {
                    //Efface la console
                    Console.Clear();

                    //Intro de l'application
                    Console.WriteLine("+-------------------------------+");
                    Console.WriteLine("| Gestionnaire de mots de passe |");
                    Console.WriteLine("+-------------------------------+\n");

                    //Affiche les options du menu
                    mainMenu.ShowMenu();

                    //Demande à l'utilisateur de choisir une option du menu
                    Console.Write("Que voulez-vous faire: ");

                    //Récupère le choix de l'utilisateur
                    string userChoice = Console.ReadLine();

                    //Essaie de convertir le choix de l'utilisateur en entier
                    //S'il peut stocke la valeur dans un int sinon demande d'entrer un numéro
                    try
                    {
                        //Récupère le choix de l'utilisateur
                        selectedOption = Convert.ToInt32(userChoice);
                    }
                    catch
                    {
                        //Affiche un message d'erreur
                        Console.WriteLine("Veuilez entrer le chiffre correspondant à l'option que vous souhaitez. Pressez ENTER pour continuer");

                        selectedOption = 0;

                        Console.ReadKey();

                    }

                    //Affiche l'option demandée selon le numéro demandé
                    switch (selectedOption)
                    {
                        case 1:
                            ShowPassword();
                            break;
                        case 2:
                            AddPassword();
                            break;
                        case 3:
                            UpdatePassword();
                            break;
                        case 4:
                            ExitApplication();
                            //DeletePassword();
                            break;
                        default:
                            mainMenu.ShowMenu();
                            break;

                    }

                } while (selectedOption == 0 || selectedOption > optionsMainMenu.Count);
            }
            
            //Affiche les mots de passe de l'application
            void ShowPassword()
            {
                //Efface la console
                Console.Clear();

                //Stocke les noms des mots de passes
                List<string> optionsShowPassword = new List<string>();

                //Ajoute l'option de retour au menu principal
                optionsShowPassword.Add("Retour au menu principal");

                //Ouvre un lecteur pour lire le fichier txt
                StreamReader readPassword = new StreamReader(path);
                
                //Liste pour pouvoir gérer les lignes lues
                List<string> fileLinesInList = new List<string>();

                //crée une variable qui va stocker la ligne à lire
                string line;

                //Pour chaque ligne pleine dans le fichier
                while ((line = readPassword.ReadLine()) != null)
                {
                    fileLinesInList.Add(line);
                }

                //Parcourt les lignes enregistrées
                for (int i = 0; i < fileLinesInList.Count; i++)
                {
                    //La ligne toutes les 4 lignes est le nom d'un nouveau site
                    if (i % 4 == 0)
                    {
                        //Ajoute le nom de chque site à la liste d'option du sous-menu
                        optionsShowPassword.Add(fileLinesInList[i]);

                    }
                }
                   
                //Crée un nouveau menu avec la liste des noms de mots de passes
                Menu showPasswordMenu = new Menu(title: "Consulter un mot de passe:", optionList: optionsShowPassword);

                //Affiche le menu
                showPasswordMenu.ShowMenu();

                //Demande le choix de l'utilisateur
                Console.Write("Votre choix: ");

                //Récupère le choix de l'utilisateur
                selectedOption = Convert.ToInt32(Console.ReadLine());

                //Première ligne du site dans le fichier
                int passwordInFile = selectedOption - 1;

                //Réaffiche le menu principal sinon affiche le site demandé
                if(selectedOption == 1)
                {
                    //Efface la console
                    Console.Clear();

                    //Ferme le fichier
                    readPassword.Close();

                    //Affiche le menu principal
                    DrawMenu();
                }
                else
                {
                    //Retrouve la dernière ligne du site dans le fichier
                    //Cette ligne correspond au mot de passe du site
                    int passwordLineInFile = passwordInFile * 4;

                    //Décrypte le login et le stocke
                    string loginDecrypted = DecryptPasswordOrLogin(fileLinesInList[passwordLineInFile - 2], masterPassword);

                    //Décrypte le mot de passe et le stocke
                    string passwordDecrypted = DecryptPasswordOrLogin(fileLinesInList[passwordLineInFile - 1], masterPassword);

                    //Affiche le mot de passe souhaité
                    Console.WriteLine($"\nURL: {fileLinesInList[passwordLineInFile - 3]}");
                    Console.WriteLine($"Login: {loginDecrypted}");
                    Console.WriteLine($"Mot de passe: {passwordDecrypted}");
                        
                }

                Console.WriteLine("\nAppuyer sur une touche pour masquer le mot de passe et revenir au menu principal");
                Console.ReadLine();

                //Ferme le fichier
                readPassword.Close();

                //Affiche le menu principal
                DrawMenu();
            }

            //Permet d'ajouter un mot de passe dans l'application
            void AddPassword()
            {
                //Efface la console
                Console.Clear();

                //Stocke les noms des mots de passes
                List<string> optionsAddPassword = new List<string>();

                //Ajoute l'option de retour au menu principal
                optionsAddPassword.Add("Retour au menu principal");

                //Ajoute l'option pour enregistrer un mot de passe
                optionsAddPassword.Add("Ajouter un mot de passe");

                //Crée un nouveau menu avec la liste des noms de mots de passes
                Menu addPasswordMenu = new Menu(title: "Ajouter un mot de passe:", optionList: optionsAddPassword);

                //Affiche le menu
                addPasswordMenu.ShowMenu();

                //Demande le choix de l'utilisateur
                Console.Write("Votre choix: ");

                //Récupère le choix de l'utilisateur
                selectedOption = Convert.ToInt32(Console.ReadLine());

                //Affiche le menu principal ou lance la saisie du formulaire
                if (selectedOption == 1)
                {
                    Console.Clear();
                    DrawMenu();
                }
                else if (selectedOption == 2)
                {
                    Console.Clear();

                    //Récupère les informations nécéssaire à la création du nouveau mot de passe
                    Console.WriteLine("Remplissez le formulaire suivant afin d'enregistrer votre mot de passe: \n");

                    Console.Write("Nom du site: ");
                    string passwordName = Console.ReadLine();

                    Console.Write("URL: ");
                    string url = Console.ReadLine();

                    Console.Write("Nom d'utilisateur: ");
                    string login = Console.ReadLine();

                    Console.Write("Mot de passe: ");
                    string password = Console.ReadLine();

                    //Crée un nouveau mot de passe avec les informations de l'utilisateur
                    Password newPassword = new Password(passwordName: passwordName, url: url, login: login, password: password);

                    //Encrypte le login
                    string finalLoginEncrypted = newPassword.EncryptLogin(masterPassword);

                    //Encrypte le mot de passe
                    string finalPasswordEncrypted = newPassword.EncryptPassword(masterPassword);

                    //Permet d'écrire dans un fichier sans écraser les données précédentes
                    StreamWriter writeSiteDotTxt = File.AppendText(path);

                    //Ecrit le nom du mot de passe, l'URL du site, le login et le mot de passe tous deux encrypté dans le fichier MyTest.txt
                    writeSiteDotTxt.WriteLine(passwordName);
                    writeSiteDotTxt.WriteLine(url);
                    writeSiteDotTxt.WriteLine(finalLoginEncrypted);
                    writeSiteDotTxt.WriteLine(finalPasswordEncrypted);

                    //Ferme le fichier
                    writeSiteDotTxt.Close();

                    //Message de confirmation pour l'utilisateur
                    Console.WriteLine("\nVous avez entré un nouveau mot de passe avec succès.");
                    Console.WriteLine($"Le fichier avec vos information se trouve ici: {path} Pressez une touche pour continuer.");
                    Console.ReadKey();
                }

                DrawMenu();
            }

            //Permet de mettre à jour le mot de passe ou l'identifiant d'un mot de passe déja existant
            void UpdatePassword()
            {
                //Efface la console
                Console.Clear();

                //Stocke les noms des mots de passes
                List<string> optionsUpdatePassword = new List<string>();

                //Ajoute l'option de retour au menu principal
                optionsUpdatePassword.Add("Retour au menu principal");

                //Ouvre un lecteur pour lire le fichier txt
                StreamReader readPassword = new StreamReader(path);

                //Liste pour pouvoir gérer les lignes lues
                List<string> fileLinesInList = new List<string>();

                //crée une variable qui va stocker la ligne à lire
                string line;

                //Pour chaque ligne pleine dans le fichier
                while ((line = readPassword.ReadLine()) != null)
                {
                    fileLinesInList.Add(line);
                }

                //Ferme le fichier
                readPassword.Close();

                //Parcourt les lignes enregistrées
                for (int i = 0; i < fileLinesInList.Count; i++)
                {
                    //La ligne toutes les 4 lignes est le nom d'un nouveau site
                    if (i % 4 == 0)
                    {
                        //Ajoute le nom de chque site à la liste d'option du sous-menu
                        optionsUpdatePassword.Add(fileLinesInList[i]);

                    }
                }

                //Crée un nouveau menu avec la liste des noms de mots de passes
                Menu updatePasswordMenu = new Menu(title: "Consulter un mot de passe:", optionList: optionsUpdatePassword);

                //Affiche le menu
                updatePasswordMenu.ShowMenu();

                //Demande le choix de l'utilisateur
                Console.Write("Votre choix: ");

                //Récupère le choix de l'utilisateur
                selectedOption = Convert.ToInt32(Console.ReadLine());

                //eface la console
                Console.Clear();

                //Première ligne du site dans le fichier
                int passwordInFile = selectedOption - 1;

                //Réaffiche le menu principal sinon affiche le site demandé
                if (selectedOption == 1)
                {
                    //Efface la console
                    Console.Clear();

                    //Affiche le menu principal
                    DrawMenu();
                }
                else
                {
                    //Retrouve la dernière ligne du site dans le fichier
                    //Cette ligne correspond au mot de passe du site
                    int passwordLineInFile = passwordInFile * 4;

                    //Demande quelle aspect du mot de passe l'utilisateur souhaite changer
                    Console.WriteLine("Que souhaitez vous changer ?\n");

                    //Affiche les 3 possibilités
                    Console.WriteLine("*****************************");
                    Console.WriteLine("1) URL");
                    Console.WriteLine("2) Login");
                    Console.WriteLine("3) Mot de passe");
                    Console.WriteLine("*****************************");

                    //Demande le choix de l'utilisateur
                    Console.Write("\nVotre choix: ");

                    //récupère le choix de l'utilisateur
                    int aspectToUpdate = Convert.ToInt32(Console.ReadLine());

                    //Efface la console
                    Console.Clear();

                    switch (aspectToUpdate)
                    {
                        case 1:
                            //Affiche l'ur actuel
                            Console.WriteLine($"URL actuel: {fileLinesInList[passwordLineInFile - 3]}");

                            //demande et récupère le nouvel URL
                            Console.Write("\nNouvel URL: ");
                            string newURL = Console.ReadLine();

                            //change dans la liste l'ancien Url par le nouveau
                            fileLinesInList[passwordLineInFile - 3] = newURL;

                            //Pour écrire dans le fichier
                            StreamWriter updateLineUrl = new StreamWriter(path);

                            //Ecrit toutes les lignes dans le fichier dont la ligne mise à jour
                            foreach (string fileLine in fileLinesInList)
                            {
                                updateLineUrl.WriteLine(fileLine);
                            }

                            //Ferme le fichier
                            updateLineUrl.Close();
                            break;
                        case 2:

                            //Affiche le login actuel
                            Console.WriteLine($"Login actuel: {DecryptPasswordOrLogin(fileLinesInList[passwordLineInFile - 2], masterPassword)}");

                            //demande et récupère le nouveau login
                            Console.Write("\nNouveau login: ");
                            string newLogin = Console.ReadLine();

                            //Encrypte le login
                            string loginEncrypted = encryptWithVigenere(newLogin);

                            //change dans la liste l'ancien login par le nouveau
                            fileLinesInList[passwordLineInFile - 2] = loginEncrypted;

                            //Pour écrire dans le fichier
                            StreamWriter updateLineLogin = new StreamWriter(path);

                            //Ecrit toutes les lignes dans le fichier dont la ligne mise à jour
                            foreach (string fileLine in fileLinesInList)
                            {
                                updateLineLogin.WriteLine(fileLine);
                            }

                            //Ferme le fichier
                            updateLineLogin.Close();

                            break;
                        case 3:
                            //Affiche le mot de passe actuel
                            Console.WriteLine($"Mot de passe actuel: {DecryptPasswordOrLogin(fileLinesInList[passwordLineInFile - 1], masterPassword)}");

                            //demande et récupère le nouveau mot de passe
                            Console.Write("\nNouveau mot de passe: ");
                            string newPassword = Console.ReadLine();

                            //Encrypte le mot de passe
                            string passwordEncrypted = encryptWithVigenere(newPassword);

                            //change dans la liste l'ancien mot de passe par le nouveau
                            fileLinesInList[passwordLineInFile - 2] = passwordEncrypted;

                            //Pour écrire dans le fichier
                            StreamWriter updateLinePassword = new StreamWriter(path);

                            //Ecrit toutes les lignes dans le fichier dont la ligne mise à jour
                            foreach (string fileLine in fileLinesInList)
                            {
                                updateLinePassword.WriteLine(fileLine);
                            }

                            //Ferme le fichier
                            updateLinePassword.Close();
                            break;
                        default:
                            break;
                    }

                    /*
                    //Décrypte le login et le stocke
                    string loginDecrypted = DecryptPasswordOrLogin(fileLinesInList[passwordLineInFile - 2], masterPassword);

                    //Décrypte le mot de passe et le stocke
                    string passwordDecrypted = DecryptPasswordOrLogin(fileLinesInList[passwordLineInFile - 1], masterPassword);

                    //Affiche le mot de passe souhaité
                    Console.WriteLine($"URL: {fileLinesInList[passwordLineInFile - 3]}");
                    Console.WriteLine($"Login: {loginDecrypted}");
                    Console.WriteLine($"Mot de passe: {passwordDecrypted}");*/

                }
                Console.WriteLine("\nAppuyer sur une touche pour masquer le mot de passe et revenir au menu principal");
                Console.ReadLine();

                //Affiche le menu principal
                DrawMenu();
            }

            //Encrypte le nouveau mot de passe ou login que l'utilisateur souhaite changer
            string encryptWithVigenere(string wordToEncrypt)
            {
                #region Transforme le mot

                //Mot transformé en lettre
                string wordTransformed = "";

                //Compteur au cas ou la clef est plus petite que le mot de passe
                //Pour que le compte reprenne à zero
                int keyCountToZero = 0;

                //Parcourt le mot de passe et remplace une a une les lettres
                for (int i = 0; i < wordToEncrypt.Length; i++)
                {
                    //Si le compteur pour la clef dépasse le dernier index du tableau
                    //Remet l'index du compteur à zero, pour recommencer la transformation depuis le début du mot
                    if (keyCountToZero == masterPassword.Length)
                    {
                        keyCountToZero = 0;
                    }

                    //Ajoute la lettre transformée par la clef dans le mot transformé par la clef
                    wordTransformed += masterPassword[keyCountToZero];

                    //Incrémente le compteur pour la clef
                    keyCountToZero++;
                }

                #endregion

                //Tableau pour les code ascii de chaque lettre du mot à encrypté
                byte[] wordToEncryptInAscii = new byte[wordToEncrypt.Length];

                //Récupère dans un tableau le code ascii de chaque lettre du mot
                for (int i = 0; i < wordToEncrypt.Length; i++)
                {
                    wordToEncryptInAscii[i] = Convert.ToByte(wordToEncrypt[i]);
                }

                //Tableau pour les code ascii de chaque lettre du mot transformé par la clef
                byte[] wordTransformedInAscii = new byte[wordTransformed.Length];

                //Récupère le code ascii de chaque lettre du mot transformé par la clef
                for (int i = 0; i < wordTransformed.Length; i++)
                {
                    wordTransformedInAscii[i] = Convert.ToByte(wordTransformed[i]);
                }

                #region Encrypte le mot
                //Mot de passe ou login final encrypté
                string finalWordEncrypted = "";

                //Encrypte chaque lettre avec le code de Vigenère
                for (int i = 0; i < wordTransformed.Length; i++)
                {
                    //Additionne le code ascii de la lettre du mot de passe ou celle du login et
                    //du mot de passe ou login transformé de chaque lettre une a une
                    int letterEncryptedInAscii = (wordTransformedInAscii[i] + wordToEncryptInAscii[i] + _TOTAL_CHAR_ANSI) % _TOTAL_CHAR_ANSI;

                    //Converti le code ASCII en lettre et l'ajoute au mot de passe ou login final encrypté
                    finalWordEncrypted += Convert.ToChar(letterEncryptedInAscii);
                }

                #endregion

                return finalWordEncrypted;
            }

            /*//Permet de supprimer un mot de passe
            void DeletePassword()
            {
                
                //Efface la console
                Console.Clear();

                //Stocke les noms des mots de passes
                List<string> optionsDeletePassword = new List<string>();

                //Ajoute l'option de retour au menu principal
                optionsDeletePassword.Add("Retour au menu principal");

                //Ajoute les noms de mots de passe enregistrés dans la liste
                for (int i = 0; i < passwordList.Count; i++)
                {
                    optionsDeletePassword.Add(passwordList[i].PasswordName);
                }

                //Crée un nouveau menu avec la liste des noms de mots de passes
                Menu deletePasswordMenu = new Menu(title: "Supprimer un mot de passe:", optionList: optionsDeletePassword);

                //Affiche le menu
                deletePasswordMenu.ShowMenu();

                //Demande le choix de l'utilisateur
                Console.Write("Votre choix: ");

                //Récupère le choix de l'utilisateur
                selectedOption = Convert.ToInt32(Console.ReadLine());

                if(selectedOption == 1)
                {
                    DrawMenu();

                }
                else
                {
                    //Supprime le mot de passe de la liste et recupère le bool pour afficher un résultat à l'utilisateur 
                    bool isDelete = passwordList.Remove(passwordList[selectedOption - 2]);

                    //Affiche un message à l'utilisateur pour savoir si oui ou non le mot de passe à bien été supprimer de la liste
                    //ReadKey pour laisser l'utilisateur lire le retour de la suppression
                    if (isDelete)
                    {
                        Console.WriteLine("\nLe mot de passe à bien été supprimé.");
                        Console.ReadKey();
                    }
                    else
                    {
                        Console.WriteLine("\nLe mot de passe n'as pas pu être supprimé.");
                        Console.ReadKey();
                    }
                }

                DrawMenu();

            }*/

            #region Déchiffrement

            /// <summary>
            /// Décode le mot de passe ou le login encodé
            /// </summary>
            /// <param name="passwordOrLoginEncrypted"></param>
            /// <param name="masterkeyPassword"></param>
            /// <returns></returns>
            string DecryptPasswordOrLogin(string passwordOrLoginEncrypted, string masterkeyPassword)
            {
                //Crée une chaine de caractère de la taille du mot de passe encrypté avec la clef 
                string keySizeOfPasswordOrLogin = TransformDecryptedPassword(passwordOrLoginEncrypted, masterkeyPassword);

                //Décrypte le mot de passe ou login
                string finalPasswordOrLoginDecrypted = DecryptEncryptedPasswordVigenere(passwordOrLoginEncrypted, keySizeOfPasswordOrLogin);

                //Retourne le login ou mot de passe décrypté
                return finalPasswordOrLoginDecrypted;
            }

            /// <summary>
            /// Crée un chaine de caractère de la taille du mot de passe encrypté
            /// </summary>
            /// <param name="passwordOrLoginEncrypted"></param>
            /// <param name="masterkeyPassword"></param>
            /// <returns></returns>
            string TransformDecryptedPassword(string passwordOrLoginEncrypted, string masterkeyPassword)
            {
                //Chaine de caractère pour stocker le "mot de passe transformé"
                string keyOnPassword = "";

                //Compteur au cas ou la clef est plus petite que le mot de passe
                //Pour que le compte reprenne à zero
                int keyCountToZero = 0;

                //Crée une chaine de caractère de la même longueur que le mot de passe avec la clef
                for (int i = 0; i < passwordOrLoginEncrypted.Length; i++)
                {
                    //Si le compteur pour la clef dépasse le dernier index du tableau
                    //Remet l'index du compteur à zero, pour recommencer la transformation depuis le début du mot
                    if (keyCountToZero == masterkeyPassword.Length)
                    {
                        keyCountToZero = 0;
                    }

                    //Ajoute la lettre de la clef à la chaine de caractère
                    keyOnPassword += masterkeyPassword[keyCountToZero];

                    //Incrémente le compteur pour la clef
                    keyCountToZero++;
                }

                return keyOnPassword;
            }

            /// <summary>
            /// Décrypte le login ou le mot de passe encrypté
            /// </summary>
            /// <param name="passwordOrLoginEncrypted"></param>
            /// <param name="keySizeOfPasswordOrLogin"></param>
            /// <returns></returns>
            string DecryptEncryptedPasswordVigenere(string passwordOrLoginEncrypted, string keySizeOfPasswordOrLogin)
            {
                //Tableau pour stocker le code ASCII de chaque lettre du mot de passe encrypté
                int[] asciiCodeEveryChar = new int[passwordOrLoginEncrypted.Length];

                //Stocke chaque code Ascii du mot de passe encrypté
                for (int i = 0; i < passwordOrLoginEncrypted.Length; i++)
                {
                    asciiCodeEveryChar[i] = Convert.ToByte(passwordOrLoginEncrypted[i]);
                }

                //Stocke chaque code Ascii de chaque lettre de la chaine de caractère faite avec la clef
                byte[] keySizeOfPasswordOrLoginInBytes = new byte[keySizeOfPasswordOrLogin.Length];

                //Récupère le code ascii de chaque lettre du mot de passe transformé
                for (int i = 0; i < keySizeOfPasswordOrLogin.Length; i++)
                {
                    keySizeOfPasswordOrLoginInBytes[i] = Convert.ToByte(keySizeOfPasswordOrLogin[i]);
                }

                //Mot de passe ou login final décrypté
                string passwordOrLoginDecrypted = "";

                //Parcourt chaque code ascii du tableau du mot de passe ou login encrypté et soustrait la clef au mot de passe ou login encrypté
                for (int i = 0; i < passwordOrLoginEncrypted.Length; i++)
                {
                    //Récupère le code ascii de la lettre déchiffrée
                    int letterDecryptedInAscii = (asciiCodeEveryChar[i] - keySizeOfPasswordOrLoginInBytes[i] + _TOTAL_CHAR_ANSI) % _TOTAL_CHAR_ANSI;

                    //Converti le code ASCII en lettre et l'ajoute au mot de passe ou login final décrypté
                    passwordOrLoginDecrypted += Convert.ToChar(letterDecryptedInAscii);
                }

                return passwordOrLoginDecrypted;

            }

            //Code césar Déchiffrement
            #region Codage CÉSAR
            /*
            //Parcourt toutes les lettres du mot de passe pour les déchiffrer
            foreach (byte letterInAscii in tabSplitedPassword)
            {
                //Change le numéro ASCII de la lettre en la décalant de 2
                int letterRestauredInAscii = letterInAscii - 2;

                //Récupère la lettre décryptée
                char letterRestaured = Convert.ToChar(letterRestauredInAscii);

                //Ajoute chaque lettre pour obtenir le mot de passe codé
                restauredPasswordOrLogin += letterRestaured;
            }*/

            #endregion

            #endregion

            //Permet de quitter l'application
            void ExitApplication()
            {
                Console.Clear();
                Console.WriteLine("Appuyer sur une touche pour fermer le programme");
                Console.ReadLine();
            }
        }
    }
}
