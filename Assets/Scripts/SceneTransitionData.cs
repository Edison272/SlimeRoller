public static class SceneTransitionData
{
    public static string NextLevelName { get; private set; }

    public static void SetNextLevel(string levelName)
    {
        NextLevelName = levelName;
    }

    public static void Clear()
    {
        NextLevelName = null;
    }
}
