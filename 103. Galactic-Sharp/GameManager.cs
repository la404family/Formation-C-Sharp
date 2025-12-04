using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using System.Collections.Generic;
using System.Linq;

namespace _103._Galactic_Sharp
{
    public class GameManager
    {
        private List<Player> _players;
        public List<Player> Players => _players;
        private List<Projectile> _projectiles;
        public List<Projectile> Projectiles => _projectiles;
        private Texture2D _projectileTexture;
        private List<Texture2D> _availableShips;
        private string _statusMessage;
        private GraphicsDevice _graphicsDevice;

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

            // Création texture bouclier (Arc de cercle)
            Texture2D shieldTexture = new Texture2D(graphicsDevice, size, size);
            Color[] shieldData = new Color[size * size];
            float shieldRadius = size / 2f - 4f; // Marge
            float thickness = 4f;

            for (int y = 0; y < size; y++)
            {
                for (int x = 0; x < size; x++)
                {
                    Vector2 pos = new Vector2(x, y);
                    float dist = Vector2.Distance(pos, center);

                    // Angle pour faire un arc (Côté Droit ')' )
                    // Atan2(y, x) : 0 est à droite.
                    float angle = (float)System.Math.Atan2(y - center.Y, x - center.X);

                    // On veut un arc entre -45 et +45 degrés (-PI/4 et PI/4)
                    if (System.Math.Abs(angle) < System.Math.PI / 4f)
                    {
                        // Distance check with soft edges
                        float distFromRing = System.Math.Abs(dist - shieldRadius);
                        if (distFromRing < thickness)
                        {
                            float alpha = 1f - (distFromRing / thickness);
                            // Halo effect
                            shieldData[y * size + x] = Color.White * alpha;
                        }
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
            foreach (var player in _players)
            {
                player.SetTextSound(sound);
            }
        }

        public void LoadContent(List<Texture2D> ships)
        {
            _availableShips = ships;
        }

        public void Update(GameTime gameTime)
        {
            bool anyControllerConnected = false;
            int activePlayersCount = _players.Count(p => p.IsActive);

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

            // Vérifier l'état des manettes
            for (int i = 0; i < 4; i++) // MonoGame supporte jusqu'à 4 manettes
            {
                GamePadState state = GamePad.GetState((PlayerIndex)i);
                if (state.IsConnected)
                {
                    anyControllerConnected = true;

                    // Si le joueur appuie sur Start
                    if (state.Buttons.Start == ButtonState.Pressed)
                    {
                        HandlePlayerJoin((PlayerIndex)i);
                    }
                }
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
            else
            {
                _statusMessage = "";
            }
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
            // Dessiner les joueurs
            foreach (var player in _players)
            {
                player.Draw(spriteBatch);
            }

            // Affichage des projectiles
            foreach (var proj in _projectiles)
            {
                proj.Draw(spriteBatch, _projectileTexture);
            }

            // Dessiner le message en haut au centre
            Vector2 textSize = TextRenderer.MeasureString(_statusMessage);
            int screenWidth = _graphicsDevice.PresentationParameters.BackBufferWidth;
            Vector2 textPos = new Vector2((screenWidth - textSize.X) / 2, 50);

            TextRenderer.DrawText(spriteBatch, _statusMessage, textPos);
        }
    }
}