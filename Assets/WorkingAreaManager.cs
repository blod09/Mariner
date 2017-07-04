using System.Collections.Generic;
using UnityEngine;

public class WorkingAreaManager : MonoBehaviour
{
    [SerializeField]
    private GameObject shadowPit;

    private List<GameObject> pitList;

    public static int pitCount
    {
        get; private set;
    }

    void Awake ()
    {
        InitializePits ();
    }

    // Update is called once per frame
    void Update ()
    {

    }

    private void InitializePits ()
    {
        pitList = new List<GameObject> ();

        foreach (Transform t in transform)
        {
            if (t.parent == this.gameObject.transform)
            {
                pitList.Add (t.gameObject);
                t.gameObject.SetActive (false);
            }
        }

        for (int i = 0; i < Constants.Instance.NumberOfPitsAtStart; i++)
        {

            pitList[pitCount].SetActive (true);
            pitCount++;
        }

        MoveShadowPit ();
    }

    public void BuildPit ()
    {
        int cost = ProgressionFormulas.CurrentPitCost ();

        if (pitList.Count > pitCount && cost < MasterManager.TimeAndScoreMan.Gold)
        {
            pitList[pitCount].SetActive (true);
            MasterManager.TimeAndScoreMan.Gold -= cost;
            pitCount++;
            MoveShadowPit ();
        }
    }

    private void MoveShadowPit ()
    {
        if (pitCount < pitList.Count)
        {
            shadowPit.transform.position = pitList[pitCount].transform.position;
        }
        else
        {
            shadowPit.SetActive (false);
        }
    }
}
