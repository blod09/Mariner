using System;
using UnityEngine;


public enum GameState
{
    Normal,
    Paused,
    Menu
}

public class GameStateManager : MonoBehaviour, IManager
{
    public Action<GameState> onGameStateChange;

    private GameState _gameState;

    public GameState GameState
    {
        get { return _gameState; }
        set
        {
            _gameState = value;
            if (onGameStateChange != null)
            {
                onGameStateChange (value);
            }
        }
    }

    public ManagerState ManState { get; private set; }

    public void BootSequence ()
    {
        ManState = ManagerState.Initializing;
        Debug.Log (string.Format ("{0} is {1}...", this.GetType ().Name, ManState));

        // TODO: add booting logic here.
        ManState = ManagerState.Online;
        Debug.Log (string.Format ("{0} is {1}.", this.GetType ().Name, ManState));

    }

}


