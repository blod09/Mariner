using System.Collections.Generic;
using UnityEngine;

public class ResourceBuildingControls : Building
{
    ResourceBuilding rb;
    BuildingMenu menu;

    private static List<GameObject> allResourceBuildings;

    protected override void Awake ()
    {
        base.Awake ();

        if (buildingMenuRefference == null)
        {
            rb = GetComponent<ResourceBuilding> ();

            BuildingMenu[] _temp = Resources.FindObjectsOfTypeAll<BuildingMenu> ();
            print (_temp.Length);


            // THIS IS WRONG ON SO MANY LEVELS
            // FIX ASAP
            // TODO
            // TODO
            // TODO !!!
            menu = _temp[_temp.Length - 1];
            buildingMenuRefference = menu.gameObject;
        }

        if (allResourceBuildings == null)
            allResourceBuildings = new List<GameObject> ();
    }


    protected override void OnEnable ()
    {
        base.OnEnable ();

        allResourceBuildings.Add (colliderRefference.gameObject);
    }

    protected override void OnDisable ()
    {
        base.OnDisable ();

        allResourceBuildings.Remove (colliderRefference.gameObject);

    }

    protected override void OnClick (GameObject clickedGameObject)
    {

        if (clickedGameObject == colliderRefference.gameObject)
        {

            menu.Show (rb);
        }
        else if (clickedGameObject != menu.gameObject && allResourceBuildings.Contains (clickedGameObject) == false)
        {

            menu.Hide ();
        }
    }
}
