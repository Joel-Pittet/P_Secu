using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
