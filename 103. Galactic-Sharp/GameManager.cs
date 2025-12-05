using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using System.Collections.Generic;
using System.Linq;

namespace _103._Galactic_Sharp
{
    public enum GameState
    {
        WaitingForPlayers,
        Countdown,
        Playing,
        GameOver
    }

    public class GameManager
    {
        private List<Player> _players;
        public List<Player> Players => _players;
        private List<Projectile> _projectiles;
        public List<Projectile> Projectiles => _projectiles;
        private Texture2D _projectileTexture;
        private List<Texture2D> _shipTextures; // Garde une copie des textures
        private List<Texture2D> _availableShips;
        private string _statusMessage;
        private GraphicsDevice _graphicsDevice;

        // État du jeu
        public GameState CurrentState { get; private set; } = GameState.WaitingForPlayers;

        // Countdown
        private float _countdownTimer;
        private int _lastCountdownNumber = -1;
        private const float CountdownDuration = 4f;

        // GameOver
        private Player _winner;
        private float _gameOverTimer;
        private float _gameOverFadeIn;
        private float _blinkTimer;
        private bool _blinkVisible = true;
        private float _winnerRotation;
        private bool[] _startWasPressed = new bool[4]; // Pour éviter le rebond

        // Son countdown (optionnel)
        private SoundEffect _textSound;

        public GameManager(GraphicsDevice graphicsDevice)
        {
            _graphicsDevice = graphicsDevice;
            _projectiles = new List<Projectile>();

            // Création texture projectile (Cercle avec Halo)
            int projSize = 32; // Texture plus grande pour le halo
            _projectileTexture = new Texture2D(graphicsDevice, projSize, projSize);
            Color[] projData = new Color[projSize * projSize];
            Vector2 projCenter = new Vector2(projSize / 2f, projSize / 2f);
            float maxRadius = projSize / 2f;

            for (int y = 0; y < projSize; y++)
            {
                for (int x = 0; x < projSize; x++)
                {
                    float dist = Vector2.Distance(new Vector2(x + 0.5f, y + 0.5f), projCenter);
                    float normalizedDist = dist / maxRadius;

                    if (normalizedDist < 1f)
                    {
                        // Cœur solide (20% du rayon)
                        // Halo dégradé (reste)
                        float alpha = 1f;
                        if (normalizedDist > 0.2f)
                        {
                            alpha = 1f - (normalizedDist - 0.2f) / 0.8f;
                            alpha = alpha * alpha; // Atténuation quadratique
                        }
                        projData[y * projSize + x] = Color.White * alpha;
                    }
                    else
                    {
                        projData[y * projSize + x] = Color.Transparent;
                    }
                }
            }
            _projectileTexture.SetData(projData);

            // Création texture lumière (Gradient radial)
            int size = 64;
            Texture2D lightTexture = new Texture2D(graphicsDevice, size, size);
            Color[] data = new Color[size * size];
            Vector2 center = new Vector2(size / 2f, size / 2f);
            float radius = size / 2f;

            for (int y = 0; y < size; y++)
            {
                for (int x = 0; x < size; x++)
                {
                    float distance = Vector2.Distance(new Vector2(x + 0.5f, y + 0.5f), center);
                    float alpha = 1f - (distance / radius);
                    if (alpha < 0) alpha = 0;
                    alpha = alpha * alpha; // Atténuation quadratique pour un effet plus doux
                    data[y * size + x] = Color.White * alpha;
                }
            }
            lightTexture.SetData(data);

            // Création texture bouclier (Cercle fin)
            Texture2D shieldTexture = new Texture2D(graphicsDevice, size, size);
            Color[] shieldData = new Color[size * size];
            float shieldRadius = size / 2f - 2f; // Marge
            float thickness = 2f; // Très fin

            for (int y = 0; y < size; y++)
            {
                for (int x = 0; x < size; x++)
                {
                    Vector2 pos = new Vector2(x, y);
                    float dist = Vector2.Distance(pos, center);

                    // Cercle complet
                    float distFromRing = System.Math.Abs(dist - shieldRadius);
                    if (distFromRing < thickness)
                    {
                        float alpha = 1f - (distFromRing / thickness);
                        // Halo effect
                        shieldData[y * size + x] = Color.White * alpha;
                    }
                    else
                    {
                        shieldData[y * size + x] = Color.Transparent;
                    }
                }
            }
            shieldTexture.SetData(shieldData);

            _players = new List<Player>
            {
                new Player(PlayerIndex.One),
                new Player(PlayerIndex.Two)
            };

            // Assigner les textures aux joueurs
            foreach (var p in _players)
            {
                p.SetLightTexture(lightTexture);
                p.SetShieldTexture(shieldTexture);
            }

            _availableShips = new List<Texture2D>();
            _statusMessage = "Initialisation...";
        }

        public void SetEngineSound(SoundEffect sound)
        {
            foreach (var player in _players)
            {
                player.SetEngineSound(sound);
            }
        }

        public void SetFireSound(SoundEffect sound)
        {
            foreach (var player in _players)
            {
                player.SetFireSound(sound);
            }
        }

        public void SetTextSound(SoundEffect sound)
        {
            _textSound = sound;
            foreach (var player in _players)
            {
                player.SetTextSound(sound);
            }
        }

        public void LoadContent(List<Texture2D> ships)
        {
            _shipTextures = new List<Texture2D>(ships); // Garder une copie
            _availableShips = new List<Texture2D>(ships);
        }

        public void Update(GameTime gameTime)
        {
            float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;

            switch (CurrentState)
            {
                case GameState.WaitingForPlayers:
                    UpdateWaitingForPlayers(gameTime);
                    break;

                case GameState.Countdown:
                    UpdateCountdown(gameTime);
                    break;

                case GameState.Playing:
                    UpdatePlaying(gameTime);
                    break;

                case GameState.GameOver:
                    UpdateGameOver(gameTime);
                    break;
            }
        }

        private void UpdateWaitingForPlayers(GameTime gameTime)
        {
            bool anyControllerConnected = false;
            int activePlayersCount = _players.Count(p => p.IsActive);

            // Vérifier l'état des manettes
            for (int i = 0; i < 4; i++)
            {
                GamePadState state = GamePad.GetState((PlayerIndex)i);
                if (state.IsConnected)
                {
                    anyControllerConnected = true;

                    // Gestion du rebond du bouton Start
                    bool startPressed = state.Buttons.Start == ButtonState.Pressed;
                    if (startPressed && !_startWasPressed[i])
                    {
                        HandlePlayerJoin((PlayerIndex)i);
                    }
                    _startWasPressed[i] = startPressed;
                }
            }

            // Vérifier si 2 joueurs sont prêts -> lancer le countdown
            activePlayersCount = _players.Count(p => p.IsActive);
            if (activePlayersCount >= 2)
            {
                StartCountdown();
                return;
            }

            // Mise à jour du message
            if (!anyControllerConnected)
            {
                _statusMessage = "Aucune manette detectee";
            }
            else if (activePlayersCount == 0)
            {
                _statusMessage = "Appuyer sur Start";
            }
            else if (activePlayersCount == 1)
            {
                _statusMessage = "En attente d'un autre joueur...";
            }
        }

        private void StartCountdown()
        {
            CurrentState = GameState.Countdown;
            _countdownTimer = 0f;
            _lastCountdownNumber = -1;
            _statusMessage = "";
        }

        private void UpdateCountdown(GameTime gameTime)
        {
            float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;
            _countdownTimer += dt;

            // Calculer le numéro actuel
            int countdownNumber = 3 - (int)_countdownTimer;

            // Jouer un son à chaque changement de chiffre
            if (countdownNumber != _lastCountdownNumber && countdownNumber >= 0)
            {
                _textSound?.Play(0.5f, 0f, 0f);
                _lastCountdownNumber = countdownNumber;
            }

            // Fin du countdown
            if (_countdownTimer >= CountdownDuration)
            {
                CurrentState = GameState.Playing;
                _statusMessage = "";
            }
        }

        private void UpdatePlaying(GameTime gameTime)
        {
            // Mise à jour physique des joueurs
            foreach (var player in _players)
            {
                player.Update(gameTime, _projectiles);
            }

            // Mise à jour des projectiles
            for (int i = _projectiles.Count - 1; i >= 0; i--)
            {
                _projectiles[i].Update(gameTime);
                if (!_projectiles[i].IsActive)
                {
                    _projectiles.RemoveAt(i);
                }
            }

            // Vérifier les conditions de victoire
            CheckVictoryCondition();
        }

        private void CheckVictoryCondition()
        {
            foreach (var player in _players)
            {
                if (player.IsActive && player.IsDead)
                {
                    // L'autre joueur gagne
                    _winner = _players.FirstOrDefault(p => p.Index != player.Index && p.IsActive);
                    StartGameOver();
                    return;
                }
            }
        }

        private void StartGameOver()
        {
            CurrentState = GameState.GameOver;
            _gameOverTimer = 0f;
            _gameOverFadeIn = 0f;
            _blinkTimer = 0f;
            _blinkVisible = true;
            _winnerRotation = _winner?.Rotation ?? 0f;
        }

        private void UpdateGameOver(GameTime gameTime)
        {
            float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;
            _gameOverTimer += dt;

            // Fade-in du texte (1 seconde)
            if (_gameOverFadeIn < 1f)
            {
                _gameOverFadeIn += dt;
                if (_gameOverFadeIn > 1f) _gameOverFadeIn = 1f;
            }

            // Clignotement du texte "START pour rejouer"
            _blinkTimer += dt;
            if (_blinkTimer >= 0.5f)
            {
                _blinkTimer = 0f;
                _blinkVisible = !_blinkVisible;
            }

            // Rotation lente du vaisseau gagnant
            _winnerRotation += dt * 1.5f;

            // Vérifier si un joueur appuie sur Start pour rejouer
            for (int i = 0; i < 4; i++)
            {
                GamePadState state = GamePad.GetState((PlayerIndex)i);
                if (state.IsConnected)
                {
                    bool startPressed = state.Buttons.Start == ButtonState.Pressed;
                    if (startPressed && !_startWasPressed[i])
                    {
                        RestartGame();
                        return;
                    }
                    _startWasPressed[i] = startPressed;
                }
            }
        }

        private void RestartGame()
        {
            // Réinitialiser les joueurs
            int screenWidth = _graphicsDevice.PresentationParameters.BackBufferWidth;
            int screenHeight = _graphicsDevice.PresentationParameters.BackBufferHeight;

            foreach (var player in _players)
            {
                if (player.IsActive)
                {
                    Vector2 position;
                    float rotation;

                    if (player.Index == PlayerIndex.One)
                    {
                        position = new Vector2(screenWidth * 0.2f, screenHeight / 2);
                        rotation = 0f;
                    }
                    else
                    {
                        position = new Vector2(screenWidth * 0.8f, screenHeight / 2);
                        rotation = (float)System.Math.PI;
                    }

                    player.Reset(position, rotation);
                }
            }

            // Vider les projectiles
            _projectiles.Clear();

            // Réinitialiser les variables
            _winner = null;
            _gameOverTimer = 0f;

            // Relancer le countdown
            StartCountdown();
        }

        private void HandlePlayerJoin(PlayerIndex index)
        {
            // On ne gère que 2 joueurs pour cet exercice
            if (index != PlayerIndex.One && index != PlayerIndex.Two) return;

            Player player = _players.FirstOrDefault(p => p.Index == index);

            if (player != null && !player.IsActive && _availableShips.Count > 0)
            {
                // Assigner un vaisseau
                Texture2D ship = _availableShips[0];
                _availableShips.RemoveAt(0);

                // Positionner le vaisseau
                Vector2 position = Vector2.Zero;
                int screenWidth = _graphicsDevice.PresentationParameters.BackBufferWidth;
                int screenHeight = _graphicsDevice.PresentationParameters.BackBufferHeight;

                if (player.Index == PlayerIndex.One)
                {
                    // Gauche
                    position = new Vector2(screenWidth * 0.2f, screenHeight / 2);
                }
                else if (player.Index == PlayerIndex.Two)
                {
                    // Droite
                    position = new Vector2(screenWidth * 0.8f, screenHeight / 2);
                }

                player.Activate(ship, position);
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            int screenWidth = _graphicsDevice.PresentationParameters.BackBufferWidth;
            int screenHeight = _graphicsDevice.PresentationParameters.BackBufferHeight;

            // Dessiner les joueurs (sauf en GameOver où on dessine le gagnant différemment)
            if (CurrentState != GameState.GameOver)
            {
                foreach (var player in _players)
                {
                    player.Draw(spriteBatch);
                }
            }
            else
            {
                // En GameOver, dessiner les joueurs normalement (le perdant est "mort")
                foreach (var player in _players)
                {
                    if (player != _winner)
                    {
                        player.Draw(spriteBatch);
                    }
                }
            }

            // Affichage des projectiles
            foreach (var proj in _projectiles)
            {
                proj.Draw(spriteBatch, _projectileTexture);
            }

            // Affichage selon l'état
            switch (CurrentState)
            {
                case GameState.WaitingForPlayers:
                    DrawStatusMessage(spriteBatch, screenWidth);
                    break;

                case GameState.Countdown:
                    DrawCountdown(spriteBatch, screenWidth, screenHeight);
                    break;

                case GameState.Playing:
                    // Rien de spécial
                    break;

                case GameState.GameOver:
                    DrawGameOver(spriteBatch, screenWidth, screenHeight);
                    break;
            }
        }

        private void DrawStatusMessage(SpriteBatch spriteBatch, int screenWidth)
        {
            Vector2 textSize = TextRenderer.MeasureString(_statusMessage);
            Vector2 textPos = new Vector2((screenWidth - textSize.X) / 2, 50);
            TextRenderer.DrawText(spriteBatch, _statusMessage, textPos);
        }

        private void DrawCountdown(SpriteBatch spriteBatch, int screenWidth, int screenHeight)
        {
            int countdownNumber = 3 - (int)_countdownTimer;
            string text;

            if (countdownNumber > 0)
            {
                text = countdownNumber.ToString();
            }
            else if (_countdownTimer < CountdownDuration)
            {
                text = "GO!";
            }
            else
            {
                return;
            }

            // Afficher au centre, grande taille
            Vector2 textSize = TextRenderer.MeasureString(text, 10);
            Vector2 textPos = new Vector2(
                (screenWidth - textSize.X) / 2,
                (screenHeight - textSize.Y) / 2
            );

            TextRenderer.DrawText(spriteBatch, text, textPos, Color.White, 10);
        }

        private void DrawGameOver(SpriteBatch spriteBatch, int screenWidth, int screenHeight)
        {
            Color fadeColor = Color.White * _gameOverFadeIn;

            // Titre : "JOUEUR X GAGNE !"
            string winnerText = _winner != null
                ? $"JOUEUR {(_winner.Index == PlayerIndex.One ? "1" : "2")} GAGNE !"
                : "MATCH NUL !";

            Vector2 titleSize = TextRenderer.MeasureString(winnerText, 6);
            Vector2 titlePos = new Vector2(
                (screenWidth - titleSize.X) / 2,
                screenHeight * 0.2f
            );
            TextRenderer.DrawText(spriteBatch, winnerText, titlePos, fadeColor, 6);

            // Vaisseau gagnant qui tourne au centre
            if (_winner != null && _winner.ShipTexture != null)
            {
                Vector2 shipPos = new Vector2(screenWidth / 2, screenHeight / 2);
                Vector2 origin = new Vector2(_winner.ShipTexture.Width / 2, _winner.ShipTexture.Height / 2);

                spriteBatch.Draw(
                    _winner.ShipTexture,
                    shipPos,
                    null,
                    Color.White * _gameOverFadeIn,
                    _winnerRotation,
                    origin,
                    2f, // Scale x2
                    SpriteEffects.None,
                    0f
                );
            }

            // Texte clignotant "START POUR REJOUER"
            if (_blinkVisible && _gameOverFadeIn >= 1f)
            {
                string restartText = "START POUR REJOUER";
                Vector2 restartSize = TextRenderer.MeasureString(restartText, 3);
                Vector2 restartPos = new Vector2(
                    (screenWidth - restartSize.X) / 2,
                    screenHeight * 0.8f
                );
                TextRenderer.DrawText(spriteBatch, restartText, restartPos, Color.Yellow, 3);
            }
        }
    }
}