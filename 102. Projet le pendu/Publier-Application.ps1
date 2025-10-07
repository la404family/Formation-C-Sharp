# ==================== SCRIPT DE PUBLICATION POUR CRÉER L'INSTALLATEUR ====================
# Ce script PowerShell prépare votre application pour la création de l'installateur
# Il compile le jeu en mode "Release" (version optimisée) et copie tous les fichiers nécessaires
#
# UTILISATION :
# 1. Ouvrir PowerShell dans le dossier du projet
# 2. Exécuter : .\Publier-Application.ps1
# 3. Les fichiers seront dans le dossier "publish"
# 4. Ensuite, compiler le script Inno Setup (Setup_Pendu.iss)

# ==================== CONFIGURATION ====================
Write-Host "╔═══════════════════════════════════════════════════════════════╗" -ForegroundColor Cyan
Write-Host "║          PUBLICATION DE L'APPLICATION PENDU                   ║" -ForegroundColor Cyan
Write-Host "╚═══════════════════════════════════════════════════════════════╝" -ForegroundColor Cyan
Write-Host ""

# Vérifier si nous sommes dans le bon dossier
$ProjetCsproj = "102. Projet le pendu.csproj"
if (-not (Test-Path $ProjetCsproj)) {
    Write-Host "❌ ERREUR : Fichier projet '$ProjetCsproj' introuvable !" -ForegroundColor Red
    Write-Host "   Assurez-vous d'exécuter ce script depuis le dossier du projet." -ForegroundColor Yellow
    Write-Host ""
    Read-Host "Appuyez sur Entrée pour quitter"
    exit 1
}

Write-Host "✓ Projet trouvé : $ProjetCsproj" -ForegroundColor Green
Write-Host ""

# ==================== NETTOYAGE ====================
Write-Host "[1/4] Nettoyage des anciens fichiers..." -ForegroundColor Yellow

# Supprimer le dossier publish s'il existe
if (Test-Path "publish") {
    Remove-Item -Path "publish" -Recurse -Force
    Write-Host "      ✓ Dossier 'publish' nettoyé" -ForegroundColor Green
}

# Supprimer le dossier Output de l'installateur s'il existe
if (Test-Path "Output") {
    Remove-Item -Path "Output" -Recurse -Force
    Write-Host "      ✓ Dossier 'Output' nettoyé" -ForegroundColor Green
}

Write-Host ""

# ==================== PUBLICATION DE L'APPLICATION ====================
Write-Host "[2/4] Publication de l'application..." -ForegroundColor Yellow

# Options de publication :
# -c Release              = Mode "Release" (optimisé, pas de debug)
# -r win-x64              = Cible Windows 64 bits
# --self-contained false  = Nécessite .NET Runtime (fichier plus petit)
# -o publish              = Dossier de sortie

# OPTION A : Version LÉGÈRE (nécessite .NET Runtime installé)
# Avantages : Fichier installateur plus petit (quelques Mo)
# Inconvénients : L'utilisateur doit installer .NET Runtime séparément
$CommandePublication = "dotnet publish -c Release -r win-x64 --self-contained false -o publish"

# OPTION B : Version AUTONOME (inclut .NET Runtime)
# Avantages : Fonctionne partout, pas besoin d'installer .NET
# Inconvénients : Fichier installateur plus gros (environ 60-80 Mo)
# Pour utiliser cette option, décommentez la ligne suivante et commentez celle du dessus :
# $CommandePublication = "dotnet publish -c Release -r win-x64 --self-contained true -o publish"

Write-Host "      Commande : $CommandePublication" -ForegroundColor Gray
Write-Host ""

# Exécuter la commande de publication
Invoke-Expression $CommandePublication

# Vérifier si la publication a réussi
if ($LASTEXITCODE -ne 0) {
    Write-Host ""
    Write-Host "❌ ERREUR : La publication a échoué !" -ForegroundColor Red
    Write-Host "   Vérifiez les messages d'erreur ci-dessus." -ForegroundColor Yellow
    Write-Host ""
    Read-Host "Appuyez sur Entrée pour quitter"
    exit 1
}

Write-Host ""
Write-Host "      ✓ Publication réussie !" -ForegroundColor Green
Write-Host ""

# ==================== VÉRIFICATION DES FICHIERS ====================
Write-Host "[3/4] Vérification des fichiers publiés..." -ForegroundColor Yellow

$FichiersRequis = @(
    "publish\102. Projet le pendu.exe",
    "publish\102. Projet le pendu.dll"
)

$TousLesFichiersTrouves = $true

foreach ($Fichier in $FichiersRequis) {
    if (Test-Path $Fichier) {
        Write-Host "      ✓ $Fichier" -ForegroundColor Green
    } else {
        Write-Host "      ❌ $Fichier (MANQUANT !)" -ForegroundColor Red
        $TousLesFichiersTrouves = $false
    }
}

if (-not $TousLesFichiersTrouves) {
    Write-Host ""
    Write-Host "❌ ERREUR : Certains fichiers essentiels sont manquants !" -ForegroundColor Red
    Write-Host ""
    Read-Host "Appuyez sur Entrée pour quitter"
    exit 1
}

# Compter le nombre total de fichiers
$NombreFichiers = (Get-ChildItem -Path "publish" -File).Count
Write-Host ""
Write-Host "      ✓ $NombreFichiers fichiers publiés au total" -ForegroundColor Green

# Afficher la taille totale
$TailleTotale = (Get-ChildItem -Path "publish" -Recurse | Measure-Object -Property Length -Sum).Sum
$TailleMo = [math]::Round($TailleTotale / 1MB, 2)
Write-Host "      ✓ Taille totale : $TailleMo Mo" -ForegroundColor Green
Write-Host ""

# ==================== INSTRUCTIONS SUIVANTES ====================
Write-Host "[4/4] Prêt pour la création de l'installateur !" -ForegroundColor Yellow
Write-Host ""
Write-Host "╔═══════════════════════════════════════════════════════════════╗" -ForegroundColor Cyan
Write-Host "║               PROCHAINES ÉTAPES                               ║" -ForegroundColor Cyan
Write-Host "╚═══════════════════════════════════════════════════════════════╝" -ForegroundColor Cyan
Write-Host ""
Write-Host "1. Téléchargez et installez Inno Setup depuis :" -ForegroundColor White
Write-Host "   https://jrsoftware.org/isinfo.php" -ForegroundColor Cyan
Write-Host ""
Write-Host "2. Ouvrez le fichier 'Setup_Pendu.iss' avec Inno Setup Compiler" -ForegroundColor White
Write-Host ""
Write-Host "3. Cliquez sur 'Build' > 'Compile' (ou appuyez sur F9)" -ForegroundColor White
Write-Host ""
Write-Host "4. L'installateur sera créé dans le dossier 'Output'" -ForegroundColor White
Write-Host "   Nom du fichier : Setup_PENDU_1.0.0.exe" -ForegroundColor Green
Write-Host ""
Write-Host "╔═══════════════════════════════════════════════════════════════╗" -ForegroundColor Cyan
Write-Host "║               FICHIERS PUBLIÉS                                ║" -ForegroundColor Cyan
Write-Host "╚═══════════════════════════════════════════════════════════════╝" -ForegroundColor Cyan
Write-Host ""

# Lister les 10 premiers fichiers publiés
Write-Host "Aperçu des fichiers dans 'publish' :" -ForegroundColor White
Get-ChildItem -Path "publish" -File | Select-Object -First 10 | ForEach-Object {
    $TailleFichier = [math]::Round($_.Length / 1KB, 2)
    Write-Host "   • $($_.Name) ($TailleFichier Ko)" -ForegroundColor Gray
}

if ($NombreFichiers -gt 10) {
    Write-Host "   ... et $($NombreFichiers - 10) autres fichiers" -ForegroundColor Gray
}

Write-Host ""
Write-Host "✓ Publication terminée avec succès !" -ForegroundColor Green
Write-Host ""

# Pause pour que l'utilisateur puisse lire les messages
Read-Host "Appuyez sur Entrée pour quitter"
