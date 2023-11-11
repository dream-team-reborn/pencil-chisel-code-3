namespace CharacterMovements
{
    public enum SurfaceType
    {
        Sliding,
        Ground
    }
    
    public interface ISurface
    {
        SurfaceType GetSurfaceType();
    }
}