using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace _103._Galactic_Sharp
{
    public class Player
    {
        public PlayerIndex Index { get; private set; }
        public Texture2D ShipTexture { get; set; }
        public Vector2 Position { get; set; }
        public bool IsActive { get; set; }
        public Color Color { get; set; } = Color.White;

        // Physique
        public Vector2 Velocity;
        public float Rotation; // En radians
        private const float Acceleration = 4f; // Vitesse réduite (était 10f)
        private const float TurnSpeed = 3f;
        private const float Friction = 0.98f;

        // Lumières
        private Texture2D _lightTexture;
        private float _leftThrust;
        private float _rightThrust;

        public Player(PlayerIndex index)
        {
            Index = index;
            IsActive = false;
        }

        public void SetLightTexture(Texture2D texture)
        {
            _lightTexture = texture;
        }

        public void Activate(Texture2D texture, Vector2 position)
        {
            ShipTexture = texture;
            Position = position;
            IsActive = true;
            Velocity = Vector2.Zero;

            // Orientation initiale : Face à face
            // Joueur 1 (Gauche) regarde à Droite (0)
            // Joueur 2 (Droite) regarde à Gauche (PI)
            Rotation = (Index == PlayerIndex.One) ? 0f : (float)System.Math.PI;
        }

        public void Update(GameTime gameTime)
        {
            if (!IsActive) return;

            GamePadState state = GamePad.GetState(Index);
            if (!state.IsConnected) return;

            // Lecture des triggers (0.0 à 1.0)
            _leftThrust = state.Triggers.Left;
            _rightThrust = state.Triggers.Right;

            float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;

            // 1. Rotation (Différence entre les poussées)
            // LT > RT -> Pousse à gauche -> Tourne à Droite (+)
            // RT > LT -> Pousse à droite -> Tourne à Gauche (-)
            float rotationChange = (_leftThrust - _rightThrust) * TurnSpeed * dt;
            Rotation += rotationChange;

            // 2. Déplacement (Somme des poussées)
            float totalThrust = (_leftThrust + _rightThrust) * Acceleration * dt;

            Vector2 direction = new Vector2((float)System.Math.Cos(Rotation), (float)System.Math.Sin(Rotation));
            Velocity += direction * totalThrust;

            // Friction
            Velocity *= Friction;

            // Mise à jour position
            Position += Velocity;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (IsActive && ShipTexture != null)
            {
                Vector2 origin = new Vector2(ShipTexture.Width / 2, ShipTexture.Height / 2);

                // Dessin des lumières (moteurs)
                if (_lightTexture != null)
                {
                    DrawEngineLight(spriteBatch, _leftThrust, -1); // Moteur Gauche (Haut)
                    DrawEngineLight(spriteBatch, _rightThrust, 1);  // Moteur Droit (Bas)
                }

                // Dessin du vaisseau
                spriteBatch.Draw(ShipTexture, Position, null, Color, Rotation, origin, 1f, SpriteEffects.None, 0f);
            }
        }

        private void DrawEngineLight(SpriteBatch spriteBatch, float thrust, int side)
        {
            if (thrust <= 0) return;

            // Position relative du moteur par rapport au centre du vaisseau
            float backOffset = -ShipTexture.Width / 2;
            float sideOffset = (ShipTexture.Height / 4) * side; // side: -1 (Gauche/Haut), 1 (Droit/Bas)

            // Rotation du vecteur offset
            Vector2 offset = new Vector2(backOffset, sideOffset);
            Vector2 rotatedOffset = Vector2.Transform(offset, Matrix.CreateRotationZ(Rotation));

            Vector2 lightPos = Position + rotatedOffset;

            // Texture est maintenant un gradient 64x64
            Vector2 lightOrigin = new Vector2(_lightTexture.Width / 2f, _lightTexture.Height / 2f);

            // Échelle réduite : 0.2 à 0.5 selon la poussée (au lieu de 10 à 30 pixels)
            float scale = 0.2f + (thrust * 0.3f);

            // Dessin en deux couches pour un effet de "cœur" chaud
            
            // 1. Halo extérieur (Orange/Rouge)
            spriteBatch.Draw(_lightTexture, lightPos, null, Color.OrangeRed * thrust, Rotation, lightOrigin, scale, SpriteEffects.None, 0f);
            
            // 2. Cœur intérieur (Jaune/Blanc, plus petit)
            spriteBatch.Draw(_lightTexture, lightPos, null, Color.LightYellow * thrust, Rotation, lightOrigin, scale * 0.5f, SpriteEffects.None, 0f);
        }
    }
}