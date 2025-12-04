using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace _103._Galactic_Sharp
{
    public class Arena
    {
        public Rectangle Bounds { get; private set; }
        private Texture2D _hGradient; // Horizontal: Blue -> Transparent
        private Texture2D _vGradient; // Vertical: Blue -> Transparent
        private int _margin = 5;
        private int _thickness = 20; // Width of the halo

        public Arena(GraphicsDevice graphicsDevice)
        {
            int width = graphicsDevice.Viewport.Width;
            int height = graphicsDevice.Viewport.Height;
            Bounds = new Rectangle(_margin, _margin, width - _margin * 2, height - _margin * 2);

            CreateTextures(graphicsDevice);
        }

        private void CreateTextures(GraphicsDevice graphicsDevice)
        {
            int size = 32;

            // Horizontal Gradient (Left=Gray -> Right=Transparent)
            _hGradient = new Texture2D(graphicsDevice, size, 1);
            Color[] hData = new Color[size];
            for (int i = 0; i < size; i++)
            {
                // Reduced intensity (0.5f alpha max)
                hData[i] = Color.Lerp(Color.Gray * 0.5f, Color.Transparent, i / (float)(size - 1));
            }
            _hGradient.SetData(hData);

            // Vertical Gradient (Top=Gray -> Bottom=Transparent)
            _vGradient = new Texture2D(graphicsDevice, 1, size);
            Color[] vData = new Color[size];
            for (int i = 0; i < size; i++)
            {
                // Reduced intensity (0.5f alpha max)
                vData[i] = Color.Lerp(Color.Gray * 0.5f, Color.Transparent, i / (float)(size - 1));
            }
            _vGradient.SetData(vData);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            // Top (Vertical Gradient, Normal)
            spriteBatch.Draw(_vGradient, new Rectangle(Bounds.X, Bounds.Y, Bounds.Width, _thickness), Color.White);

            // Bottom (Vertical Gradient, Flipped Vertically)
            spriteBatch.Draw(_vGradient, new Rectangle(Bounds.X, Bounds.Bottom - _thickness, Bounds.Width, _thickness), null, Color.White, 0f, Vector2.Zero, SpriteEffects.FlipVertically, 0f);

            // Left (Horizontal Gradient, Normal)
            spriteBatch.Draw(_hGradient, new Rectangle(Bounds.X, Bounds.Y, _thickness, Bounds.Height), Color.White);

            // Right (Horizontal Gradient, Flipped Horizontally)
            spriteBatch.Draw(_hGradient, new Rectangle(Bounds.Right - _thickness, Bounds.Y, _thickness, Bounds.Height), null, Color.White, 0f, Vector2.Zero, SpriteEffects.FlipHorizontally, 0f);
        }
    }
}