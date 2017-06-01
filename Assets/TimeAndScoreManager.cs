using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TimeAndScoreManager : MonoBehaviour, IManager
{
    public ManagerState ManState { get; private set; }

    [SerializeField]
    private int startingGold;

    private int _gold;
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
        }
    }

    [SerializeField]
    private Text timeText;
    [SerializeField]
    private Text goldText;
    [SerializeField]
    private Text moneyChangeText;
    [SerializeField]
    private Animator moneyChangeAnimator;

    private WorkerController workerController;

    public Action oneMinuteTick;


    public void BootSequence ()
    {

        ManState = ManagerState.Initializing;
        Debug.Log (string.Format ("{0} is {1}...", this.GetType ().Name, ManState));

        workerController = FindObjectOfType<WorkerController> ();

        _gold = startingGold;

        hours = 9;
        minutes = 0;
        StartCoroutine (Clock ());

        ManState = ManagerState.Online;
        Debug.Log (string.Format ("{0} is {1}.", this.GetType ().Name, ManState));

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
                if (Gold <= 0)
                {
                    Debug.Log ("You ran out of cash, Game Over.");
                    SceneManager.LoadScene (0);
                }
            }

            if (hours >= 24)
            {
                hours = 0;
            }



            string timeString = (hours >= 10) ? string.Format ("{0}:", hours) : string.Format ("0{0}:", hours);
            timeString += (minutes > 9) ? (minutes - minutes % 10).ToString () : "0" + (minutes - minutes % 10);
            timeText.text = timeString;
            goldText.text = Gold + "$";

            if (oneMinuteTick != null)
                oneMinuteTick ();


            yield return new WaitForSeconds (.1f);
        }

    }
}
