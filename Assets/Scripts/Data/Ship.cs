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
    private Vector3 positionAtDragStart;
    private bool isSelected;

    public Dock parentDock;


    public ShipLocation location = ShipLocation.Sea;

    private void Awake ()
    {
        rend = GetComponentInChildren<Renderer> ();
        rend.material = normalMaterial;
    }

    private void OnEnable ()
    {
        MouseManager.onHover += OnHover;
        MouseManager.onClick += OnClick;
        MouseManager.onDrag += OnDrag;
        MouseManager.onRelease += OnRelease;

    }

    private void OnDisable ()
    {

        MouseManager.onHover -= OnHover;
        MouseManager.onClick -= OnClick;
        MouseManager.onDrag -= OnDrag;
        MouseManager.onRelease -= OnRelease;
    }

    private void OnHover (GameObject go)
    {
        if (go == this.gameObject || isSelected)
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
        if (go == this.gameObject)
        {
            isSelected = true;
            positionAtDragStart = transform.position;
        }
        else
        {
            isSelected = false;
        }


    }

    private void OnDrag (GameObject go, Vector3 start, Vector3 end, GameObject ground)
    {
        if (go != this.gameObject || location != ShipLocation.Dock)
            return;
        transform.position = end;

    }

    private void OnRelease (GameObject go, GameObject ground)
    {
        if (go != this.gameObject || location != ShipLocation.Dock)
            return;

        if (ground == null)
        {
            go.transform.position = positionAtDragStart;
        }

        if (ground.tag == "WorkingArea" && ground.GetComponent<Job> () == null)
        {
            go.transform.position = ground.transform.position;
            location = ShipLocation.WorkArea;
            ground.AddComponent<Job> ().progressBarPrefab = this.progressBarPrefab;
            transform.SetParent (ground.transform);
            parentDock.isOccupied = false;
            return;

        }

        go.transform.position = positionAtDragStart;
    }
}
