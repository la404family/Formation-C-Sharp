using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace _103._Galactic_Sharp
{
    public static class TextRenderer
    {
        private static Texture2D _pixelTexture;
        private static Dictionary<char, int[]> _glyphs;

        public static void Initialize(GraphicsDevice graphicsDevice)
        {
            _pixelTexture = new Texture2D(graphicsDevice, 1, 1);
            _pixelTexture.SetData(new[] { Color.White });

            _glyphs = new Dictionary<char, int[]>
            {
                {'A', new[]{0,1,0, 1,0,1, 1,1,1, 1,0,1, 1,0,1}},
                {'B', new[]{1,1,0, 1,0,1, 1,1,0, 1,0,1, 1,1,0}},
                {'C', new[]{0,1,1, 1,0,0, 1,0,0, 1,0,0, 0,1,1}},
                {'D', new[]{1,1,0, 1,0,1, 1,0,1, 1,0,1, 1,1,0}},
                {'E', new[]{1,1,1, 1,0,0, 1,1,0, 1,0,0, 1,1,1}},
                {'F', new[]{1,1,1, 1,0,0, 1,1,0, 1,0,0, 1,0,0}},
                {'G', new[]{0,1,1, 1,0,0, 1,0,1, 1,0,1, 0,1,1}},
                {'H', new[]{1,0,1, 1,0,1, 1,1,1, 1,0,1, 1,0,1}},
                {'I', new[]{1,1,1, 0,1,0, 0,1,0, 0,1,0, 1,1,1}},
                {'J', new[]{0,0,1, 0,0,1, 0,0,1, 1,0,1, 0,1,0}},
                {'K', new[]{1,0,1, 1,0,1, 1,1,0, 1,0,1, 1,0,1}},
                {'L', new[]{1,0,0, 1,0,0, 1,0,0, 1,0,0, 1,1,1}},
                {'M', new[]{1,0,1, 1,1,1, 1,0,1, 1,0,1, 1,0,1}},
                {'N', new[]{1,1,0, 1,0,1, 1,0,1, 1,0,1, 1,0,1}}, // Simplified
                {'O', new[]{0,1,0, 1,0,1, 1,0,1, 1,0,1, 0,1,0}},
                {'P', new[]{1,1,0, 1,0,1, 1,1,0, 1,0,0, 1,0,0}},
                {'Q', new[]{0,1,0, 1,0,1, 1,0,1, 1,1,0, 0,0,1}},
                {'R', new[]{1,1,0, 1,0,1, 1,1,0, 1,0,1, 1,0,1}},
                {'S', new[]{0,1,1, 1,0,0, 0,1,0, 0,0,1, 1,1,0}},
                {'T', new[]{1,1,1, 0,1,0, 0,1,0, 0,1,0, 0,1,0}},
                {'U', new[]{1,0,1, 1,0,1, 1,0,1, 1,0,1, 1,1,1}},
                {'V', new[]{1,0,1, 1,0,1, 1,0,1, 1,0,1, 0,1,0}},
                {'W', new[]{1,0,1, 1,0,1, 1,0,1, 1,1,1, 1,0,1}},
                {'X', new[]{1,0,1, 1,0,1, 0,1,0, 1,0,1, 1,0,1}},
                {'Y', new[]{1,0,1, 1,0,1, 0,1,0, 0,1,0, 0,1,0}},
                {'Z', new[]{1,1,1, 0,0,1, 0,1,0, 1,0,0, 1,1,1}},
                {' ', new[]{0,0,0, 0,0,0, 0,0,0, 0,0,0, 0,0,0}},
                {'.', new[]{0,0,0, 0,0,0, 0,0,0, 0,0,0, 0,1,0}},
                {'\'', new[]{0,1,0, 0,1,0, 0,0,0, 0,0,0, 0,0,0}},
                {'é', new[]{0,1,0, 1,1,1, 1,0,0, 1,1,0, 1,1,1}}, // Approx
                {'è', new[]{0,1,0, 1,1,1, 1,0,0, 1,1,0, 1,1,1}}, // Same for simplicity
                {'ê', new[]{0,1,0, 1,1,1, 1,0,0, 1,1,0, 1,1,1}}, // Same
                {'0', new[]{0,1,0, 1,0,1, 1,0,1, 1,0,1, 0,1,0}},
                {'1', new[]{0,1,0, 1,1,0, 0,1,0, 0,1,0, 1,1,1}},
                {'2', new[]{0,1,0, 1,0,1, 0,0,1, 0,1,0, 1,1,1}},
                {'3', new[]{1,1,0, 0,0,1, 0,1,0, 0,0,1, 1,1,0}},
                {'4', new[]{1,0,1, 1,0,1, 1,1,1, 0,0,1, 0,0,1}},
                {'5', new[]{1,1,1, 1,0,0, 1,1,0, 0,0,1, 1,1,0}},
                {'6', new[]{0,1,1, 1,0,0, 1,1,0, 1,0,1, 0,1,0}},
                {'7', new[]{1,1,1, 0,0,1, 0,1,0, 0,1,0, 0,1,0}},
                {'8', new[]{0,1,0, 1,0,1, 0,1,0, 1,0,1, 0,1,0}},
                {'9', new[]{0,1,0, 1,0,1, 0,1,1, 0,0,1, 0,1,0}},
                {'%', new[]{1,0,1, 0,0,1, 0,1,0, 1,0,0, 1,0,1}}
            };
        }

        public static void DrawText(SpriteBatch spriteBatch, string text, Vector2 position, Color? color = null, int scale = 3)
        {
            if (_pixelTexture == null) return;

            Color drawColor = color ?? Color.White;
            int charWidth = 3 * scale;
            int spacing = 1 * scale;

            text = text.ToUpper();

            for (int i = 0; i < text.Length; i++)
            {
                char c = text[i];
                if (_glyphs.ContainsKey(c))
                {
                    int[] pixels = _glyphs[c];
                    for (int y = 0; y < 5; y++)
                    {
                        for (int x = 0; x < 3; x++)
                        {
                            if (pixels[y * 3 + x] == 1)
                            {
                                spriteBatch.Draw(_pixelTexture,
                                    new Rectangle((int)position.X + i * (charWidth + spacing) + x * scale, (int)position.Y + y * scale, scale, scale),
                                    drawColor);
                            }
                        }
                    }
                }
            }
        }

        public static Vector2 MeasureString(string text, int scale = 3)
        {
            int charWidth = 3 * scale;
            int spacing = 1 * scale;
            return new Vector2(text.Length * (charWidth + spacing), 5 * scale);
        }
    }
}