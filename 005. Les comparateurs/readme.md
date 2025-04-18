# Les comparateurs en C-Sharp

Les comparateurs sont des symboles qui permettent de comparer des valeurs. Ils sont souvent utilisés dans les structures de contrôle pour prendre des décisions en fonction des résultats de ces comparaisons.

## Le if / else

le if / else est une structure de contrôle qui permet d'exécuter un bloc de code si une condition est vraie, et un autre bloc de code si la condition est fausse.

exemple :

```csharp
int a = 5, b = 3;
if (a > b)
{
    Console.WriteLine("a est supérieur à b");
}
else
{
    Console.WriteLine("a n'est pas supérieur à b");
}
```

## Le switch

Le switch est une structure de contrôle qui permet de choisir entre plusieurs blocs de code en fonction de la valeur d'une expression.

exemple :

```csharp
int a = 2;
switch (a)
{
    case 1:
        Console.WriteLine("a est égal à 1");
        break;
    case 2:
        Console.WriteLine("a est égal à 2");
        break;
    default:
        Console.WriteLine("a n'est ni égal à 1 ni égal à 2");
        break;
}
```

## L'opérateur ternaire

L'opérateur ternaire est une forme abrégée de l'instruction if / else. Il permet d'écrire une condition et deux expressions en une seule ligne.
Il a la forme suivante : `condition ? expression1 : expression2`. Si la condition est vraie, l'expression1 est exécutée, sinon l'expression2 est exécutée.

exemple :

```csharp
int a = 5, b = 3;
int max = (a > b) ? a : b;
Console.WriteLine("Le maximum est : " + max);
```
