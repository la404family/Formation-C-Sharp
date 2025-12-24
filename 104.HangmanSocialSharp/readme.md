<p align="center">
  <img src="https://img.shields.io/badge/COOP-Multijoueur-blue?style=for-the-badge" alt="Multijoueur">
</p>

<h1 align="center">🔤 Hangman Social Sharp</h1>

<p align="center">
  <strong>Le Jeu du Pendu Multijoueur en Temps Réel avec WPF et SignalR</strong>
</p>

<p align="center">
  <img src="https://img.shields.io/badge/.NET-9.0-512BD4?style=for-the-badge&logo=dotnet" alt=".NET 9.0">
  <img src="https://img.shields.io/badge/WPF-Windows-0078D6?style=for-the-badge&logo=windows" alt="WPF">
  <img src="https://img.shields.io/badge/SignalR-RealTime-1ec8ee?style=for-the-badge&logo=signalr" alt="SignalR">
  <img src="https://img.shields.io/badge/EF%20Core-Database-success?style=for-the-badge&logo=nuget" alt="EF Core">
</p>

---

## 📖 Description

**HangmanSocialSharp** est une évolution moderne du jeu du pendu classique. Contrairement à la version console solo, ce projet introduit une dimension **sociale et temps réel**.

Les joueurs peuvent se connecter, rejoindre des parties (Lobby), et deviner le mot ensemble ou s'affronter, tout en voyant la progression des autres en direct grâce à la technologie **SignalR**. L'interface est réalisée en **WPF** pour une expérience utilisateur riche sous Windows.

---

## 🚀 Démarrage Rapide

Ce projet est composé de deux parties principales : le **Serveur** (API) et le **Client** (WPF). Il faut lancer les deux pour jouer.

### Prérequis
*   **.NET 9.0 SDK** installé
*   **SQL Server** (ou LocalDB) pour la base de données

### 1️⃣ Lancer le Serveur (Backend)
1.  Ouvrez un terminal dans le dossier `2. Backend/HangmanSocial.API/`
2.  Lancez la commande : `dotnet run`
3.  Le serveur démarrera (par défaut sur `https://localhost:5001` ou `http://localhost:5000`)

### 2️⃣ Lancer le Client (Frontend)
1.  Ouvrez un terminal dans le dossier `3. Frontend/HangmanSocial.Client/`
2.  Lancez la commande : `dotnet run`
3.  Connectez-vous ou créez un compte pour jouer !

---

## 🛠️ Architecture Technique

Le projet suit une architecture **N-Tier** stricte pour séparer les responsabilités.

### 📂 Structure des Dossiers

```
104.HangmanSocialSharp/
├── HangmanSocial.sln                     # Le fichier solution global
│
├── 1. Common/                            # COUCHE PARTAGÉE
│   └── HangmanSocial.Shared/             # Class Library (.NET Standard/Core)
│       ├── DTOs/
│       │   ├── Auth/
│       │   │   ├── RegisterRequest.cs
│       │   │   ├── LoginRequest.cs
│       │   │   └── AuthResponse.cs       # Token JWT + Refresh Token
│       │   ├── Game/
│       │   │   ├── GameStateDto.cs       # État (ex: "_ A _ _", Vies: 5)
│       │   │   └── GameResultDto.cs
│       │   └── User/
│       │       ├── UserProfileDto.cs
│       │       └── UserStatsDto.cs
│       ├── Enums/
│       │   ├── GameDifficulty.cs
│       │   └── GameStatus.cs             # Waiting, Playing, Won, Lost
│       └── Validators/                   # NOUVEAU : Règles de validation (FluentValidation)
│           ├── RegisterRequestValidator.cs
│           └── LoginRequestValidator.cs
│
├── 2. Backend/                           # CÔTÉ SERVEUR
│   ├── HangmanSocial.Database/           # Class Library (Data Access Layer)
│   │   ├── Data/
│   │   │   ├── HangmanDbContext.cs
│   │   │   └── DbInitializer.cs          # Pour créer les données de base (Seed)
│   │   ├── Entities/
│   │   │   ├── User.cs
│   │   │   ├── GameSession.cs
│   │   │   ├── GameRound.cs              # Détail d'une manche
│   │   │   └── Word.cs
│   │   └── Migrations/                   # Fichiers générés par EF Core
│   │
│   └── HangmanSocial.API/                # ASP.NET Core Web API
│       ├── Controllers/
│       │   ├── AuthController.cs
│       │   ├── UsersController.cs
│       │   └── GameController.cs         # Pour l'historique et le fallback
│       ├── Hubs/
│       │   └── GameHub.cs                # SignalR : Le cœur du temps réel
│       ├── Services/                     # Logique Métier
│       │   ├── Implementations/
│       │   │   ├── AuthService.cs
│       │   │   ├── GameService.cs        # Logique pure du jeu (règles)
│       │   │   └── StatsService.cs
│       │   └── Interfaces/
│       │       ├── IAuthService.cs
│       │       └── IGameService.cs
│       ├── Mappings/                     # NOUVEAU : AutoMapper
│       │   └── AutoMapperProfile.cs      # Entity -> DTO
│       ├── Middleware/                   # NOUVEAU : Gestion globale
│       │   └── ExceptionMiddleware.cs    # Gestion propre des erreurs (Try/Catch global)
│       ├── Program.cs
│       └── appsettings.json
│
├── 3. Frontend/                          # CÔTÉ CLIENT (WPF)
│   └── HangmanSocial.Client/
│       ├── App.xaml.cs                   # DI Container (Injection de dépendances)
│       ├── Assets/
│       │   ├── Images/
│       │   ├── Fonts/
│       │   └── Styles/                   # ResourceDictionaries
│       │       ├── Colors.xaml
│       │       ├── Controls.xaml
│       │       └── Text.xaml
│       ├── Components/                   # UserControls
│       │   ├── HangmanDrawing.xaml
│       │   ├── VirtualKeyboard.xaml
│       │   ├── ChatBox.xaml
│       │   └── Loader.xaml               # Spinner de chargement
│       ├── Converters/
│       │   ├── BoolToVisibilityConverter.cs
│       │   └── InverseBoolConverter.cs
│       ├── Infrastructure/               # NOUVEAU : Outils techniques
│       │   ├── Behaviors/                # Ex: AutoScrollBehavior (Chat)
│       │   └── Extensions/               # Ex: StringExtensions, ObservableExtensions
│       ├── Services/                     # Communication API/SignalR
│       │   ├── Api/
│       │   │   ├── AuthApiService.cs
│       │   │   ├── UserApiService.cs
│       │   ├── RealTime/
│       │   │   └── SignalRService.cs     # Gestion connexion/déconnexion Hub
│       │   └── Navigation/
│       │       └── NavigationService.cs
│       ├── Stores/                       # Gestion d'État (State Management)
│       │   ├── MainStore.cs              # État global (Loading, Error)
│       │   ├── UserStore.cs              # Utilisateur connecté
│       │   └── GameStore.cs              # Partie en cours
│       ├── ViewModels/
│       │   ├── Base/
│       │   │   └── ViewModelBase.cs      # Implémente INotifyPropertyChanged
│       │   ├── Auth/
│       │   │   ├── LoginViewModel.cs
│       │   │   └── RegisterViewModel.cs
│       │   ├── Dashboard/
│       │   │   └── HomeViewModel.cs
│       │   └── Game/
│       │       ├── GameViewModel.cs
│       │       └── LobbyViewModel.cs
│       └── Views/
│           ├── Windows/
│           │   └── MainWindow.xaml
│           └── Pages/
│               ├── LoginPage.xaml
│               ├── HomePage.xaml
│               └── GamePage.xaml
│
└── 4. Tests/                             # ASSURANCE QUALITÉ
    └── HangmanSocial.Tests/              # xUnit
        ├── API/
        │   └── GameServiceTests.cs       # Test unitaire de la logique du pendu
        └── Shared/
            └── ValidatorTests.cs         # Vérifie que les validateurs marchent
```

---

## 🔍 Détails et Responsabilités des Fichiers

Cette section détaille le contenu attendu pour chaque fichier clé du projet, afin d'aider un développeur débutant à comprendre **"quoi mettre où"**.

### 1. Common (Partagé)
Ce projet est une bibliothèque de classes (`.dll`) référencée par le Backend **et** le Frontend.
> **Pourquoi ?** Pour éviter de copier-coller les mêmes classes (ex: `RegisterRequest`) dans les deux projets. Si on change une propriété, tout le monde est à jour.

-   **`DTOs/Auth/RegisterRequest.cs`** :  
    Contient simplement les propriétés nécessaires à l'inscription : `Email`, `Username`, `Password`, `ConfirmPassword`. C'est une classe "anémique" (juste des données, pas de logique).
-   **`DTOs/Game/GameStateDto.cs`** :  
    Représente l'état visuel du jeu à un instant T. Doit contenir : Le mot masqué (ex: `"_ O _ _"`), le nombre de vies restantes, les lettres déjà proposées, et le statut actuel (En cours, Gagné, Perdu).
-   **`Validators/RegisterRequestValidator.cs`** :  
    Utilise la librairie *FluentValidation*. Contient les règles : L'email doit être valide, le post est requis, le mot de passe doit faire au moins 8 caractères, etc.

### 2. Backend (HangmanSocial.API)
C'est le cerveau de l'application. Il reçoit les demandes, traite la logique, et parle à la base de données.

-   **`Entities/User.cs`** :  
    La classe qui reflète la table `Users` dans la base de données. Elle contient `Id`, `Username`, `PasswordHash`, `CreatedAt`, etc. C'est l'objet "réel" stocké.
-   **`Data/HangmanDbContext.cs`** :  
    Hérite de `DbContext` (Entity Framework). C'est ici qu'on configure les tables (`DbSet<User>`, `DbSet<GameSession>`) et les relations (ex: Un joueur a plusieurs parties).
-   **`Controllers/AuthController.cs`** :  
    Point d'entrée pour l'authentification (HTTP POST). Reçoit `RegisterRequest`, appelle `AuthService`, et retourne un `AuthResponse` (avec le Token) ou une erreur 400.
-   **`Hubs/GameHub.cs`** :  
    Classe spécifique à *SignalR*. Permet la communication temps réel. Contient des méthodes comme `JoinGame(gameId)` ou `SendGuess(letter)`. Contrairement aux contrôleurs, il garde une connexion ouverte avec le joueur.
-   **`Services/Implementations/GameService.cs`** :  
    Contient la **Logique Métier**. C'est lui qui sait que "Si je propose 'A' et que le mot est 'CHAT', alors je révèle les 'A'". Il ne s'occupe pas de HTTP ni de JSON, juste des règles du jeu.
-   **`Mappings/AutoMapperProfile.cs`** :  
    Configure comment transformer une `Entity` (Base de données) en `DTO` (Objet de transport). Ex: `CreateMap<User, UserProfileDto>();`.

### 3. Frontend (HangmanSocial.Client - WPF)
L'interface utilisateur. Elle utilise le pattern **MVVM** (Model-View-ViewModel).
> **Règle d'or MVVM** : La Vue (`.xaml`) ne connaît **QUE** le ViewModel. Le ViewModel ne connaît **PAS** la Vue (pas de textbox, pas de bouton).

-   **`Api/AuthApiService.cs`** :  
    Utilise `HttpClient` pour envoyer les requêtes POST vers `https://localhost:5000/api/auth`. Gère la sérialisation JSON.
-   **`RealTime/SignalRService.cs`** :  
    Gère la connexion `HubConnection` avec le serveur. Il écoute les événements ("PlayerJoined", "GameUpdated") et notifie les ViewModels via des `Action` ou des événements C#.
-   **`Stores/Types/UserStore.cs`** :  
    Un singleton (classe unique) qui garde en mémoire "Qui est connecté". Si `UserStore.CurrentUser` est null, on redirige vers le Login.
-   **`ViewModels/GameViewModel.cs`** :  
    Le chef d'orchestre de l'écran de jeu.
    *   Propriétés : `WordDisplay` (string), `Lives` (int), `GuessedLetters` (ObservableCollection).
    *   Commandes : `GuessLetterCommand` (exécutée quand on clique sur une lettre).
    *   Logique : Quand `SignalRService` reçoit un nouvel état de jeu, ce ViewModel met à jour ses propriétés, et la Vue se rafraîchit toute seule grâce au DataBinding.
-   **`Views/Pages/GamePage.xaml`** :  
    Le design pur. Contient le XAML (Grids, Buttons, TextBlocks). Utilise `{Binding WordDisplay}` pour afficher ce que le ViewModel lui donne.
-   **`Components/HangmanDrawing.xaml`** :  
    Un composant réutilisable qui dessine le pendu. Il reçoit juste un nombre (ex: `Lives = 3`) et affiche le bon dessin (tête + corps + un bras).

### 4. Tests
-   **`GameServiceTests.cs`** :  
    Vérifie que la logique du jeu est infaillible.
    *   *Exemple de test* : "Si le mot est 'TEST' et que je propose 'E', la méthode doit retourner un état avec 'E' révélé et ne pas décrémenter les vies."

---

## 👥 Crédits

Développé dans le cadre de la **Formation C#**.
> **Apprentissage** : Ce projet met en œuvre les concepts avancés (Web API, SignalR, WPF, MVVM, Entity Framework) pour créer une application complète.