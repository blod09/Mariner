using UnityEngine;

[System.Serializable]
public enum TerrainType
{
    Walkable,
    Sea,
    Dock,
    Extra
}

public class MapArea : MonoBehaviour
{
    [SerializeField]
    private TerrainType _type;

    public TerrainType Type { get { return _type; } }

}