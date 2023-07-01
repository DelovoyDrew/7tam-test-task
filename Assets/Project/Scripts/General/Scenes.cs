using System.Collections.Generic;

public class Scenes 
{
    private static List<SceneData> _scenes = new List<SceneData>()
    {
        new SceneData("Loading", Types.Loading),
        new SceneData("Lobby", Types.Lobby),
        new SceneData("Game", Types.Game)
    };

    public enum Types
    {
        Loading,
        Lobby,
        Game
    }

    public static string GetNameByType(Types type )
    {
        SceneData data = _scenes.Find(scene => scene.TYPE == type);
        return data.VALUE;
    }

    public static Types GetTypeBuyName(string name)
    {
        SceneData data = _scenes.Find(scene => scene.VALUE == name);
        return data.TYPE;
    }
}

[System.Serializable]
public class SceneData
{
    public readonly string VALUE = "Loading";
    public readonly Scenes.Types TYPE;

    public SceneData(string value, Scenes.Types type)
    {
        VALUE = value;
        TYPE = type;
    }
}