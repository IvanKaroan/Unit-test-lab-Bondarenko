namespace AV.FurnaceLoading.Model;

/// <summary>
/// Схема загрузки кассеты печи
/// </summary>
public class LoadSchema
{
    /// <summary>
    /// Места загрузки
    /// </summary>
    public IList<LoadPlace> Places { get; set; } = new List<LoadPlace>();
}

/// <summary>
/// Место загрузки
/// </summary>
public abstract class LoadPlace
{
    /// <summary>
    /// Координаты центра стержня
    /// </summary>
    public Coordinate Center { get; set; } = new Coordinate(0, 0);
}

/// <summary>
/// Место для загрузки круглого стержня
/// </summary>
public class CircleLoadPlace : LoadPlace
{
    //Конструктор 
    public CircleLoadPlace(int diametr, Coordinate center)
    {
        Diameter = diametr;
        Center = center;
    }

    /// <summary>
    /// Диаметр
    /// </summary>
    public int Diameter { get; set; }
}

/// <summary>
/// Место для прямоугльного стержня
/// </summary>
public class RectLoadPlace : LoadPlace
{
    public RectLoadPlace(int height, int width, RotationType type, Coordinate center)
    {
        Height = height;
        Width = width;
        Rotation = type;
        Center = center;
    }

    /// <summary>
    /// Возвращает целочисленное значение градуса поворота 
    /// </summary>
    /// <returns></returns>
    public int GetRotation()
    {
        switch (this.Rotation)
        {
            case RotationType.None:
                return 0;
            case RotationType.By45Degree:
                return 45;
            case RotationType.By90Degree:
                return 90;
            case RotationType.By135Degree:
                return 135;
        }
        return 0;
    }

    public int GetAxis()
    {
        return 0;
    }

    /// <summary>
    /// Варианты поворота прямоугольных стержней
    /// </summary>
    public enum RotationType
    {
        None,
        By45Degree,
        By90Degree,
        By135Degree
    }

    /// <summary>
    /// Высота
    /// </summary>
    public int Height { get; set; }
    
    /// <summary>
    /// Ширина
    /// </summary>
    public int Width { get; set; }

    /// <summary>
    /// Поворот стержня
    /// </summary>
    public RotationType Rotation { get; set; }


}
