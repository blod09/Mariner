using System.Linq;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private Vector3 cameraStartPos;

    private GameObject[] ground;

    private void Awake ()
    {
        MapArea[] _tempArray = FindObjectsOfType<MapArea> ();
        ground = new GameObject[_tempArray.Length];
        for (int i = 0; i < _tempArray.Length; i++)
        {
            ground[i] = _tempArray[i].gameObject;
        }
    }

    private void OnEnable ()
    {
        MouseManager.onClick += OnClick;
        MouseManager.onDrag += OnDrag;
    }

    private void OnDisable ()
    {
        MouseManager.onClick -= OnClick;
        MouseManager.onDrag -= OnDrag;
    }

    private void OnClick (GameObject go)
    {
        if (ground.Contains (go) == false)
            return;

        cameraStartPos = transform.position;
    }

    private void OnDrag (GameObject go, Vector3 startPos, Vector3 endPos, GameObject _ground)
    {
        if (ground.Contains (go) == false)
            return;
        transform.position = Vector3.Lerp (transform.position, cameraStartPos + (startPos - endPos), 0.5f);
    }
}
