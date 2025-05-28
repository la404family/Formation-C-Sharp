// Configuration de la console pour afficher correctement les caractères spéciaux (accents, etc.)
Console.OutputEncoding = System.Text.Encoding.UTF8;

// Définir d'abord les couleurs : fond bleu, texte jaune
Console.BackgroundColor = ConsoleColor.Blue;
Console.ForegroundColor = ConsoleColor.Yellow;

// Effacer tout ce qui était affiché avant pour appliquer les couleurs sur toute la console
Console.Clear();




// On donne un titre à la fenêtre de la console
Console.Title = "Application Bonjour !";

// On affiche un petit message d'accueil pour l'utilisateur
Console.WriteLine("██ ██   ██  ██████  ██   ██ \r\n██ ██   ██ ██  ████ ██   ██ \r\n██ ███████ ██ ██ ██ ███████ \r\n        ██ ████  ██      ██ \r\n██      ██  ██████       ██ \r\n                            \r\n                            ");
Console.WriteLine("Vous allez entrer votre nom et votre âge.");
Console.WriteLine(); // Ligne vide pour l'aération

//------------------------------------------------------------
// FONCTION GÉNÉRIQUE POUR DEMANDER UNE SAISIE AVEC VALIDATION
//------------------------------------------------------------
string DemanderSaisie(string message, string messageErreur, Func<string, bool> verifier)
{
    Console.Write(message); // Affiche le message pour l'utilisateur
    string saisie = Console.ReadLine(); // Lit ce que l'utilisateur a tapé

    // Tant que la saisie est invalide, on affiche un message d'erreur et on redemande
    while (!verifier(saisie))
    {
        Console.Write(messageErreur);
        saisie = Console.ReadLine();
    }

    return saisie; // Retourne la saisie valide
}

//------------------------------------------------------------
// FONCTION DE VALIDATION DU NOM
//------------------------------------------------------------
bool NomEstValide(string nom)
{
    // Vérifie que la saisie n'est pas vide ou composée uniquement d'espaces
    if (string.IsNullOrWhiteSpace(nom)) return false;

    // On vérifie que tous les caractères sont des lettres ou des espaces
    bool tousCaracteresValides = nom.All(c => char.IsLetter(c) || char.IsWhiteSpace(c));

    // On compte le nombre de lettres (en ignorant les espaces)
    int nombreDeLettres = nom.Count(char.IsLetter);

    // Le nom est valide si tous les caractères sont corrects et qu'il y a au moins 3 lettres
    return tousCaracteresValides && nombreDeLettres >= 3;
}

//------------------------------------------------------------
// FONCTION DE VALIDATION DE L'ÂGE
//------------------------------------------------------------
bool AgeEstValide(string saisie)
{
    // On vérifie que la saisie est un entier >= 0
    return !string.IsNullOrWhiteSpace(saisie)
           && int.TryParse(saisie, out int age)
           && age >= 0;
}

//------------------------------------------------------------
// DEMANDE DU NOM DE L'UTILISATEUR
//------------------------------------------------------------
string nomUtilisateur = DemanderSaisie(
    message: "Quel est votre nom ? ",
    messageErreur: "Nom invalide. Entrez au moins 3 lettres, sans chiffres ni caractères spéciaux : ",
    verifier: NomEstValide
);

//------------------------------------------------------------
// DEMANDE DE L'ÂGE DE L'UTILISATEUR
//------------------------------------------------------------
string saisieAge = DemanderSaisie(
    message: "Quel est votre âge ? ",
    messageErreur: "Âge invalide. Entrez un nombre entier positif : ",
    verifier: AgeEstValide
);

//------------------------------------------------------------
// CONVERSION DE L'ÂGE ET AFFICHAGE DES RÉSULTATS
//------------------------------------------------------------
int ageUtilisateur = int.Parse(saisieAge); // Conversion de la chaîne en entier

// Affichage final avec le nom et l'âge
Console.WriteLine(); // Ligne vide
Console.WriteLine($"Bonjour {nomUtilisateur} !");
Console.WriteLine($"Vous avez {ageUtilisateur} ans.");
Console.WriteLine($"L'année prochaine, vous aurez {ageUtilisateur + 1} ans.");
