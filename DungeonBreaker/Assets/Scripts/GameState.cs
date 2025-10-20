using UnityEngine;
using UnityEngine.SceneManagement;

public abstract class GameState
{
    public virtual void Enter() { }
    public virtual void Update() { }
    public virtual void Exit() { }
}
