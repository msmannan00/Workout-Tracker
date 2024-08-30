using UnityEngine;

public class DataManager : GenericSingletonClass<DataManager>
{
    private ExerciseData exerciseData = new ExerciseData();
    
    public void loadData()
    {
        TextAsset jsonFile = Resources.Load<TextAsset>("data/exercise");
        this.exerciseData = JsonUtility.FromJson<ExerciseData>(jsonFile.text);
    }

    public ExerciseData getExerciseData()
    {
        return this.exerciseData;
    }

}
