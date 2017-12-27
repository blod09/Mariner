using UnityEngine;

[System.Serializable]
public struct ShipMinMaxRequirements
{
    [SerializeField]
    public int minLeave;
    [SerializeField]
    public int maxLeave;
    [SerializeField]
    public int minPayment;
    [SerializeField]
    public int maxPayment;
    [SerializeField]
    public int minTimeRequirement;
    [SerializeField]
    public int maxTimeRequirement;
    [SerializeField]
    public int minWoodRequirement;
    [SerializeField]
    public int maxWoodRequirement;
    [SerializeField]
    public int minClothRequirement;
    [SerializeField]
    public int maxClothRequirement;
    [SerializeField]
    public int minTarRequirement;
    [SerializeField]
    public int maxTarRequirement;
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


    public ShipStats GetRandomStatsByTier (ShipTier tier)
    {
        ShipStats s = new ShipStats ();
        s.name = NameGenerator.GetRandomShipName ();
        s.tier = tier;
        s.isInspected = false;

        switch (tier)
        {
            case ShipTier.Tier0:

                s.minutesToLeaveAtStart = Random.Range (tier0.minLeave, tier0.maxLeave + 1);

                s.payment = Random.Range (tier0.minPayment, tier0.maxPayment + 1);
                s.penaltyForLeave = ProgressionFormulas.GetShipPenalty (s.payment);

                s.timeRequirement = Random.Range (tier0.minTimeRequirement, tier0.maxTimeRequirement + 1);
                s.woodRequirement = Random.Range (tier0.minWoodRequirement, tier0.maxWoodRequirement + 1);
                s.clothRequirement = Random.Range (tier0.minClothRequirement, tier0.maxClothRequirement + 1);
                s.tarRequirement = Random.Range (tier0.minTarRequirement, tier0.maxTarRequirement + 1);

                break;
            case ShipTier.Tier1:
                s.minutesToLeaveAtStart = Random.Range (tier1.minLeave, tier1.maxLeave + 1);

                s.payment = Random.Range (tier1.minPayment, tier1.maxPayment + 1);
                s.penaltyForLeave = ProgressionFormulas.GetShipPenalty (s.payment);

                s.timeRequirement = Random.Range (tier1.minTimeRequirement, tier1.maxTimeRequirement + 1);
                s.woodRequirement = Random.Range (tier1.minWoodRequirement, tier1.maxWoodRequirement + 1);
                s.clothRequirement = Random.Range (tier1.minClothRequirement, tier1.maxClothRequirement + 1);
                s.tarRequirement = Random.Range (tier1.minTarRequirement, tier1.maxTarRequirement + 1);
                break;
            case ShipTier.Tier2:
                s.minutesToLeaveAtStart = Random.Range (tier2.minLeave, tier2.maxLeave + 1);

                s.payment = Random.Range (tier2.minPayment, tier2.maxPayment + 1);
                s.penaltyForLeave = ProgressionFormulas.GetShipPenalty (s.payment);

                s.timeRequirement = Random.Range (tier2.minTimeRequirement, tier2.maxTimeRequirement + 1);
                s.woodRequirement = Random.Range (tier2.minWoodRequirement, tier2.maxWoodRequirement + 1);
                s.clothRequirement = Random.Range (tier2.minClothRequirement, tier2.maxClothRequirement + 1);
                s.tarRequirement = Random.Range (tier2.minTarRequirement, tier2.maxTarRequirement + 1);
                break;
            case ShipTier.Tier3:
                s.minutesToLeaveAtStart = Random.Range (tier3.minLeave, tier3.maxLeave + 1);

                s.payment = Random.Range (tier3.minPayment, tier3.maxPayment + 1);
                s.penaltyForLeave = ProgressionFormulas.GetShipPenalty (s.payment);

                s.timeRequirement = Random.Range (tier3.minTimeRequirement, tier3.maxTimeRequirement + 1);
                s.woodRequirement = Random.Range (tier3.minWoodRequirement, tier3.maxWoodRequirement + 1);
                s.clothRequirement = Random.Range (tier3.minClothRequirement, tier3.maxClothRequirement + 1);
                s.tarRequirement = Random.Range (tier3.minTarRequirement, tier3.maxTarRequirement + 1);
                break;
            case ShipTier.Tier4:
                s.minutesToLeaveAtStart = Random.Range (tier4.minLeave, tier4.maxLeave + 1);

                s.payment = Random.Range (tier4.minPayment, tier4.maxPayment + 1);
                s.penaltyForLeave = ProgressionFormulas.GetShipPenalty (s.payment);

                s.timeRequirement = Random.Range (tier4.minTimeRequirement, tier4.maxTimeRequirement + 1);
                s.woodRequirement = Random.Range (tier4.minWoodRequirement, tier4.maxWoodRequirement + 1);
                s.clothRequirement = Random.Range (tier4.minClothRequirement, tier4.maxClothRequirement + 1);
                s.tarRequirement = Random.Range (tier4.minTarRequirement, tier4.maxTarRequirement + 1);
                break;
            default:
                break;

        }

        return s;
    }
}
