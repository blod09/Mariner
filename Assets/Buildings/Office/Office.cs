using UnityEngine;

public class Office : MonoBehaviour
{
    [SerializeField]
    private Material normalMat;
    [SerializeField]
    private Material hoveredMat;
    [SerializeField]
    private GameObject officeMenuRefference;

    private Collider colliderRefference;
    private Renderer[] renderersRefference;

    private void OnEnable ()
    {
        MouseManager.onClick += OnClick;
        MouseManager.onHover += OnHover;
    }
    private void Awake ()
    {
        colliderRefference = GetComponentInChildren<Collider> ();
        renderersRefference = GetComponentsInChildren<Renderer> ();

        if (colliderRefference == null || renderersRefference == null)
        {
            Debug.LogError ("Awake() something is null");
        }
    }

    private void OnDisable ()
    {
        MouseManager.onClick -= OnClick;
        MouseManager.onHover -= OnHover;
    }



    void OnClick (GameObject clickedGameObject)
    {
        if (clickedGameObject != colliderRefference.gameObject)
        {
            officeMenuRefference.SetActive (false);
            return;
        }

        officeMenuRefference.SetActive (!officeMenuRefference.activeInHierarchy);
    }

    void OnHover (GameObject hoveredGameObject)
    {

        if (hoveredGameObject == colliderRefference.gameObject)
            foreach (Renderer rend in renderersRefference)
            {
                rend.material = hoveredMat;
            }
        else if (officeMenuRefference.activeInHierarchy == false)
            foreach (Renderer rend in renderersRefference)
            {
                rend.material = normalMat;
            }
    }
}
