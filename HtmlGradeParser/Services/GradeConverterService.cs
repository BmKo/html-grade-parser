namespace HtmlGradeParser.Services;

public static class GradeConverterService
{
    private static readonly Dictionary<string, Tuple<float, float>> GradeBoundaries = new()
    {
        { "E", Tuple.Create(0f, 39f) },
        { "D", Tuple.Create(40f, 49f) },
        { "C-", Tuple.Create(50f, 54f) },
        { "C", Tuple.Create(55f, 59f) },
        { "C+", Tuple.Create(60f, 64f) },
        { "B-", Tuple.Create(65f, 69f) },
        { "B", Tuple.Create(70f, 74f) },
        { "B+", Tuple.Create(75f, 79f) },
        { "A-", Tuple.Create(80f, 84f) },
        { "A", Tuple.Create(85f, 89f) },
        { "A+", Tuple.Create(90f, 100f) }
    };

    private static readonly SortedDictionary<string, Tuple<float, float>> SortedGradeBoundaries =
        new(GradeBoundaries,
            Comparer<string>.Create((a, b) => GradeBoundaries[b].Item1.CompareTo(GradeBoundaries[a].Item1)));

    public static string ConvertToLetter(float grade)
    {
        foreach (var boundary in SortedGradeBoundaries.Where(boundary =>
                     grade >= boundary.Value.Item1 && grade <= boundary.Value.Item2))
        {
            return boundary.Key;
        }

        return "Invalid";
    }

    public static float ConvertToNumber(string grade)
    {
        if (string.IsNullOrEmpty(grade))
        {
            throw new ArgumentException("Grade cannot be null or empty", nameof(grade));
        }

        if (!GradeBoundaries.TryGetValue(grade, out var numberGrade))
        {
            throw new ArgumentException("Grade not found in map", nameof(grade));
        }

        return numberGrade.Item2;
    }
}