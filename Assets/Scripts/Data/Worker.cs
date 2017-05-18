using System;
using System.Collections.Generic;

public class Worker
{
    public string Name { get; set; }

    public Job CurrentJob { get; set; }

    public List<Skill> Skills { get; private set; }
    public List<float> SkillAffinities { get; private set; }

    public int CostPerHour { get; private set; }


    public Worker (string name, List<float> skillAffinities, int cost)
    {
        Name = name;
        CurrentJob = null;

        Skills = new List<Skill> ();
        SkillAffinities = new List<float> ();
        for (int i = 0; i < Enum.GetValues (typeof (SkillType)).Length; i++)
        {
            Skills.Add (new Skill ((SkillType)i));
            SkillAffinities.Add (1.0f);
        }

        CostPerHour = cost;
    }
}
