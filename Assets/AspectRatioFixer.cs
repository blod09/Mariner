using UnityEngine;

public enum ScreenRatioMode
{
    AdaptToWidth,
    AdaptToHeight
}

public class AspectRatioFixer : MonoBehaviour
{
    [SerializeField]
    private ScreenRatioMode ratioFixMode;
    [SerializeField]
    private int targetWidth;
    [SerializeField]
    private int targetHeight;

    private Camera cam;

    private void Awake ()
    {
        cam = Camera.main;

        FixScreenRatio ();
    }

    public void FixScreenRatio ()
    {
        // obtain camera component so we can modify its viewport


        // determine the game window's current aspect ratio
        float ratio = (float)targetWidth / targetHeight;
        float windowaspect = (float)Screen.width / Screen.height;

        if (Mathf.Abs (ratio - windowaspect) > 0.01f)
        {
            //print (string.Format ("{0}, {1}", ratio, windowaspect));

            float scaleheight = windowaspect / ratio;


            if (scaleheight < 1.0f)
            {
                Rect rect = cam.rect;

                rect.width = 1.0f;
                rect.height = scaleheight;
                rect.x = 0;
                rect.y = (1.0f - scaleheight) / 2.0f;

                cam.rect = rect;
            }
            else // add pillarbox
            {
                float scalewidth = 1.0f / scaleheight;

                Rect rect = cam.rect;

                rect.width = scalewidth;
                rect.height = 1.0f;
                rect.x = (1.0f - scalewidth) / 2.0f;
                rect.y = 0;

                cam.rect = rect;
            }
        }
    }
}
