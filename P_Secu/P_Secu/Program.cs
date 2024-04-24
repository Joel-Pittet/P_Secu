﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.IO;

namespace P_Secu
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //Chemin du fichier texte qui stocke les mots de passes
            string path = @"c:\Temp\Password.txt";

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
            optionsMainMenu.Add("Supprimer un mot de passe");
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
                    Console.WriteLine("+-------------------------------+");

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
                            DeletePassword();
                            break;
                        case 5:
                            ExitApplication();
                            break;
                        default:
                            break;

                    }


                } while (selectedOption == 0 || selectedOption > 5);
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
                using (StreamReader readPassword = new StreamReader(path))
                {
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
                        string loginDecrypted = DecryptPasswordOrLogin(fileLinesInList[passwordLineInFile - 2]);

                        //Décrypte le mot de passe et le stocke
                        string passwordDecrypted = DecryptPasswordOrLogin(fileLinesInList[passwordLineInFile - 1]);

                        //Affiche le mot de passe souhaité
                        Console.WriteLine($"URL: {fileLinesInList[passwordLineInFile - 3]}");
                        Console.WriteLine($"Login: {loginDecrypted}");
                        Console.WriteLine($"Mot de passe: {passwordDecrypted}");
                        
                    }

                    Console.WriteLine("Appuyer sur une touche pour masquer le mot de passe et revenir au menu principal");
                    Console.ReadLine();

                    //Ferme le fichier
                    readPassword.Close();
                };

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
                    string finalLoginEncrypted = newPassword.EncryptLogin();

                    //Encrypte le mot de passe
                    string finalPasswordEncrypted = newPassword.EncryptPassword();

                    //Permet d'écrire dans un fichier sans écraser les données précédentes
                    StreamWriter writeSiteDotTxt = File.AppendText(path);

                    //Ecrit le nom du mot de passe, l'URL du site, le login et le mot de passe tous deux encrypté dans le fichier MyTest.txt
                    writeSiteDotTxt.WriteLine(passwordName);
                    writeSiteDotTxt.WriteLine(url);
                    writeSiteDotTxt.WriteLine(finalLoginEncrypted);
                    writeSiteDotTxt.WriteLine(finalPasswordEncrypted);

                    //Ferme le fichier
                    writeSiteDotTxt.Close();

                    //Ajoute le mot de passe à la liste des mots de passe
                    //passwordList.Add(newPassword);

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

            }

            //Permet de supprimer un mot de passe
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

            }

            //Decrypte le mot de passe et le login
            string DecryptPasswordOrLogin(string passwordOrLogin)
            {
                //Instancie un tableau pour stocker les lettres du mot de passe
                byte[] tabSplitedPassword = Encoding.ASCII.GetBytes(passwordOrLogin);

                //Mot de passe final restauré
                string restauredPasswordOrLogin = "";

                //Parcourt toutes les lettres du mot de passe pour les déchiffrer
                foreach (byte letterInAscii in tabSplitedPassword)
                {
                    //Change le numéro ASCII de la lettre en la décalant de 2
                    int letterRestauredInAscii = letterInAscii - 2;

                    //Récupère la lettre décryptée
                    char letterRestaured = Convert.ToChar(letterRestauredInAscii);

                    //Ajoute chaque lettre pour obtenir le mot de passe codé
                    restauredPasswordOrLogin += letterRestaured;
                }


                return restauredPasswordOrLogin;
            }

            //Permet de quitter l'application
            void ExitApplication()
            {
                Console.Clear();
                Console.WriteLine("Appuyer sur une touche pour fermer le programme");
                Console.ReadKey();
                
            }
        }
    }
}
