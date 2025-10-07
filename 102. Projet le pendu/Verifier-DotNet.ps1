# ==================== VÉRIFICATEUR DE .NET RUNTIME ====================
# Ce script PowerShell vérifie si .NET 9.0 Runtime est installé
# Il peut être distribué avec votre application pour aider les utilisateurs
#
# UTILISATION :
# 1. Double-cliquer sur ce fichier
# 2. Le script affichera si .NET 9.0 est installé ou non
# 3. Si absent, il proposera de télécharger l'installateur

# ==================== CONFIGURATION ====================
$RequiredVersion = "9.0"
$DownloadUrl = "https://dotnet.microsoft.com/download/dotnet/9.0"

# ==================== AFFICHAGE DE L'EN-TÊTE ====================
Clear-Host
Write-Host "╔═══════════════════════════════════════════════════════════════╗" -ForegroundColor Cyan
Write-Host "║      VÉRIFICATION DE .NET RUNTIME POUR LE JEU DU PENDU       ║" -ForegroundColor Cyan
Write-Host "╚═══════════════════════════════════════════════════════════════╝" -ForegroundColor Cyan
Write-Host ""

# ==================== FONCTION DE VÉRIFICATION ====================
function Test-DotNetRuntime {
    param (
        [string]$Version
    )
    
    Write-Host "🔍 Recherche de .NET $Version Runtime..." -ForegroundColor Yellow
    Write-Host ""
    
    # Méthode 1 : Vérifier avec la commande dotnet
    try {
        # Exécuter "dotnet --list-runtimes" et capturer la sortie
        $runtimes = & dotnet --list-runtimes 2>&1
        
        if ($LASTEXITCODE -eq 0) {
            # La commande a réussi, analyser les runtimes
            Write-Host "📋 Runtimes .NET installés :" -ForegroundColor Cyan
            Write-Host ""
            
            $foundVersion = $false
            foreach ($runtime in $runtimes) {
                Write-Host "   • $runtime" -ForegroundColor Gray
                
                # Vérifier si c'est Microsoft.NETCore.App version 9.x
                if ($runtime -match "Microsoft\.NETCore\.App $Version") {
                    $foundVersion = $true
                }
            }
            
            Write-Host ""
            
            if ($foundVersion) {
                Write-Host "✅ .NET $Version Runtime est INSTALLÉ !" -ForegroundColor Green
                Write-Host ""
                Write-Host "Vous pouvez lancer le jeu du pendu sans problème." -ForegroundColor White
                return $true
            } else {
                Write-Host "❌ .NET $Version Runtime n'est PAS installé." -ForegroundColor Red
                return $false
            }
        } else {
            # La commande a échoué, dotnet n'est probablement pas installé
            throw "Commande dotnet introuvable"
        }
    }
    catch {
        # Méthode 2 : Vérifier les dossiers d'installation directement
        Write-Host "⚠️  La commande 'dotnet' n'est pas disponible." -ForegroundColor Yellow
        Write-Host "   Vérification manuelle des dossiers d'installation..." -ForegroundColor Yellow
        Write-Host ""
        
        $paths = @(
            "C:\Program Files\dotnet\shared\Microsoft.NETCore.App",
            "C:\Program Files (x86)\dotnet\shared\Microsoft.NETCore.App"
        )
        
        foreach ($path in $paths) {
            if (Test-Path $path) {
                $versions = Get-ChildItem -Path $path -Directory | Where-Object { $_.Name -like "$Version.*" }
                
                if ($versions) {
                    Write-Host "✅ .NET $Version Runtime trouvé dans :" -ForegroundColor Green
                    foreach ($v in $versions) {
                        Write-Host "   • $($v.FullName)" -ForegroundColor Gray
                    }
                    Write-Host ""
                    Write-Host "Vous pouvez lancer le jeu du pendu sans problème." -ForegroundColor White
                    return $true
                }
            }
        }
        
        Write-Host "❌ .NET $Version Runtime n'est PAS installé." -ForegroundColor Red
        return $false
    }
}

# ==================== EXÉCUTION DE LA VÉRIFICATION ====================
$isInstalled = Test-DotNetRuntime -Version $RequiredVersion

Write-Host ""
Write-Host "═══════════════════════════════════════════════════════════════" -ForegroundColor Cyan

# ==================== SI .NET N'EST PAS INSTALLÉ ====================
if (-not $isInstalled) {
    Write-Host ""
    Write-Host "📥 TÉLÉCHARGEMENT REQUIS" -ForegroundColor Yellow
    Write-Host ""
    Write-Host "Le jeu du pendu nécessite .NET $RequiredVersion Runtime pour fonctionner." -ForegroundColor White
    Write-Host ""
    Write-Host "Options disponibles :" -ForegroundColor White
    Write-Host "  1. Télécharger et installer .NET $RequiredVersion Runtime Desktop" -ForegroundColor Cyan
    Write-Host "  2. Quitter ce programme" -ForegroundColor Gray
    Write-Host ""
    
    $choix = Read-Host "Entrez votre choix (1 ou 2)"
    
    if ($choix -eq "1") {
        Write-Host ""
        Write-Host "🌐 Ouverture de la page de téléchargement..." -ForegroundColor Green
        Write-Host ""
        Write-Host "INSTRUCTIONS :" -ForegroundColor Yellow
        Write-Host "  1. Sur la page web, cherchez '.NET Desktop Runtime'" -ForegroundColor White
        Write-Host "  2. Téléchargez la version pour votre système (x64 recommandé)" -ForegroundColor White
        Write-Host "  3. Installez le fichier téléchargé" -ForegroundColor White
        Write-Host "  4. Relancez ce vérificateur pour confirmer l'installation" -ForegroundColor White
        Write-Host ""
        
        # Ouvrir le navigateur
        Start-Process $DownloadUrl
        
        Write-Host "Appuyez sur une touche pour quitter..." -ForegroundColor Gray
        $null = $Host.UI.RawUI.ReadKey("NoEcho,IncludeKeyDown")
    } else {
        Write-Host ""
        Write-Host "Programme terminé." -ForegroundColor Gray
        Write-Host ""
    }
} else {
    # .NET est installé, tout va bien !
    Write-Host ""
    Write-Host "✨ Tout est prêt pour jouer au jeu du pendu !" -ForegroundColor Green
    Write-Host ""
    Write-Host "Appuyez sur une touche pour quitter..." -ForegroundColor Gray
    $null = $Host.UI.RawUI.ReadKey("NoEcho,IncludeKeyDown")
}

# ==================== INFORMATIONS SUPPLÉMENTAIRES ====================
Write-Host ""
Write-Host "═══════════════════════════════════════════════════════════════" -ForegroundColor Cyan
Write-Host ""
Write-Host "ℹ️  INFORMATIONS COMPLÉMENTAIRES" -ForegroundColor Cyan
Write-Host ""
Write-Host "Qu'est-ce que .NET Runtime ?" -ForegroundColor White
Write-Host "  .NET Runtime est un logiciel gratuit de Microsoft" -ForegroundColor Gray
Write-Host "  qui permet d'exécuter des applications C#." -ForegroundColor Gray
Write-Host ""
Write-Host "Quelle version télécharger ?" -ForegroundColor White
Write-Host "  • .NET Desktop Runtime (recommandé) : Pour applications avec interface" -ForegroundColor Gray
Write-Host "  • x64 : Pour Windows 64 bits (la plupart des ordinateurs modernes)" -ForegroundColor Gray
Write-Host "  • x86 : Pour Windows 32 bits (anciens ordinateurs)" -ForegroundColor Gray
Write-Host ""
Write-Host "Taille du téléchargement : ~50 Mo" -ForegroundColor Gray
Write-Host "Temps d'installation : ~1-2 minutes" -ForegroundColor Gray
Write-Host ""
Write-Host "Support : https://github.com/kduchevreuil/Formation-C-Sharp" -ForegroundColor Gray
Write-Host ""
