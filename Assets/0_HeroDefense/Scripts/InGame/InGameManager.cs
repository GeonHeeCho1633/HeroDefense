using UnityEngine;

public enum InGameState
{ 
    None,
    Play,
    Win_Hero,
    Win_Enemy,
}

public class InGameManager : MonoBehaviour
{
    private EnemyManager enemyManager;
    private HeroManager heroManager;
    private InGameState stateGame;
    public void Initialized()
    {
    }

    public bool CheckInGameState()
    {
        bool _isGameState;
        if (enemyManager == null || heroManager == null)
        {
            stateGame = InGameState.None;
            _isGameState = false;
        }

        if (enemyManager.IsGameEnd)
        {
            stateGame = InGameState.Win_Hero;
            _isGameState = true;
        }
        else if(heroManager.IsGameEnd)
        {
            stateGame = InGameState.Win_Enemy;
            _isGameState = true;
        }
        else
        {
            stateGame = InGameState.Play;
            _isGameState = false;
        }

        return _isGameState;
    }
}
