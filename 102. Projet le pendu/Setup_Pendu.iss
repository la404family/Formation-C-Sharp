; ==================== SCRIPT INNO SETUP POUR LE JEU DU PENDU ====================
; Ce fichier configure l'installateur pour votre jeu du pendu
; Inno Setup est un outil gratuit pour créer des installateurs Windows professionnels
; 
; PRÉREQUIS : Télécharger et installer Inno Setup depuis https://jrsoftware.org/isinfo.php
;
; UTILISATION :
; 1. Ouvrir ce fichier avec Inno Setup Compiler
; 2. Cliquer sur "Compile" (ou appuyer sur F9)
; 3. L'installateur sera créé dans le dossier "Output"

; ==================== SECTION [Setup] ====================
; Cette section contient les informations principales sur l'application
[Setup]
; === Informations de base de l'application ===
AppName=PENDU
AppVersion=1.5.18
AppPublisher=Kevin Du Chevreuil
AppPublisherURL=https://github.com/la404family/
AppSupportURL=https://github.com/la404family/Formation-C-Sharp/tree/main/102.%20Projet%20le%20pendu
AppUpdatesURL=https://github.com/la404family/Formation-C-Sharp/tree/main/102.%20Projet%20le%20pendu

; === Identification unique de l'application ===
; IMPORTANT : Ne changez JAMAIS cet ID après la première installation !
; Cet identifiant permet à Windows de reconnaître votre application
; Généré avec : https://www.guidgenerator.com/
AppId={{8E9A7B2C-5D4F-4A3E-9B1C-6E8F7A2D5C4B}

; === Configuration du dossier d'installation ===
; Par défaut : C:\Program Files\PENDU
DefaultDirName={autopf}\PENDU
; Autoriser l'utilisateur à changer le dossier d'installation
DisableDirPage=no

; === Configuration du groupe dans le menu Démarrer ===
; Crée un dossier "PENDU" dans le menu Démarrer de Windows
DefaultGroupName=PENDU
; Autoriser l'utilisateur à changer le nom du groupe
DisableProgramGroupPage=no

; === Fichier de licence (optionnel) ===
; Décommentez la ligne suivante si vous avez un fichier LICENSE.txt
; LicenseFile=LICENSE.txt

; === Icône de l'installateur ===
; Cette icône apparaîtra sur l'installateur lui-même
SetupIconFile=icons.ico

; === Fichier de sortie (l'installateur créé) ===
; Sera créé dans le sous-dossier "Output"
OutputDir=Output
; Nom du fichier installateur (exemple : Setup_PENDU_1.0.0.exe)
OutputBaseFilename=Setup_PENDU_{#SetupSetting("AppVersion")}

; === Compression ===
; lzma2/ultra = meilleure compression (fichier plus petit)
Compression=lzma2/ultra
SolidCompression=yes

; === Interface utilisateur ===
; Utilise le style moderne de Windows (visuellement plus agréable)
WizardStyle=modern

; === Privilèges d'administration ===
; "admin" = nécessite les droits administrateur pour installer dans Program Files
; Si vous voulez installer sans admin, utilisez "lowest" et changez DefaultDirName
PrivilegesRequired=admin

; === Enregistrement dans Windows ===
; Cela permet de voir l'application dans "Ajout/Suppression de programmes"
; et de pouvoir la désinstaller proprement
UninstallDisplayIcon={app}\icons.ico
UninstallDisplayName=PENDU - Jeu du Pendu
UninstallFilesDir={app}\Uninstall

; === Architecture supportée ===
; x64 = 64 bits uniquement (Windows moderne)
; Si vous voulez supporter 32 bits aussi, utilisez : ArchitecturesAllowed=x64 x86
ArchitecturesInstallIn64BitMode=x64

; ==================== SECTION [Languages] ====================
; Configure la langue de l'installateur
[Languages]
; Français comme langue principale
Name: "french"; MessagesFile: "compiler:Languages\French.isl"
; Anglais comme langue alternative (optionnel)
Name: "english"; MessagesFile: "compiler:Default.isl"

; ==================== SECTION [Tasks] ====================
; Permet à l'utilisateur de choisir des options pendant l'installation
[Tasks]
; Option pour créer une icône sur le bureau (non cochée par défaut)
Name: "desktopicon"; Description: "Créer une icône sur le bureau"; GroupDescription: "Icônes supplémentaires:"; Flags: unchecked

; ==================== SECTION [Files] ====================
; Liste de TOUS les fichiers à installer
; Cette section copie vos fichiers depuis votre projet vers le dossier d'installation
[Files]
; === L'application principale ===
; Source : où se trouve le fichier sur votre ordinateur (avant compilation)
; DestDir : où il sera copié sur l'ordinateur de l'utilisateur
; {app} = le dossier d'installation choisi (exemple : C:\Program Files\PENDU)

; *** IMPORTANT : Vous devez d'abord PUBLIER votre application ! ***
; Exécutez cette commande PowerShell dans le dossier de votre projet :
; dotnet publish -c Release -r win-x64 --self-contained false -o ".\publish"
;
; --self-contained false = nécessite .NET Runtime (fichier plus petit)
; --self-contained true = inclut .NET dans l'application (fichier plus gros, mais fonctionne partout)

; L'exécutable principal du jeu
Source: "publish\102. Projet le pendu.exe"; DestDir: "{app}"; Flags: ignoreversion

; La DLL de l'application (générée par .NET)
Source: "publish\102. Projet le pendu.dll"; DestDir: "{app}"; Flags: ignoreversion

; Le fichier de configuration (si présent)
Source: "publish\102. Projet le pendu.runtimeconfig.json"; DestDir: "{app}"; Flags: ignoreversion

; Toutes les autres DLL nécessaires
Source: "publish\*.dll"; DestDir: "{app}"; Flags: ignoreversion

; === Fichiers de données du jeu ===
; Le fichier JSON contenant les mots à deviner
Source: "mots.json"; DestDir: "{app}"; Flags: ignoreversion onlyifdoesntexist
; onlyifdoesntexist = ne pas écraser si l'utilisateur a personnalisé ses mots

; === L'icône de l'application ===
Source: "icons.ico"; DestDir: "{app}"; Flags: ignoreversion

; NOTE : Les statistiques (statistiques_pendu.json) ne sont PAS copiées
; Elles seront créées automatiquement au premier lancement du jeu

; ==================== SECTION [Icons] ====================
; Crée les raccourcis dans Windows
[Icons]
; === Raccourci dans le menu Démarrer ===
; {group} = le groupe dans le menu Démarrer (PENDU)
Name: "{group}\PENDU"; Filename: "{app}\102. Projet le pendu.exe"; IconFilename: "{app}\icons.ico"; Comment: "Jouer au jeu du Pendu"

; === Raccourci pour le guide d'utilisation ===
Name: "{group}\Guide des mots personnalisés"; Filename: "{app}\GUIDE_MOTS_JSON.md"; Comment: "Comment personnaliser les mots du jeu"

; === Raccourci vers le dossier d'installation ===
Name: "{group}\Ouvrir le dossier du jeu"; Filename: "{app}"; Comment: "Ouvrir le dossier où est installé le jeu"

; === Raccourci de désinstallation ===
Name: "{group}\Désinstaller PENDU"; Filename: "{uninstallexe}"; Comment: "Supprimer le jeu du Pendu de votre ordinateur"

; === Raccourci sur le bureau (optionnel) ===
; Créé seulement si l'utilisateur coche l'option pendant l'installation
Name: "{autodesktop}\PENDU"; Filename: "{app}\102. Projet le pendu.exe"; IconFilename: "{app}\icons.ico"; Tasks: desktopicon; Comment: "Jouer au jeu du Pendu"

; ==================== SECTION [Run] ====================
; Actions à exécuter après l'installation
[Run]
; Lancer le jeu après l'installation (l'utilisateur peut décocher cette option)
; postinstall = afficher cette option à la fin de l'installation
; nowait = ne pas attendre que le jeu se ferme pour terminer l'installateur
; skipifsilent = ne pas lancer si installation silencieuse
; unchecked = l'option n'est pas cochée par défaut (l'utilisateur doit la cocher)
Filename: "{app}\102. Projet le pendu.exe"; Description: "Lancer PENDU maintenant"; Flags: nowait postinstall skipifsilent unchecked

; ==================== SECTION [UninstallDelete] ====================
; Fichiers à supprimer lors de la désinstallation
; Utile pour les fichiers créés par l'application (pas copiés par l'installateur)
[UninstallDelete]
; Supprimer le fichier de statistiques lors de la désinstallation
Type: files; Name: "{app}\statistiques_pendu.json"

; ==================== SECTION [Code] - DÉTECTION .NET RUNTIME ====================
; Code Pascal pour vérifier si .NET Runtime est installé
; Cette section est CRUCIALE pour un fonctionnement correct !
[Code]

// ==================== FONCTION : EstDotNet9Installe ====================
// Vérifie si .NET 9.0 Runtime (ou SDK) est installé sur l'ordinateur
// Retourne True si installé, False sinon
function EstDotNet9Installe: Boolean;
begin
  // Méthode simple : vérifier l'existence du dossier d'installation de .NET 9
  // Le dossier standard pour .NET 9 sur Windows 64 bits
  // DirExists() retourne True si le dossier existe, False sinon
  Result := DirExists('C:\Program Files\dotnet\shared\Microsoft.NETCore.App\9.0') or
            DirExists('C:\Program Files (x86)\dotnet\shared\Microsoft.NETCore.App\9.0');
  
  // Alternative : vous pouvez aussi vérifier dans le registre Windows
  // en utilisant RegKeyExists() ou RegQueryStringValue()
  // mais la méthode ci-dessus est plus simple et plus fiable
end;

// ==================== FONCTION : InitializeSetup ====================
// Cette fonction est appelée AU DÉBUT de l'installation
// C'est ici qu'on vérifie les prérequis (comme .NET Runtime)
// Si elle retourne False, l'installation est annulée
function InitializeSetup(): Boolean;
var
  ReponseUtilisateur: Integer;
  ExitCode: Integer;
  ErrorCode: Integer;
begin
  // Par défaut, on suppose que l'installation peut continuer
  Result := True;
  
  // Vérifier si .NET 9.0 est installé
  if not EstDotNet9Installe then
  begin
    // .NET 9.0 n'est PAS installé : afficher un message d'avertissement
    ReponseUtilisateur := MsgBox(
      '.NET 9.0 Runtime n''est pas installé sur votre ordinateur.' + #13#10 + #13#10 +
      'Ce jeu nécessite .NET 9.0 pour fonctionner.' + #13#10 + #13#10 +
      'Voulez-vous :' + #13#10 +
      '- Cliquer sur OUI pour ouvrir la page de téléchargement de .NET 9.0' + #13#10 +
      '- Cliquer sur NON pour continuer l''installation quand même (le jeu ne fonctionnera pas)' + #13#10 +
      '- Cliquer sur ANNULER pour arrêter l''installation',
      mbConfirmation,
      MB_YESNOCANCEL
    );
    
    case ReponseUtilisateur of
      IDYES: 
        begin
          // Ouvrir la page de téléchargement officielle de Microsoft
          // L'utilisateur pourra télécharger .NET 9.0 Runtime (Desktop)
          // ShellExec ouvre l'URL dans le navigateur par défaut
          ShellExec('open', 'https://dotnet.microsoft.com/download/dotnet/9.0', '', '', SW_SHOW, ewNoWait, ErrorCode);
          
          // Arrêter l'installation pour que l'utilisateur installe .NET d'abord
          Result := False;
          
          MsgBox(
            'L''installation de PENDU va s''arrêter.' + #13#10 + #13#10 +
            'Après avoir installé .NET 9.0 Runtime, relancez cet installateur.',
            mbInformation,
            MB_OK
          );
        end;
      
      IDNO:
        begin
          // L'utilisateur veut continuer quand même (déconseillé)
          MsgBox(
            'Installation en cours...' + #13#10 + #13#10 +
            'ATTENTION : Le jeu ne fonctionnera pas sans .NET 9.0 Runtime !',
            mbInformation,
            MB_OK
          );
          // Result reste True, l'installation continue
        end;
      
      IDCANCEL:
        begin
          // L'utilisateur annule l'installation
          Result := False;
        end;
    end;
  end;
end;

// ==================== FONCTION : CurStepChanged ====================
// Cette fonction est appelée à chaque étape de l'installation
// Utile pour afficher des messages ou faire des actions spécifiques
procedure CurStepChanged(CurStep: TSetupStep);
begin
  if CurStep = ssPostInstall then
  begin
    // Cette étape est APRÈS la copie des fichiers
    // On peut afficher un message de succès ou faire des configurations
    
    // Exemple : afficher un message personnalisé
    // MsgBox('Installation terminée avec succès !', mbInformation, MB_OK);
  end;
end;

// ==================== FONCTION : InitializeUninstall ====================
// Cette fonction est appelée au DÉBUT de la désinstallation
// Utile pour demander confirmation ou sauvegarder des données
function InitializeUninstall(): Boolean;
var
  ReponseUtilisateur: Integer;
begin
  // Demander confirmation avant de désinstaller
  ReponseUtilisateur := MsgBox(
    'Êtes-vous sûr de vouloir désinstaller PENDU ?' + #13#10 + #13#10 +
    'Vos statistiques de jeu seront supprimées.',
    mbConfirmation,
    MB_YESNO
  );
  
  // Si l'utilisateur clique sur OUI, continuer la désinstallation
  Result := (ReponseUtilisateur = IDYES);
end;
