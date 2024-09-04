using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateNewWorkoutController : MonoBehaviour,PageController
{
    public void onInit(Dictionary<string, object> data, Action<object> callback)
    {
       
    }
    public void OnClose()
    {
        StateManager.Instance.HandleBackAction(gameObject);
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
