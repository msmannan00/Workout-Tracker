using Unity.VisualScripting;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

[CreateAssetMenu(fileName = "New Exercise", menuName = "Exercise/Create New Exercise")]
public class ExerciseData : ScriptableObject
{
    public string exerciseName;
    public string category;
    public Sprite icon;
    
    
}
