using UnityEngine;

public class Building : MonoBehaviour
{
    [SerializeField]
    private Material normalMat;
    [SerializeField]
    private Material hoveredMat;
    [SerializeField]
    protected GameObject buildingMenuRefference;

    [SerializeField]
    protected Collider colliderRefference;

    private Renderer[] renderersRefference;


    protected virtual void Awake ()
    {
        colliderRefference = GetComponentInChildren<Collider> ();
        renderersRefference = GetComponentsInChildren<Renderer> ();

        if (colliderRefference == null || renderersRefference == null)
        {
            Debug.LogError ("Awake() something is null at " + gameObject.name);
        }

    }

    protected virtual void OnEnable ()
    {
        MouseManager.onClick += OnClick;
        MouseManager.onHover += OnHover;
    }

    protected virtual void OnDisable ()
    {
        MouseManager.onClick -= OnClick;
        MouseManager.onHover -= OnHover;
    }



    protected virtual void OnClick (GameObject clickedGameObject)
    {
        if (clickedGameObject != colliderRefference.gameObject)
        {
            buildingMenuRefference.SetActive (false);
            return;
        }

        buildingMenuRefference.SetActive (!buildingMenuRefference.activeInHierarchy);
    }

    void OnHover (GameObject hoveredGameObject)
    {

        if (hoveredGameObject == colliderRefference.gameObject)
            foreach (Renderer rend in renderersRefference)
            {
                rend.material = hoveredMat;
            }
        else
        {
            foreach (Renderer rend in renderersRefference)
            {
                rend.material = normalMat;
            }
        }
    }
}
