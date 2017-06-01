using UnityEngine;

public class Worker
{
    public string Name;

    public Job CurrentJob { get; set; }

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
        CurrentJob = null;

        salary = costPerHour;
    }

    public void AddExp (float ammount)
    {
        if (ammount + Exp >= _expToLevelUp)
        {
            level += 1;
            Exp = Exp + ammount - _expToLevelUp;
            _expToLevelUp = _expToLevelUp + level * 10;
        }
        else
        {
            Exp += ammount;
        }
    }


}
