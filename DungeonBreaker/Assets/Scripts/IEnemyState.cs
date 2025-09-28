public abstract class IEnemyState
{
  public virtual void Enter() { }   // נקרא כשמצב מתחיל
  public virtual void Execute() { } // נקרא בכל Update
  public virtual void Exit() { }  // נקרא כשמצב מסתיים
}
