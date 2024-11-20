namespace Scripts.Utils
{
    public enum TileType
    {
        Normal,
        Obstacle,
        Unwalkable
    }
    
    public enum GameState
    {
        Boot,
        Title,
        InGame,
        Pause,
        GameOver
    }
    
    public enum InGameState
    {
        UnitPlacement,
        Wave,
        StageClear,
        StageOver
    }
    
    public enum UnitType
    {
        Pyosa,
    }
}