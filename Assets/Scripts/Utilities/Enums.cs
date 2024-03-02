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
            FastEnemy,
            LargeEnemy,
            FollowEnemy,
            ExplosionText
        };
        
        public enum LevelIndex
        {
            Default = 0,
            Level1,
            Level2,
            Level3,
            Level4,
            Level5,
            Level6,
        };
        
        public enum Emotion
        {
            Default = 0,
            Normal,
            Happy,
            Angry,
            Excited,
        }
        
        public enum Sound
        {
            Default = 0,
            LaserFire,
            PlayerHit,
            PlayerDie,
            EnemyHit,
            EnemyDie,
            Win,
            Lose
        }
    }
}
