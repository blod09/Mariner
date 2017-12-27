using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class BuildSpotMenu : MonoBehaviour
{

    GameObject linkedBuildSpot;

    [SerializeField]
    private Text lumbermillCost;
    [SerializeField]
    private Text tailorCost;
    [SerializeField]
    private Text refineryCost;

    [SerializeField]
    private Button lumbermillButton;
    [SerializeField]
    private Button tailorButton;
    [SerializeField]
    private Button refineryButton;


    [SerializeField]
    private GameObject lumbermillPrefab;
    [SerializeField]
    private GameObject tailorPrefab;
    [SerializeField]
    private GameObject refineryPrefab;

    private void Start ()
    {
        Hide ();
    }


    public void Show (GameObject buildspot)
    {

        linkedBuildSpot = buildspot;
        transform.position = Camera.main.WorldToScreenPoint (buildspot.transform.position);
        gameObject.SetActive (true);
    }

    public void Hide ()
    {
        gameObject.SetActive (false);
    }

    private void OnEnable ()
    {
        StartCoroutine (UpdateContent ());

    }

    private void OnDisable ()
    {
        StopAllCoroutines ();

    }

    private IEnumerator UpdateContent ()
    {
        while (true)
        {
            int lcost = ProgressionFormulas.BuildingBuildCost (ResourceType.Wood);
            int tcost = ProgressionFormulas.BuildingBuildCost (ResourceType.Cloth);
            int rcost = ProgressionFormulas.BuildingBuildCost (ResourceType.Tar);

            lumbermillCost.text = lcost + "$";
            tailorCost.text = tcost + "$";
            refineryCost.text = rcost + "$";

            if (lcost > MasterManager.TimeAndScoreMan.Gold)
                lumbermillButton.interactable = false;
            else
                lumbermillButton.interactable = true;

            if (tcost > MasterManager.TimeAndScoreMan.Gold)
                tailorButton.interactable = false;
            else
                tailorButton.interactable = true;

            if (rcost > MasterManager.TimeAndScoreMan.Gold)
                refineryButton.interactable = false;
            else
                refineryButton.interactable = true;


            yield return new WaitForSeconds (0.1f);
        }
    }


    public void BuyLumbermill ()
    {
        int cost = ProgressionFormulas.BuildingBuildCost (ResourceType.Wood);

        if (cost > MasterManager.TimeAndScoreMan.Gold)
            return;

        MasterManager.TimeAndScoreMan.Gold -= cost;
        Instantiate (lumbermillPrefab, linkedBuildSpot.transform.position, Quaternion.identity);
        linkedBuildSpot.SetActive (false);
        Hide ();
    }

    public void BuyTailor ()
    {
        int cost = ProgressionFormulas.BuildingBuildCost (ResourceType.Cloth);

        if (cost > MasterManager.TimeAndScoreMan.Gold)
            return;

        MasterManager.TimeAndScoreMan.Gold -= cost;
        Instantiate (tailorPrefab, linkedBuildSpot.transform.position, Quaternion.identity);
        linkedBuildSpot.SetActive (false);
        Hide ();
    }

    public void BuyRefinery ()
    {
        int cost = ProgressionFormulas.BuildingBuildCost (ResourceType.Tar);

        if (cost > MasterManager.TimeAndScoreMan.Gold)
            return;

        MasterManager.TimeAndScoreMan.Gold -= cost;
        Instantiate (refineryPrefab, linkedBuildSpot.transform.position, Quaternion.identity);
        linkedBuildSpot.SetActive (false);
        Hide ();
    }
}
