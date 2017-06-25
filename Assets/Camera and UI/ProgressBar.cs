using UnityEngine;
using UnityEngine.UI;

public class ProgressBar : MonoBehaviour
{

    [SerializeField]
    private Vector3 offset = Vector3.zero;
    [SerializeField]
    public GameObject background;
    [SerializeField]
    public GameObject bar;


    public Transform targetToFollow;

    private float _progress;

    public float Progress
    {
        get { return _progress; }
        set
        {
            _progress = Mathf.Clamp01 (value);
            UpdateProgress ();
        }
    }

    void Awake ()
    {
        transform.SetAsFirstSibling ();
        UpdateProgress ();
    }

    public void SetColor (Color bgColor, Color barColor)
    {
        background.GetComponent<Image> ().color = bgColor;
        bar.GetComponent<Image> ().color = barColor;

    }

    void Update ()
    {
        if (targetToFollow != null)
        {

            Vector3 targetPos = Vector3.Lerp (transform.position, Camera.main.WorldToScreenPoint (targetToFollow.position) + offset, 0.5f);
            targetPos.z = 0.0f;
            transform.position = targetPos;
        }
    }
    void UpdateProgress ()
    {
        bar.transform.localScale = new Vector3 (_progress / 1.0f, bar.transform.localScale.y, bar.transform.localScale.z);
    }


}
