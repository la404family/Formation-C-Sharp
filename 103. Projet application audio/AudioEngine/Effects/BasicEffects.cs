using AudioEditor.Core.Enums;
using AudioEditor.Core.Interfaces;

namespace AudioEditor.AudioEngine.Effects
{
    /// <summary>
    /// Classe de base pour tous les effets audio
    /// </summary>
    public abstract class AudioEffectBase : IAudioEffect
    {
        public abstract string Name { get; }
        public abstract EffectType Type { get; }
        public bool IsEnabled { get; set; } = true;

        protected Dictionary<string, float> parameters = new Dictionary<string, float>();

        public abstract void Process(float[] inputBuffer, float[] outputBuffer);

        public virtual void SetParameter(string paramName, float value)
        {
            parameters[paramName] = value;
        }

        public virtual float GetParameter(string paramName)
        {
            return parameters.TryGetValue(paramName, out float value) ? value : 0f;
        }
    }

    /// <summary>
    /// Effet de volume simple
    /// </summary>
    public class VolumeEffect : AudioEffectBase
    {
        public override string Name => "Volume";
        public override EffectType Type => EffectType.Volume;

        public VolumeEffect()
        {
            SetParameter("Volume", 1.0f);
        }

        public override void Process(float[] inputBuffer, float[] outputBuffer)
        {
            if (!IsEnabled)
            {
                Array.Copy(inputBuffer, outputBuffer, inputBuffer.Length);
                return;
            }

            float volume = GetParameter("Volume");
            for (int i = 0; i < inputBuffer.Length; i++)
            {
                outputBuffer[i] = inputBuffer[i] * volume;
            }
        }
    }

    /// <summary>
    /// Effet de panoramique (Pan)
    /// </summary>
    public class PanEffect : AudioEffectBase
    {
        public override string Name => "Pan";
        public override EffectType Type => EffectType.Pan;

        public PanEffect()
        {
            SetParameter("Pan", 0.0f); // Centre
        }

        public override void Process(float[] inputBuffer, float[] outputBuffer)
        {
            if (!IsEnabled || inputBuffer.Length != outputBuffer.Length)
            {
                Array.Copy(inputBuffer, outputBuffer, inputBuffer.Length);
                return;
            }

            float pan = GetParameter("Pan"); // -1.0 (gauche) à 1.0 (droite)

            // Assumer que inputBuffer est stéréo (2 canaux)
            for (int i = 0; i < inputBuffer.Length; i += 2)
            {
                float leftGain = pan <= 0 ? 1.0f : 1.0f - pan;
                float rightGain = pan >= 0 ? 1.0f : 1.0f + pan;

                outputBuffer[i] = inputBuffer[i] * leftGain;     // Canal gauche
                outputBuffer[i + 1] = inputBuffer[i + 1] * rightGain; // Canal droit
            }
        }
    }
}