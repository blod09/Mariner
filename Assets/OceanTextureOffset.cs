using UnityEngine;

public class OceanTextureOffset : MonoBehaviour
{
    [SerializeField]
    private float scrollSpeed = 0.1F;

    private Renderer rend;


    void Start ()
    {
        rend = GetComponent<Renderer> ();
    }
    void Update ()
    {
        float offset = Time.time * scrollSpeed;
        rend.material.SetTextureOffset ("_MainTex", (Vector2.down).normalized * offset);
    }
}
