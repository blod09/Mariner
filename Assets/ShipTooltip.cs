using System.Collections;
using UnityEngine;
using UnityEngine.UI;


public class ShipTooltip : MonoBehaviour
{
    // Editor Links

    #region Editor Links

    // Title
    [SerializeField]
    private Text name;
    [SerializeField]
    private Text tier;

    // Labels
    [SerializeField]
    private Text leaveLabel;
    [SerializeField]
    private Image woodLabel;
    [SerializeField]
    private Image clothLabel;
    [SerializeField]
    private Image tarLabel;

    // Values
    [SerializeField]
    private Text leaveValue;
    [SerializeField]
    private Text timeValue;
    [SerializeField]
    private Text paymentValue;
    [SerializeField]
    private Text woodValue;
    [SerializeField]
    private Text clothValue;
    [SerializeField]
    private Text tarValue;

    #endregion

    public Transform target { get; private set; }
    Ship ship;

    private Vector3 offset
    {
        get
        {
            Vector3 o = Vector3.zero;
            o.y = (Screen.height / 13.0f) * -1.0f;
            return o;
        }
    }

    public void Start ()
    {
        Hide ();
    }

    public void Show (Ship ship, Transform target)
    {
        this.target = target;
        this.ship = ship;
        this.gameObject.SetActive (true);
        StartCoroutine (UpdateStats (0.1f));
    }

    public void Hide ()
    {
        target = null;
        StopAllCoroutines ();
        this.gameObject.SetActive (false);
    }

    private void LateUpdate ()
    {
        if (target != null)
        {
            transform.position = Camera.main.WorldToScreenPoint (target.position) + offset;
        }
        else if (gameObject.activeInHierarchy)
        {
            Hide ();
        }
    }

    private IEnumerator UpdateStats (float updateDelay)
    {
        while (true)
        {

            name.text = ship.stats.name;
            tier.text = ((int)ship.stats.tier + 1).ToString ();


            if (ship.location == ShipLocation.Dock)
            {
                leaveLabel.gameObject.SetActive (true);
                leaveValue.gameObject.SetActive (true);
                leaveValue.text = ConvertTimeToString (ship.minutesToLeaveCurrent);
            }
            else
            {
                leaveLabel.gameObject.SetActive (false);
                leaveValue.gameObject.SetActive (false);

            }




            if (ship.stats.isInspected == true)
            {
                timeValue.text = ship.stats.timeRequirement.ToString () + " Minutes";
                paymentValue.text = ship.stats.payment.ToString () + "$";
            }
            else
            {
                timeValue.text = ship.stats.timeRequirement.ToString () + " Minutes";
                paymentValue.text = ship.stats.payment.ToString () + "$";
            }


            yield return new WaitForSeconds (updateDelay);
        }

    }

    private string ConvertTimeToString (int timeInMinutes)
    {
        int minutes = (timeInMinutes % 60);
        int hours = (timeInMinutes - minutes) / 60;

        return string.Format ("{0:00}:{1:00}", hours, minutes);
    }
}
