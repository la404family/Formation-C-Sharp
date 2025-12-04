using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System.Linq;

namespace _103._Galactic_Sharp
{
    public class GameManager
    {
        private List<Player> _players;
        private List<Texture2D> _availableShips;
        private string _statusMessage;
        private GraphicsDevice _graphicsDevice;

        public GameManager(GraphicsDevice graphicsDevice)
        {
            _graphicsDevice = graphicsDevice;

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

            _players = new List<Player>
            {
                new Player(PlayerIndex.One),
                new Player(PlayerIndex.Two)
            };

            // Assigner la texture de lumière aux joueurs
            foreach (var p in _players)
            {
                p.SetLightTexture(lightTexture);
            }

            _availableShips = new List<Texture2D>();
            _statusMessage = "Initialisation...";
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
                player.Update(gameTime);
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
                _statusMessage = "Pret au combat !";
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

            // Dessiner le message en haut au centre
            Vector2 textSize = TextRenderer.MeasureString(_statusMessage);
            int screenWidth = _graphicsDevice.PresentationParameters.BackBufferWidth;
            Vector2 textPos = new Vector2((screenWidth - textSize.X) / 2, 50);

            TextRenderer.DrawText(spriteBatch, _statusMessage, textPos);
        }
    }
}