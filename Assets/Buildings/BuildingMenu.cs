using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class BuildingMenu : MonoBehaviour
{

    [SerializeField]
    private Text title;

    [SerializeField]
    private Text capacityText;
    [SerializeField]
    private Text productionText;

    [SerializeField]
    private Button capacityButton;
    [SerializeField]
    private Button productionButton;
    [SerializeField]
    private Text capacityButtonText;
    [SerializeField]
    private Text productionButtonText;

    ResourceBuilding building;

    private void Start ()
    {
        Hide ();
    }

    private void OnEnable ()
    {

        productionButton.onClick.AddListener (OnProductionClick);
        capacityButton.onClick.AddListener (OnCapacityClick);

    }

    private void OnDisable ()
    {
        StopAllCoroutines ();

        productionButton.onClick.RemoveAllListeners ();
        capacityButton.onClick.RemoveAllListeners ();
    }

    public void Show (ResourceBuilding _building)
    {

        building = _building;
        gameObject.SetActive (true);

        transform.position = Camera.main.WorldToScreenPoint (building.gameObject.transform.position);
    }

    public void Hide ()
    {
        gameObject.SetActive (false);
    }

    private IEnumerator UpdateContent ()
    {


        while (true)
        {
            switch (building.BuildingType)
            {
                case ResourceType.Wood:
                    title.text = "Lumbermill";
                    break;
                case ResourceType.Cloth:
                    title.text = "Tailor";
                    break;
                case ResourceType.Tar:
                    title.text = "Refinery";
                    break;
                default:
                    title.text = "<UNKNOWN>";
                    break;
            }

            productionText.text = string.Format ("Production: {0}", ProgressionFormulas.ResourcesGeneratedByLevel (building.ResourceGenerationLevel));

            if (building.ResourceGenerationLevel >= Constants.Instance.BuildingMaxLevel)
            {
                productionText.text += "\nLevel: MAX";
                productionButton.gameObject.SetActive (false);
            }
            else
            {
                productionText.text += "\nLevel: " + building.ResourceGenerationLevel;
                productionButton.gameObject.SetActive (true);

            }

            capacityText.text = string.Format ("Storage: {0}", ProgressionFormulas.ResourcesCapacityByLevel (building.ResourceCapacityLevel));

            if (building.ResourceCapacityLevel >= Constants.Instance.BuildingMaxLevel)
            {
                capacityText.text += "\nLevel: MAX";
                capacityButton.gameObject.SetActive (false);
            }
            else
            {
                capacityText.text += "\nLevel: " + building.ResourceCapacityLevel;
                capacityButton.gameObject.SetActive (true);

            }

            productionButtonText.text = "Upgrade\n" + ProgressionFormulas.BuildingUpgradeCost (building.ResourceGenerationLevel) + "$";
            capacityButtonText.text = "Upgrade\n" + ProgressionFormulas.BuildingUpgradeCost (building.ResourceCapacityLevel) + "$";

            if (ProgressionFormulas.BuildingUpgradeCost (building.ResourceGenerationLevel) > MasterManager.TimeAndScoreMan.Gold)
                productionButton.interactable = false;
            else
                productionButton.interactable = true;

            if (ProgressionFormulas.BuildingUpgradeCost (building.ResourceCapacityLevel) > MasterManager.TimeAndScoreMan.Gold)
                capacityButton.interactable = false;
            else
                capacityButton.interactable = true;


            yield return new WaitForSeconds (0.1f);
        }
    }

    private void OnProductionClick ()
    {
        building.LevelUpBuildingGeneration ();
    }

    private void OnCapacityClick ()
    {
        building.LevelUpBuildingCapacity ();
    }


}
