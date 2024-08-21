using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OnClickAddElement : MonoBehaviour
{
    public Button AddElementBtn;
    public GameObject ElementPrefabsRef;
    List<GameObject> AllElements = new List<GameObject>();
    private void Start()
    {
        AddElementBtn.onClick.AddListener(AddNewSet);
    }

    public void AddNewSet()
    {
        AllElements.Add(Instantiate(ElementPrefabsRef, this.transform));
    }
}
