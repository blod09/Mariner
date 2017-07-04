using System;
using System.Collections.Generic;
using UnityEngine;

public class ShadowBuilding : MonoBehaviour
{
    [SerializeField]
    BuildType type;

    BuildPopup buildPopup;

    Renderer rend;

    private bool isActive;
    private static List<GameObject> allShadowBuildings;

    void Awake ()
    {
        rend = GetComponentInChildren<Renderer> ();
        buildPopup = GameObject.Find ("Build Popup").GetComponent<BuildPopup> ();
        if (buildPopup == null)
            throw new Exception ("Couldn't find the build popup object --> Awake ()");

        if (allShadowBuildings == null)
            allShadowBuildings = new List<GameObject> ();
        allShadowBuildings.Add (GetComponentInChildren<Collider> ().gameObject);
    }

    private void Update ()
    {
        // Pulse


    }
    void OnEnable ()
    {
        MouseManager.onHover += OnHover;
        MouseManager.onClick += OnClick;
    }

    void OnDisable ()
    {
        MouseManager.onHover -= OnHover;
        MouseManager.onClick -= OnClick;
    }


    void OnClick (GameObject clickedGO)
    {
        if (clickedGO == rend.gameObject || clickedGO == buildPopup.gameObject)
        {
            buildPopup.Show (transform, type);
            isActive = true;
        }
        else if (isActive && allShadowBuildings.Contains (clickedGO) == false)
        {
            isActive = false;
            buildPopup.Hide ();
        }
        else
        {
            isActive = false;
        }


    }

    void OnHover (GameObject hoveredGO)
    {
        Color col = rend.material.color;

        if (hoveredGO == rend.gameObject || (isActive == true && buildPopup.isActiveAndEnabled == true))
        {
            col.a = 140f / 255f;
            transform.localScale = Vector3.one;
        }
        else
        {
            col.a = 50f / 255f;
            Pulse ();
        }
        rend.material.color = col;
    }

    void Pulse ()
    {
        float frequency = .4f;
        float amplitude = .05f;
        float phase = 0.0f;

        transform.localScale = Vector3.one + Vector3.one * amplitude * (Mathf.Sin (Mathf.PI * 2.0f * frequency * Time.time + phase));
    }


}
