using System;

class Program
{
    static void Main()
    {
        // Déclaration des variables de base
        int entier = 42;
        double flottant = 3.14159;
        bool condition = true;
        char caractere = 'Z';

        // Affichage avec interpolation de chaîne
        Console.WriteLine($"Entier: {entier}, Flottant: {flottant:N5}");
        Console.WriteLine($"Booléen: {condition}, Caractère: {caractere}");

        // Manipulation de chaînes
        string prenom = "Jean";
        string nom = "Dupont";

        // 1. Concaténation classique
        string nomComplet = prenom + " " + nom;
        Console.WriteLine("Nom complet (concaténation) : " + nomComplet);

        // 2. Utilisation de verbatim pour les guillemets
        string citation = @"Il a dit : ""Bonjour !"""; // Syntaxe verbatim
        Console.WriteLine("Citation : " + citation);

        // 3. Interpolation de chaîne
        string messageInterpolation = $"Bonjour, {prenom} {nom} !";
        Console.WriteLine(messageInterpolation);

        // 4. Formatage de nombres
        Console.WriteLine($"Nombre formaté : {flottant:0.##}");

        // 5. Utilisation de StringBuilder pour des concaténations multiples
        var sb = new System.Text.StringBuilder();
        sb.Append("Construction ");
        sb.Append("de ");
        sb.Append("chaîne ");
        sb.Append("optimisée");
        Console.WriteLine(sb.ToString());
    }
}