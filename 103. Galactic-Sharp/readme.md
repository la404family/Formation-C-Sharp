<p align="center">
  <img src="Icon.ico" alt="Galactic-Sharp Icon" width="128" height="128">
</p>

<h1 align="center">🚀 Galactic-Sharp</h1>

<p align="center">
  <strong>Jeu de combat spatial en local pour 2 joueurs avec manettes</strong>
</p>

<p align="center">
  <a href="./bin/Debug/net9.0/103. Galactic-Sharp.exe">
    <img src="https://img.shields.io/badge/Jouer-Lancer%20le%20jeu-green?style=for-the-badge&logo=gamepad" alt="Jouer">
  </a>
  <img src="https://img.shields.io/badge/.NET-9.0-512BD4?style=for-the-badge&logo=dotnet" alt=".NET 9.0">
  <img src="https://img.shields.io/badge/MonoGame-3.8-red?style=for-the-badge" alt="MonoGame">
  <img src="https://img.shields.io/badge/Joueurs-2-orange?style=for-the-badge" alt="2 Joueurs">
</p>

---

## 📥 Lancer le jeu

### Prérequis

- **Windows 10 / 11**
- **2 manettes Xbox** (ou compatibles XInput)
- **.NET 9.0 Runtime** installé

### Lancement

1. 🎮 **Branchez 2 manettes** sur votre PC
2. ▶️ **[Lancez le jeu](./bin/Debug/net9.0/103.%20Galactic-Sharp.exe)**
3. 🕹️ **Appuyez sur Start** sur chaque manette pour rejoindre
4. ⚔️ **Combattez !**

---
## 📹 Vidéo du jeu

[![Le jeu Galactic-Sharp.exe en C#](https://img.youtube.com/vi/xTc7SD2C8G0/hqdefault.jpg)](https://www.youtube.com/watch?v=xTc7SD2C8G0)

---
## 📖 Description

**Galactic-Sharp** est un jeu de combat spatial en **1 contre 1** développé en C# avec MonoGame. Affrontez votre ami dans une arène spatiale et soyez le dernier vaisseau en vol !

Chaque joueur contrôle un vaisseau avec des **propulseurs indépendants** (gauche et droite), peut activer des **boucliers défensifs** et dispose de **4 types d'armes** différentes pour détruire son adversaire.

---

## 🎮 Contrôles

| Action                     | Bouton Manette       |
| -------------------------- | -------------------- |
| **Rejoindre la partie**    | Start                |
| **Propulseur gauche**      | LT (Gâchette gauche) |
| **Propulseur droit**       | RT (Gâchette droite) |
| **Bouclier**               | LB                   |
| **Bouclier**               | RB                   |
| **Tir Vert** (oscillant)   | A                    |
| **Tir Rouge** (dispersé)   | B                    |
| **Tir Bleu** (orbital)     | X                    |
| **Tir Jaune** (convergent) | Y                    |
| **Quitter**                | Back / Échap         |

### 🕹️ Système de propulsion

Le vaisseau utilise un système de **double propulseur** :

- **LT seul** → Tourne à droite en avançant
- **RT seul** → Tourne à gauche en avançant
- **LT + RT** → Avance tout droit

---

## 🌟 Fonctionnalités

### ⚔️ Combat

- **4 types de projectiles** avec comportements uniques
- **Système de vie** en pourcentage (100% → 0%)
- **Dégâts de collision** entre vaisseaux
- **Rebonds** sur les bords de l'arène

### 🛡️ Défense

- **Boucliers**
- **Protection temporaire** contre les projectiles
- **Durée limitée** avec temps de recharge

### 🎯 Boucle de jeu

1. **Écran d'attente** : Connectez vos manettes et appuyez sur Start
2. **Compte à rebours** : 3... 2... 1... GO !
3. **Combat** : Réduisez la vie de votre adversaire à 0%
4. **Victoire** : Le gagnant est affiché avec son vaisseau
5. **Rejouer** : Appuyez sur Start pour une nouvelle partie

---

## 🎨 Ambiance

- 🌌 **Fond étoilé animé** avec effet de parallaxe
- 🔥 **Effets de lumière** sur les propulseurs
- 💥 **Sons de tir et de collision**
- 🎵 **Retour sonore** sur les dégâts

---

## 🏆 Règles du jeu

| Événement                 | Dégâts     |
| ------------------------- | ---------- |
| Collision entre vaisseaux | -5% chacun |
| Touché par un projectile  | -10%       |
| Collision avec le bord    | -2%        |

> **Astuce** : Utilisez vos boucliers pour bloquer les projectiles ennemis !

---

## 🛠️ Technologies

| Technologie       | Utilisation          |
| ----------------- | -------------------- |
| **C# / .NET 9.0** | Langage et runtime   |
| **MonoGame**      | Framework de jeu     |
| **XInput**        | Gestion des manettes |

---

## 👥 Crédits

Développé dans le cadre de la **Formation C#**

---

<p align="center">
  <strong>🚀 Bon jeu et que le meilleur gagne ! 🚀</strong>
</p>
