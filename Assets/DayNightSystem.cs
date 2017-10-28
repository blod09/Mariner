using UnityEngine;

public class DayNightSystem : MonoBehaviour
{

    private TimeAndScoreManager time;
    private const int minutesInDay = 60 * 24;


    private float degreesToRotate;

    private void Awake ()
    {
        time = MasterManager.TimeAndScoreMan;
        degreesToRotate = 360.0f / (float)minutesInDay;

    }

    //private void OnEnable ()
    //{
    //    time.oneMinuteTick += Rotate;
    //}

    //private void OnDisable ()
    //{
    //    time.oneMinuteTick -= Rotate;
    //}

    // Update is called once per frame
    private void Update ()
    {

        transform.Rotate (Vector3.forward * degreesToRotate * 10.0f * Time.deltaTime, Space.World);
    }
}
