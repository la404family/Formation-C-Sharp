# Les bases de la programmation en C-Sharp

## Installation de l'environnement de développement

Pour commencer à programmer en C-Sharp, vous devez installer un environnement de développement intégré (IDE). Le plus populaire pour C-Sharp est Visual Studio. Vous pouvez le télécharger depuis le site officiel de Microsoft.
Vous pouvez également utiliser Visual Studio Code (VScode) avec l'extension C# si vous préférez un éditeur plus léger.

### Utilisation de Visual Studio

1. Rendez-vous sur [le site de Visual Studio](https://visualstudio.microsoft.com/).
2. Téléchargez la version Community (gratuite).
3. Suivez les instructions d'installation et assurez-vous de sélectionner le workload ".NET desktop development" lors de l'installation.

### Utilisation de VScode

1. Téléchargez et installez [Visual Studio Code](https://code.visualstudio.com/).
2. Ouvrez Visual Studio Code et allez dans l'onglet Extensions.
3. Recherchez et installez l'extension "C#".
4. Assurez-vous d'avoir le SDK .NET installé sur votre machine. Vous pouvez le télécharger depuis [le site officiel de .NET](https://dotnet.microsoft.com/download).

#### Les extensions recommandées pour VScode

- C# Dev Kit
- C# Extensions

## Qu'est-ce que le C# et l'écosystème .NET ?

### Vue d'ensemble

| Terme            | Description                                                     |
| ---------------- | --------------------------------------------------------------- |
| **C#**           | Le langage de programmation orienté objet créé par Microsoft    |
| **.NET**         | La plateforme d'exécution (runtime) qui fait tourner le code C# |
| **ASP.NET Core** | Le framework pour créer des applications web                    |

### 🎯 C# - Le langage

**C#** (prononcé "C-Sharp") est un langage de programmation moderne, orienté objet, créé par Microsoft en 2000. Il combine la puissance du C++ avec la simplicité de langages comme Java.

**Caractéristiques principales :**

- Typage fort et statique (détection d'erreurs à la compilation)
- Gestion automatique de la mémoire (Garbage Collector)
- Syntaxe claire et lisible
- Support de la programmation orientée objet, fonctionnelle et asynchrone

### 🖥️ .NET - La plateforme

**.NET** est l'environnement d'exécution qui permet de faire tourner les applications C#. Il fournit les bibliothèques de base, le compilateur et le runtime.

**Historique simplifié :**
| Version | Période | Caractéristique |
| ------- | ------- | --------------- |
| **.NET Framework** | 2002-2019 | Windows uniquement (legacy) |
| **.NET Core** | 2016-2020 | Multiplateforme, open-source |
| **.NET 5/6/7/8+** | 2020+ | Unification, version moderne à utiliser |

> 💡 **Aujourd'hui**, utilisez simplement **.NET 8** (ou la dernière version LTS). Les termes "Core" et "Framework" sont historiques.

### 🌐 Ce que vous pouvez créer avec C# et .NET

| Type d'application                | Technologies                  | Exemples                                 |
| --------------------------------- | ----------------------------- | ---------------------------------------- |
| **Applications Console**          | .NET                          | Scripts, outils CLI, automatisation      |
| **Applications Desktop**          | WPF, WinForms, MAUI           | Logiciels Windows, apps multiplateformes |
| **Sites Web**                     | ASP.NET Core MVC, Razor Pages | Sites vitrines, e-commerce, portails     |
| **API REST**                      | ASP.NET Core Web API          | Backend pour apps mobiles/web            |
| **Applications Web interactives** | Blazor                        | SPA sans JavaScript (ou presque)         |
| **Jeux vidéo**                    | Unity                         | Jeux 2D/3D sur PC, consoles, mobile      |
| **Applications mobiles**          | .NET MAUI, Xamarin            | Apps iOS et Android                      |
| **Microservices**                 | ASP.NET Core, gRPC            | Architecture distribuée                  |
| **Cloud & Serverless**            | Azure Functions               | Fonctions cloud événementielles          |

### 🔧 ASP.NET Core - Le framework web

**ASP.NET Core** est le framework moderne pour créer des applications web avec C#.

**Il permet de créer :**

- **Web API** : Services REST pour alimenter des applications frontend (React, Angular, Vue, mobile)
- **MVC** : Sites web avec le pattern Model-View-Controller
- **Razor Pages** : Pages web simplifiées (idéal pour débuter)
- **Blazor** : Applications web interactives en C# (alternative à JavaScript)
- **SignalR** : Communication temps réel (chat, notifications)
