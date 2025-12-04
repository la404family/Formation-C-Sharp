using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using System.Collections.Generic;

namespace _103._Galactic_Sharp
{
    public class CollisionManager
    {
        private Rectangle _bounds;
        private SoundEffect _collisionSound;

        public CollisionManager(Rectangle bounds, SoundEffect collisionSound)
        {
            _bounds = bounds;
            _collisionSound = collisionSound;
        }

        public void Update(List<Player> players, List<Projectile> projectiles)
        {
            // Check Player vs Border
            foreach (var player in players)
            {
                if (!player.IsActive) continue;
                CheckBorderCollision(player);
            }

            // Check Player vs Player
            for (int i = 0; i < players.Count; i++)
            {
                for (int j = i + 1; j < players.Count; j++)
                {
                    if (players[i].IsActive && players[j].IsActive)
                    {
                        CheckPlayerCollision(players[i], players[j]);
                    }
                }
            }

            // Check Projectile vs Player
            for (int i = projectiles.Count - 1; i >= 0; i--)
            {
                var proj = projectiles[i];
                if (!proj.IsActive) continue;

                foreach (var player in players)
                {
                    if (!player.IsActive) continue;
                    if (player.Index == proj.OwnerIndex) continue; // No friendly fire

                    if (CheckProjectileHit(player, proj))
                    {
                        proj.IsActive = false; // Destroy projectile
                        break; // One hit per projectile
                    }
                }
            }
        }

        private bool CheckProjectileHit(Player player, Projectile proj)
        {
            float dist = Vector2.Distance(player.Position, proj.Position);
            if (dist < player.Radius + proj.Radius)
            {
                // Hit!
                Vector2 hitDir = Vector2.Normalize(proj.Velocity);
                if (hitDir == Vector2.Zero) hitDir = Vector2.Normalize(player.Position - proj.Position);

                // Check Shield
                if (IsShielded(player, hitDir))
                {
                    // Shield blocks it
                    // Maybe play shield sound?
                }
                else
                {
                    // Impact
                    player.Velocity += hitDir * 2f; // Push
                    player.Disorient(0.2f, 10f); // Small disorientation
                    player.TakeDamage(10f); // Dégâts projectile (10%)
                    _collisionSound?.Play();
                }
                return true;
            }
            return false;
        }

        private void CheckBorderCollision(Player player)
        {
            bool collided = false;
            Vector2 normal = Vector2.Zero;
            Vector2 pos = player.Position;

            // Left
            if (pos.X - player.Radius < _bounds.Left)
            {
                pos.X = _bounds.Left + player.Radius;
                normal = new Vector2(1, 0);
                collided = true;
            }
            // Right
            else if (pos.X + player.Radius > _bounds.Right)
            {
                pos.X = _bounds.Right - player.Radius;
                normal = new Vector2(-1, 0);
                collided = true;
            }

            // Top
            if (pos.Y - player.Radius < _bounds.Top)
            {
                pos.Y = _bounds.Top + player.Radius;
                normal = new Vector2(0, 1);
                collided = true;
            }
            // Bottom
            else if (pos.Y + player.Radius > _bounds.Bottom)
            {
                pos.Y = _bounds.Bottom - player.Radius;
                normal = new Vector2(0, -1);
                collided = true;
            }

            if (collided)
            {
                player.Position = pos;
                ResolveCollision(player, normal);
                player.TakeDamage(2f); // Dégâts collision cadre (2%)
            }
        }

        private void CheckPlayerCollision(Player p1, Player p2)
        {
            float distance = Vector2.Distance(p1.Position, p2.Position);
            float minDistance = p1.Radius + p2.Radius;

            if (distance < minDistance)
            {
                // Push apart to avoid sticking
                Vector2 collisionNormal = p1.Position - p2.Position;
                if (collisionNormal == Vector2.Zero) collisionNormal = new Vector2(1, 0);
                collisionNormal.Normalize();

                float overlap = minDistance - distance;
                Vector2 separation = collisionNormal * (overlap / 2f);

                p1.Position += separation;
                p2.Position -= separation;

                ResolvePlayerBounce(p1, p2, collisionNormal);

                // Dégâts collision simple (5%)
                // Vérification des boucliers
                // P1 est frappé par P2 (direction -collisionNormal)
                if (!IsShielded(p1, -collisionNormal))
                {
                    p1.TakeDamage(5f);
                }

                // P2 est frappé par P1 (direction collisionNormal)
                if (!IsShielded(p2, collisionNormal))
                {
                    p2.TakeDamage(5f);
                }
            }
        }

        private void ResolveCollision(Player player, Vector2 normal)
        {
            // Reflect velocity
            player.Velocity = Vector2.Reflect(player.Velocity, normal);

            // Check if shielded
            // Hit comes from opposite of normal
            // Pour le cadre, le bouclier ne protège pas de la désorientation ni des dégâts
            // if (!IsShielded(player, -normal))
            {
                // Disorient only if not shielded (Désactivé pour le cadre)
                player.Disorient(0.5f, 15f); // 0.5 second, spin speed 15
            }

            // Play Sound
            _collisionSound?.Play();
        }

        private void ResolvePlayerBounce(Player p1, Player p2, Vector2 normal)
        {
            Vector2 relativeVelocity = p1.Velocity - p2.Velocity;
            float velocityAlongNormal = Vector2.Dot(relativeVelocity, normal);

            if (velocityAlongNormal > 0) return;

            float e = 1.0f; // Elasticity
            float j = -(1 + e) * velocityAlongNormal;
            j /= (1 / p1.Mass + 1 / p2.Mass);

            Vector2 impulse = j * normal;
            p1.Velocity += (1 / p1.Mass) * impulse;
            p2.Velocity -= (1 / p2.Mass) * impulse;

            // Disorient both if not shielded
            // Normal points from P2 to P1.
            // P1 is hit by P2 (Force direction is Normal). So Hit direction is -Normal?
            // Wait. Impulse on P1 is +Impulse * Normal. So P1 is pushed in direction Normal.
            // So P1 is hit from direction -Normal.
            if (!IsShielded(p1, -normal))
            {
                p1.Disorient(1.0f, 20f);
            }

            // P2 is pushed in direction -Normal.
            // So P2 is hit from direction Normal.
            if (!IsShielded(p2, normal))
            {
                p2.Disorient(1.0f, -20f);
            }

            _collisionSound?.Play();
        }

        private bool IsShielded(Player player, Vector2 hitDirection)
        {
            // Transform hit direction to local space
            // Rotate by -Rotation
            float cos = (float)System.Math.Cos(-player.Rotation);
            float sin = (float)System.Math.Sin(-player.Rotation);

            float localX = hitDirection.X * cos - hitDirection.Y * sin;
            float localY = hitDirection.X * sin + hitDirection.Y * cos;

            // Local Y < 0 is Left, > 0 is Right
            // (Assuming standard XNA coordinates where Y is Down, and 0 rot is Right)
            // Left of ship (facing Right) is Up (-Y).
            // Right of ship is Down (+Y).

            if (localY < 0) // Hit on Left
            {
                return player.IsShieldActive(-1);
            }
            else // Hit on Right
            {
                return player.IsShieldActive(1);
            }
        }
    }
}