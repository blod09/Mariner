using UnityEngine;

public class NameGenerator : MonoBehaviour
{

    private static string[] pirateNames;
    private static string[] shipNames;

    public static string GetRandomPirateName ()
    {
        if (pirateNames == null)
        {
            TextAsset t;
            t = Resources.Load ("piratenames", typeof (TextAsset)) as TextAsset;
            char newLine = '\n';
            pirateNames = t.text.Split (newLine);
        }

        return pirateNames[UnityEngine.Random.Range (0, pirateNames.Length)];

    }

    public static string GetRandomShipName ()
    {
        if (shipNames == null)
        {
            TextAsset t;
            t = Resources.Load ("shipnames", typeof (TextAsset)) as TextAsset;
            char newLine = '\n';
            shipNames = t.text.Split (newLine);
        }

        return shipNames[UnityEngine.Random.Range (0, shipNames.Length)];
    }
}
