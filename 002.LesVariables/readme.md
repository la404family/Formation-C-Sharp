# Les variables en C-Sharp

Les variables sont des éléments fondamentaux en programmation. Elles permettent de stocker des données en mémoire pour les manipuler dans un programme. En C#, les variables doivent être déclarées avant d'être utilisées. Voici comment déclarer et utiliser des variables en C#.

## Déclaration des variables en C-Sharp

En C#, les variables doivent être déclarées avant d'être utilisées. La déclaration d'une variable se fait en spécifiant son type et son nom. Voici un exemple de déclaration de variables en C# :

```csharp
int age;
string nom;
double prix;
```

Dans cet exemple, nous avons déclaré trois variables : `age` de type `int`, `nom` de type `string` et `prix` de type `double`.

## Initialisation des variables en C-Sharp

Après avoir déclaré une variable, vous pouvez lui attribuer une valeur en utilisant l'opérateur d'assignation `=`. Voici un exemple d'initialisation de variables en C# :

```csharp
int age = 30;
string nom = "Jean";
double prix = 19.99;
```

Dans cet exemple, nous avons initialisé les variables `age`, `nom` et `prix` avec les valeurs `30`, `"Jean"` et `19.99` respectivement.

## Utilisation des variables en C-Sharp

Une fois que vous avez déclaré et initialisé des variables, vous pouvez les utiliser dans votre programme. Voici un exemple d'utilisation de variables en C# :

```csharp
Console.WriteLine("Nom : " + nom);
Console.WriteLine("Age : " + age);
Console.WriteLine("Prix : " + prix);
```

Il n'est pas nécessaire de spécifier le type de la variable lors de son utilisation. Vous pouvez simplement utiliser son nom pour accéder à sa valeur. Mais il est important de noter que les variables doivent être déclarées avant d'être utilisées.

Spécifier le type de la variable lors de sa déclaration permet de définir la taille de l'espace mémoire alloué pour stocker la valeur de la variable. Cela permet également au compilateur de vérifier que les opérations effectuées sur la variable sont valides.

Exemple de programme utilisant des variables en C# :

```csharp
// Déclaration des espaces de noms utilisés
using System;
// Déclaration de l'espace de nom du projet
namespace LesVariables
{
    // Déclaration de la classe du projet
    class Program
    {
        static void Main(string[] args)
        {
            int age = 30;
            string nom = "Jean";
            double prix = 19.99;
            var estActif = true; // Le type de la variable est déduit automatiquement

            Console.WriteLine("Nom : " + nom);
            Console.WriteLine("Age : " + age);
            Console.WriteLine("Prix : " + prix);
            Console.WriteLine("Est actif : " + estActif);
        }
    }
}
```

## Les constantes en C-Sharp

En plus des variables, vous pouvez également déclarer des constantes en C#. Les constantes sont des valeurs immuables qui ne peuvent pas être modifiées une fois qu'elles ont été définies. Voici un exemple de déclaration de constante en C# :

```csharp
const double PI = 3.14159;
const string MESSAGE = "Bonjour, monde !";
```

Dans cet exemple, nous avons déclaré deux constantes : `PI` de type `double` et `MESSAGE` de type `string`. Les constantes doivent être initialisées lors de leur déclaration et ne peuvent pas être modifiées par la suite.

Exemple de programme utilisant des constantes en C# :

```csharp
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

            // Déclaration d'une constante
            const double PI = 3.14159;
            Console.WriteLine("PI = " + PI);
        }
    }
}
```

## La concaténation de chaînes en C-Sharp

En C#, vous pouvez concaténer des chaînes en utilisant l'opérateur `+`. Voici un exemple de concaténation de chaînes en C# :

```csharp
string prenom = "Jean";
string nom = "Dupont";
string nomComplet = prenom + " " + nom;
Console.WriteLine("Nom complet : " + nomComplet);
```

Stocker une valeur donnée par l'utilisateur dans une variable :

```csharp
Console.WriteLine("Entrez votre nom : ");
string nom = Console.ReadLine();
Console.WriteLine("Bonjour, " + nom + " !");
```

## Les types de données en C-Sharp

En C#, les types de données sont des catégories de valeurs que les variables peuvent stocker. Les types de données déterminent la taille de l'espace mémoire alloué pour stocker la valeur de la variable, ainsi que les opérations qui peuvent être effectuées sur cette valeur. Voici les principaux types de données en C# :

## Portée des variables en C-Sharp

La portée d'une variable détermine où elle peut être utilisée dans le code. En C#, il existe plusieurs types de portée :

- **Variable locale** : déclarée à l'intérieur d'une méthode, accessible uniquement dans cette méthode.
- **Champ de classe** : déclarée dans une classe mais en dehors des méthodes, accessible dans toute la classe.
- **Variable statique** : partagée par toutes les instances de la classe.

```csharp
using System;

class ExemplePortee
{
    int champClasse = 5; // Champ de classe
    static int champStatique = 10; // Champ statique

    void Methode()
    {
        int variableLocale = 2; // Variable locale
        Console.WriteLine(variableLocale);
        Console.WriteLine(champClasse);
        Console.WriteLine(champStatique);
    }
}
```

## Variables implicites : `var` et `dynamic`

- **`var`** : le type est déterminé à la compilation, il ne peut pas changer par la suite.
- **`dynamic`** : le type est déterminé à l'exécution, il peut changer par la suite.

```csharp
var nombre = 42; // Le compilateur comprend que c'est un int
var texte = "Bonjour"; // string
// nombre = "texte"; // Erreur de compilation

dynamic valeur = 10;
valeur = "Maintenant une chaîne"; // Pas d'erreur à la compilation
Console.WriteLine(valeur);
```

Voici ce que ça implique :

- Le type réel de la variable dynamic est résolu au moment de l’exécution, pas à la compilation.

-Tu peux lui assigner une valeur de n’importe quel type, puis lui réassigner une autre valeur d’un type complètement différent.

- Cela permet une grande flexibilité, mais au détriment de la sécurité de type à la compilation.

## Variables non initialisées et valeurs par défaut

En C#, les variables locales doivent être initialisées avant d'être utilisées. Les champs de classe reçoivent une valeur par défaut.

```csharp
class ExempleDefaut
{
    int champClasse; // Par défaut : 0
    bool estActif;   // Par défaut : false

    void Afficher()
    {
        int local;
        // Console.WriteLine(local); // Erreur : la variable locale n'est pas initialisée
        Console.WriteLine(champClasse); // Affiche 0
        Console.WriteLine(estActif);    // Affiche False
    }
}
```

## Conversion de types (casting)

Il est parfois nécessaire de convertir une variable d'un type à un autre.

```csharp
int entier = 10;
double reel = entier; // Conversion implicite (int vers double)

// Conversion explicite (cast)
double d = 9.7;
int i = (int)d; // i vaut 9, la partie décimale est perdue

// Conversion de string vers int
string texte = "123";
int nombre = int.Parse(texte); // nombre vaut 123
// int nombre2 = int.Parse("abc"); // Provoque une exception
```

## Bonnes pratiques et erreurs courantes

- Toujours initialiser les variables locales avant utilisation.
- Utiliser des noms de variables explicites et significatifs.
- Privilégier `var` pour les types évidents, mais éviter pour les types ambigus.
- Attention à la casse : `maVariable` et `mavariable` sont différents.

```csharp
// Mauvaise pratique :
int a = 1, b = 2, c = 3; // Peu lisible

// Bonne pratique :
int nombreApples = 1;
int nombreOranges = 2;
int nombreBananes = 3;
```

// Erreur courante : utiliser une variable non initialisée

```csharp
int x;
// Console.WriteLine(x); // Erreur de compilation
```

// Erreur courante : confusion entre affectation (=) et comparaison (==)

```csharp
int y = 5;
if (y == 5) // Comparaison correcte
{
    Console.WriteLine("y vaut 5");
}
// if (y = 5) // Erreur : affectation au lieu de comparaison
```

---

Ces notions avancées sur les variables vous permettront d'écrire un code plus robuste et plus lisible en C#.
