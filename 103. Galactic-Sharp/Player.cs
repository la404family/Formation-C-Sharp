using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;

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
        public float Radius { get; private set; } = 20f; // Rayon de collision par défaut
        public float Mass { get; private set; } = 1f;

        private const float Acceleration = 4f;
        private const float TurnSpeed = 3f;
        private const float Friction = 0.98f;

        // État de désorientation
        private float _disorientationTimer = 0f;
        private float _spinVelocity = 0f;

        // Lumières
        private Texture2D _lightTexture;
        private float _leftThrust;
        private float _rightThrust;

        // Boucliers
        private Texture2D _shieldTexture;
        private float _leftShieldTimer = 0f;
        private float _rightShieldTimer = 0f;
        private const float ShieldDuration = 2.0f;
        private const float ShieldFadeIn = 0.5f;

        // Sons
        private SoundEffectInstance _leftEngineSound;
        private SoundEffectInstance _rightEngineSound;
        private SoundEffect _fireSound;

        // Tir
        private float _fireCooldown = 0f;
        private const float FireRate = 0.5f; // 2 tirs par seconde max

        // Vie
        public float Health { get; private set; } = 100f;
        private float _displayedHealth = 100f;
        private float _healthDisplayTimer = 3.0f; // Affiche au démarrage
        private const float HealthDisplayDuration = 3.0f;
        private const float HealthFadeDuration = 1.0f;
        private SoundEffect _textSound;
        private float _healthTickTimer = 0f;

        public Player(PlayerIndex index)
        {
            Index = index;
            IsActive = false;
        }

        public void SetTextSound(SoundEffect sound)
        {
            _textSound = sound;
        }

        public void TakeDamage(float amount)
        {
            Health -= amount;
            if (Health < 0) Health = 0;

            // Réinitialiser le timer d'affichage
            _healthDisplayTimer = HealthDisplayDuration + HealthFadeDuration;
        }

        public void SetFireSound(SoundEffect sound)
        {
            _fireSound = sound;
        }

        public void Disorient(float duration, float spinIntensity)
        {
            _disorientationTimer = duration;
            _spinVelocity = spinIntensity;
        }

        public void SetLightTexture(Texture2D texture)
        {
            _lightTexture = texture;
        }

        public void SetShieldTexture(Texture2D texture)
        {
            _shieldTexture = texture;
        }

        public bool IsShieldActive(int side)
        {
            // side: -1 (Left), 1 (Right)
            if (side == -1) return _leftShieldTimer > 0;
            if (side == 1) return _rightShieldTimer > 0;
            return false;
        }

        public void SetEngineSound(SoundEffect sound)
        {
            if (sound == null) return;

            _leftEngineSound = sound.CreateInstance();
            _leftEngineSound.IsLooped = true;

            _rightEngineSound = sound.CreateInstance();
            _rightEngineSound.IsLooped = true;
        }

        public void Activate(Texture2D texture, Vector2 position)
        {
            ShipTexture = texture;
            Position = position;
            IsActive = true;
            Velocity = Vector2.Zero;

            // Calcul du rayon basé sur la texture (moyenne largeur/hauteur / 2)
            Radius = (texture.Width + texture.Height) / 4f * 0.8f; // 0.8 pour une hitbox un peu plus petite que le sprite

            // Orientation initiale : Face à face
            // Joueur 1 (Gauche) regarde à Droite (0)
            // Joueur 2 (Droite) regarde à Gauche (PI)
            Rotation = (Index == PlayerIndex.One) ? 0f : (float)System.Math.PI;
        }

        public void Update(GameTime gameTime, System.Collections.Generic.List<Projectile> projectiles)
        {
            if (!IsActive) return;

            float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;

            // Cooldown tir
            if (_fireCooldown > 0) _fireCooldown -= dt;

            // Animation Vie
            if (_displayedHealth > Health)
            {
                _healthTickTimer -= dt;
                if (_healthTickTimer <= 0)
                {
                    _displayedHealth -= 1f;
                    _healthTickTimer = 0.05f; // Vitesse du décompte
                    _textSound?.Play(0.3f, 0f, 0f); // Volume réduit
                }
            }

            // Timer affichage
            if (_healthDisplayTimer > 0)
            {
                _healthDisplayTimer -= dt;
            }

            // Gestion de la désorientation
            if (_disorientationTimer > 0)
            {
                _disorientationTimer -= dt;
                Rotation += _spinVelocity * dt;

                // Ralentissement du spin
                _spinVelocity *= 0.95f;

                // Pendant la désorientation, on n'a pas le contrôle
                _leftThrust = 0;
                _rightThrust = 0;
            }
            else
            {
                GamePadState state = GamePad.GetState(Index);
                if (state.IsConnected)
                {
                    // Lecture des triggers (0.0 à 1.0)
                    _leftThrust = state.Triggers.Left;
                    _rightThrust = state.Triggers.Right;

                    // Gestion des boucliers (LB / RB)
                    if (state.Buttons.LeftShoulder == ButtonState.Pressed && _leftShieldTimer <= 0)
                    {
                        _leftShieldTimer = ShieldDuration;
                    }
                    if (state.Buttons.RightShoulder == ButtonState.Pressed && _rightShieldTimer <= 0)
                    {
                        _rightShieldTimer = ShieldDuration;
                    }

                    // Gestion des tirs
                    if (_fireCooldown <= 0)
                    {
                        if (state.Buttons.A == ButtonState.Pressed)
                        {
                            FireGreen(projectiles);
                            _fireCooldown = FireRate;
                        }
                        else if (state.Buttons.B == ButtonState.Pressed)
                        {
                            FireRed(projectiles);
                            _fireCooldown = FireRate;
                        }
                        else if (state.Buttons.X == ButtonState.Pressed)
                        {
                            FireBlue(projectiles);
                            _fireCooldown = FireRate;
                        }
                        else if (state.Buttons.Y == ButtonState.Pressed)
                        {
                            FireYellow(projectiles);
                            _fireCooldown = FireRate;
                        }
                    }

                    // 1. Rotation (Différence entre les poussées)
                    float rotationChange = (_leftThrust - _rightThrust) * TurnSpeed * dt;
                    Rotation += rotationChange;

                    // 2. Déplacement (Somme des poussées)
                    float totalThrust = (_leftThrust + _rightThrust) * Acceleration * dt;

                    Vector2 direction = new Vector2((float)System.Math.Cos(Rotation), (float)System.Math.Sin(Rotation));
                    Velocity += direction * totalThrust;
                }
            }

            // Mise à jour des timers boucliers
            if (_leftShieldTimer > 0) _leftShieldTimer -= dt;
            if (_rightShieldTimer > 0) _rightShieldTimer -= dt;

            // Friction
            Velocity *= Friction;            // Mise à jour position
            Position += Velocity;

            // Gestion du son
            UpdateEngineSound(_leftEngineSound, _leftThrust);
            UpdateEngineSound(_rightEngineSound, _rightThrust);
        }

        private void UpdateEngineSound(SoundEffectInstance sound, float thrust)
        {
            if (sound == null) return;

            if (thrust > 0.05f) // Petite zone morte pour éviter le bruit
            {
                if (sound.State != SoundState.Playing)
                    sound.Play();

                sound.Volume = thrust;
                // Pitch varie légèrement avec la poussée pour plus de réalisme (-0.5 à 0.5)
                sound.Pitch = -0.2f + (thrust * 0.4f);
            }
            else
            {
                if (sound.State == SoundState.Playing)
                    sound.Stop();
            }
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

                // Dessin des boucliers
                if (_shieldTexture != null)
                {
                    DrawShield(spriteBatch, _leftShieldTimer, -1);
                    DrawShield(spriteBatch, _rightShieldTimer, 1);
                }

                // Dessin du vaisseau
                spriteBatch.Draw(ShipTexture, Position, null, Color, Rotation, origin, 1f, SpriteEffects.None, 0f);

                // Dessin Vie
                if (_healthDisplayTimer > 0)
                {
                    DrawHealth(spriteBatch);
                }
            }
        }

        private void DrawHealth(SpriteBatch spriteBatch)
        {
            string text = $"{(int)_displayedHealth}%";
            Vector2 textSize = TextRenderer.MeasureString(text, 2); // Scale 2 to match DrawText

            // Position : À côté du vaisseau
            // On le place à "Droite" du vaisseau (axe Y local positif)
            Vector2 offset = new Vector2(0, 50);
            Vector2 rotatedOffset = Vector2.Transform(offset, Matrix.CreateRotationZ(Rotation));
            Vector2 textPos = Position + rotatedOffset - textSize / 2f;

            // Clamp to screen (1280x720)
            // Idéalement on devrait passer les bornes, mais on va utiliser des constantes pour l'instant
            float margin = 10f;
            float minX = margin;
            float maxX = 1280 - margin - textSize.X;
            float minY = margin;
            float maxY = 720 - margin - textSize.Y;

            textPos.X = MathHelper.Clamp(textPos.X, minX, maxX);
            textPos.Y = MathHelper.Clamp(textPos.Y, minY, maxY);

            // Fade out
            float alpha = 1f;
            if (_healthDisplayTimer < HealthFadeDuration)
            {
                alpha = _healthDisplayTimer / HealthFadeDuration;
            }

            TextRenderer.DrawText(spriteBatch, text, textPos, Color.White * alpha, 2); // Scale 2 for small text
        }

        private void DrawShield(SpriteBatch spriteBatch, float timer, int side)
        {
            if (timer <= 0) return;

            float alpha = 1f;
            // Fade In (0.5s)
            if (timer > (ShieldDuration - ShieldFadeIn))
            {
                alpha = (ShieldDuration - timer) / ShieldFadeIn;
            }
            // Fade Out (last 0.2s)
            else if (timer < 0.2f)
            {
                alpha = timer / 0.2f;
            }

            Vector2 origin = new Vector2(_shieldTexture.Width / 2f, _shieldTexture.Height / 2f);

            // Calcul de la rotation du bouclier
            // La texture est une parenthèse ')' orientée vers la Droite (0 rad).
            // Side -1 (Gauche du vaisseau) -> Doit être orienté vers le Haut (-PI/2) par rapport au vaisseau.
            // Side 1 (Droite du vaisseau) -> Doit être orienté vers le Bas (+PI/2) par rapport au vaisseau.
            float rotationOffset = side * MathHelper.PiOver2;
            float shieldRotation = Rotation + rotationOffset;

            // Calcul de la position
            // On veut que l'arc du bouclier soit juste à l'extérieur du vaisseau.
            // Rayon de l'arc dans la texture ~ 28px (32 - 4).
            float shieldTextureRadius = _shieldTexture.Width / 2f - 4f;
            float shipRadius = ShipTexture.Width / 2f;
            float margin = 5f;

            // On doit décaler le centre de la texture pour que l'arc tombe au bon endroit
            float distOffset = (shipRadius + margin) - shieldTextureRadius;

            // Le décalage se fait sur l'axe Y local (Gauche/Droite du vaisseau)
            // Side -1 (Gauche) -> Y négatif
            // Side 1 (Droite) -> Y positif
            Vector2 localOffset = new Vector2(0, side * distOffset);

            // Rotation du décalage selon l'orientation du vaisseau
            Vector2 worldOffset = Vector2.Transform(localOffset, Matrix.CreateRotationZ(Rotation));

            Vector2 pos = Position + worldOffset;

            spriteBatch.Draw(_shieldTexture, pos, null, Color.Purple * alpha, shieldRotation, origin, 1f, SpriteEffects.None, 0f);
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

        private void FireGreen(System.Collections.Generic.List<Projectile> projectiles)
        {
            // 3 boules vertes
            Vector2 direction = new Vector2((float)System.Math.Cos(Rotation), (float)System.Math.Sin(Rotation));
            float speed = 10f;

            // Centre (Droit)
            projectiles.Add(new Projectile(Position + direction * 20, direction * speed, Color.Lime, Index, ProjectileType.GreenStraight));

            // Gauche (Oscille)
            var pLeft = new Projectile(Position + direction * 20, direction * speed, Color.Lime, Index, ProjectileType.GreenOscillate);
            pLeft.OscillationAmp = 20f;
            pLeft.OscillationFreq = 10f;
            pLeft.OscillationPhase = 0f;
            projectiles.Add(pLeft);

            // Droite (Oscille en opposition de phase)
            var pRight = new Projectile(Position + direction * 20, direction * speed, Color.Lime, Index, ProjectileType.GreenOscillate);
            pRight.OscillationAmp = 20f;
            pRight.OscillationFreq = 10f;
            pRight.OscillationPhase = (float)System.Math.PI; // Opposition de phase
            projectiles.Add(pRight);

            _fireSound?.Play();
        }

        private void FireRed(System.Collections.Generic.List<Projectile> projectiles)
        {
            // Triangle : 1 milieu, 2 à +/- 30 degrés
            float speed = 10f;
            float angle30 = MathHelper.ToRadians(30);

            // Centre
            Vector2 dirCenter = new Vector2((float)System.Math.Cos(Rotation), (float)System.Math.Sin(Rotation));
            projectiles.Add(new Projectile(Position + dirCenter * 20, dirCenter * speed, Color.Red, Index, ProjectileType.RedSpread));

            // +30 deg
            Vector2 dirLeft = new Vector2((float)System.Math.Cos(Rotation - angle30), (float)System.Math.Sin(Rotation - angle30));
            projectiles.Add(new Projectile(Position + dirLeft * 20, dirLeft * speed, Color.Red, Index, ProjectileType.RedSpread));

            // -30 deg
            Vector2 dirRight = new Vector2((float)System.Math.Cos(Rotation + angle30), (float)System.Math.Sin(Rotation + angle30));
            projectiles.Add(new Projectile(Position + dirRight * 20, dirRight * speed, Color.Red, Index, ProjectileType.RedSpread));

            _fireSound?.Play();
        }

        private void FireBlue(System.Collections.Generic.List<Projectile> projectiles)
        {
            // Rotation autour d'un point central
            // On lance 3 projectiles qui vont orbiter autour d'un centre virtuel qui avance
            Vector2 direction = new Vector2((float)System.Math.Cos(Rotation), (float)System.Math.Sin(Rotation));
            float speed = 8f;
            Vector2 centerVelocity = direction * speed;
            Vector2 startPos = Position + direction * 30;

            for (int i = 0; i < 3; i++)
            {
                var p = new Projectile(startPos, centerVelocity, Color.Cyan, Index, ProjectileType.BlueOrbit);
                p.OrbitAngle = i * (MathHelper.TwoPi / 3f); // Répartis équitablement (0, 120, 240 deg)
                p.OrbitSpeed = 5f; // Vitesse de rotation
                projectiles.Add(p);
            }

            _fireSound?.Play();
        }

        private void FireYellow(System.Collections.Generic.List<Projectile> projectiles)
        {
            // Divergent en éventail depuis le même point
            float speed = 10f;
            float spreadAngle = MathHelper.ToRadians(15); // 15 degrés d'écart

            // Centre
            Vector2 dirCenter = new Vector2((float)System.Math.Cos(Rotation), (float)System.Math.Sin(Rotation));
            projectiles.Add(new Projectile(Position + dirCenter * 20, dirCenter * speed, Color.Yellow, Index, ProjectileType.YellowConverge));

            // Gauche (-15 deg)
            Vector2 dirLeft = new Vector2((float)System.Math.Cos(Rotation - spreadAngle), (float)System.Math.Sin(Rotation - spreadAngle));
            projectiles.Add(new Projectile(Position + dirLeft * 20, dirLeft * speed, Color.Yellow, Index, ProjectileType.YellowConverge));

            // Droite (+15 deg)
            Vector2 dirRight = new Vector2((float)System.Math.Cos(Rotation + spreadAngle), (float)System.Math.Sin(Rotation + spreadAngle));
            projectiles.Add(new Projectile(Position + dirRight * 20, dirRight * speed, Color.Yellow, Index, ProjectileType.YellowConverge));

            _fireSound?.Play();
        }
    }
}