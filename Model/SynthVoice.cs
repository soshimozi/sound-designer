using SynthesizerEngine.DSP;

namespace SoundDesigner.Model;

class SynthVoice
{
    public SynthVoice(bool playing, Voice voice, int noteIndex)
    {
        Voice = voice;
        Playing = playing;
        NoteIndex = noteIndex;
    }

    public Voice Voice { get; }
    public bool Playing { get; set; }
    public int NoteIndex { get; set; }

    public float VolumeAttack { get => Voice.VolumeAttack; set => Voice.VolumeAttack = value; }
    public float VolumeDecay { get => Voice.VolumeDecay; set => Voice.VolumeDecay = value; }
    public float VolumeSustain { get => Voice.VolumeSustain; set => Voice.VolumeSustain = value; }
    public float VolumeRelease { get => Voice.VolumeRelease; set => Voice.VolumeRelease = value; }
    public void SetOsc1WaveShape(WaveShape shape) { Voice.SetOsc1WaveShape(shape);}
    public void SetOsc2WaveShape(WaveShape shape) { Voice.SetOsc2WaveShape(shape); }

}