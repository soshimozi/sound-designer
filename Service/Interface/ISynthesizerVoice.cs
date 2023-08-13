using SynthesizerEngine.Core.Audio;
using SynthesizerEngine.Core.Audio.Interface;

namespace SoundDesigner.Service.Interface;

public interface ISynthesizerVoice
{
    IAudioNode Output { get; set; }
}