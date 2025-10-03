# Projet du pendu

## Description

Ce projet est un jeu de pendu en C# qui permet de jouer contre l'ordinateur.

## Fonctionnalités

### 🎮 **Jeu de base**

- Le joueur doit deviner un mot en proposant des lettres une par une
- 6 tentatives maximum avant d'être "pendu"
- Affichage progressif du dessin du pendu selon les erreurs
- Interface console colorée (fond vert, texte noir/blanc/rouge/vert selon le contexte)
- Gestion des lettres déjà essayées (évite les doublons)
- Base de données riche (1000+ mots)

### 🔤 **Gestion intelligente des caractères**

- **Support des accents français** : É, È, Ê, Ë = E / À, Â, Ä = A / Ç = C, etc.
- **Caractères spéciaux** : Les tirets (-) et apostrophes (') sont automatiquement affichés
- **Normalisation** : "Saint-Pierre" affiche "S A I N T - P I E R R E" (tiret visible dès le début)

### 📊 **Système de statistiques complet**

- **Parties jouées** : Compteur total des parties
- **Parties gagnées/perdues** : Suivi détaillé des victoires et défaites
- **Taux de réussite** : Pourcentage de victoires calculé automatiquement
- **Mots trouvés** : Nombre total de mots devinés avec succès
- **Lettres utilisées** : Total des lettres tentées dans toutes les parties
- **Moyenne par partie** : Nombre moyen de lettres utilisées par partie

### 💾 **Sauvegarde persistante**

- **Fichier JSON** : Sauvegarde automatique dans `statistiques_pendu.json`
- **Conservation des données** : Les statistiques persistent entre les sessions
- **Chargement automatique** : Récupération des anciens scores au démarrage
- **Gestion d'erreurs** : Système robuste en cas de fichier corrompu

### 🎨 **Interface utilisateur soignée**

- **Codes couleur** :
  - Vert : Messages de victoire
  - Rouge : Messages de défaite
  - Blanc : Mot à deviner
  - Jaune : En-têtes des statistiques
- **Tableau de statistiques** : Affichage structuré avec symboles [*] [+] [-] [O] [#] [%] [~]
- **Messages contextuels** : Feedback adapté selon les actions du joueur
- **Interface claire** : Écran nettoyé à chaque tour pour une expérience fluide

### 🔄 **Gestion de session**

- **Parties multiples** : Possibilité de rejouer indéfiniment
- **Menu de sortie** : Choix "Voulez-vous rejouer ? (O/N)"
- **Validation des saisies** : Vérification que l'utilisateur saisit bien une lettre
- **Attente utilisateur** : Pauses avec "Appuyez sur une touche..."

### 🏆 **Système de progression**

- **Feedback immédiat** : Message de victoire/défaite coloré après chaque partie
- **Historique complet** : Affichage des statistiques après chaque partie
- **Motivation** : Taux de réussite affiché pour encourager l'amélioration

### 🛡️ **Robustesse**

- **Gestion des erreurs** : Try/catch pour les opérations fichier
- **Saisies sécurisées** : Validation des entrées utilisateur
- **Valeurs par défaut** : Comportement prévisible en cas de problème
- **Code commenté** : Plus de 800 lignes de commentaires pour la compréhension

## Aperçu de l'application

![image](./readme.gif)

## Code

```csharp
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
Console.ForegroundColor = ConsoleColor.Black;   // Met le texte en noir (contraste avec le vert)
Console.Clear();                                // Efface tout ce qui était affiché avant

// ÉTAPE 2 : Charger les statistiques des parties précédentes
// Si le joueur a déjà joué avant, on récupère ses anciens scores depuis un fichier
// Si c'est la première fois, on crée des statistiques vides (tout à zéro)
StatistiquesJeu statistiques = StatistiquesJeu.ChargerStatistiques();

// ÉTAPE 3 : Afficher un message de bienvenue sympa
Console.WriteLine("*** Bienvenue dans le Jeu du Pendu ! ***");

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

        // On change la couleur du texte en rouge pour montrer la défaite
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("\n:( Dommage ! Vous avez perdu cette partie.");
    }

    // Remettre la couleur du texte en noir (couleur par défaut de notre jeu)
    Console.ForegroundColor = ConsoleColor.Black;

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
        Console.ForegroundColor = ConsoleColor.Black; // Retour à la couleur de notre jeu
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
    public static readonly string[] Aliments = new string[]
{
    // On range les mots par catégories pour que ce soit plus organisé

    // Fruits (des mots plutôt faciles pour commencer)
    "Pomme", "Banane", "Orange", "Raisin", "Fraise", "Cerise", "Mangue", "Ananas", "Melon", "Pastèque",
    "Poire", "Pêche", "Abricot", "Prune", "Kiwi", "Citron", "Pamplemousse", "Clémentine", "Mandarine", "Figue",
    "Datte", "Grenade", "Papaye", "Litchi", "Fruit de la passion", "Noix de coco", "Goyave", "Cassis", "Groseille", "Myrtille",
    "Framboise", "Mûre", "Airelle", "Nectarine", "Brugnon", "Mirabelle", "Quetsche", "Reine-claude", "Coing", "Nèfle",
    "Avocat", "Olive", "Tomate cerise", "Physalis", "Kumquat", "Bergamote", "Yuzu", "Carambole", "Ramboutan", "Durian",

    // Légumes (un peu plus variés)
    "Tomate", "Carotte", "Poivron", "Concombre", "Courgette", "Aubergine", "Brocoli", "Chou", "Laitue", "Épinards",
    "Haricot", "Petit pois", "Artichaut", "Asperge", "Betterave", "Céleri", "Fenouil", "Radis", "Navet", "Panais",
    "Potiron", "Courge", "Butternut", "Patate douce", "Topinambour", "Rutabaga", "Chou-fleur", "Chou de Bruxelles", "Chou rouge", "Chou-rave",
    "Endive", "Cresson", "Roquette", "Mâche", "Chicorée", "Persil", "Coriandre", "Basilic", "Ciboulette", "Estragon",
    "Oignon", "Échalote", "Ail", "Poireau", "Champignon", "Pleurote", "Shiitake", "Cèpe", "Morille", "Truffe",

    // Viandes et Poissons
    "Poulet", "Bœuf", "Porc", "Agneau", "Veau", "Canard", "Dinde", "Lapin", "Gibier", "Sanglier",
    "Saumon", "Thon", "Truite", "Cabillaud", "Sole", "Dorade", "Bar", "Maquereau", "Sardine", "Anchois",
    "Crevette", "Homard", "Crabe", "Langouste", "Moule", "Huître", "Coquille Saint-Jacques", "Calmar", "Seiche", "Poulpe",

    // Produits laitiers
    "Lait", "Fromage", "Yaourt", "Beurre", "Crème", "Camembert", "Roquefort", "Comté", "Brie", "Gruyère",
    "Emmental", "Mozzarella", "Parmesan", "Feta", "Chèvre", "Ricotta", "Mascarpone", "Cheddar", "Reblochon", "Munster",

    // Céréales et Féculents
    "Pain", "Riz", "Pâtes", "Blé", "Maïs", "Avoine", "Orge", "Seigle", "Quinoa", "Boulgour",
    "Couscous", "Semoule", "Sarrasin", "Épeautre", "Millet", "Polenta", "Pomme de terre", "Tapioca", "Vermicelle", "Nouilles",

    // Sucreries et Desserts
    "Chocolat", "Gâteau", "Tarte", "Biscuit", "Bonbon", "Caramel", "Glace", "Crêpe", "Gaufre", "Macaron",
    "Éclair", "Profiterole", "Flan", "Tiramisu", "Brownie", "Muffin", "Cookie", "Cupcake", "Meringue", "Nougat",
    "Praline", "Truffe au chocolat", "Fondant", "Financier", "Madeleine", "Cannelé", "Clafoutis", "Millefeuille", "Paris-Brest", "Saint-Honoré",

    // Boissons
    "Eau", "Café", "Thé", "Jus", "Soda", "Limonade", "Sirop", "Chocolat chaud", "Smoothie", "Milkshake",
    "Vin", "Bière", "Cidre", "Champagne", "Cognac", "Whisky", "Rhum", "Vodka", "Gin", "Pastis",

    // Épices et Condiments
    "Sel", "Poivre", "Paprika", "Cumin", "Curry", "Safran", "Cannelle", "Muscade", "Gingembre", "Clou de girofle",
    "Vanille", "Cardamome", "Anis", "Curcuma", "Piment", "Moutarde", "Ketchup", "Mayonnaise", "Vinaigre", "Huile",

    // Métiers (mots plus longs et complexes)
    "Médecin", "Professeur", "Ingénieur", "Avocat", "Architecte", "Cuisinier", "Électricien", "Plombier", "Menuisier", "Boulanger",
    "Infirmier", "Dentiste", "Pharmacien", "Vétérinaire", "Chirurgien", "Pompier", "Policier", "Gendarme", "Militaire", "Pilote",
    "Journaliste", "Écrivain", "Artiste", "Musicien", "Chanteur", "Acteur", "Danseur", "Peintre", "Sculpteur", "Photographe",
    "Mécanicien", "Chauffeur", "Facteur", "Coiffeur", "Esthéticien", "Maçon", "Peintre en bâtiment", "Couvreur", "Charpentier", "Serrurier",
    "Comptable", "Banquier", "Agent immobilier", "Vendeur", "Commerçant", "Caissier", "Serveur", "Barman", "Réceptionniste", "Secrétaire",
    "Informaticien", "Développeur", "Designer", "Graphiste", "Webmaster", "Community manager", "Marketeur", "Commercial", "Consultant", "Manager",

    // Animaux (mots amusants pour les enfants)
    "Chien", "Chat", "Lion", "Tigre", "Éléphant", "Girafe", "Zèbre", "Cheval", "Lapin", "Écureuil",
    "Ours", "Loup", "Renard", "Cerf", "Sanglier", "Hérisson", "Souris", "Rat", "Hamster", "Cochon d'Inde",
    "Vache", "Mouton", "Chèvre", "Cochon", "Poule", "Coq", "Canard", "Oie", "Dindon", "Pigeon",
    "Aigle", "Faucon", "Hibou", "Chouette", "Corbeau", "Pie", "Moineau", "Hirondelle", "Merle", "Rouge-gorge",
    "Perroquet", "Toucan", "Flamant rose", "Autruche", "Manchot", "Pingouin", "Mouette", "Albatros", "Pélican", "Cygne",
    "Crocodile", "Alligator", "Serpent", "Lézard", "Tortue", "Grenouille", "Crapaud", "Salamandre", "Caméléon", "Iguane",
    "Requin", "Baleine", "Dauphin", "Orque", "Phoque", "Otarie", "Morse", "Hippopotame", "Rhinocéros", "Kangourou",
    "Koala", "Panda", "Singe", "Gorille", "Chimpanzé", "Orang-outan", "Léopard", "Guépard", "Panthère", "Jaguar",
    "Chameau", "Dromadaire", "Lama", "Alpaga", "Renne", "Élan", "Bison", "Buffle", "Yak", "Antilope",

    // Pays (pour apprendre la géographie en jouant !)
    "France", "Allemagne", "Espagne", "Italie", "Portugal", "Belgique", "Suisse", "Canada", "Brésil", "Japon",
    "Angleterre", "Irlande", "Écosse", "Pays de Galles", "Pays-Bas", "Luxembourg", "Autriche", "Pologne", "Tchéquie", "Hongrie",
    "Roumanie", "Bulgarie", "Grèce", "Turquie", "Russie", "Ukraine", "Norvège", "Suède", "Finlande", "Danemark",
    "Islande", "Croatie", "Slovénie", "Serbie", "Albanie", "Macédoine", "Bosnie", "Monténégro", "Slovaquie", "Lituanie",
    "Lettonie", "Estonie", "Biélorussie", "Moldavie", "Arménie", "Géorgie", "Azerbaïdjan", "Kazakhstan", "Ouzbékistan", "Kirghizistan",
    "États-Unis", "Mexique", "Argentine", "Chili", "Pérou", "Colombie", "Venezuela", "Équateur", "Bolivie", "Paraguay",
    "Uruguay", "Costa Rica", "Panama", "Cuba", "Jamaïque", "Haïti", "République dominicaine", "Guatemala", "Honduras", "Nicaragua",
    "Chine", "Inde", "Corée du Sud", "Corée du Nord", "Thaïlande", "Vietnam", "Cambodge", "Laos", "Myanmar", "Malaisie",
    "Singapour", "Indonésie", "Philippines", "Taïwan", "Mongolie", "Népal", "Bangladesh", "Pakistan", "Afghanistan", "Iran",
    "Irak", "Syrie", "Liban", "Israël", "Jordanie", "Arabie saoudite", "Émirats arabes unis", "Qatar", "Koweït", "Oman",
    "Yémen", "Égypte", "Libye", "Tunisie", "Algérie", "Maroc", "Mauritanie", "Mali", "Niger", "Tchad",
    "Soudan", "Éthiopie", "Kenya", "Tanzanie", "Ouganda", "Rwanda", "Burundi", "Somalie", "Sénégal", "Côte d'Ivoire",
    "Ghana", "Nigeria", "Cameroun", "Gabon", "Congo", "Angola", "Namibie", "Botswana", "Zimbabwe", "Mozambique",
    "Madagascar", "Afrique du Sud", "Zambie", "Malawi", "Australie", "Nouvelle-Zélande", "Papouasie", "Fidji", "Tonga", "Samoa",

    // Villes
    "Paris", "Londres", "Madrid", "Rome", "Berlin", "Bruxelles", "Genève", "Montréal", "Tokyo", "Sydney",
    "Lyon", "Marseille", "Toulouse", "Nice", "Nantes", "Strasbourg", "Bordeaux", "Lille", "Rennes", "Reims",
    "Barcelone", "Séville", "Valence", "Bilbao", "Milan", "Naples", "Florence", "Venise", "Turin", "Bologne",
    "Munich", "Hambourg", "Cologne", "Francfort", "Stuttgart", "Düsseldorf", "Dortmund", "Essen", "Leipzig", "Dresde",
    "Amsterdam", "Rotterdam", "La Haye", "Vienne", "Varsovie", "Prague", "Budapest", "Bucarest", "Athènes", "Lisbonne",
    "Dublin", "Édimbourg", "Manchester", "Liverpool", "Glasgow", "Copenhague", "Stockholm", "Oslo", "Helsinki", "Moscou",
    "Saint-Pétersbourg", "New York", "Los Angeles", "Chicago", "San Francisco", "Boston", "Miami", "Las Vegas", "Seattle", "Washington",
    "Pékin", "Shanghai", "Hong Kong", "Séoul", "Bangkok", "Singapour", "Dubaï", "Le Caire", "Istanbul", "Johannesburg",
    "Rio de Janeiro", "Buenos Aires", "Mexico", "Lima", "Bogota", "Santiago", "Caracas", "Quito", "La Paz", "Asuncion",

    // Objets du quotidien
    "Table", "Chaise", "Lit", "Armoire", "Canapé", "Fauteuil", "Bureau", "Lampe", "Miroir", "Horloge",
    "Téléphone", "Ordinateur", "Tablette", "Télévision", "Radio", "Appareil photo", "Caméra", "Clavier", "Souris", "Écran",
    "Livre", "Cahier", "Stylo", "Crayon", "Gomme", "Règle", "Ciseaux", "Colle", "Agrafeuse", "Trombone",
    "Sac", "Valise", "Portefeuille", "Montre", "Lunettes", "Chapeau", "Écharpe", "Gants", "Parapluie", "Canne",

    // Vêtements
    "Pantalon", "Jean", "Short", "Jupe", "Robe", "Chemise", "T-shirt", "Pull", "Gilet", "Veste",
    "Manteau", "Blouson", "Imperméable", "Parka", "Cardigan", "Sweat", "Polo", "Débardeur", "Bustier", "Combinaison",
    "Chaussette", "Collant", "Bas", "Caleçon", "Slip", "Culotte", "Soutien-gorge", "Maillot de bain", "Bikini", "Pyjama",
    "Chaussure", "Basket", "Botte", "Bottine", "Sandale", "Tong", "Escarpin", "Mocassin", "Ballerine", "Sabot",

    // Sports et Loisirs
    "Football", "Basketball", "Tennis", "Volleyball", "Handball", "Rugby", "Golf", "Natation", "Cyclisme", "Athlétisme",
    "Ski", "Snowboard", "Patinage", "Hockey", "Boxe", "Judo", "Karaté", "Taekwondo", "Escrime", "Lutte",
    "Escalade", "Alpinisme", "Randonnée", "Course", "Marathon", "Triathlon", "Gymnastique", "Danse", "Yoga", "Pilates",
    "Équitation", "Voile", "Surf", "Plongée", "Kayak", "Canoë", "Aviron", "Pêche", "Chasse", "Tir à l'arc",

    // Couleurs
    "Rouge", "Bleu", "Vert", "Jaune", "Orange", "Violet", "Rose", "Noir", "Blanc", "Gris",
    "Marron", "Beige", "Turquoise", "Cyan", "Magenta", "Bordeaux", "Pourpre", "Indigo", "Lavande", "Écarlate",

    // Transports
    "Voiture", "Moto", "Vélo", "Trottinette", "Bus", "Tramway", "Métro", "Train", "Avion", "Hélicoptère",
    "Bateau", "Yacht", "Ferry", "Sous-marin", "Camion", "Ambulance", "Taxi", "Scooter", "Tracteur", "Bulldozer",
    // Fruits supplémentaires
"Mangoustan", "Jujube", "Kaki", "Sureau", "Cynorhodon", "Arbouse", "Nashi", "Feijoa", "Pitaya", "Cherimoya",
"Tamarillo", "Sapotille", "Jaboticaba", "Acérola", "Jaque", "Longane", "Açaï", "Cupuaçu", "Baobab", "Salak",

// Légumes supplémentaires
"Salsifis", "Crosne", "Oseille", "Pourpier", "Arroche", "Tétragone", "Mizuna", "Pak-choï", "Chou chinois", "Edamame",
"Piment d'Espelette", "Okra", "Gingembre", "Galanga", "Citronnelle", "Wasabi", "Raifort", "Daikon", "Taro", "Igname",

// Viandes et Poissons supplémentaires
"Brochet", "Perche", "Carpe", "Espadon", "Raie", "Turbot", "Merlu", "Lieu", "Rouget", "Grondin",
"Caille", "Faisan", "Perdrix", "Pintade", "Chevreuil", "Cerf", "Biche", "Marcassin", "Lièvre", "Oie",
"Anguille", "Limande", "Plie", "Flétan", "Carrelet", "Barbue", "Saint-Pierre", "Lotte", "Congre", "Roussette",
"Écrevisse", "Langoustine", "Tourteau", "Araignée de mer", "Bulot", "Bigorneau", "Palourde", "Praire", "Couteau", "Ormeau",

// Produits laitiers supplémentaires
"Beaufort", "Abondance", "Tomme", "Raclette", "Fourme d'Ambert", "Bleu d'Auvergne", "Saint-Nectaire", "Cantal", "Salers", "Laguiole",
"Ossau-Iraty", "Pélardon", "Picodon", "Rocamadour", "Cabécou", "Crottin de Chavignol", "Valençay", "Selles-sur-Cher", "Pouligny-Saint-Pierre", "Sainte-Maure",
"Époisses", "Maroilles", "Livarot", "Pont-l'Évêque", "Neufchâtel", "Langres", "Chaource", "Coulommiers", "Brillat-Savarin", "Boursin",

// Céréales et Féculents supplémentaires
"Farro", "Kamut", "Teff", "Amarante", "Fonio", "Sorgho", "Gnocchi", "Lasagne", "Ravioli", "Tortellini",
"Cannelloni", "Macaroni", "Spaghetti", "Linguine", "Penne", "Fusilli", "Farfalle", "Rigatoni", "Tagliatelle", "Fettuccine",

// Sucreries et Desserts supplémentaires
"Panna cotta", "Crème brûlée", "Mousse au chocolat", "Bavarois", "Charlotte", "Soufflé", "Îles flottantes", "Baba au rhum", "Savarin", "Kouglof",
"Brioche", "Pain d'épices", "Spéculoos", "Canistrelli", "Calisson", "Navette", "Bêtise de Cambrai", "Bergamote de Nancy", "Anis de Flavigny", "Violette de Toulouse",
"Pâte de fruits", "Guimauve", "Marshmallow", "Réglisse", "Berlingot", "Sucre d'orge", "Dragée", "Nougat de Montélimar", "Touron", "Polvorone",
"Churros", "Beignet", "Donut", "Pain perdu", "Pancake", "Blini", "Scone", "Cheesecake", "Strudel", "Baklava",

// Boissons supplémentaires
"Tisane", "Infusion", "Cappuccino", "Expresso", "Latte", "Mokaccino", "Chai", "Maté", "Rooibos", "Kombucha",
"Kéfir", "Lassi", "Horchata", "Sangria", "Mojito", "Caipirinha", "Piña colada", "Margarita", "Daiquiri", "Cosmopolitan",
"Martini", "Manhattan", "Negroni", "Spritz", "Bloody Mary", "Long Island", "Tequila sunrise", "Sex on the beach", "Blue lagoon", "Mai tai",
"Porto", "Sherry", "Vermouth", "Limoncello", "Amaretto", "Baileys", "Cointreau", "Grand Marnier", "Chartreuse", "Absinthe",

// Épices et Condiments supplémentaires
"Sumac", "Za'atar", "Ras el hanout", "Garam masala", "Tandoori", "Piment de Cayenne", "Piment de la Jamaïque", "Baies roses", "Fenugrec", "Nigelle",
"Coriandre en graines", "Fenouil en graines", "Moutarde en graines", "Sésame", "Pavot", "Carvi", "Aneth", "Laurier", "Thym", "Romarin",
"Origan", "Marjolaine", "Sarriette", "Sauge", "Menthe", "Mélisse", "Verveine", "Tamarin", "Harissa", "Sambal",
"Nuoc-mâm", "Sauce soja", "Sauce hoisin", "Sauce teriyaki", "Sauce worcestershire", "Tabasco", "Sriracha", "Chimichurri", "Pesto", "Tapenade",

// Métiers supplémentaires
"Astrophysicien", "Biologiste", "Chimiste", "Physicien", "Mathématicien", "Géologue", "Botaniste", "Zoologiste", "Archéologue", "Anthropologue",
"Psychologue", "Psychiatre", "Sociologue", "Économiste", "Historien", "Géographe", "Philosophe", "Théologien", "Linguiste", "Traducteur",
"Interprète", "Bibliothécaire", "Archiviste", "Documentaliste", "Éditeur", "Imprimeur", "Relieur", "Libraire", "Galeriste", "Conservateur",
"Restaurateur d'art", "Antiquaire", "Commissaire-priseur", "Notaire", "Huissier", "Greffier", "Magistrat", "Procureur", "Juge", "Commissaire",
"Détective", "Agent secret", "Douanier", "Garde-côte", "Sauveteur", "Maître-nageur", "Moniteur de ski", "Guide de montagne", "Spéléologue", "Explorateur",
"Astronaute", "Cosmonaute", "Pilote de chasse", "Pilote de ligne", "Contrôleur aérien", "Hôtesse de l'air", "Steward", "Marin", "Capitaine", "Amiral",
"Bûcheron", "Forestier", "Agriculteur", "Viticulteur", "Arboriculteur", "Maraîcher", "Éleveur", "Berger", "Apiculteur", "Ostréiculteur",
"Fromager", "Boucher", "Charcutier", "Poissonnier", "Caviste", "Sommelier", "Barista", "Pâtissier", "Chocolatier", "Glacier",
"Traiteur", "Nutritionniste", "Diététicien", "Kinésithérapeute", "Ostéopathe", "Chiropracteur", "Acupuncteur", "Sophrologue", "Naturopathe", "Homéopathe",
"Opticien", "Audioprothésiste", "Orthophoniste", "Orthoptiste", "Podologue", "Pédicure", "Prothésiste dentaire", "Radiologue", "Anesthésiste", "Cardiologue",
"Dermatologue", "Gynécologue", "Pédiatre", "Gériatre", "Neurologue", "Ophtalmologue", "ORL", "Urologue", "Cancérologue", "Oncologue",

// Animaux supplémentaires
"Tatou", "Fourmilier", "Paresseux", "Tapir", "Capybara", "Loutreoutremur", "Putois", "Belette", "Hermine", "Vison",
"Loutre", "Castor", "Ragondin", "Surmulot", "Musaraigne", "Taupe", "Chauve-souris", "Pipistrelle", "Hérisson d'Europe", "Blaireau",
"Martre", "Fouine", "Lynx", "Chat sauvage", "Genette", "Mouflon", "Bouquetin", "Chamois", "Isard", "Marmotte",
"Lémurien", "Tarsier", "Loris", "Babouin", "Mandrill", "Macaque", "Gibbon", "Siamang", "Ouistiti", "Capucin",
"Tamanoir", "Numbat", "Wombat", "Diable de Tasmanie", "Quokka", "Wallaby", "Opossum", "Sarigue", "Kinkajou", "Coati",
"Porc-épic", "Chinchilla", "Cobaye", "Agouti", "Paca", "Viscache", "Octodon", "Gerbille", "Lérot", "Loir",
"Cachalot", "Narval", "Béluga", "Marsouin", "Dugong", "Lamantin", "Éléphant de mer", "Lion de mer", "Léopard de mer", "Rorqual",
"Thon rouge", "Barracuda", "Piranha", "Murène", "Raie manta", "Poisson-clown", "Poisson-perroquet", "Poisson-chirurgien", "Rascasse", "Diable de mer",
"Méduse", "Anémone de mer", "Corail", "Étoile de mer", "Oursin", "Concombre de mer", "Bernard-l'ermite", "Cloporte", "Mille-pattes", "Scolopendre",
"Scorpion", "Araignée", "Tarentule", "Mygale", "Veuve noire", "Tique", "Puce", "Pou", "Punaise", "Cafard",
"Termite", "Fourmi", "Abeille", "Bourdon", "Guêpe", "Frelon", "Libellule", "Demoiselle", "Éphémère", "Mante religieuse",
"Sauterelle", "Criquet", "Grillon", "Cigale", "Puceron", "Coccinelle", "Scarabée", "Carabe", "Hanneton", "Lucane",
"Papillon", "Chenille", "Chrysalide", "Sphinx", "Monarque", "Machaon", "Vulcain", "Paon du jour", "Citron", "Aurore",
"Moustique", "Mouche", "Taon", "Tipule", "Moucheron", "Phrygane", "Perce-oreille", "Thrips", "Charançon", "Doryphore",

// Pays supplémentaires
"Belize", "Salvador", "Barbade", "Trinité-et-Tobago", "Bahamas", "Grenade", "Sainte-Lucie", "Dominique", "Saint-Vincent", "Antigua",
"Guyana", "Suriname", "Guyane française", "Kirribati", "Tuvalu", "Nauru", "Palau", "Micronésie", "Vanuatu", "Salomon",
"Comores", "Seychelles", "Maurice", "Maldives", "Cap-Vert", "Sao Tomé", "Guinée équatoriale", "Bénin", "Togo", "Burkina Faso",
"Guinée", "Guinée-Bissau", "Sierra Leone", "Liberia", "Gambie", "Érythrée", "Djibouti", "Lesotho", "Swaziland", "Centrafrique",
"Liechenstein", "Monaco", "Andorre", "Vatican", "Saint-Marin", "Malte", "Chypre", "Bhoutan", "Brunei", "Timor oriental",
"Laos", "Sri Lanka", "Tadjikistan", "Turkménistan", "Bahreïn", "Palestine", "Mauritanie", "Érythrée", "Soudan du Sud", "Sahara occidental",

// Villes supplémentaires
"Zurich", "Lausanne", "Bâle", "Berne", "Lucerne", "Porto", "Cracovie", "Gdansk", "Bratislava", "Ljubljana",
"Zagreb", "Belgrade", "Sofia", "Minsk", "Kiev", "Riga", "Tallinn", "Vilnius", "Reykjavik", "Tbilissi",
"Bakou", "Tachkent", "Almaty", "Astana", "Bichkek", "Douchanbé", "Achgabat", "Oulan-Bator", "Katmandou", "Thimphou",
"Dacca", "Islamabad", "Kaboul", "Téhéran", "Bagdad", "Damas", "Beyrouth", "Amman", "Jérusalem", "Tel-Aviv",
"Riyad", "Abou Dhabi", "Doha", "Mascate", "Sanaa", "Alexandrie", "Casablanca", "Tunis", "Tripoli", "Alger",
"Rabat", "Tanger", "Marrakech", "Fès", "Dakar", "Abidjan", "Accra", "Lagos", "Kinshasa", "Luanda",
"Nairobi", "Dar es Salaam", "Kampala", "Kigali", "Addis-Abeba", "Mogadiscio", "Khartoum", "Pretoria", "Le Cap", "Durban",
"Melbourne", "Brisbane", "Perth", "Adélaïde", "Canberra", "Wellington", "Auckland", "Christchurch", "Vancouver", "Toronto",
"Ottawa", "Québec", "Calgary", "Edmonton", "Winnipeg", "Halifax", "Sao Paulo", "Brasilia", "Salvador", "Fortaleza",
"Belo Horizonte", "Curitiba", "Recife", "Manaus", "Belém", "Porto Alegre", "Guadalajara", "Monterrey", "Puebla", "Tijuana",
"Medellin", "Cali", "Quito", "Guayaquil", "La Paz", "Santa Cruz", "Montevideo", "Asuncion", "San José", "Panama City",
"La Havane", "Kingston", "Port-au-Prince", "Saint-Domingue", "San Juan", "San Salvador", "Tegucigalpa", "Managua", "Belize City", "Guatemala City",
"Guangzhou", "Shenzhen", "Chengdu", "Wuhan", "Chongqing", "Tianjin", "Hangzhou", "Nanjing", "Xi'an", "Suzhou",
"Mumbai", "Delhi", "Bangalore", "Hyderabad", "Chennai", "Kolkata", "Pune", "Ahmedabad", "Jaipur", "Lucknow",
"Manille", "Quezon City", "Jakarta", "Surabaya", "Bandung", "Medan", "Kuala Lumpur", "Penang", "Hanoï", "Hô Chi Minh-Ville",
"Phnom Penh", "Vientiane", "Yangon", "Naypyidaw", "Karachi", "Lahore", "Faisalabad", "Rawalpindi", "Peshawar", "Multan",

// Objets du quotidien supplémentaires
"Réfrigérateur", "Congélateur", "Four", "Micro-ondes", "Lave-vaisselle", "Lave-linge", "Sèche-linge", "Aspirateur", "Fer à repasser", "Cafetière",
"Bouilloire", "Grille-pain", "Mixeur", "Blender", "Robot", "Centrifugeuse", "Presse-agrumes", "Balance", "Thermomètre", "Minuteur",
"Casserole", "Poêle", "Marmite", "Cocotte", "Wok", "Sauteuse", "Faitout", "Plat", "Saladier", "Passoire",
"Fouet", "Spatule", "Louche", "Écumoire", "Couteau", "Fourchette", "Cuillère", "Assiette", "Bol", "Tasse",
"Verre", "Carafe", "Théière", "Cafetière", "Sucrier", "Beurrier", "Salière", "Poivrière", "Huilier", "Vinaigrier",
"Coussin", "Oreiller", "Couverture", "Drap", "Édredon", "Couette", "Traversin", "Plaid", "Tapis", "Rideau",
"Store", "Volet", "Persienne", "Portière", "Tenture", "Tableau", "Cadre", "Poster", "Affiche", "Photographie",
"Vase", "Pot", "Jardinière", "Cache-pot", "Cendrier", "Bougeoir", "Chandelier", "Lanterne", "Bougie", "Encens",
"Brosse", "Peigne", "Sèche-cheveux", "Lisseur", "Fer à friser", "Rasoir", "Tondeuse", "Coupe-ongles", "Lime", "Pince à épiler",
"Dentifrice", "Brosse à dents", "Fil dentaire", "Bain de bouche", "Savon", "Shampooing", "Gel douche", "Déodorant", "Parfum", "Crème",

// Vêtements supplémentaires
"Smoking", "Costume", "Tailleur", "Ensemble", "Survêtement", "Jogging", "Legging", "Tregging", "Salopette", "Tablier",
"Poncho", "Cape", "Châle", "Étole", "Foulard", "Bandana", "Casquette", "Béret", "Bonnet", "Cagoule",
"Bandeau", "Serre-tête", "Barrette", "Broche", "Épingle", "Bouton de manchette", "Cravate", "Nœud papillon", "Ceinture", "Bretelle",
"Bague", "Collier", "Bracelet", "Gourmette", "Chaîne", "Pendentif", "Médaille", "Boucle d'oreille", "Anneau", "Piercing",
"Mitaine", "Moufle", "Guêtre", "Jambière", "Manchette", "Genouillère", "Coudière", "Protège-tibias", "Attelle", "Bandage",

// Sports et Loisirs supplémentaires
"Badminton", "Squash", "Ping-pong", "Baseball", "Softball", "Cricket", "Polo", "Water-polo", "Beach-volley", "Pétanque",
"Boules", "Billard", "Snooker", "Fléchettes", "Bowling", "Curling", "Biathlon", "Décathlon", "Heptathlon", "Pentathlon",
"Saut en hauteur", "Saut en longueur", "Triple saut", "Saut à la perche", "Lancer du poids", "Lancer du disque", "Lancer du javelot", "Lancer du marteau", "Sprint", "Relais",
"Haies", "Steeple", "Demi-fond", "Fond", "Cross-country", "Trail", "Ultra-trail", "Marche athlétique", "Marche nordique", "Jogging",
"Cardio", "Musculation", "Fitness", "Crossfit", "Zumba", "Aérobic", "Step", "Spinning", "Body-combat", "Body-pump",
"Aquagym", "Aquabike", "Hydrospeed", "Rafting", "Canyoning", "Via ferrata", "Parapente", "Deltaplane", "Planeur", "Montgolfière",
"Parachutisme", "Saut à l'élastique", "Tyrolienne", "Accrobranche", "Paintball", "Laser game", "Airsoft", "Kart", "Quad", "Moto-cross",
"BMX", "VTT", "Trial", "Roller", "Skateboard", "Longboard", "Waveboard", "Hoverboard", "Segway", "Gyropode",

// Couleurs supplémentaires
"Vermillon", "Cramoisi", "Grenat", "Rubis", "Carmin", "Cerise", "Framboise", "Fuchsia", "Mauve", "Lilas",
"Prune", "Aubergine", "Améthyste", "Pervenche", "Bleu marine", "Bleu roi", "Bleu ciel", "Azur", "Cobalt", "Saphir",
"Émeraude", "Jade", "Olive", "Kaki", "Chartreuse", "Citron", "Or", "Ambre", "Ocre", "Terre de Sienne",
"Rouille", "Brique", "Terracotta", "Saumon", "Corail", "Pêche", "Abricot", "Crème", "Ivoire", "Perle",
"Argent", "Platine", "Acier", "Ardoise", "Anthracite", "Charbon", "Jais", "Ébène", "Sépia", "Taupe",

// Transports supplémentaires
"Triporteur", "Pousse-pousse", "Rickshaw", "Calèche", "Diligence", "Cabriolet", "Berline", "Limousine", "Coupé", "Break",
"Monospace", "SUV", "Pick-up", "Camping-car", "Caravane", "Remorque", "Semi-remorque", "Poids lourd", "Fourgon", "Fourgonnette",
"Autobus", "Autocar", "Trolleybus", "Téléphérique", "Funiculaire", "Remonte-pente", "Télésiège", "Télécabine", "Gondole", "Nacelle",
"Locomotive", "Wagon", "Rame", "TGV", "TER", "RER", "Intercité", "Eurostar", "Thalys", "Shinkansen",
"Tramway", "Monorail", "Métro", "Omnibus", "Express", "Rapide", "Cargo", "Paquebot", "Transatlantique", "Croisière",
"Voilier", "Catamaran", "Trimaran", "Goélette", "Péniche", "Barge", "Chaland", "Gabarre", "Vedette", "Canot",
"Chaloupe", "Barque", "Pirogue", "Kayak", "Canoë", "Radeau", "Pédalo", "Jet-ski", "Scooter des mers", "Planche à voile",
"Kitesurf", "Planche de surf", "Bodyboard", "Paddle", "Aviron", "Dériveur", "Optimist", "Laser", "Cargo", "Pétrolier",
"Porte-conteneurs", "Brise-glace", "Remorqueur", "Chalutier", "Thonier", "Baleinier", "Dragueur", "Hydravion", "Planeur", "ULM",
"Hélicoptère", "Autogire", "Drone", "Dirigeable", "Ballon", "Fusée", "Navette spatiale", "Satellite", "Sonde", "Rover",

// Instruments de musique
"Piano", "Guitare", "Violon", "Violoncelle", "Contrebasse", "Alto", "Harpe", "Flûte", "Clarinette", "Hautbois",
"Basson", "Cor", "Trompette", "Trombone", "Tuba", "Saxophone", "Accordéon", "Harmonica", "Orgue", "Clavecin",
"Batterie", "Tambour", "Cymbale", "Xylophone", "Marimba", "Vibraphone", "Glockenspiel", "Triangle", "Castagnettes", "Maracas",
"Tambourin", "Djembé", "Bongo", "Conga", "Timbales", "Gong", "Cloche", "Carillon", "Lyre", "Mandoline",
"Banjo", "Ukulélé", "Sitar", "Balalaïka", "Luth", "Cithare", "Cornemuse", "Didgeridoo", "Ocarina", "Kazoo",

// Matières scolaires
"Mathématiques", "Français", "Anglais", "Espagnol", "Allemand", "Italien", "Histoire", "Géographie", "Sciences", "Physique",
"Chimie", "Biologie", "Géologie", "Astronomie", "Informatique", "Technologie", "Philosophie", "Économie", "Éducation civique", "Arts plastiques",
"Musique", "Théâtre", "Éducation physique", "Sport", "Latin", "Grec", "Littérature", "Grammaire", "Orthographe", "Conjugaison",

// Phénomènes naturels
"Pluie", "Neige", "Grêle", "Verglas", "Givre", "Rosée", "Brouillard", "Brume", "Nuage", "Orage",
"Éclair", "Foudre", "Tonnerre", "Arc-en-ciel", "Aurore boréale", "Vent", "Brise", "Tempête", "Ouragan", "Cyclone",
"Typhon", "Tornade", "Trombe", "Mistral", "Tramontane", "Sirocco", "Harmattan", "Mousson", "Alizé", "Tsunami",
"Raz-de-marée", "Séisme", "Tremblement de terre", "Éruption", "Volcan", "Lave", "Magma", "Geyser", "Avalanche", "Éboulement",
"Glissement de terrain", "Inondation", "Crue", "Sécheresse", "Canicule", "Vague de froid", "Gel", "Dégel", "Marée", "Courant",

// Émotions et sentiments
"Joie", "Bonheur", "Gaieté", "Allégresse", "Euphorie", "Extase", "Ravissement", "Enchantement", "Enthousiasme", "Excitation",
"Tristesse", "Chagrin", "Peine", "Mélancolie", "Nostalgie", "Cafard", "Déprime", "Désespoir", "Angoisse", "Anxiété",
"Peur", "Crainte", "Frayeur", "Terreur", "Effroi", "Épouvante", "Panique", "Horreur", "Colère", "Rage",
"Fureur", "Irritation", "Agacement", "Exaspération", "Amour", "Affection", "Tendresse", "Passion", "Adoration", "Dévotion",
"Haine", "Aversion", "Répulsion", "Dégoût", "Mépris", "Jalousie", "Envie", "Convoitise", "Fierté", "Orgueil",
"Humilité", "Modestie", "Honte", "Gêne", "Embarras", "Confusion", "Surprise", "Étonnement", "Stupéfaction", "Admiration",

// Formes géométriques
"Cercle", "Carré", "Triangle", "Rectangle", "Losange", "Trapèze", "Parallélogramme", "Pentagone", "Hexagone", "Heptagone",
"Octogone", "Décagone", "Polygone", "Ellipse", "Ovale", "Sphère", "Cube", "Pyramide", "Prisme", "Cylindre",
"Cône", "Tétraèdre", "Dodécaèdre", "Icosaèdre", "Tore", "Spirale", "Hélice", "Étoile", "Croissant", "Arc", "Segment",

// Parties du corps
"Tête", "Cerveau", "Crâne", "Cheveu", "Front", "Sourcil", "Œil", "Paupière", "Cil", "Pupille",
"Iris", "Nez", "Narine", "Bouche", "Lèvre", "Dent", "Gencive", "Langue", "Palais", "Joue",
"Menton", "Mâchoire", "Oreille", "Lobe", "Cou", "Nuque", "Gorge", "Larynx", "Trachée", "Épaule",
"Bras", "Coude", "Avant-bras", "Poignet", "Main", "Paume", "Doigt", "Pouce", "Index", "Majeur",
"Annulaire", "Auriculaire", "Ongle", "Torse", "Poitrine", "Sein", "Ventre", "Dos", "Colonne vertébrale", "Hanche", "Fesse", "Cuisse", "Genou",
"Jambe", "Mollet", "Cheville", "Pied", "Plante", "Talons", "Orteil", "Cœur", "Poumon", "Foie",
"Estomac", "Intestin", "Rein", "Vessie", "Cerveau", "Muscle", "Os", "Articulation", "Veine", "Artère",
"Nerf", "Peau", "Poil", "Sang", "Lymphe", "Cellule", "ADN", "Gène", "Chromosome"

};

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
        "\n\n\n\n\n\n\n",                                    // 0 erreur : rien
        "\n\n\n\n\n\n____\n",                               // 1 erreur : base
        " |\n |\n |\n |\n |\n_|___\n",                     // 2 erreurs : potence
        " _______\n |/      |\n |\n |\n |\n_|___\n",        // 3 erreurs : potence complète
        " _______\n |/      |\n |      (_)\n |\n |\n_|___\n", // 4 erreurs : tête
        " _______\n |/      |\n |      (_)\n |      /|\\\n |\n_|___\n", // 5 erreurs : corps
        " _______\n |/      |\n |      (_)\n |      /|\\\n |      / \\\n_|___\n" // 6 erreurs : pendu complet
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

            // Demander au joueur de proposer une lettre
            Console.Write("Proposez une lettre : ");
            string? saisieInput = Console.ReadLine();
            string saisie = saisieInput?.ToUpperInvariant() ?? "";

            // ==================== VALIDATION DE LA SAISIE ====================
            // Vérifier que la saisie est valide (une seule lettre)
            if (string.IsNullOrWhiteSpace(saisie) || saisie.Length != 1 || !char.IsLetter(saisie[0]))
            {
                Console.WriteLine("Veuillez entrer une seule lettre.");
                Console.ReadKey(); // Attendre que le joueur appuie sur une touche
                continue; // Recommencer la boucle
            }

            char lettre = saisie[0]; // Extraire la lettre saisie

            // Vérifier si la lettre a déjà été essayée
            if (lettresEssayees.Contains(lettre))
            {
                Console.WriteLine("Vous avez déjà essayé cette lettre.");
                Console.ReadKey(); // Attendre que le joueur appuie sur une touche
                continue; // Recommencer la boucle
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
```
