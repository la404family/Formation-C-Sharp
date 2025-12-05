# Les bases du framework .NET

Le framework .NET est une plateforme de développement logicielle développée par Microsoft. Elle permet de créer des applications pour le web, le mobile, le bureau, les jeux, l'IoT, etc. Comprendre son fonctionnement interne est essentiel pour écrire du code performant et robuste.

## 1. Architecture globale

L'architecture .NET repose sur plusieurs piliers fondamentaux qui permettent l'exécution du code indépendamment de la machine (dans une certaine mesure) et du langage utilisé (C#, F#, VB.NET).

### Les composants principaux :

1.  **Langages** : C#, F#, Visual Basic.
2.  **Compilateurs** : Transforment le code source en langage intermédiaire (IL).
3.  **CLR (Common Language Runtime)** : Le moteur d'exécution qui gère le code.
4.  **BCL (Base Class Library)** : La bibliothèque de classes standard.

---

## 2. CLR (Common Language Runtime)

Le **CLR** est le cœur de .NET. C'est une machine virtuelle qui gère l'exécution des programmes .NET.

### Ses responsabilités :

- **Gestion de la mémoire** : Allocation et libération automatique (Garbage Collector).
- **Gestion des exceptions** : Système unifié de traitement des erreurs.
- **Sécurité de type** : Vérifie que les opérations sont sûres (type-safety).
- **Gestion des threads** : Exécution parallèle.
- **Compilation JIT** : Transformation du code intermédiaire en code machine.

---

## 3. CTS (Common Type System)

Le **CTS** définit comment les types de données sont déclarés, utilisés et gérés dans le runtime. Il assure que les types sont compatibles entre différents langages .NET.

- Un `int` en C# est strictement identique à un `Integer` en VB.NET car ils correspondent tous deux au type `System.Int32` du CTS.
- Cela permet l'interopérabilité : une classe écrite en F# peut être utilisée en C#.

---

## 4. BCL (Base Class Library)

La **BCL** est un ensemble complet de classes, interfaces et types de valeur standard fournis par Microsoft. Elle contient des milliers de classes pour effectuer des tâches courantes.

### Espaces de noms courants :

- `System` : Types de base (Int32, String, Boolean, Math).
- `System.IO` : Lecture/écriture de fichiers.
- `System.Collections` : Listes, dictionnaires, files.
- `System.Net` : Communication réseau.
- `System.Linq` : Manipulation de données.

---

## 5. Processus de Compilation (MSIL & JIT)

Contrairement au C++ qui compile directement en code machine, le C# passe par deux étapes :

### Étape 1 : Compilation vers MSIL (Microsoft Intermediate Language)

Le compilateur C# (`csc.exe`) transforme votre code source `.cs` en un langage intermédiaire appelé **MSIL** (ou simplement IL) et génère un **Assembly** (.dll ou .exe).
Ce code IL n'est pas exécutable directement par le processeur.

### Étape 2 : Compilation JIT (Just-In-Time)

Au moment de l'exécution, le **JIT Compiler** du CLR prend le code IL et le compile en **code machine natif** spécifique au processeur de l'ordinateur (x64, ARM, etc.).

- Cette compilation se fait méthode par méthode, uniquement quand elles sont appelées (d'où "Just-In-Time").

---

## 6. Les Assemblies

Un **Assembly** est l'unité fondamentale de déploiement, de versionnage et de sécurité dans .NET. Il prend généralement la forme d'un fichier `.dll` (bibliothèque) ou `.exe` (exécutable).

### Contenu d'un Assembly :

1.  **Code MSIL** : Le code intermédiaire.
2.  **Métadonnées** : Description des types, méthodes et propriétés définis dans le code.
3.  **Manifeste** : Informations sur l'assembly lui-même (nom, version, dépendances).

---

## 7. Gestion de la mémoire : Stack vs Heap

La mémoire en .NET est divisée en deux zones principales : la **Stack** (Pile) et le **Heap** (Tas).

### La Stack (Pile)

- **Fonctionnement** : LIFO (Last In, First Out). Empilement de blocs de mémoire.
- **Vitesse** : Très rapide (allocation/libération immédiate).
- **Usage** : Stocke les variables locales des méthodes et les appels de fonction.
- **Nettoyage** : Automatique dès que l'exécution sort du bloc (fin de méthode).

### Le Heap (Tas)

- **Fonctionnement** : Zone de mémoire désordonnée pour le stockage dynamique.
- **Vitesse** : Plus lent (nécessite de trouver un espace libre).
- **Usage** : Stocke les objets et les données complexes.
- **Nettoyage** : Géré par le **Garbage Collector** (GC) qui passe périodiquement pour libérer la mémoire des objets qui ne sont plus utilisés.

---

## 8. Types Valeur vs Types Référence

En C#, tous les types se classent en deux catégories qui déterminent où ils sont stockés en mémoire.

### Types Valeur (Value Types)

- **Stockage** : Directement sur la **Stack** (sauf s'ils sont membres d'une classe).
- **Contenu** : Contiennent directement la donnée.
- **Exemples** : `int`, `double`, `bool`, `char`, `struct`, `enum`.
- **Comportement** : Lors d'une assignation (`a = b`), la valeur est **copiée**. Modifier l'un n'affecte pas l'autre.

```csharp
int a = 10;
int b = a; // Copie de la valeur 10
b = 20;    // a vaut toujours 10
```

### Types Référence (Reference Types)

- **Stockage** : La donnée réelle est sur le **Heap**, la **Stack** contient seulement une **référence** (adresse mémoire) vers cette donnée.
- **Exemples** : `class`, `interface`, `delegate`, `string`, `array`, `object`.
- **Comportement** : Lors d'une assignation, seule la **référence est copiée**. Les deux variables pointent vers le même objet en mémoire.

```csharp
class Personne { public string Nom; }

Personne p1 = new Personne();
p1.Nom = "Alice";

Personne p2 = p1; // Copie de la référence (adresse mémoire)
p2.Nom = "Bob";   // Modifie l'objet pointé par p1 également

Console.WriteLine(p1.Nom); // Affiche "Bob"
```

---

## 9. Boxing et Unboxing

Le **Boxing** et l'**Unboxing** sont des opérations coûteuses en performance qui permettent de passer d'un type valeur à un type référence et inversement.

### Boxing (Mise en boîte)

Conversion implicite d'un **type valeur** vers le type `object` (ou une interface).

- **Action** : .NET crée un nouvel objet sur le **Heap** et y copie la valeur.
- **Coût** : Allocation mémoire + copie.

```csharp
int i = 123;
object o = i; // Boxing : 123 est copié dans un objet sur le Heap
```

### Unboxing (Déballage)

Conversion explicite d'un `object` vers un **type valeur**.

- **Action** : Vérifie le type et copie la valeur du Heap vers la Stack.
- **Coût** : Vérification de type + copie.

```csharp
object o = 123;
int i = (int)o; // Unboxing : extraction de la valeur
```

### Pourquoi éviter le Boxing/Unboxing ?

Ces opérations ajoutent une pression inutile sur le processeur et le Garbage Collector. Dans les collections modernes (comme `List<T>`), l'utilisation des **Génériques** permet d'éviter ces conversions.

```csharp
// Mauvais (ArrayList utilise object, donc Boxing)
ArrayList list = new ArrayList();
list.Add(10); // Boxing

// Bon (List<T> est typé, pas de Boxing)
List<int> list = new List<int>();
list.Add(10); // Pas de Boxing
```
