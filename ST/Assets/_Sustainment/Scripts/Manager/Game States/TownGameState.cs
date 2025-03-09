using UnityEngine;

public abstract class TownGameState
{
    public abstract void EnterState(Town_GameManager gameManager);

    public abstract void UpdateState(Town_GameManager gameManager);

    public abstract void ExitState(Town_GameManager gameManager);
}
