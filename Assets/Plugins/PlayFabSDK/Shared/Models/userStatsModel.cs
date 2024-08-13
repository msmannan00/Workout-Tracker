using System;

[Serializable]
public class userStatsModel
{
    public float sCarbs { get; set; }
    public float sFats { get; set; }
    public float sProteins { get; set; }
    public float sKiloCalories { get; set; }
    public DateTime sStartingDate { get; set; }
    public DateTime sEndingDate { get; set; }
    public bool sStatus { get; set; }
    public bool sContinueWeeklyPlan { get; set; }

    public userStatsModel()
    {
        sCarbs = 0f;
        sFats = 0f;
        sProteins = 0f;
        sKiloCalories = 0f;
        sStartingDate = DateTime.Now;
        sEndingDate = DateTime.Now.AddDays(7);
        sStatus = false;
        sContinueWeeklyPlan = true;
    }

    public void OnInitialize(float pCarbs = 0f, float pFats = 0f, float pProteins = 0f, float pKiloCalories = 0f, DateTime? pStartingDate = null, DateTime? pEndingDate = null, bool pStatus = false, bool pContinueWeeklyPlan = true)
    {
        sCarbs = pCarbs;
        sFats = pFats;
        sProteins = pProteins;
        sKiloCalories = pKiloCalories;
        sStartingDate = pStartingDate ?? DateTime.Now;
        sEndingDate = pEndingDate ?? DateTime.Now.AddDays(7);
        sStatus = pStatus;
        sContinueWeeklyPlan = pContinueWeeklyPlan;
    }
}
