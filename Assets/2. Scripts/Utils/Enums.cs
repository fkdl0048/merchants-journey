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
        Upgrade,
        UnitPlacement,
        Wave,
        StageClear,
        StageOver
    }
    
    public enum UnitType
    {
        Pyodu,
        Pyosa,
        Cargo
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
    
    public enum StatType
    {
        MoveSpeed,
        AttackDamage,
        Defense,
        UnitCount
    }
}