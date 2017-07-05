using UnityEngine;

public class Worker
{
    public string Name;
    public WorkerType type;

    public BaseJob CurrentJob { get; set; }

    // Cost per in-game hour.
    public int salary;

    private int level = 1;
    public int Level { get { return level; } }
    public float Exp
    {
        get; private set;
    }
    // TODO: remove magic number on initialization.
    private float _expToLevelUp = 10f;
    public float ExpToLevelUp { get { return _expToLevelUp; } }

    // Returns the current level progress as percentage (0 to 1).
    public float levelProgressInPercent { get { return Mathf.Clamp01 (Exp / _expToLevelUp); } }






    public Worker (string name, int costPerHour)
    {
        Name = name;
        type = (WorkerType)Random.Range (0, System.Enum.GetNames (typeof (WorkerType)).Length);
        CurrentJob = null;

        salary = costPerHour;
    }

    public void AddExp (float ammount)
    {
        if (ammount + Exp >= _expToLevelUp)
        {
            LevelUp (Exp + ammount - _expToLevelUp);
        }
        else
        {
            Exp += ammount;
        }
    }

    private void LevelUp (float overflowExp)
    {
        level += 1;
        Exp = overflowExp;
        _expToLevelUp = _expToLevelUp + level * 10;
        salary = Mathf.RoundToInt (salary * Mathf.Log (level + 2));
    }


}
