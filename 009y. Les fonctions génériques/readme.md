# Les fonctions génériques

Les fonctions génériques permettent de créer des méthodes qui peuvent fonctionner avec n'importe quel type de données. Elles sont utiles pour écrire du code réutilisable et flexible.

## Exemples de fonctions génériques

```csharp
int nb = 5;
string str = "Hello";
bool isTrue = true;

void Affiche<T>(T value)
{
    Console.WriteLine($"valeur: " + value + " de type: " + typeof(T));
}
```

La fonction `Affiche` est générique et peut afficher n'importe quel type de valeur. Voici comment l'utiliser :

```csharp
Affiche(nb); // Affiche valeur: 5 de type: System.Int32
Affiche(str); // Affiche valeur: Hello de type: System.String
Affiche(isTrue); // Affiche valeur: True de type: System.Boolean
```

### Fonction générique pour l'échange de deux variables

```csharp
void Swap<T>(ref T a, ref T b)
{
    T temp = a;
    a = b;
    b = temp;
}
```

### Utilisation de la fonction générique

```csharp
int x = 5, y = 10;
Swap(ref x, ref y);
Console.WriteLine($"x: {x}, y: {y}"); // Affiche x: 10, y: 5
string str1 = "Hello", str2 = "World";
Swap(ref str1, ref str2);
Console.WriteLine($"str1: {str1}, str2: {str2}");
// Affiche str1: World, str2: Hello
```

### Fonction générique pour trouver le maximum de deux valeurs

```csharp
T Max<T>(T a, T b) where T : IComparable<T>
{
    return a.CompareTo(b) > 0 ? a : b;
}
```

### Utilisation de la fonction générique Max

```csharp
Console.WriteLine(Max(5, 10)); // Affiche 10
Console.WriteLine(Max(3.14, 2.71)); // Affiche 3.14
Console.WriteLine(Max("Apple", "Banana")); // Affiche Banana
```

### Fonction générique pour afficher les éléments d'une collection

```csharp
void PrintCollection<T>(IEnumerable<T> collection)
{
    foreach (var item in collection)
    {
        Console.WriteLine(item);
    }
}
```
