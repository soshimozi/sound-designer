namespace SoundDesigner.Service.Interface;

public interface ISynthesizerService
{
    void NotePressed(int note);
    void NoteReleased(int note);
}