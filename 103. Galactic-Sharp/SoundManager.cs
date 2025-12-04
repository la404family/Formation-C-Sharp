using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.IO;

namespace _103._Galactic_Sharp
{
    public class SoundManager
    {
        private Dictionary<string, SoundEffect> _soundEffects;

        public SoundManager()
        {
            _soundEffects = new Dictionary<string, SoundEffect>();
        }

        public void LoadSound(string name, string filepath)
        {
            if (File.Exists(filepath))
            {
                using (var stream = new FileStream(filepath, FileMode.Open))
                {
                    SoundEffect effect = SoundEffect.FromStream(stream);
                    _soundEffects[name] = effect;
                }
            }
        }

        public SoundEffect GetSound(string name)
        {
            if (_soundEffects.ContainsKey(name))
            {
                return _soundEffects[name];
            }
            return null;
        }

        public SoundEffectInstance CreateInstance(string name)
        {
            var sound = GetSound(name);
            if (sound != null)
            {
                return sound.CreateInstance();
            }
            return null;
        }
    }
}