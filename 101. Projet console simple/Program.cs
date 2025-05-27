// Permet d'utiliser les caractères spéciaux dans la console
Console.OutputEncoding = System.Text.Encoding.UTF8;
// Recuperer le nom de l'utilisateur (Write sans saut de ligne)
Console.Write("Quel est votre nom ? ");
// Recuperer le nom de l'utilisateur 
string username = Console.ReadLine();
// Si l'utilisateur n'a pas saisi de nom, on lui demande de le saisir
// IsNullOrWhiteSpace vérifie si la chaîne est vide ou ne contient que des espaces
while (string.IsNullOrWhiteSpace(username))
{
    Console.Write("Veuillez saisir votre nom : ");
    username = Console.ReadLine();
}
// Demander l'age de l'utilisateur (Write sans saut de ligne)
Console.Write("Quel est votre age ? ");
// Recuperer l'age de l'utilisateur
string ageInput = Console.ReadLine();
// Si l'utilisateur n'a pas saisi d'age, on lui demande de le saisir
// la condition vérifie si l'input est vide ou si la conversion en entier échoue
// TryParse tente de convertir la chaîne en entier et retourne false si cela échoue
while (string.IsNullOrWhiteSpace(ageInput) || !int.TryParse(ageInput, out int age))
{
    Console.Write("Veuillez saisir votre age : ");
    ageInput = Console.ReadLine();
}
// Convertir l'age en entier
int userAge = int.Parse(ageInput);
// Afficher l'age de l'utilisateur
//Afficher un message de bienvenue
Console.WriteLine("Hello, World! mais surtout bonjour : " + username);
// Afficher l'age de l'utilisateur
Console.WriteLine("Vous avez " + userAge + " ans.");


