using UnityEngine;
using UnityEngine.UI;

public class ProgressBar : MonoBehaviour
{

    public Vector3 offset
    {
        get
        {
            Vector3 o = Vector3.zero;
            o.y = (Screen.height / 13.0f) * -1.0f;
            return o;
        }
    }
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

    void LateUpdate ()
    {
        if (targetToFollow != null)
        {

            Vector3 targetPos = Vector3.Lerp (transform.position, Camera.main.WorldToScreenPoint (targetToFollow.position) + offset, 0.8f);
            targetPos.z = 0.0f;
            transform.position = targetPos;
        }
    }
    void UpdateProgress ()
    {
        bar.transform.localScale = new Vector3 (_progress / 1.0f, bar.transform.localScale.y, bar.transform.localScale.z);
    }


}
