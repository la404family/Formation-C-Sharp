# Comment creer une application console en C# ?

Pour créer une application console en C#, il faut suivre les étapes suivantes :

## Étapes de création d'une application console en C-Sharp

- Ouvrir le terminal ou l'invite de commandes dans le dossier où vous souhaitez créer le projet console.
- Taper dotnet new console pour créer un projet console
- Pour créer un projet console nommé "MyConsoleApp", taper `dotnet new console -n MyConsoleApp`

## Les dossiers et fichiers d'un projet console en C-Sharp

- Le fichier Program.cs est le point d'entrée de l'application.
- Le fichier MyConsoleApp.csproj contient les informations sur le projet et les dépendances.
- Le dossier bin contient les fichiers binaires de l'application.

## Questions / Réponses

1. Que signifie le "-n" dans la commande `dotnet new console -n MyConsoleApp` ?

   - Le "-n" est un argument qui permet de spécifier le nom du projet à créer. Dans cet exemple, le projet sera nommé "MyConsoleApp".

2. Peut-on changer le nom du fichier Program.cs ?
   - Oui, il est possible de renommer le fichier Program.cs, mais il faut s'assurer que le point d'entrée de l'application est correctement défini dans le fichier .csproj si nécessaire.
