using UnityEngine;

public class FloatingIcon : MonoBehaviour
{
    Vector3 baseScale;


    // 0.4, 8 , -2.3
    void Start ()
    {
        baseScale = transform.localScale;
        transform.LookAt (Camera.main.transform);
    }

    // Update is called once per frame
    void Update ()
    {
        transform.LookAt (Camera.main.transform);
        Pulse ();
    }

    void Pulse ()
    {
        float frequency = .8f;
        float amplitude = .3f;
        float phase = 0.0f;

        transform.localScale = baseScale + Vector3.one * amplitude * (Mathf.Sin (Mathf.PI * 2.0f * frequency * Time.time + phase));
    }
}
