// Les différents types de variables

// Déclaration des espaces de noms utilisés
using System;
// Déclaration de l'espace de nom du projet
namespace LesVariables
{
    // Déclaration de la classe du projet
    class Program
    {
        static void Main()
        {
            // Déclaration d'une variable de type entier
            int entier = 10;
            Console.WriteLine("entier = " + entier);

            // Déclaration d'une variable nombre à virgule 
            float reel = 3.14f;
            Console.WriteLine("nombre à virgule = " + reel);

            // Déclaration d'une variable de type caractère
            char caractere = 'A';
            Console.WriteLine("caractere = " + caractere);

            // Déclaration d'une variable de type chaîne de caractères
            string chaine = "Hello World!";
            Console.WriteLine("chaine = " + chaine);

            // Déclaration d'une variable de type booléen (true ou false)
            bool booleen = true;
            Console.WriteLine("booleen = " + booleen);
        }
    }
}