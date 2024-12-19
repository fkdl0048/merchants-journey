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
        WorldMap,
        PreCombat,
        UnitPlacement,
        Wave,
        StageClear,
        StageOver
    }
    
    public enum UnitType
    {
        Pyodu,
        Pyosa,
    }
    
    public enum UnitClass
    {
        None,
        Sword,
        Lance,
        Bow,
        MartialArts,
    }
    
    public enum SkillType
    {
        None,
        MountainMountainSwordStrike,
        TripleSpearCombo,
        SingleShotTechnique,
        SixHarmonyFist
    }
}