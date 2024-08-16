using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExerciseController : MonoBehaviour,PageController
{
    public void onInit(Dictionary<string, object> data)
    {
        //throw new System.NotImplementedException();
    }
    public void Close()
    {
        StateManager.Instance.onRemoveBackHistory();
        Dictionary<string, object> mData = new Dictionary<string, object> { };
        StateManager.Instance.OpenStaticScreen("dashboard", gameObject, "dashboardScreen", mData);
        //Destroy(this.gameObject);
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
