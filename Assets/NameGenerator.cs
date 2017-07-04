using UnityEngine;

public class NameGenerator : MonoBehaviour
{

    private static string[] pirateNames;

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
}
