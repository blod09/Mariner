using UnityEngine;

public class Job : MonoBehaviour
{
    private int jobTimeInMinutes = 60;
    private int jobPayout = 1000;

    public float jobProgress = 0.0f;
    public bool isBeingWorked = false;

    private void Start ()
    {
        FindObjectOfType<MasterManager> ().TimeAndScoreMan.oneMinuteTick += OnTimeTick;
    }

    private void OnDisable ()
    {
        FindObjectOfType<MasterManager> ().TimeAndScoreMan.oneMinuteTick -= OnTimeTick;
    }

    private void OnTimeTick ()
    {
        if (jobProgress >= 1.0f)
        {
            Debug.Log ("Job Finished");
            isBeingWorked = false;
            FindObjectOfType<MasterManager> ().TimeAndScoreMan.gold += jobPayout;
            Destroy (gameObject);
        }
        if (isBeingWorked)
        {
            jobProgress += 1.0f / jobTimeInMinutes;
        }
    }


}