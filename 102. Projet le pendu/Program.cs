﻿// Titre de la console : Le Pendu
Console.Title = "Le Pendu";
// Remplacer les lettres par des ascii art

strint asciiA = @"
 █████╗
██╔══██╗
███████║ 
██╔══██║ 
██║  ██║ 
╚═╝  ╚═╝
";
string asciiB = @"
██████╗ 
██╔══██╗
██████╔╝
██╔══██╗
██████╔╝
╚═════╝
";
string asciiC = @"
 ██████╗
██╔════╝
██║     
██║     
╚██████╗
 ╚═════╝
";
string asciiD = @"
██████╗ 
██╔══██╗
██║  ██║
██║  ██║
██████╔╝
╚═════╝ 
";
string asciiE = @"
███████╗
██╔════╝
█████╗  
██╔══╝  
███████╗
╚══════╝
";
string asciiF = @"
███████╗
██╔════╝
█████╗  
██╔══╝  
██║     
╚═╝     
";
string asciiG = @"
 ██████╗ 
██╔════╝ 
██║  ███╗
██║   ██║
╚██████╔╝
 ╚═════╝ 
";
string asciiH = @"
██╗  ██╗
██║  ██║
███████║
██╔══██║
██║  ██║
╚═╝  ╚═╝
";
string asciiI = @"
██╗
██║
██║
██║
██║
╚═╝
";
string asciiJ = @"
     ██╗
     ██║
     ██║
██║  ██║
╚█████╔╝
 ╚════╝
";
string asciiK = @"
██╗  ██╗
██║ ██╔╝
█████╔╝ 
██╔═██╗ 
██║  ██╗
╚═╝  ╚═╝
";
string asciiL = @"
██╗
██║
██║
██║
███████╗
╚══════╝
";
string asciiM = @"
███╗   ███╗
████╗ ████║
██╔████╔██║
██║╚██╔╝██║
██║ ╚═╝ ██║
╚═╝     ╚═╝
";
string asciiN = @"
███╗   ██╗
████╗  ██║
██╔██╗ ██║
██║╚██╗██║
██║ ╚████║
╚═╝  ╚═══╝
";
string asciiO = @"
 ██████╗ 
██╔═══██╗
██║   ██║
██║   ██║
╚██████╔╝
 ╚═════╝ 
";
string asciiP = @"
██████╗
██╔══██╗
██████╔╝
██╔═══╝
██║
╚═╝
";
string asciiQ = @"
 ██████╗ 
██╔═══██╗
██║   ██║
██║▄▄ ██║
╚██████╔╝
 ╚══▀▀═╝ 
";
string asciiR = @"
██████╗ 
██╔══██╗
██████╔╝
██╔══██╗
██║  ██║
╚═╝  ╚═╝
";
string asciiS = @"
███████╗
██╔════╝
███████╗
╚════██║
███████║
╚══════╝
";
string asciiT = @"
████████╗
╚══██╔══╝
   ██║   
   ██║   
   ██║   
   ╚═╝
";
string asciiU = @"
██╗   ██╗
██║   ██║
██║   ██║
██║   ██║
╚██████╔╝
 ╚═════╝ 
";
string asciiV = @"
██╗   ██╗
██║   ██║
██║   ██║
╚██╗ ██╔╝
 ╚████╔╝ 
  ╚═══╝
";
string asciiW = @"
██╗    ██╗
██║    ██║
██║ █╗ ██║
██║███╗██║
╚███╔███╔╝
 ╚══╝╚══╝
";
string asciiX = @"
██╗  ██╗
╚██╗██╔╝
 ╚███╔╝ 
 ██╔██╗ 
██╔╝ ██╗
╚═╝  ╚═╝
";
string asciiY = @"
██╗   ██╗
╚██╗ ██╔╝
 ╚████╔╝ 
  ╚██╔╝  
   ██║   
   ╚═╝
";
string asciiZ = @"
███████╗
╚══███╔╝
  ███╔╝ 
 ███╔╝  
███████╗
╚══════╝
";

// Tableau des lettres ASCII
string asciiSpace = @"


espace



";
string asciiUnderscore = @"





██████╗
╚═════╝
";
string[] asciiLetters = new string[]
{
    asciiA, asciiB, asciiC, asciiD, asciiE, asciiF, asciiG,
    asciiH, asciiI, asciiJ, asciiK, asciiL, asciiM, asciiN,
    asciiO, asciiP, asciiQ, asciiR, asciiS, asciiT, asciiU,
    asciiV, asciiW, asciiX, asciiY, asciiZ, asciiSpace, asciiUnderscore
};
string[] aliments = new string[]
{
    "Pomme", "Banane", "Orange", "Raisin", "Fraise", "Cerise", "Mangue", "Ananas", "Melon", "Pastèque",
    "Tomate", "Carotte", "Poivron", "Concombre", "Courgette", "Aubergine", "Brocoli", "Chou", "Laitue", "Épinards",
    "Pommes de terre", "Oignon", "Ail", "Radis", "Betterave", "Navet", "Maïs", "Petit pois", "Haricot vert", "Champignon",
    "Pain", "Baguette", "Croissant", "Brioche", "Pain complet", "Pain de mie", "Céréales", "Riz", "Pâtes", "Semoule",
    "Lentilles", "Pois chiches", "Quinoa", "Soja", "Tofu", "Tempeh", "Viande", "Bœuf", "Poulet", "Porc",
    "Agneau", "Canard", "Jambon", "Saucisse", "Lardons", "Poisson", "Saumon", "Thon", "Cabillaud", "Sardine",
    "Crevette", "Crabe", "Homard", "Moules", "Huîtres", "Fromage", "Yaourt", "Lait", "Beurre", "Crème",
    "Œuf", "Chocolat", "Sucre", "Miel", "Confiture", "Compote", "Gâteau", "Tarte", "Crêpe", "Biscuits",
    "Glace", "Bonbon", "Jus de fruit", "Soda", "Eau", "Café", "Thé", "Vin", "Bière", "Huile",
    "Vinaigre", "Sel", "Poivre", "Épices", "Herbes", "Basilic", "Persil", "Coriandre", "Menthe", "Romarin",
    "Actuaire", "Agriculteur", "Aide-soignant", "Architecte", "Artisan",
    "Astronaute", "Avocat", "Barman", "Bibliothécaire", "Boulanger",
    "Capitaine", "Carreleur", "Charpentier", "Chauffeur", "Chirurgien",
    "Coiffeur", "Comptable", "Conducteur", "Couturier", "Cuisinier",
    "Dentiste", "Designer", "Développeur", "Diététicien", "Directeur",
    "Docteur", "Éboueur", "Électricien", "Enseignant", "Entrepreneur",
    "Épicier", "Esthéticienne", "Expert", "Facteur", "Ferronnier",
    "Fleuriste", "Forestier", "Géologue", "Graphiste", "Horloger",
    "Infirmier", "Ingénieur", "Instituteur", "Interprète", "Jardinier",
    "Journaliste", "Juge", "Libraire", "Livreur", "Maçon",
    "Magasinier", "Maître-nageur", "Mannequin", "Marin", "Mécanicien",
    "Menuisier", "Médecin", "Militaire", "Musicien", "Notaire",
    "Opticien", "Ouvrier", "Pâtissier", "Peintre", "Pédiatre",
    "Pharmacien", "Photographe", "Pilote", "Plombier", "Policier",
    "Pompier", "Professeur", "Programmeur", "Prothésiste", "Psychologue",
    "Puéricultrice", "Rédacteur", "Restaurateur", "Sapeur-pompier", "Secrétaire",
    "Serveur", "Soudeur", "Styliste", "Surveillant", "Tailleur",
    "Technicien", "Télévendeur", "Traducteur", "Urbaniste", "Vendeur",
    "Vétérinaire", "Webdesigner", "Zoologiste", "Ébéniste", "Éducateur",
    "Étancheur", "Garagiste", "Géomètre", "Horticulteur", "Imprimeur",
    "Juge d'instruction", "Kinésithérapeute", "Maître d'hôtel", "Mécanicien automobile", "Négociant",
    "chien", "chat", "lion", "tigre", "éléphant", "girafe", "zèbre", "cheval", "cerf", "sanglier",
    "loup", "renard", "ours", "lapin", "écureuil", "koala", "kangourou", "panda", "gorille", "chimpanzé",
    "hippopotame", "rhinocéros", "dauphin", "requin", "baleine", "poisson", "pieuvre", "crabe", "homard", "méduses",
    "perroquet", "canari", "hibou", "aigle", "faucon", "colibri", "cygne", "paon", "autruche", "pingouin",
    "serpent", "lézard", "crocodile", "caméléon", "gecko", "tortue", "grenouille", "salamandre", "escargot", "araignée",
    "fourmi", "abeille", "papillon", "mouche", "libellule", "coccinelle", "scarabée", "sauterelle", "ver de terre", "chenille",
    "hérisson", "taupe", "rat", "souris", "castor", "loutre", "bison", "antilopes", "chameau", "dromadaire",
    "chèvre", "mouton", "vache", "taureau", "cochon", "poule", "canard", "oie", "dindon", "cigogne",
    "furet", "hamster", "marmotte", "raton laveur", "opossum", "blaireau", "lémurien", "mandrill", "tatou", "puma",
    "jaguar", "panthère", "lynx", "chacal", "guépard", "hibiscus", "ornithorynque", "axolotl", "narval", "okapi",
    "pangolin", "France", "Allemagne", "Espagne", "Italie", "Portugal", "Belgique", "Pays-Bas", "Suisse", "Autriche", "Royaume-Uni",
    "Irlande", "Suède", "Norvège", "Danemark", "Finlande", "Islande", "Grèce", "Turquie", "Russie", "Ukraine",
    "États-Unis", "Canada", "Mexique", "Brésil", "Argentine", "Chili", "Colombie", "Pérou", "Venezuela", "Équateur",
    "Paraguay", "Uruguay", "Bolivie", "Costa Rica", "Panama", "Cuba", "Jamaïque", "Haïti", "République Dominicaine", "Guatemala",
    "Australie", "Nouvelle-Zélande", "Indonésie", "Malaisie", "Philippines", "Thaïlande", "Vietnam", "Singapour", "Japon", "Corée du Sud",
    "Inde", "Pakistan", "Bangladesh", "Sri Lanka", "Maldives", "Afghanistan", "Kazakhstan", "Ouzbékistan", "Turkménistan", "Iran",
    "Irak", "Syrie", "Liban", "Israël", "Jordanie", "Arabie Saoudite", "Égypte", "Libye", "Tunisie", "Maroc",
    "Algérie", "Soudan", "Somalie", "Éthiopie", "Kenya", "Ouganda", "Tanzanie", "Madagascar", "Afrique du Sud", "Nigeria",
    "Ghana", "Sénégal", "Mali", "Côte d'Ivoire", "Cameroun", "Angola", "Zambie", "Zimbabwe", "Namibie", "Botswana",
    "Chine", "Mongolie", "Taïwan", "Hong Kong", "Bhoutan", "Népal", "Arménie", "Azerbaïdjan", "Géorgie", "Chypre",
    "Paris", "Londres", "New York", "Tokyo", "Shanghai", "Pékin", "Moscou", "Berlin", "Rome", "Madrid",
    "Lisbonne", "Bruxelles", "Amsterdam", "Oslo", "Stockholm", "Helsinki", "Copenhague", "Dublin", "Athènes", "Vienne",
    "Zurich", "Prague", "Budapest", "Varsovie", "Istanbul", "Le Caire", "Casablanca", "Dakar", "Nairobi", "Johannesburg",
    "Lagos", "Abidjan", "Tunis", "Alger", "Rabat", "Doha", "Dubaï", "Riyad", "Téhéran", "Bagdad",
    "Kaboul", "Islamabad", "New Delhi", "Bombay", "Calcutta", "Bangalore", "Katmandou", "Bangkok", "Hanoï", "Manille",
    "Jakarta", "Singapour", "Séoul", "Hong Kong", "Taipei", "Canberra", "Sydney", "Melbourne", "Auckland", "Wellington",
    "Ottawa", "Toronto", "Montréal", "Vancouver", "Mexico", "Buenos Aires", "Rio de Janeiro", "São Paulo", "Lima", "Santiago",
    "Bogota", "Caracas", "Quito", "La Paz", "Panama", "San José", "La Havane", "Port-au-Prince", "Saint-Domingue", "Kingston",
    "Washington", "Chicago", "Los Angeles", "San Francisco", "Miami", "Houston", "Atlanta", "Boston", "Seattle", "Denver",
    "Phoenix", "Philadelphie", "Détroit", "Minneapolis", "Las Vegas", "Anchorage", "Honolulu", "Reykjavik", "Tallinn", "Vilnius"
};

