# Les types de variables en C-Sharp

## Types entiers

Un type entier est un type de données qui peut stocker des nombres entiers, c'est-à-dire des nombres sans décimales. En C#, il existe plusieurs types d'entiers, chacun ayant une taille et une plage de valeurs différentes. Voici les principaux types d'entiers en C# :

Signés et non signés : Les types entiers peuvent être signés ou non signés. Un type entier signé peut stocker des nombres négatifs et positifs, tandis qu'un type entier non signé ne peut stocker que des nombres positifs. Par exemple, le type `int` est un entier signé, tandis que le type `uint` est un entier non signé.
Les types entiers sont utilisés pour stocker des valeurs numériques entières, telles que des compteurs, des indices de tableau, des identifiants, etc. Ils sont également utilisés pour effectuer des opérations arithmétiques et logiques sur des nombres entiers.

- Information : Les types les plus couramment utilisés sont `int`, `decimal` et `double`. Le reste est utilisé pour des cas spécifiques notamment pour les calculs financiers et les calculs de grande précision ou d'optimisation de la mémoire.

### Entiers signés

| Type    | Taille  | Plage de valeurs                                       | Exemple                                  |
| ------- | ------- | ------------------------------------------------------ | ---------------------------------------- |
| `sbyte` | 8 bits  | -128 à 127                                             | `sbyte val = -10;`                       |
| `short` | 16 bits | -32 768 à 32 767                                       | `short val = 30000;`                     |
| `int`   | 32 bits | -2 147 483 648 à 2 147 483 647                         | `int val = -2000000;`                    |
| `long`  | 64 bits | -9 223 372 036 854 775 808 à 9 223 372 036 854 775 807 | `long val = 9_223_372_036_854_775_807L;` |

### Entiers non signés

| Type     | Taille  | Plage de valeurs               | Exemple                                     |
| -------- | ------- | ------------------------------ | ------------------------------------------- |
| `byte`   | 8 bits  | 0 à 255                        | `byte val = 255;`                           |
| `ushort` | 16 bits | 0 à 65 535                     | `ushort val = 65000;`                       |
| `uint`   | 32 bits | 0 à 4 294 967 295              | `uint val = 4000000000;`                    |
| `ulong`  | 64 bits | 0 à 18 446 744 073 709 551 615 | `ulong val = 18_446_744_073_709_551_615UL;` |

## Types flottants

| Type      | Taille   | Précision                             | Exemple                                  |
| --------- | -------- | ------------------------------------- | ---------------------------------------- |
| `float`   | 32 bits  | ~6-9 chiffres significatifs           | `float val = 3.14f;`                     |
| `double`  | 64 bits  | ~15-17 chiffres significatifs         | `double val = 3.1415926535;`             |
| `decimal` | 128 bits | Précision financière (28-29 chiffres) | `decimal val = 3.14159265358979323846m;` |

## Autres types

| Type   | Taille  | Description       | Exemple            |
| ------ | ------- | ----------------- | ------------------ |
| `bool` | 1 bit   | `true` ou `false` | `bool val = true;` |
| `char` | 16 bits | Caractère Unicode | `char val = 'A';`  |

## Suffixes de littéraux

```csharp
var monLong = 123L;       // Suffixe 'L' pour long
var monULong = 123UL;     // Suffixe 'UL' pour ulong
var monFloat = 123.45f;   // Suffixe 'f' pour float
var monDecimal = 123.45m; // Suffixe 'm' pour decimal
```

## Les types textuels

Les types textuels sont utilisés pour stocker des chaînes de caractères. En C#, il existe deux types principaux pour représenter des chaînes de caractères : `string` et `char`.

exemple :

```csharp
string nom = "Jean Dupont"; // Chaîne de caractères
char initiale = 'J'; // Caractère unique
```

- Le char est à mettre entre simple quote `'` et le string entre double quote `"`.
- L'antislash `\` est utilisé pour échapper les caractères spéciaux dans une chaîne de caractères. Par exemple, pour inclure un guillemet dans une chaîne, vous pouvez utiliser `\"`.
- Les chaînes de caractères peuvent être concaténées à l'aide de l'opérateur `+` ou en utilisant la méthode `String.Concat()`.

exemple :

```csharp
string prenom = "Jean";
string nom = "Dupont";
string nomComplet = prenom + " " + nom; // Concaténation
Console.WriteLine("Nom complet : " + nomComplet);
// exemple d'utilisation de l'antislash
string message = "Il a dit : \"Bonjour !\""; // Utilisation de l'antislash
Console.WriteLine(message);
// exemple d'interpolation de chaîne
string nom = "Jean";
string prenom = "Dupont";
string message = $"Bonjour, {prenom} {nom}!"; // Interpolation de chaîne
Console.WriteLine(message);

```

## Convertir une chaîne en entier

Pour convertir une chaîne de caractères en entier, vous pouvez utiliser la méthode `int.Parse()` ou `Convert.ToInt32()`.
Il faudra passer la chaine de caractères dans une nouvelle variable de type entier.

```csharp
string chaine = "123";
int entier = int.Parse(chaine); // Conversion de chaîne en entier
Console.WriteLine(entier); // Affiche 123
```
