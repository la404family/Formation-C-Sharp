using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace _103._Galactic_Sharp
{
    public class Starfield
    {
        private struct Star
        {
            public Vector2 Position;
            public float Speed;
            public float Size;
            public Color Color;
        }

        private List<Star> _stars;
        private Texture2D _pixelTexture;
        private Random _random;
        private int _screenWidth;
        private int _screenHeight;

        public Starfield(GraphicsDevice graphicsDevice, int count)
        {
            _screenWidth = graphicsDevice.PresentationParameters.BackBufferWidth;
            _screenHeight = graphicsDevice.PresentationParameters.BackBufferHeight;
            _random = new Random();
            _stars = new List<Star>();

            // Création d'une texture 1x1 blanche pour les étoiles
            _pixelTexture = new Texture2D(graphicsDevice, 1, 1);
            _pixelTexture.SetData(new[] { Color.White });

            for (int i = 0; i < count; i++)
            {
                _stars.Add(GenerateStar(true)); // true = position aléatoire partout
            }
        }

        private Star GenerateStar(bool randomX)
        {
            // Vitesse : la plupart sont lentes (fond), certaines plus rapides (premier plan)
            float speedBase = (float)(_random.NextDouble() * 0.5 + 0.2);
            bool isFast = _random.NextDouble() > 0.85; // 15% d'étoiles rapides

            float speed = isFast ? speedBase * 3.0f : speedBase;
            float size = isFast ? 2.0f : 1.0f; // Les rapides sont plus grosses (plus proches)

            // Couleur : Blanc avec légère variation d'opacité
            float opacity = (float)(_random.NextDouble() * 0.5 + 0.5);
            Color color = Color.White * opacity;

            float x = randomX ? _random.Next(_screenWidth) : _screenWidth;
            float y = _random.Next(_screenHeight);

            return new Star
            {
                Position = new Vector2(x, y),
                Speed = speed,
                Size = size,
                Color = color
            };
        }

        public void Update(GameTime gameTime)
        {
            for (int i = 0; i < _stars.Count; i++)
            {
                Star star = _stars[i];

                // Déplacement vers la gauche
                star.Position.X -= star.Speed;

                // Si l'étoile sort de l'écran à gauche, on la remet à droite
                if (star.Position.X < -5)
                {
                    star = GenerateStar(false); // false = spawn à droite
                }

                _stars[i] = star;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (var star in _stars)
            {
                spriteBatch.Draw(_pixelTexture, star.Position, null, star.Color, 0f, Vector2.Zero, star.Size, SpriteEffects.None, 0f);
            }
        }
    }
}