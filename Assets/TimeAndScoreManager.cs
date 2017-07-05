using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class TimeAndScoreManager : MonoBehaviour, IManager
{
    public ManagerState ManState { get; private set; }

    // Resources
    private int _gold;
    private int _currentWood;
    private int _currentCloth;
    private int _currentTar;

    public int maxWood;
    public int maxCloth;
    public int maxTar;

    private bool _shouldWoodShow = false;
    private bool _shouldClothShow = false;
    private bool _shouldTarShow = false;
    private bool _shouldResourcesShow { get { return _shouldWoodShow || _shouldClothShow || _shouldTarShow; } }





    private int minutes;
    private int hours;

    public int Gold
    {
        get { return _gold; }
        set
        {
            if (value - _gold > 0)
            {
                moneyChangeText.text = (value - _gold) + "$";
                moneyChangeAnimator.SetTrigger ("Increase");
            }
            else if (value - _gold < 0)
            {
                moneyChangeText.text = (value - _gold) + "$";
                moneyChangeAnimator.SetTrigger ("Decrease");
            }
            _gold = value;
            //if (_gold <= 0)
            //{
            //    {
            //        Debug.Log ("You ran out of cash, Game Over.");
            //        //SceneManager.LoadScene (0);
            //    }
            //}
        }
    }

    public int CurrentWood { get { return _currentWood; } set { _currentWood = Mathf.Clamp (value, 0, maxWood); } }
    public int CurrentCloth { get { return _currentCloth; } set { _currentCloth = Mathf.Clamp (value, 0, maxCloth); } }
    public int CurrentTar { get { return _currentTar; } set { _currentTar = Mathf.Clamp (value, 0, maxTar); } }


    [SerializeField]
    private Text timeText;
    [SerializeField]
    private Text goldText;
    [SerializeField]
    private Text moneyChangeText;
    [SerializeField]
    private Animator moneyChangeAnimator;

    [SerializeField]
    GameObject resourcesWindow;
    [SerializeField]
    GameObject woodLabel;
    [SerializeField]
    GameObject clothLabel;
    [SerializeField]
    GameObject tarLabel;
    [SerializeField]
    Text woodText;
    [SerializeField]
    Text clothText;
    [SerializeField]
    Text tarText;

    private WorkerController workerController;

    public Action oneMinuteTick;


    public void BootSequence ()
    {

        ManState = ManagerState.Initializing;
        Debug.Log (string.Format ("{0} is {1}...", this.GetType ().Name, ManState));

        workerController = FindObjectOfType<WorkerController> ();

        _gold = Constants.Instance.StartingGold;

        hours = 9;
        minutes = 0;
        StartCoroutine (Clock ());

        // Resources
        woodLabel.SetActive (false);
        clothLabel.SetActive (false);
        tarLabel.SetActive (false);
        resourcesWindow.SetActive (false);



        ManState = ManagerState.Online;
        Debug.Log (string.Format ("{0} is {1}.", this.GetType ().Name, ManState));

        // DEBUUGG

        //SetResourcesToMax ();


    }

    IEnumerator Clock ()
    {
        while (true)
        {
            minutes += 1;

            if (minutes >= 60.0f)
            {
                minutes = 0;
                hours += 1;

                Gold -= workerController.TotalWorkerSalary;

            }

            if (hours >= 24)
            {
                hours = 0;
            }


            // TEST TESET TEST
            //if (minutes == 10)
            //{
            //    SetResourcesToMax ();
            //}

            timeText.text = string.Format ("{0:00}:{1:00}", hours, (minutes - (minutes % 10)));
            goldText.text = Gold + "$";

            if (oneMinuteTick != null)
                oneMinuteTick ();

            UpdateResourceDisplay ();

            yield return new WaitForSeconds (.1f);
        }

    }

    // TODO:REMOVE TEST FUNCTION 
    private void SetResourcesToMax ()
    {
        _currentWood = 0;
        _currentCloth = 0;
        _currentTar = 0;

        maxWood = 9999;
        maxCloth = 9999;
        maxTar = 9999;
    }

    private void UpdateResourceDisplay ()
    {
        if (maxWood > 0)
        {
            _shouldWoodShow = true;
            resourcesWindow.SetActive (true);
            woodLabel.SetActive (true);
        }
        if (maxCloth > 0)
        {
            _shouldClothShow = true;
            resourcesWindow.SetActive (true);
            clothLabel.SetActive (true);
        }
        if (maxTar > 0)
        {
            _shouldTarShow = true;
            resourcesWindow.SetActive (true);
            tarLabel.SetActive (true);
        }

        if (_shouldResourcesShow == true)
        {
            woodText.text = string.Format ("{0}/{1}", _currentWood, maxWood);
            clothText.text = string.Format ("{0}/{1}", _currentCloth, maxCloth);
            tarText.text = string.Format ("{0}/{1}", _currentTar, maxTar);

        }



    }


    #region Helper Stuff
    public bool IsEnoughResources (int woodReq, int clothReq, int tarReq)
    {
        return (_currentWood >= woodReq) && (_currentCloth >= clothReq) && (_currentTar >= tarReq);
    }

    public void AddWood (int ammount)
    {
        CurrentWood += ammount;
    }

    public void AddCloth (int ammount)
    {
        CurrentCloth += ammount;
    }

    public void AddTar (int ammount)
    {
        CurrentTar += ammount;
    }

    public void RemoveWood (int ammount)
    {
        CurrentWood -= ammount;
    }

    public void RemoveCloth (int ammount)
    {
        CurrentCloth -= ammount;
    }

    public void RemoveTar (int ammount)
    {
        CurrentTar -= ammount;
    }

    #endregion
}
