using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public enum BuildType
{
    Dock,
    Pit,
    Building
}

public class BuildPopup : MonoBehaviour
{

    BuildType type;

    [SerializeField]
    Button buildButton;
    [SerializeField]
    Text titleText;
    [SerializeField]
    Text ButtonText;
    [SerializeField]
    DockManager dockManager;
    [SerializeField]
    WorkingAreaManager pitManager;

    private void Start ()
    {
        gameObject.SetActive (false);
    }

    private void OnEnable ()
    {
        StartCoroutine (UpdateText ());
    }

    private void OnDisable ()
    {
        StopAllCoroutines ();
    }

    public void Show (Transform target, BuildType buildType)
    {
        transform.position = Camera.main.WorldToScreenPoint (target.position + Vector3.down * 10.0f);
        type = buildType;
        this.gameObject.SetActive (true);
    }

    public void Hide ()
    {
        this.gameObject.SetActive (false);
    }

    public void OnClick ()
    {
        switch (type)
        {
            case BuildType.Dock:
                dockManager.BuildDock ();
                break;
            case BuildType.Pit:
                pitManager.BuildPit ();
                break;
            case BuildType.Building:
                break;
            default:
                break;
        }
        Hide ();
    }

    private IEnumerator UpdateText ()
    {
        while (true)
        {
            titleText.text = type.ToString ();

            int cost = 0;
            switch (type)
            {
                case BuildType.Dock:
                    cost = ProgressionFormulas.CurerntDockCost ();

                    break;
                case BuildType.Pit:
                    cost = ProgressionFormulas.CurrentPitCost ();
                    break;
                case BuildType.Building:
                    break;
                default:
                    break;
            }
            ButtonText.text = string.Format ("Build\n({0}$)", cost);

            if (cost > MasterManager.TimeAndScoreMan.Gold)
                buildButton.interactable = false;
            else
                buildButton.interactable = true;

            yield return new WaitForSeconds (0.1f);
        }
    }
}
