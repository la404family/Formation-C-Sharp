# La surcharge de fonction

La surcharge de fonction permet de définir plusieurs méthodes avec le même nom mais des signatures différentes. Cela permet d'utiliser le même nom de méthode pour des opérations similaires mais avec des paramètres différents.

## Exemples de surcharge de fonction

### Addition de deux entiers

```csharp
int Add(int a, int b)
{
    return a + b;
}
```

### Addition de trois entiers

```csharp
int Add(int a, int b, int c)
{
    return a + b + c;
}
```

### Addition de deux nombres à virgule flottante

```csharp
double Add(double a, double b)
{
    return a + b;
}
```

### Addition de deux chaînes de caractères

```csharp
string Add(string a, string b)
{
    return a + b;
}
```

### Utilisation de la surcharge de fonction

```csharp
Console.WriteLine(Add(1, 2)); // Affiche 3
Console.WriteLine(Add(1, 2, 3)); // Affiche 6
Console.WriteLine(Add(1.5, 2.5)); // Affiche 4
Console.WriteLine(Add("Hello, ", "World!")); // Affiche Hello, World!
```

L'utilité de la surcharge de fonction est de permettre une meilleure lisibilité du code et de regrouper des opérations similaires sous un même nom, ce qui facilite la maintenance et l'utilisation des fonctions.
