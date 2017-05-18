using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof (GameStateManager))]
public class MasterManager : MonoBehaviour
{
    public GameStateManager GameStateMan { get; private set; }
    public TimeAndScoreManager TimeAndScoreMan { get; private set; }
    private List<IManager> _managersList;

    private void Awake ()
    {
        _managersList = new List<IManager> ();

        GameStateMan = GetComponent<GameStateManager> ();
        TimeAndScoreMan = GetComponent<TimeAndScoreManager> ();

        _managersList.Add (GameStateMan);
        _managersList.Add (TimeAndScoreMan);


        BootAllManagers ();

    }

    private void BootAllManagers ()
    {
        foreach (IManager manager in _managersList)
        {
            manager.BootSequence ();
        }
    }
}
