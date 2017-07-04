using UnityEngine;

[System.Serializable]
public struct ShipMinMaxRequirements
{
    [SerializeField]
    private int minLeave;
    [SerializeField]
    private int maxLeave;
    [SerializeField]
    private int minPayment;
    [SerializeField]
    private int maxPayment;
    [SerializeField]
    private int minTimeRequirement;
    [SerializeField]
    private int maxTimeRequirement;
    [SerializeField]
    private int minWoodRequirement;
    [SerializeField]
    private int maxWoodRequirement;
    [SerializeField]
    private int minClothRequirement;
    [SerializeField]
    private int maxClothRequirement;
    [SerializeField]
    private int minTarRequirement;
    [SerializeField]
    private int maxTarRequirement;
}

public class ShipTiers : MonoBehaviour
{
    [SerializeField]
    private ShipMinMaxRequirements tier0;
    [SerializeField]
    private ShipMinMaxRequirements tier1;
    [SerializeField]
    private ShipMinMaxRequirements tier2;
    [SerializeField]
    private ShipMinMaxRequirements tier3;
    [SerializeField]
    private ShipMinMaxRequirements tier4;



}
