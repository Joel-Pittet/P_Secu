///Auteur: Joël Pittet
///Date: 31.05.2024
///Lieu: Lausanne - ETML
///Description: Cette classe contient une méthode pour afficher le menu ainsi qu'une liste qui stocke les options du menu
///             Le menu est également utilisé pour les sous menus du programme
///
using System;
using System.Collections.Generic;

namespace P_Secu
{
    internal class Menu
    {
        /// <summary>
        /// Liste des options du menu
        /// </summary>
        private List<string> _optionList;

        /// <summary>
        /// Première ligne du menu
        /// </summary>
        private string _title;

        /// <summary>
        /// Constructeur
        /// </summary>
        /// <param name="optionList">Liste des options du menu</param>
        public Menu(string title, List<string> optionList)
        {
            _optionList = optionList;
            _title = title;
        }


        /// <summary>
        /// Affiche toutes les options du menu
        /// </summary>
        public void ShowMenu()
        {

            //Première ligne du menu
            Console.WriteLine(_title);

            //Mise en forme du menu 
            Console.WriteLine("\n********************************");

            //Affichage des options du menu
            for (int i = 0; i < _optionList.Count; i++)
            {
                //Affiche l'option et son numéro devant
                Console.WriteLine($"{i + 1}. {_optionList[i]}");

            }

            //Mise en forme du menu 
            Console.WriteLine("********************************");
        }

    }
}
