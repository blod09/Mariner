using UnityEngine;

public enum ShipLocation
{
    Sea,
    Dock,
    WorkArea
}

public enum ShipTier
{
    Tier0,
    Tier1,
    Tier2,
    Tier3,
    Tier4
}

public struct ShipStats
{
    public string name;
    public ShipTier tier;

    public bool isInspected;


    public int minutesToLeaveAtStart;

    public int payment;
    public int penaltyForLeave;


    public int timeRequirement;
    public int woodRequirement;
    public int clothRequirement;
    public int tarRequirement;

}

public class Ship : MonoBehaviour
{
    public static int currentNumberOfShips { get; private set; }


    [SerializeField]
    private GameObject progressBarPrefab;
    [SerializeField]
    private Material normalMaterial;
    [SerializeField]
    private Material hoveredMaterial;

    [SerializeField]
    private Color leaveBarColor;
    [SerializeField]
    private Color leaveBarBG;

    public ShipTooltip tooltip;

    private Renderer rend;

    public ShipStats stats
    {
        get; set;
    }


    public int minutesToLeaveCurrent;


    private bool isSelected;

    [HideInInspector]
    public Dock parentDock;
    private ProgressBar leaveBar;
    [HideInInspector]
    public ShipLocation location;

    private GameObject goWithCollider;
    private Vector3 spawnPosition;

    private void Awake ()
    {

        goWithCollider = GetComponentInChildren<Collider> ().gameObject;
        spawnPosition = transform.position;
        rend = GetComponentInChildren<Renderer> ();
        rend.material = normalMaterial;
        leaveBar = Instantiate (progressBarPrefab, GameObject.Find ("Canvas").transform, false).GetComponent<ProgressBar> ();
        leaveBar.Progress = 1.0f;
        leaveBar.targetToFollow = this.gameObject.transform;

        leaveBar.SetColor (leaveBarBG, leaveBarColor);
        minutesToLeaveCurrent = stats.minutesToLeaveAtStart;

    }

    private void Start ()
    {
        minutesToLeaveCurrent = stats.minutesToLeaveAtStart;
    }

    private void OnEnable ()
    {

        MouseManager.onHover += OnHover;
        MouseManager.onClick += OnClick;
        MouseManager.onDrag += OnDrag;
        MouseManager.onRelease += OnRelease;
        MasterManager.TimeAndScoreMan.oneMinuteTick += OnOneMinuteTick;

        currentNumberOfShips++;


    }

    private void OnDisable ()
    {

        MouseManager.onHover -= OnHover;
        MouseManager.onClick -= OnClick;
        MouseManager.onDrag -= OnDrag;
        MouseManager.onRelease -= OnRelease;
        MasterManager.TimeAndScoreMan.oneMinuteTick -= OnOneMinuteTick;

        currentNumberOfShips--;

    }

    private void OnHover (GameObject go)
    {
        if (go == goWithCollider || isSelected)
        {
            rend.material = hoveredMaterial;
        }
        else
        {
            rend.material = normalMaterial;
        }
    }

    private void OnClick (GameObject go)
    {
        if (go == goWithCollider)
        {
            isSelected = true;
            tooltip.Show (this, this.gameObject.transform);
        }
        else
        {
            if (tooltip.target == this.gameObject.transform)
                tooltip.Hide ();

            isSelected = false;
        }

    }

    private void OnDrag (GameObject go, Vector3 start, Vector3 end, GameObject ground)
    {
        if (go != goWithCollider || location != ShipLocation.Dock)
            return;
        end.y += 3.0f;
        transform.position = end;

    }

    private void OnRelease (GameObject go, GameObject ground)
    {
        if (go != goWithCollider || go == null || location != ShipLocation.Dock)
            return;

        if (ground == null)
        {
            transform.position = spawnPosition;
        }

        //print (ground.tag);
        if (ground.tag == "WorkingArea" && ground.GetComponent<ShipJob> () == null)
        {
            go.transform.position = ground.transform.position;
            location = ShipLocation.WorkArea;
            ShipJob j = ground.AddComponent<ShipJob> ();
            j.AssignShip (this);
            transform.SetParent (ground.transform);
            parentDock.isOccupied = false;
            return;

        }

        transform.position = spawnPosition;
    }

    private void OnOneMinuteTick ()
    {
        if (location != ShipLocation.Dock || leaveBar == null)
        {
            if (leaveBar != null)
                Destroy (leaveBar.gameObject);
            return;
        }

        minutesToLeaveCurrent--;
        if (minutesToLeaveCurrent <= 0)
        {
            Destroy (leaveBar.gameObject);
            parentDock.isOccupied = false;
            Destroy (gameObject);
            MasterManager.TimeAndScoreMan.Gold -= Random.Range (3, 6) * 17;
            return;

        }

        leaveBar.Progress = (float)minutesToLeaveCurrent / (float)stats.minutesToLeaveAtStart;

    }

    public void ShowTooltip ()
    {
        tooltip.Show (this, gameObject.transform);
    }
}
