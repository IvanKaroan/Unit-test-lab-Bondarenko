namespace AV.FurnaceLoading.Model;

/// <summary>
/// Правила для определения безопасных расстояний
/// </summary>
public static class SafetyParameters
{
    /// <summary>
    /// Какое свободное пространство должно быть до стенок камеры
    /// </summary>
    /// <param name="place">Место размещения</param>
    /// <returns></returns>
    public static int SafeDistanceToWalls(LoadPlace place) => place switch
    {
        CircleLoadPlace circlePlace =>
            circlePlace switch
            {
                { Diameter: <= 200 } => 80,
                _ => 100,
            },
        RectLoadPlace rectPlace =>
            rectPlace switch
            {
                { Width: <= 200, Height: <= 200 } => 80,
                _ => 100
            },

        _ => throw new NotImplementedException()
    };


    /// <summary>
    /// Какое свободное пространство должно быть до соседних стержней
    /// </summary>
    /// <param name="place">Место размещения</param>
    /// <returns></returns>
    public static int SafeDistanceBetweenPlaces(LoadPlace place) => place switch
    {
        CircleLoadPlace circlePlace =>
            circlePlace switch
            {
                { Diameter: <= 200 } => 60,
                _ => 80,
            },
        RectLoadPlace rectPlace =>
            rectPlace switch
            {
                { Width: <= 200, Height: <= 200 } => 60,
                _ => 80
            },

        _ => throw new NotImplementedException()
    };
}
