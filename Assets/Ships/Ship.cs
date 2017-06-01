using UnityEngine;

public enum ShipLocation
{
    Sea,
    Dock,
    WorkArea
}

public class Ship : MonoBehaviour
{
    [SerializeField]
    private GameObject progressBarPrefab;
    [SerializeField]
    private Material normalMaterial;
    [SerializeField]
    private Material hoveredMaterial;

    private Renderer rend;
    [HideInInspector]
    public int minutesToLeaveNow;
    [HideInInspector]
    public int minutesToLeaveAtStart;

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
        leaveBar = Instantiate (progressBarPrefab, GameObject.Find ("Canvas").transform, true).GetComponent<ProgressBar> ();
        leaveBar.Progress = 1.0f;
        leaveBar.targetToFollow = this.gameObject.transform;

        leaveBar.SetColor (new Color (67f / 255f, 106f / 255f, 149f / 255f), Color.blue);

    }

    private void OnEnable ()
    {
        MouseManager.onHover += OnHover;
        MouseManager.onClick += OnClick;
        MouseManager.onDrag += OnDrag;
        MouseManager.onRelease += OnRelease;
        MasterManager.TimeAndScoreMan.oneMinuteTick += OnOneMinuteTick;
    }

    private void OnDisable ()
    {

        MouseManager.onHover -= OnHover;
        MouseManager.onClick -= OnClick;
        MouseManager.onDrag -= OnDrag;
        MouseManager.onRelease -= OnRelease;
        MasterManager.TimeAndScoreMan.oneMinuteTick -= OnOneMinuteTick;

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
        }
        else
        {
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

        print (ground.tag);
        if (ground.tag == "WorkingArea" && ground.GetComponent<Job> () == null)
        {
            go.transform.position = ground.transform.position;
            location = ShipLocation.WorkArea;
            ground.AddComponent<Job> ().progressBarPrefab = this.progressBarPrefab;
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

        minutesToLeaveNow--;
        if (minutesToLeaveNow <= 0)
        {
            Destroy (leaveBar.gameObject);
            parentDock.isOccupied = false;
            Destroy (gameObject);
            MasterManager.TimeAndScoreMan.Gold -= Random.Range (3, 6) * 17;
            return;
        }

        leaveBar.Progress = (float)minutesToLeaveNow / (float)minutesToLeaveAtStart;

    }
}
