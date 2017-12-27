using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OfficeMenu : MonoBehaviour
{
    [SerializeField]
    private float updateDelay = .1f;
    [SerializeField]
    private Transform contentWindow;
    [SerializeField]
    private GameObject workerTilePrefab;
    [SerializeField]
    WorkerController workerController;


    private List<WorkerTile> tiles;

    private void Start ()
    {
        tiles = new List<WorkerTile> ();
        gameObject.SetActive (false);
    }

    private void OnEnable ()
    {
        StartCoroutine (UpdateStatsOverTime (updateDelay));
    }

    private void OnDisable ()
    {
        StopAllCoroutines ();
    }

    public void OnDrag (Transform title)
    {

        if (MouseInGameWindow () == true)
        {
            Vector3 offset = transform.position - title.position;
            transform.position = Input.mousePosition + offset;
        }

    }

    private IEnumerator UpdateStatsOverTime (float updateDelay)
    {
        yield return null;
        while (true)
        {
            if (workerController.WorkerList.Count != tiles.Count)
            {
                tiles = new List<WorkerTile> ();
                foreach (Transform child in contentWindow)
                {
                    Destroy (child.gameObject);
                }

                foreach (Worker worker in workerController.WorkerList)
                {
                    GameObject go = Instantiate (workerTilePrefab, contentWindow, false);
                    WorkerTile tile = go.GetComponent<WorkerTile> ();
                    tile.SetTile (worker);
                    tiles.Add (tile);
                }
            }
            else
            {
                for (int i = 0; i < tiles.Count; i++)
                {
                    tiles[i].SetTile (workerController.WorkerList[i]);
                }
            }

            yield return new WaitForSeconds (updateDelay);

        }

    }

    private bool MouseInGameWindow ()
    {
        if (Input.mousePosition.x <= 0 ||
            Input.mousePosition.y <= 0 ||
            Input.mousePosition.x >= Screen.width ||
            Input.mousePosition.y >= Screen.height)
        {
            return false;
        }

        return true;
    }

}
