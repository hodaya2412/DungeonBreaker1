public interface IEnemyState
{
    void Enter();   // נקרא כשמצב מתחיל
    void Execute(); // נקרא בכל Update
    void Exit();    // נקרא כשמצב מסתיים
}
