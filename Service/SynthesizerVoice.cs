using Microsoft.VisualBasic.Devices;
using SynthesizerEngine.Core.Audio;
using SynthesizerEngine.Core.Audio.Interface;

namespace SoundDesigner.Service;

public class SynthesizerVoice : AudioNode
{
    protected SynthesizerVoice(IAudioProvider lib, float frequency = 440.0f, float gain = 1.0f) : base(lib, 2, 1, true)
    {
        // our voice is super simple for now
        // a single sine oscillator
        // the first input is the frequency
        // the second is the gain

    }
}