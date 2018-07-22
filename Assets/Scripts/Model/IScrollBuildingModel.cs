namespace Assets.Scripts.Model
{

    /// <summary>
    /// It is an interface for buildings
    /// it includes name,vertical size,horizotnal size.
    /// </summary>
    public interface IScrollBuildingModel
    {
        string Name { get; }
        int VerticalSize { get; }
        int HorizontalSize { get; }
        
    }
}

