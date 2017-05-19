using UnityEngine;

public class ProgressBar : MonoBehaviour
{

    [SerializeField]
    private Vector3 offset;

    [SerializeField]
    private RectTransform greenBar;

    public Transform targetToFollow;

    private float _progress;

    public float Progress
    {
        get { return _progress; }
        set
        {
            _progress = Mathf.Clamp01 (value);
            greenBar.localScale = new Vector3 (Mathf.Lerp (greenBar.localScale.x, _progress, 0.5f), greenBar.localScale.y, greenBar.localScale.z);
        }
    }


    void Awake ()
    {
        greenBar.localScale = new Vector3 (0.0f, greenBar.localScale.y, greenBar.localScale.z);
    }

    void Update ()
    {
        if (targetToFollow == null)
            return;

        transform.position = Vector3.Lerp (transform.position, Camera.main.WorldToScreenPoint (targetToFollow.position) + offset, 0.5f);
    }


}
