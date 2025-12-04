using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace _103._Galactic_Sharp
{
    public enum ProjectileType
    {
        GreenStraight,
        GreenOscillate,
        RedSpread,
        BlueOrbit,
        YellowConverge
    }

    public class Projectile
    {
        public Vector2 Position;
        public Vector2 Velocity; // Used for the "Center" movement in Orbit type, or actual movement in others
        public Color Color;
        public bool IsActive;
        public PlayerIndex OwnerIndex;
        public float Radius = 2f; // Réduit de 3f à 2f (Collision)

        public ProjectileType Type;

        // Lifetime
        public float TimeAlive;
        private const float MaxLifetime = 2.0f;

        // Orbit specifics
        public float OrbitAngle;
        public float OrbitRadius;
        public float OrbitSpeed;
        public Vector2 OrbitCenter;

        // Oscillation specifics
        public float OscillationAmp;
        public float OscillationFreq;
        public float OscillationPhase;

        // Trail
        private Queue<Vector2> _trail;
        private const int TrailLength = 10;

        public Projectile(Vector2 position, Vector2 velocity, Color color, PlayerIndex owner, ProjectileType type)
        {
            Position = position;
            Velocity = velocity;
            Color = color;
            OwnerIndex = owner;
            Type = type;
            IsActive = true;
            TimeAlive = 0f;
            _trail = new Queue<Vector2>();

            // For Orbit, Position is the starting Center
            OrbitCenter = position;
            OrbitRadius = 10f;
        }

        public void Update(GameTime gameTime)
        {
            float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;
            TimeAlive += dt;

            if (TimeAlive > MaxLifetime)
            {
                IsActive = false;
                return;
            }

            // Update Trail
            _trail.Enqueue(Position);
            if (_trail.Count > TrailLength)
            {
                _trail.Dequeue();
            }

            // Movement Logic
            if (Type == ProjectileType.BlueOrbit)
            {
                // Move the center forward
                OrbitCenter += Velocity;

                // Expand radius
                OrbitRadius += 50f * dt; // Expands by 50px per second

                // Rotate
                OrbitAngle += OrbitSpeed * dt;

                // Calculate actual position
                Vector2 offset = new Vector2((float)System.Math.Cos(OrbitAngle), (float)System.Math.Sin(OrbitAngle)) * OrbitRadius;
                Position = OrbitCenter + offset;
            }
            else if (Type == ProjectileType.GreenOscillate)
            {
                // Linear movement
                Position += Velocity;

                // Add oscillation perpendicular to velocity
                // Right vector
                Vector2 right = new Vector2(-Velocity.Y, Velocity.X);
                if (right != Vector2.Zero) right.Normalize();

                // Derivative of Sin(wt + phi) is w * Cos(wt + phi)
                // We add the displacement delta
                float angle = OscillationFreq * TimeAlive + OscillationPhase;
                float displacement = OscillationAmp * (float)System.Math.Cos(angle) * OscillationFreq * dt;

                Position += right * displacement;
            }
            else
            {
                // Standard linear movement
                Position += Velocity;
            }
        }

        public void Draw(SpriteBatch spriteBatch, Texture2D texture)
        {
            float lifeRatio = 1f - (TimeAlive / MaxLifetime);
            float alpha = lifeRatio;
            float scale = 0.5f; // Échelle réduite pour texture 32px (donne 16px visuel avec halo)

            Vector2 origin = new Vector2(texture.Width / 2f, texture.Height / 2f);

            // Draw Trail
            int i = 0;
            foreach (var pos in _trail)
            {
                float trailAlpha = (float)i / _trail.Count * alpha * 0.3f; // Traînée plus subtile
                float trailScale = (float)i / _trail.Count * scale * 0.8f;
                spriteBatch.Draw(texture, pos, null, Color * trailAlpha, 0f, origin, trailScale, SpriteEffects.None, 0f);
                i++;
            }

            // Draw Projectile
            // Dessin avec Additive blending simulé par la texture halo
            spriteBatch.Draw(texture, Position, null, Color * alpha, 0f, origin, scale, SpriteEffects.None, 0f);
        }
    }
}