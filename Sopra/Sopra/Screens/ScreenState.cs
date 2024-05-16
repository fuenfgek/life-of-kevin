namespace Sopra.Screens
{
    /// <summary>
    /// Store rendering information for a screen.
    /// </summary>
    /// <author>Michael Fleig</author>
    internal sealed class ScreenState
    {
        public IScreen Screen { get; }
        public int Order { get; }
        public bool UpdateLower { get; }
        public bool DrawLower { get; }

        public ScreenState(IScreen screen, int order, bool updateLower = true, bool drawLower = true)
        {
            Screen = screen;
            Order = order;
            UpdateLower = updateLower;
            DrawLower = drawLower;
        }
    }
}