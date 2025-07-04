# Introduction à la Programmation Orientée Objet (POO) en C#

## Qu'est-ce que la Programmation Orientée Objet ?

La programmation orientée objet (POO) est un paradigme de programmation qui organise le code autour d'**objets**. Un objet est une entité qui regroupe des **données** (appelées attributs ou propriétés) et des **comportements** (appelés méthodes ou fonctions).

La POO permet de modéliser des éléments du monde réel (comme une voiture, un animal, un compte bancaire) sous forme de classes et d'objets, ce qui rend le code plus structuré, réutilisable et facile à maintenir.

---

## Concepts Fondamentaux de la POO

dans la POO, on crée des classes, des objets, des encapsulations, des héritages et des polymorphismes. Pour les utiliser, on crée des instances de ces classes.

### 1. Classe

Une **classe** est un plan (ou un modèle) qui définit les attributs et les méthodes d'un objet.

```csharp
public class Voiture
{
    public string Marque;
    public string Couleur;
    public void Klaxonner()
    {
        Console.WriteLine("Bip bip !");
    }
}
```

### 2. Objet

Un **objet** est une instance concrète d'une classe. On peut créer plusieurs objets à partir d'une même classe.

```csharp
Voiture maVoiture = new Voiture();
maVoiture.Marque = "Renault";
maVoiture.Couleur = "Rouge";
maVoiture.Klaxonner(); // Affiche : Bip bip !
```

### 3. Encapsulation

L'**encapsulation** consiste à protéger les données d'un objet en les rendant privées et en fournissant des méthodes pour y accéder ou les modifier.

```csharp
public class CompteBancaire
{
    private double solde;
    public void Deposer(double montant) { solde += montant; }
    public double ConsulterSolde() { return solde; }
}
```

### 4. Héritage

L'**héritage** permet de créer une nouvelle classe à partir d'une classe existante. La nouvelle classe hérite des attributs et méthodes de la classe parente.

```csharp
public class Animal
{
    public void Manger() { Console.WriteLine("Je mange"); }
}

public class Chien : Animal
{
    public void Aboyer() { Console.WriteLine("Wouf !"); }
}
```

### 5. Polymorphisme

Le **polymorphisme** permet d'utiliser une même méthode de différentes manières, selon l'objet qui l'utilise.

```csharp
public class Animal
{
    public virtual void Crier() { Console.WriteLine("Un animal crie"); }
}

public class Chat : Animal
{
    public override void Crier() { Console.WriteLine("Miaou"); }
}

public class Chien : Animal
{
    public override void Crier() { Console.WriteLine("Wouf"); }
}

// Utilisation :
Animal a1 = new Chat();
Animal a2 = new Chien();
a1.Crier(); // Affiche : Miaou
a2.Crier(); // Affiche : Wouf
```

---

## Avantages de la POO

- **Organisation** : Le code est structuré autour d'objets réels ou abstraits.
- **Réutilisabilité** : Les classes peuvent être réutilisées dans d'autres programmes.
- **Facilité de maintenance** : Les modifications sont localisées dans les classes concernées.
- **Extensibilité** : On peut facilement ajouter de nouvelles fonctionnalités via l'héritage.

---

## Résumé

La programmation orientée objet est un outil puissant pour organiser et structurer vos programmes. En C#, elle repose sur les concepts de classe, objet, encapsulation, héritage et polymorphisme. Maîtriser la POO vous permettra de créer des applications robustes, évolutives et faciles à maintenir.

---
