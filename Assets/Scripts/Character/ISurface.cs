namespace CharacterMovements
{
    public enum SurfaceType
    {
        Sliding,
        Ground,
        Oil
    }
    
    public interface ISurface
    {
        SurfaceType GetSurfaceType();
    }
}