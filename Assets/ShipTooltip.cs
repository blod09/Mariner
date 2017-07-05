using System.Collections;
using UnityEngine;
using UnityEngine.UI;


public class ShipTooltip : MonoBehaviour
{
    // Editor Links

    #region Editor Links

    // Title
    [SerializeField]
    private Text shipName;
    [SerializeField]
    private Text shipTier;

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

            // Name and Tier
            shipName.text = ship.stats.name;
            shipTier.text = ((int)ship.stats.tier + 1).ToString ();




            // Leave Timer, Should disappear once ship is in the pit
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


            // Resourves

            if (ship.stats.isInspected == true)
            {
                timeValue.text = ship.stats.timeRequirement.ToString () + " Minutes";
                paymentValue.text = ship.stats.payment.ToString () + " $";
            }
            else
            {
                timeValue.text = "????";
                paymentValue.text = "????";
            }


            if (ship.stats.woodRequirement > 0)
            {
                woodLabel.gameObject.SetActive (true);
                woodValue.text = (ship.stats.isInspected) ? string.Format ("{0}/{1}", Mathf.Clamp (MasterManager.TimeAndScoreMan.CurrentWood, 0, ship.stats.woodRequirement), ship.stats.woodRequirement) : "??/??";
            }
            else
            {
                woodLabel.gameObject.SetActive (false);
            }



            if (ship.stats.clothRequirement > 0)
            {
                clothLabel.gameObject.SetActive (true);
                clothValue.text = (ship.stats.isInspected) ? string.Format ("{0}/{1}", Mathf.Clamp (MasterManager.TimeAndScoreMan.CurrentCloth, 0, ship.stats.clothRequirement), ship.stats.clothRequirement) : "??/??";
            }
            else
            {
                clothLabel.gameObject.SetActive (false);
            }



            if (ship.stats.tarRequirement > 0)
            {
                tarLabel.gameObject.SetActive (true);
                tarValue.text = (ship.stats.isInspected) ? string.Format ("{0}/{1}", Mathf.Clamp (MasterManager.TimeAndScoreMan.CurrentTar, 0, ship.stats.tarRequirement), ship.stats.tarRequirement) : "??/??";
            }
            else
            {
                tarLabel.gameObject.SetActive (false);
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
