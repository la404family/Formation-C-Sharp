// ==================== JEU DU PENDU - PROGRAMME PRINCIPAL ====================
// Ce programme implémente le jeu classique du pendu en C#
// Le joueur doit deviner un mot en proposant des lettres une par une
// Il a 6 essais avant d'être "pendu" (perdre la partie)

// ==================== IMPORTATION DES BIBLIOTHÈQUES ====================
// En C#, nous devons importer ("using") les bibliothèques dont nous avons besoin
// C'est comme dire à C# : "J'ai besoin d'utiliser ces outils dans mon programme"

using System;        // Bibliothèque de base : Console (pour afficher du texte), Random (nombres aléatoires), etc.
using System.Linq;   // Bibliothèque LINQ : permet d'utiliser des méthodes comme Contains(), Distinct(), etc.
using System.IO;     // Bibliothèque pour les fichiers : lire et écrire des fichiers sur le disque dur
using System.Text.Json; // Bibliothèque JSON : pour sauvegarder nos statistiques dans un fichier texte structuré

// ==================== PROGRAMME PRINCIPAL ====================
// Ici commence le code qui s'exécute quand on lance le programme
// C'est le "point d'entrée" - là où tout commence !

// ÉTAPE 1 : Configuration de l'apparence de la console (la fenêtre noire)
Console.Title = "Le Pendu";                     // Change le titre de la fenêtre
Console.BackgroundColor = ConsoleColor.Green;   // Met un fond vert (plus joli que noir !)
Console.ForegroundColor = ConsoleColor.White;   // Met le texte en blanc (contraste avec le vert)
Console.Clear();                                // Efface tout ce qui était affiché avant

// ÉTAPE 2 : Charger les statistiques des parties précédentes
// Si le joueur a déjà joué avant, on récupère ses anciens scores depuis un fichier
// Si c'est la première fois, on crée des statistiques vides (tout à zéro)
StatistiquesJeu statistiques = StatistiquesJeu.ChargerStatistiques();

// ÉTAPE 3 : Afficher un message de bienvenue sympa
// Le @ devant la chaîne crée un "verbatim string literal" qui préserve les sauts de ligne
// et permet d'écrire du texte multi-lignes facilement (utile pour l'ASCII art)
Console.WriteLine(@"
╔═══════════════════════════════════════════════════════════════╗
║                                                               ║
║   ░█░░░█▀▀░░░▀▀█░█▀▀░█░█░░░█▀▄░█░█░░░█▀█░█▀▀░█▀█░█▀▄░█░█      ║
║   ░█░░░█▀▀░░░░░█░█▀▀░█░█░░░█░█░█░█░░░█▀▀░█▀▀░█░█░█░█░█░█      ║
║   ░▀▀▀░▀▀▀░░░▀▀░░▀▀▀░▀▀▀░░░▀▀░░▀▀▀░░░▀░░░▀▀▀░▀░▀░▀▀░░▀▀▀      ║
║                                                               ║
╚═══════════════════════════════════════════════════════════════╝
");

// Si le joueur a déjà joué avant (PartiesJouees > 0), on lui montre ses anciens résultats
if (statistiques.PartiesJouees > 0)
{
    // Le $ devant la chaîne permet d'insérer des variables avec {nomVariable}
    Console.WriteLine($"Vous avez déjà joué {statistiques.PartiesJouees} partie(s) avec un taux de réussite de {statistiques.PourcentageReussite:F1}%");
}

// Demander au joueur d'appuyer sur une touche pour continuer
Console.WriteLine("Appuyez sur une touche pour commencer...");
Console.ReadKey(); // Attend qu'une touche soit pressée avant de continuer

// ÉTAPE 4 : La boucle infinie du jeu - c'est le cœur du programme !
// "while (true)" = "tant que c'est vrai" = "pour toujours" (jusqu'à ce qu'on dise "break")
// Cela permet de rejouer autant de fois qu'on veut
while (true)
{
    // Lancer UNE partie du jeu et récupérer le résultat (gagné ou perdu + détails)
    // UtilitairesPendu.JouerPendu() est notre fonction qui gère tout le jeu
    ResultatPartie resultat = UtilitairesPendu.JouerPendu();

    // Maintenant qu'on a fini une partie, on regarde si le joueur a gagné ou perdu
    // et on met à jour nos statistiques en conséquence

    if (resultat.Gagne)  // Si le joueur a gagné...
    {
        // On enregistre cette victoire dans nos statistiques
        statistiques.EnregistrerVictoire(resultat.NombreLettresUtilisees);

        // On change la couleur du texte en vert pour fêter la victoire
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("\n*** VICTOIRE ! Vous avez gagné cette partie ! ***");
    }
    else  // Sinon (le joueur a perdu)...
    {
        // On enregistre cette défaite dans nos statistiques
        statistiques.EnregistrerDefaite(resultat.NombreLettresUtilisees);

        // On change la couleur du texte en jaune pour montrer la défaite
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine("\n:( Dommage ! Vous avez perdu cette partie.");
    }

    // Remettre la couleur du texte en blanc (couleur par défaut de notre jeu)
    Console.ForegroundColor = ConsoleColor.White;

    // Afficher un tableau avec toutes les statistiques du joueur
    statistiques.AfficherStatistiques();

    // Sauvegarder les statistiques dans un fichier sur le disque dur
    // Comme ça, même si on ferme le jeu, on garde les scores !
    statistiques.SauvegarderStatistiques();

    // Demander au joueur s'il veut refaire une partie
    Console.WriteLine("\nVoulez-vous rejouer ? (O/N)");

    // Lire la réponse du joueur au clavier
    string? reponseInput = Console.ReadLine();  // Peut être null si problème

    // Sécuriser la réponse : enlever les espaces, mettre en majuscules, ou "N" par défaut
    string reponse = reponseInput?.Trim().ToUpperInvariant() ?? "N";

    // Si la réponse n'est pas "O" (pour "Oui"), on arrête le jeu
    if (reponse != "O")
        break;  // "break" = sortir de la boucle while = arrêter le jeu

    // Si on arrive ici, c'est que le joueur a tapé "O", donc on recommence une partie !
}

// ÉTAPE 5 : Fin du programme - on arrive ici quand le joueur ne veut plus jouer
// On affiche un message et on attend qu'il appuie sur une touche
// Sinon la fenêtre se fermerait immédiatement et on ne verrait rien !
Console.WriteLine("\nAppuyez sur une touche pour quitter...");
Console.ReadKey();  // Attend qu'une touche soit pressée, puis le programme se termine



// ==================== CLASSES POUR LES STATISTIQUES ====================
// Une CLASSE en C# = un "modèle" ou "plan" pour créer des objets
// C'est comme un moule à gâteau : on peut faire plusieurs gâteaux avec le même moule

/// <summary>
/// CLASSE 1 : ResultatPartie
/// Cette classe sert à "emballer" toutes les informations sur UNE partie qui vient de finir
/// C'est comme une petite boîte qui contient : "est-ce que j'ai gagné ?", "combien de lettres j'ai utilisées ?", etc.
/// </summary>
public class ResultatPartie
{
    // Ces 3 lignes sont des "PROPRIÉTÉS" = des variables que la classe peut contenir
    // "{ get; set; }" = on peut lire ET modifier ces valeurs (comme une boîte qu'on peut ouvrir et fermer)

    public bool Gagne { get; set; }                     // true = victoire, false = défaite
    public int NombreLettresUtilisees { get; set; }      // Combien de lettres le joueur a essayées dans cette partie
    public string MotADeviner { get; set; } = "";       // Le mot qu'il fallait deviner (ex: "CHAT")
}

/// <summary>
/// CLASSE 2 : StatistiquesJeu  
/// Cette classe est comme un "carnet de scores" qui garde en mémoire TOUTES les parties du joueur
/// Elle sait calculer des pourcentages, des moyennes, etc. Très pratique !
/// </summary>
public class StatistiquesJeu
{
    // Ces variables gardent le "total" de tout ce que le joueur a fait depuis le début
    // Le "= 0" à la fin signifie : "au début, tout est à zéro"

    public int PartiesJouees { get; set; } = 0;         // Combien de parties au total (gagnées + perdues)
    public int PartiesGagnees { get; set; } = 0;        // Combien de victoires
    public int PartiesPerdues { get; set; } = 0;        // Combien de défaites  
    public int TotalLettresTentees { get; set; } = 0;   // Combien de lettres essayées dans TOUTES les parties
    public int TotalMotsTrouves { get; set; } = 0;      // Combien de mots devinés avec succès

    // Ces 2 lignes sont des "PROPRIÉTÉS CALCULÉES" - elles se calculent automatiquement !
    // "=>" signifie "est égal au résultat de ce calcul"

    // Calcule le pourcentage de réussite (exemple: 75.5% si on a gagné 3 parties sur 4)
    public double PourcentageReussite => PartiesJouees > 0 ? (double)PartiesGagnees / PartiesJouees * 100 : 0;

    // Calcule combien de lettres on utilise en moyenne par partie (exemple: 8.2 lettres par partie)
    public double MoyenneLettresParPartie => PartiesJouees > 0 ? (double)TotalLettresTentees / PartiesJouees : 0;

    // ========== MÉTHODES (= fonctions) DE CETTE CLASSE ==========
    // Une méthode = une "action" que peut faire un objet de cette classe
    // C'est comme des "boutons" qu'on peut appuyer sur notre calculatrice de statistiques

    /// <summary>
    /// MÉTHODE 1 : EnregistrerVictoire - à appeler quand le joueur gagne une partie
    /// "void" = cette méthode ne renvoie rien, elle fait juste son travail
    /// "int lettresUtilisees" = on doit lui dire combien de lettres le joueur a utilisées
    /// </summary>
    public void EnregistrerVictoire(int lettresUtilisees)
    {
        // On met à jour TOUS les compteurs concernés par une victoire
        PartiesJouees++;                        // "++" = ajouter 1 (on a joué une partie de plus)
        PartiesGagnees++;                       // Une victoire de plus !
        TotalMotsTrouves++;                     // Le joueur a trouvé le mot, donc +1
        TotalLettresTentees += lettresUtilisees; // "+=" = ajouter à ce qui existe déjà
    }

    /// <summary>
    /// MÉTHODE 2 : EnregistrerDefaite - à appeler quand le joueur perd une partie
    /// Pareil que EnregistrerVictoire, mais pour les défaites
    /// </summary>
    public void EnregistrerDefaite(int lettresUtilisees)
    {
        // On met à jour les compteurs concernés par une défaite
        PartiesJouees++;                        // On a quand même joué une partie
        PartiesPerdues++;                       // Une défaite de plus :(
        TotalLettresTentees += lettresUtilisees; // Les lettres utilisées comptent quand même
        // Note: on n'ajoute PAS 1 à TotalMotsTrouves car le joueur n'a pas trouvé le mot
    }

    /// <summary>
    /// MÉTHODE 3 : AfficherStatistiques - crée un joli tableau coloré avec toutes les stats
    /// Cette méthode ne reçoit aucun paramètre, elle utilise les données déjà dans la classe
    /// </summary>
    public void AfficherStatistiques()
    {
        // Changer la couleur du texte en jaune pour le titre
        Console.ForegroundColor = ConsoleColor.Yellow;

        // Créer une ligne de 50 signes "=" pour faire joli (décoration)
        Console.WriteLine("\n" + new string('=', 50));  // "\n" = nouvelle ligne
        Console.WriteLine("           [STATS] STATISTIQUES DE JEU [STATS]");
        Console.WriteLine(new string('=', 50));

        // Changer la couleur en blanc pour les données
        Console.ForegroundColor = ConsoleColor.White;

        // Afficher chaque statistique avec son symbole. Le $ permet d'insérer des variables
        Console.WriteLine($"[*] Parties jouées      : {PartiesJouees}");
        Console.WriteLine($"[+] Parties gagnées     : {PartiesGagnees}");
        Console.WriteLine($"[-] Parties perdues     : {PartiesPerdues}");
        Console.WriteLine($"[O] Mots trouvés        : {TotalMotsTrouves}");
        Console.WriteLine($"[#] Lettres tentées     : {TotalLettresTentees}");
        Console.WriteLine($"[%] Taux de réussite    : {PourcentageReussite:F1}%");    // :F1 = 1 chiffre après la virgule
        Console.WriteLine($"[~] Moy. lettres/partie : {MoyenneLettresParPartie:F1}");

        // Finir avec une ligne jaune et remettre la couleur par défaut
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine(new string('=', 50));
        Console.ForegroundColor = ConsoleColor.White; // Retour à la couleur de notre jeu
    }

    /// <summary>
    /// MÉTHODE 4 : SauvegarderStatistiques - écrit toutes nos stats dans un fichier sur le disque dur
    /// Comme ça, même si on ferme le jeu et qu'on redémarre l'ordinateur, on garde nos scores !
    /// "string cheminFichier = ..." = paramètre OPTIONNEL (si on ne précise rien, il utilise ce nom par défaut)
    /// </summary>
    public void SauvegarderStatistiques(string cheminFichier = "statistiques_pendu.json")
    {
        // "try" = "Essaie de faire ça, mais si ça plante, ne casse pas tout le programme"
        try
        {
            // Convertir nos statistiques en format JSON (un format de fichier très courant)
            // JSON ressemble à ça : {"PartiesJouees": 5, "PartiesGagnees": 3, ...}
            // "WriteIndented = true" = mettre en forme pour que ce soit lisible par un humain
            string json = JsonSerializer.Serialize(this, new JsonSerializerOptions { WriteIndented = true });

            // Écrire ce texte JSON dans un fichier sur le disque dur
            File.WriteAllText(cheminFichier, json);
        }
        catch (Exception ex)  // "catch" = "Si il y a eu un problème, faire ça"
        {
            // Afficher un message d'erreur gentil (au lieu de planter)
            Console.WriteLine($"Erreur lors de la sauvegarde des statistiques : {ex.Message}");
        }
    }

    /// <summary>
    /// MÉTHODE 5 (STATIQUE) : ChargerStatistiques - lit un fichier de stats et recrée un objet StatistiquesJeu
    /// "static" = cette méthode appartient à la classe, pas à un objet particulier
    /// On peut l'appeler avec StatistiquesJeu.ChargerStatistiques() sans créer d'objet d'abord
    /// </summary>
    public static StatistiquesJeu ChargerStatistiques(string cheminFichier = "statistiques_pendu.json")
    {
        try
        {
            // Vérifier si le fichier existe sur le disque dur
            if (File.Exists(cheminFichier))
            {
                // Lire tout le contenu du fichier en tant que texte
                string json = File.ReadAllText(cheminFichier);

                // Convertir le texte JSON en objet StatistiquesJeu
                var stats = JsonSerializer.Deserialize<StatistiquesJeu>(json);

                // "?? new StatistiquesJeu()" = "si stats est null, créer un objet vide à la place"
                return stats ?? new StatistiquesJeu();
            }
        }
        catch (Exception ex)  // Si quelque chose s'est mal passé (fichier corrompu, etc.)
        {
            Console.WriteLine($"Erreur lors du chargement des statistiques : {ex.Message}");
        }

        // Si on arrive ici, c'est soit que le fichier n'existe pas, soit qu'il y a eu une erreur
        // Dans tous les cas, on renvoie des statistiques vides (tout à zéro)
        return new StatistiquesJeu();
    }
}

// ==================== CLASSE POUR FONCTIONS UTILITAIRES ====================
// Cette classe contient toutes les "fonctions outils" de notre jeu du pendu
// "static" = on peut utiliser ses méthodes sans créer d'objet, comme UtilitairesPendu.JouerPendu()
// C'est comme une boîte à outils : on y range toutes nos fonctions pratiques

public static class UtilitairesPendu
{
    // ========== BASE DE DONNÉES DES MOTS ==========
    // "readonly" = on peut lire ce tableau mais pas le modifier (protection)
    // "string[]" = tableau de chaînes de caractères (mots)
    // Ce tableau contient tous les mots que le joueur peut avoir à deviner
    // On charge les mots depuis un fichier JSON au démarrage, avec des mots par défaut en secours
    public static readonly string[] Aliments = ChargerMotsDepuisJson();

    /// <summary>
    /// MÉTHODE : ChargerMotsDepuisJson
    /// Cette méthode lit un fichier JSON contenant la liste des mots à deviner
    /// Si le fichier n'existe pas ou est invalide, elle retourne une liste de mots par défaut
    /// C'est une méthode "static" car elle est appelée avant même la création d'un objet
    /// </summary>
    /// <param name="cheminFichier">Le chemin vers le fichier JSON (optionnel, par défaut "mots.json")</param>
    /// <returns>Un tableau de chaînes de caractères contenant les mots à deviner</returns>
    private static string[] ChargerMotsDepuisJson(string cheminFichier = "mots.json")
    {
        // Liste de mots par défaut en cas de problème avec le fichier
        // Ces mots garantissent que le jeu fonctionne toujours, même sans fichier JSON
        string[] motsParDefaut = new string[]
        {
            // Fruits faciles
            "Pomme", "Banane", "Orange", "Raisin", "Fraise", "Cerise", "Mangue", "Ananas", "Melon", "Pastèque",
            
            // Légumes courants
            "Tomate", "Carotte", "Poivron", "Concombre", "Courgette", "Aubergine", "Brocoli", "Chou", "Laitue", "Épinards",
            
            // Animaux populaires
            "Chien", "Chat", "Lion", "Tigre", "Éléphant", "Girafe", "Zèbre", "Cheval", "Lapin", "Écureuil",
            
            // Pays
            "France", "Allemagne", "Espagne", "Italie", "Portugal", "Belgique", "Suisse", "Canada", "Brésil", "Japon",
            
            // Villes
            "Paris", "Londres", "Madrid", "Rome", "Berlin", "Bruxelles", "Genève", "Montréal", "Tokyo", "Sydney",
            
            // Couleurs
            "Rouge", "Bleu", "Vert", "Jaune", "Orange", "Violet", "Rose", "Noir", "Blanc", "Gris"
        };

        // "try" = "Essaie de faire ça, mais si ça plante, ne casse pas le programme"
        try
        {
            // Vérifier si le fichier JSON existe sur le disque dur
            if (File.Exists(cheminFichier))
            {
                // ÉTAPE 1 : Lire tout le contenu du fichier en tant que texte
                // File.ReadAllText() lit le fichier d'un coup et retourne une chaîne de caractères
                string contenuJson = File.ReadAllText(cheminFichier);

                // ÉTAPE 2 : Désérialiser (= convertir) le texte JSON en tableau C#
                // JsonSerializer.Deserialize transforme du texte JSON en objets C# utilisables
                // Le <string[]> indique qu'on attend un tableau de chaînes de caractères
                string[]? mots = JsonSerializer.Deserialize<string[]>(contenuJson);

                // ÉTAPE 3 : Vérification de sécurité
                // Si la désérialisation a réussi ET que le tableau n'est pas vide
                if (mots != null && mots.Length > 0)
                {
                    // Afficher un message de confirmation (pour le débogage)
                    Console.WriteLine($"✓ {mots.Length} mots chargés depuis {cheminFichier}");

                    // Retourner les mots chargés depuis le fichier
                    return mots;
                }
                else
                {
                    // Le fichier existe mais est vide ou mal formaté
                    Console.WriteLine($"! Le fichier {cheminFichier} est vide ou invalide. Utilisation des mots par défaut.");
                }
            }
            else
            {
                // Le fichier n'existe pas, on va en créer un avec les mots par défaut
                Console.WriteLine($"! Le fichier {cheminFichier} n'existe pas. Création d'un fichier avec les mots par défaut...");

                // Créer le fichier JSON avec les mots par défaut
                CreerFichierMotsJson(cheminFichier, motsParDefaut);
            }
        }
        catch (Exception ex)  // "catch" = "Si il y a eu un problème, faire ça"
        {
            // Afficher un message d'erreur explicatif (sans planter le programme)
            Console.WriteLine($"! Erreur lors du chargement de {cheminFichier} : {ex.Message}");
            Console.WriteLine($"! Utilisation des mots par défaut.");
        }

        // Si on arrive ici, c'est qu'il y a eu un problème
        // On retourne les mots par défaut pour que le jeu fonctionne quand même
        return motsParDefaut;
    }

    /// <summary>
    /// MÉTHODE UTILITAIRE : CreerFichierMotsJson
    /// Crée un fichier JSON avec une liste de mots
    /// Utile pour générer automatiquement le fichier si l'utilisateur ne l'a pas
    /// </summary>
    /// <param name="cheminFichier">Le chemin où créer le fichier</param>
    /// <param name="mots">Les mots à sauvegarder dans le fichier</param>
    private static void CreerFichierMotsJson(string cheminFichier, string[] mots)
    {
        try
        {
            // Convertir le tableau de mots en format JSON
            // WriteIndented = true rend le fichier lisible (avec indentation et retours à la ligne)
            string json = JsonSerializer.Serialize(mots, new JsonSerializerOptions
            {
                WriteIndented = true,
                // Encoder = null permet d'écrire les caractères accentués correctement (é, è, à, etc.)
                Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
            });

            // Écrire le texte JSON dans le fichier sur le disque dur
            File.WriteAllText(cheminFichier, json);

            Console.WriteLine($"✓ Fichier {cheminFichier} créé avec succès avec {mots.Length} mots !");
        }
        catch (Exception ex)
        {
            // Si la création échoue, afficher l'erreur mais continuer le jeu
            Console.WriteLine($"! Impossible de créer le fichier {cheminFichier} : {ex.Message}");
        }
    }

    /// <summary>
    /// Normalise un caractère pour la comparaison (ex: ç => c, â/ä => a, etc.)
    /// Cette méthode est cruciale pour un jeu en français car elle permet de comparer
    /// les lettres avec et sans accents comme étant identiques.
    /// Par exemple, si le mot contient "É" et que le joueur tape "e", 
    /// cette fonction permettra de reconnaître que c'est la même lettre.
    /// </summary>
    /// <param name="c">Le caractère à normaliser (peut être accentué)</param>
    /// <returns>Le caractère normalisé sans accent en majuscule</returns>
    public static char NormalizeChar(char c)
    {
        // Première étape : convertir le caractère en majuscule
        // ToUpperInvariant() est utilisé pour éviter les problèmes de locale/culture
        // Par exemple, en turc, le 'i' majuscule n'est pas 'I' mais 'İ'
        // ToUpperInvariant() garantit un comportement cohérent peu importe la langue du système
        c = char.ToUpperInvariant(c);

        // Utilisation d'un switch pour convertir chaque caractère accentué vers sa version non-accentuée
        // Le switch en C# est très efficace pour ce type de comparaisons multiples
        switch (c)
        {
            // Toutes les variantes de la lettre A avec accents => A simple
            case 'À': case 'Â': case 'Ä': return 'A';

            // La cédille française => C simple
            case 'Ç': return 'C';

            // Toutes les variantes de la lettre E avec accents => E simple
            case 'É': case 'È': case 'Ê': case 'Ë': return 'E';

            // Toutes les variantes de la lettre I avec accents => I simple
            case 'Î': case 'Ï': return 'I';

            // Toutes les variantes de la lettre O avec accents => O simple
            case 'Ô': case 'Ö': return 'O';

            // Toutes les variantes de la lettre U avec accents => U simple
            case 'Ù': case 'Û': case 'Ü': return 'U';

            // Le Y avec tréma => Y simple
            case 'Ÿ': return 'Y';

            // Par défaut : si le caractère n'a pas d'accent, le retourner tel quel
            // Cela inclut toutes les lettres normales A-Z et les caractères non-lettres
            default: return c;
        }
    }

    /// <summary>
    /// Affiche le dessin du pendu selon le nombre d'erreurs commises
    /// </summary>
    /// <param name="erreurs">Nombre d'erreurs (0 à 6)</param>
    public static void AfficherPendu(int erreurs)
    {
        // Tableau contenant les différentes étapes du dessin du pendu
        // Chaque élément représente une étape de plus dans la construction du pendu
        string[] pendu = new string[]
    { 
    // 0 erreur : potence vide 
@" 
    +-------------+
    |             |
    |              
    |              
    |              
    |              
    |              
    |              
    +------------- 
    | 
    |   Pret a jouer ? 
    ", 
     
    // 1 erreur : base + corde 
@" 
    +-------------+
    |             |
    |             O 
    |              
    |              
    |              
    |              
    |              
    +------------- 
    | 
    |   1ere erreur... 
    ", 
     
    // 2 erreurs : tête complète 
@" 
    +-------------+
    |             |
    |             O 
    |            - - 
    |              
    |              
    |              
    |              
    +------------- 
    | 
    |   Oups... 
    ", 
     
    // 3 erreurs : torse 
@" 
    +-------------+
    |             |
    |             O 
    |            - - 
    |             # 
    |             | 
    |              
    |              
    +------------- 
    | 
    |   C'est pas gagné... 
    ", 
     
    // 4 erreurs : bras gauche 
@" 
    +-------------+
    |             |
    |             O 
    |            - - 
    |             # 
    |            /| 
    |              
    |              
    +------------- 
    | 
    |   Aie aie aie ! 
    ", 
     
    // 5 erreurs : bras droit 
@" 
    +-------------+
    |             |
    |             O 
    |            - - 
    |             # 
    |            /|\ 
    |              
    |              
    +------------- 
    | 
    |   Plus que 2 chances ! 
    ", 
     
    // 6 erreurs : pendu complet 
@" 
    +-------------+
    |             |
    |             O 
    |            X X 
    |             # 
    |            /|\ 
    |            / \ 
    |              
    +------------- 
    | 
    |   PERDU !  
    |   R.I.P (Repose en paix. Bro !) 
    ",
    };

        // Affiche le dessin correspondant au nombre d'erreurs
        // Math.Min garantit qu'on ne dépasse pas la taille du tableau
        Console.WriteLine(pendu[Math.Min(erreurs, pendu.Length - 1)]);
    }

    /// <summary>
    /// Affiche le mot à deviner avec les lettres trouvées et les underscores pour les lettres manquantes
    /// </summary>
    /// <param name="mot">Le mot complet à deviner</param>
    /// <param name="lettresTrouvees">Tableau booléen indiquant quelles lettres ont été trouvées</param>
    public static void AfficherMot(string mot, bool[] lettresTrouvees)
    {
        // Sauvegarder la couleur actuelle pour pouvoir la restaurer après
        ConsoleColor couleurActuelle = Console.ForegroundColor;

        // Changer la couleur en blanc pour mettre le mot en évidence
        Console.ForegroundColor = ConsoleColor.White;

        // Parcourir chaque caractère du mot
        for (int i = 0; i < mot.Length; i++)
        {
            char c = mot[i]; // Caractère actuel

            // Vérifier si c'est une lettre (pas un espace, tiret, etc.)
            if (char.IsLetter(c))
            {
                // Si la lettre a été trouvée, l'afficher
                if (lettresTrouvees[i])
                    Console.Write(c + " ");
                else
                    // Sinon, afficher un underscore pour indiquer une lettre manquante
                    Console.Write("_ ");
            }
            else
            {
                // Pour les caractères non-lettres (espaces, tirets), les afficher tels quels
                Console.Write(c + " ");
            }
        }
        Console.WriteLine(); // Retour à la ligne après le mot

        // Restaurer la couleur jaune pour le reste de l'interface
        Console.ForegroundColor = couleurActuelle;
    }

    /// <summary>
    /// Fonction principale qui gère une partie complète du jeu du pendu
    /// </summary>
    /// <returns>Résultat de la partie avec les statistiques</returns>
    public static ResultatPartie JouerPendu()
    {
        // Initialisation du générateur de nombres aléatoires
        var rand = new Random();

        // Sélection aléatoire d'un mot dans la base de données
        // ToUpperInvariant() convertit le mot en majuscules pour faciliter la comparaison
        string motADeviner = Aliments[rand.Next(Aliments.Length)].ToUpperInvariant();

        // Tableau booléen pour suivre quelles lettres ont été trouvées
        // La taille correspond à la longueur du mot
        bool[] lettresTrouvees = new bool[motADeviner.Length];

        // Nombre d'essais restants (le joueur perd après 6 erreurs)
        int essaisRestants = 6;

        // Chaîne pour stocker les lettres déjà essayées par le joueur
        string lettresEssayees = "";

        // Variable pour indiquer si le joueur a gagné
        bool gagne = false;

        // ==================== BOUCLE PRINCIPALE DU JEU ====================
        // Continue tant que le joueur a des essais et n'a pas encore gagné
        while (essaisRestants > 0 && !gagne)
        {
            // Effacer l'écran pour une interface propre
            Console.Clear();

            // Affichage de l'interface de jeu
            Console.WriteLine("==== Jeu du Pendu ====");

            // Afficher le pendu avec le nombre d'erreurs actuelles
            UtilitairesPendu.AfficherPendu(6 - essaisRestants);

            // Afficher le nombre d'essais restants
            Console.WriteLine($"Essais restants : {essaisRestants}");

            // Afficher le mot à deviner avec les lettres trouvées
            Console.Write("Mot à deviner : ");
            UtilitairesPendu.AfficherMot(motADeviner, lettresTrouvees);

            // Afficher les lettres déjà essayées
            Console.WriteLine($"Lettres essayées : {lettresEssayees}");

            // ==================== SAISIE D'UNE SEULE LETTRE ====================
            // Demander au joueur de proposer une lettre
            Console.Write("Proposez une lettre : ");

            // ReadKey() au lieu de ReadLine() = le joueur ne peut taper qu'UNE SEULE touche
            // L'avantage : pas besoin d'appuyer sur Entrée, c'est plus rapide !
            // "true" = ne pas afficher la touche pressée à l'écran (on l'affichera nous-mêmes)
            ConsoleKeyInfo touchePressee = Console.ReadKey(true);

            // Extraire le caractère de la touche pressée et le convertir en majuscule
            // touchePressee.KeyChar donne le caractère correspondant à la touche
            char caractereSaisi = char.ToUpperInvariant(touchePressee.KeyChar);

            // Afficher la lettre saisie par l'utilisateur (en majuscule)
            // Cela donne un feedback visuel : l'utilisateur voit ce qu'il a tapé
            Console.WriteLine(caractereSaisi);

            // Convertir le caractère en chaîne de caractères pour la suite du code
            // (certaines méthodes ont besoin d'une string et pas d'un char)
            string saisie = caractereSaisi.ToString();

            // ==================== VALIDATION DE LA SAISIE ====================
            // Vérifier que la saisie est valide (une lettre alphabétique)
            // char.IsLetter() vérifie si c'est bien une lettre (A-Z, a-z) et pas un chiffre ou symbole
            if (!char.IsLetter(caractereSaisi))
            {
                // Message d'erreur si l'utilisateur a tapé autre chose qu'une lettre
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("\n❌ Ce n'est pas une lettre ! Veuillez entrer une lettre (A-Z).");
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("\nAppuyez sur une touche pour continuer...");
                Console.ReadKey(); // Attendre que le joueur appuie sur une touche
                continue; // Recommencer la boucle (retour au début du while)
            }

            // La lettre est valide, on peut continuer
            char lettre = caractereSaisi;

            // Vérifier si la lettre a déjà été essayée
            if (lettresEssayees.Contains(lettre))
            {
                // Afficher un message d'avertissement en orange/jaune
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine($"\n⚠️  Vous avez déjà essayé la lettre '{lettre}' !");
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("\nAppuyez sur une touche pour continuer...");
                Console.ReadKey(); // Attendre que le joueur appuie sur une touche
                continue; // Recommencer la boucle (retour au début du while)
            }

            // Ajouter la lettre à la liste des lettres essayées
            lettresEssayees += lettre + " ";

            // ==================== VÉRIFICATION DE LA LETTRE ====================
            bool trouve = false; // Variable pour indiquer si la lettre a été trouvée
            char lettreNorm = UtilitairesPendu.NormalizeChar(lettre);
            // Parcourir le mot pour chercher la lettre proposée
            for (int i = 0; i < motADeviner.Length; i++)
            {
                if (char.IsLetter(motADeviner[i]) && UtilitairesPendu.NormalizeChar(motADeviner[i]) == lettreNorm)
                {
                    // Marquer cette position comme trouvée
                    lettresTrouvees[i] = true;
                    trouve = true; // La lettre a été trouvée au moins une fois
                }
            }

            // Si la lettre n'a pas été trouvée, décrémenter le nombre d'essais
            if (!trouve)
                essaisRestants--;

            // ==================== VÉRIFICATION DE LA VICTOIRE ====================
            // Vérifier si toutes les lettres ont été trouvées
            gagne = true; // On suppose que le joueur a gagné

            // Parcourir le mot pour vérifier si toutes les lettres sont trouvées
            for (int i = 0; i < motADeviner.Length; i++)
            {
                // Si c'est une lettre et qu'elle n'a pas été trouvée
                if (char.IsLetter(motADeviner[i]) && !lettresTrouvees[i])
                {
                    gagne = false; // Le joueur n'a pas encore gagné
                    break; // Sortir de la boucle
                }
            }
        }

        // ==================== FIN DE PARTIE ====================
        // Effacer l'écran pour afficher le résultat final
        Console.Clear();

        if (gagne)
        {
            // Le joueur a gagné : toutes les lettres ont été trouvées
            Console.WriteLine("Bravo ! Vous avez trouvé le mot : " + motADeviner);
        }
        else
        {
            // Le joueur a perdu : afficher le pendu complet et le mot
            UtilitairesPendu.AfficherPendu(6); // Pendu complet
            Console.WriteLine("Dommage ! Le mot était : " + motADeviner);
        }

        // Compter le nombre de lettres uniques utilisées
        var lettresUniques = lettresEssayees.Split(' ', StringSplitOptions.RemoveEmptyEntries).Length;

        // Retourner le résultat de la partie
        return new ResultatPartie
        {
            Gagne = gagne,
            NombreLettresUtilisees = lettresUniques,
            MotADeviner = motADeviner
        };
    }
}