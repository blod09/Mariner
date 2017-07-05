using System.Collections.Generic;
using UnityEngine;

public class BuildSpot : Building
{


    BuildSpotMenu menu;

    static List<GameObject> allBuildSpots;

    protected override void Awake ()
    {
        base.Awake ();

        if (menu == null)
        {
            menu = FindObjectOfType<BuildSpotMenu> ();
            buildingMenuRefference = menu.gameObject;
        }

        if (allBuildSpots == null)
            allBuildSpots = new List<GameObject> ();



    }

    protected override void OnEnable ()
    {
        base.OnEnable ();

        allBuildSpots.Add (colliderRefference.gameObject);
    }

    protected override void OnDisable ()
    {
        base.OnDisable ();

        allBuildSpots.Remove (colliderRefference.gameObject);

    }

    protected override void OnClick (GameObject clickedGameObject)
    {
        if (clickedGameObject == colliderRefference.gameObject)
        {
            print (colliderRefference.gameObject);
            menu.Show (this.gameObject);
        }
        else if (clickedGameObject != menu &&
            allBuildSpots.Contains (clickedGameObject) == false)
        {
            menu.Hide ();
        }

    }
}
