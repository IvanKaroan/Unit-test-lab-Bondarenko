namespace AV.FurnaceLoading.Model;

/// <summary>
/// Кассета печи
/// </summary>
public class Cassette
{
    public Cassette(int height, int width)
    {
        Height = height;
        Width = width;
    }

    /// <summary>
    /// Высота
    /// </summary>
    public int Height { get; set; }

    /// <summary>
    /// Ширина
    /// </summary>
    public int Width { get; set; }
}
