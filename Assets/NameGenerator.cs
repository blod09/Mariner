using UnityEngine;

public class NameGenerator : MonoBehaviour
{

    private static string[] pirateNames;

    void Awake ()
    {
        pirateNames = System.IO.File.ReadAllLines (@"Assets\Resources\piratenames.txt");
    }

    // Update is called once per frame
    public static string GetRandomPirateName ()
    {
        if (pirateNames == null)
        {
            pirateNames = System.IO.File.ReadAllLines (@"Assets\Resources\piratenames.txt");

        }

        return pirateNames[Random.Range (0, pirateNames.Length)];

    }
}
