namespace Utilities
{
    public static class Enums
    {
        public enum GameState
        {
            Default = 0,
            MainMenu,
            Starting,
            Playing,
            Win,
            Lose,
            Paused,
            GameEnded,
        }
        
        public enum ObjectPoolType
        {
            Default = 0,
            Enemy,
            Bullet,
        };
        
        public enum LevelIndex
        {
            Default = 0,
            Level1,
            Level2,
        };
    }
}
