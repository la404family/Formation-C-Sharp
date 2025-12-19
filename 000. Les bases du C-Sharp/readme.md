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

**C#** (prononcé "Ci-Sharp") est un langage de programmation moderne, orienté objet, créé par Microsoft en 2000. Il combine la puissance du C++ avec la simplicité de langages comme Java.

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

> 💡 **Aujourd'hui**, utilisez simplement **.NET X** (la dernière version LTS). Les termes "Core" et "Framework" sont historiques.

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
| **IoT & Embedded**                | .NET nanoFramework            | Applications embarquées                  |

### Qu'est ce que CLI et CLR ?

#### 1. Qu'est-ce que la CLI ? (La Théorie)

**CLI** signifie **Common Language Infrastructure**.
Ce n'est pas un logiciel que vous installez. C'est une **spécification** (un document technique, une norme). Elle a été créée par Microsoft (et standardisée par l'ECMA) pour définir comment les langages de programmation .NET (comme C#, VB.NET, F#) doivent fonctionner.

**À quoi ça sert ?**
Elle permet à différents langages de fonctionner ensemble sur différentes machines. Grâce à la CLI, vous pouvez écrire une bibliothèque en C# et l'utiliser dans un projet écrit en F#.

**Ce que la CLI définit :**
*   **Le langage intermédiaire (CIL - Common Intermediate Language)** : Quand vous compilez votre code C#, il ne devient pas tout de suite du code "machine" (compréhensible par le processeur). Il devient du code "Intermédiaire" (IL). La CLI définit à quoi ressemble ce code.
*   **Le système de types (CTS)** : Elle définit ce qu'est un `int` (entier) ou une `string` (chaîne de caractères) pour que tous les langages soient d'accord sur la taille et le comportement de ces données.

#### 2. Qu'est-ce que le CLR ? (La Pratique)

**CLR** signifie **Common Language Runtime**.
C'est le **moteur**. C'est le logiciel réel (une "machine virtuelle") qui est installé sur votre ordinateur et qui exécute votre code. C'est l'implémentation concrète de la norme CLI.

**Les responsabilités du CLR (Le travail du Chef) :**
Le CLR prend votre code intermédiaire (celui défini par la CLI) et le fait tourner. Il gère des tâches très lourdes pour vous :

*   **Compilation JIT (Just-In-Time)** : Le CLR traduit le code intermédiaire en code machine natif (celui que votre processeur comprend : 010110) au dernier moment, juste avant l'exécution.
*   **Gestion de la mémoire (Garbage Collector)** : C'est le rôle le plus connu du CLR. Il nettoie automatiquement la mémoire de votre ordinateur en supprimant les objets que votre programme n'utilise plus, pour éviter les fuites de mémoire.
*   **Gestion des exceptions** : Il gère les erreurs (les "plantages") de manière structurée.
*   **Sécurité** : Il vérifie que le code a le droit de s'exécuter.

#### 3. Comment ils travaillent ensemble (Le Flux)

Voici ce qui se passe quand vous appuyez sur "Play" dans Visual Studio :

1.  **Écriture** : Vous écrivez du code C#.
2.  **Compilation (Roslyn)** : Le compilateur C# vérifie votre code. S'il est bon, il le transforme non pas en code machine, mais en fichier `.dll` ou `.exe` contenant du **Code IL** (le code standardisé par la CLI).
3.  **Exécution (CLR)** :
    *   Vous lancez le programme.
    *   Le CLR démarre.
    *   Il lit le Code IL.
    *   Son compilateur **JIT** traduit ce Code IL en instructions machines spécifiques à votre ordinateur (Windows, Mac, Linux).
    *   Le processeur exécute les instructions.

#### 4. En résumé

| Acronyme | Nom complet | Rôle | Analogie |
| :--- | :--- | :--- | :--- |
| **CLI** | Common Language Infrastructure | **Les Règles**. Une norme qui dit comment le code doit être structuré. | Le Code de la route (le livre). |
| **CLR** | Common Language Runtime | **L'Exécutant**. Le moteur qui fait tourner le code, gère la mémoire et la sécurité. | La Voiture et le Conducteur. |

#### 5. Ce qu'il faut retenir pour débuter :

En tant que développeur C# débutant, vous interagissez directement avec les règles de la **CLI** (en écrivant du code compatible), et vous vous reposez sur le **CLR** pour que votre programme ne plante pas et ne sature pas la mémoire de l'ordinateur.


**Question : CLR est il le compilateur ?**

- Non, le CLR est le runtime, il compile le code IL en code machine natif.

**Question : Roslyn est il le compilateur ?**

- Oui, Roslyn est le compilateur, il compile le code C# en code IL.

**Si on vous demande :**

- "Qui est le compilateur C# ?" ➔ Réponse : Roslyn.
- "Qui exécute le code ?" ➔ Réponse : Le CLR.