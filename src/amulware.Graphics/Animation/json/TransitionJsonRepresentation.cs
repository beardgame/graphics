namespace amulware.Graphics.Animation
{
    internal class TransitionJsonRepresentation
    {
    	public string Frame { get; set; }
    	public float Time { get; set; }
    	public float Delay { get; set; }
    	public FrameTransition.TransitionType Transition { get; set; }
    }
}
