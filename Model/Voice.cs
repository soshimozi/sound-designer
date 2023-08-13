using NAudio.MediaFoundation;
using Syncfusion.UI.Xaml.Diagram;
using Syncfusion.UI.Xaml.Diagram.Controls;
using SynthesizerEngine.Core;
using SynthesizerEngine.Core.Audio;
using SynthesizerEngine.Core.Audio.Interface;
using SynthesizerEngine.DSP;
using SynthesizerEngine.Operators;

namespace SoundDesigner.Model;

public class Voice : AudioNode
{
    //private readonly ADSREnvelope _envelope;
    private readonly ADSR _envelope;
    private readonly Oscillator _osc1;
    private readonly Oscillator _osc2;

    private float _volumeAttack;
    private float _volumeDecay;
    private float _volumeSustain;
    private float _volumeRelease;

    public Voice(IAudioProvider provider, int gate) : base(provider, 0, 1)
    {
        _osc1 = new Oscillator(provider);
        _osc2 = new Oscillator(provider);

        var gain = new Gain(provider);

        _osc1.Connect(gain);
        _osc2.Connect(gain);
        
        _envelope = new ADSR(provider);
            
            //,
            //gate, // Gate
            //0.4, // Attack
            //0.2, // Decay
            //0.9, // Sustain
            //.2); // Release
        // 

        _envelope.Connect(gain, 0, 1);
        gain.Connect(OutputPassThroughNodes[0]);
    }

    public float VolumeAttack
    {
        get => _volumeAttack;
        set => _volumeAttack = value;
    }

    public float VolumeDecay
    {
        get => _volumeDecay;
        set => _volumeDecay = value;
    }


    public float VolumeSustain
    {
        get => _volumeSustain;
        set => _volumeSustain = value;
    }

    public float VolumeRelease
    {
        get => _volumeRelease;
        set => _volumeRelease = value;
    }

    public void SetOsc1WaveShape(WaveShape shape)
    {
        _osc1.SetWaveform(shape);
    }

    public void SetOsc2WaveShape(WaveShape shape)
    {
        _osc2.SetWaveform(shape);
    }


    public void NoteOn(double frequency)
    {
        _osc1.SetFrequency(frequency);
        _osc2.SetFrequency(frequency);
        
        _envelope.AttackTime = _volumeAttack;
        _envelope.DecayTime = _volumeDecay;
        _envelope.SustainLevel = _volumeSustain;
        _envelope.ReleaseTime = _volumeRelease;

        //_envelope.Gate.SetValue(1);
        _envelope.NoteOn();
    }

    public void NoteOff()
    {
        //_envelope.Gate.SetValue(0);
        _envelope.NoteOff();
    }
}