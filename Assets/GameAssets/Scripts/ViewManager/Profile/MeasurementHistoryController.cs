using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MeasurementHistoryController : MonoBehaviour,PageController
{
    public TextMeshProUGUI labelText;
    public GameObject noHistory;
    public Button backButton;
    public Transform content;
    void PageController.onInit(Dictionary<string, object> data, Action<object> callback)
    {
        string name = (string)data["name"];
        labelText.text = name;
        List<MeasurementHistoryItem> items = GetFilteredAndSortedItems(ApiDataHandler.Instance.getMeasurementHistory(),name);
        if (items.Count == 0) { noHistory.SetActive(true); }
        else
        {
            noHistory.SetActive(false);
            foreach (MeasurementHistoryItem item in items)
            {
                AddItems(item, name);
            }
        }
        backButton.onClick.AddListener(Back);
        backButton.onClick.AddListener(AudioController.Instance.OnButtonClick);
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Back();
        }
    }
    private void AddItems(MeasurementHistoryItem historyItem, string name)
    {
        GameObject prefab = Resources.Load<GameObject>("Prefabs/profile/measurementHistoryItem");
        GameObject newItem = Instantiate(prefab, content);
        MeasurementHistorySubItem itemcript = newItem.GetComponent<MeasurementHistorySubItem>();

        Dictionary<string, object> initData = new Dictionary<string, object>
        {
            {  "dateTime", historyItem.dateTime   },
            {"value", historyItem.value  },
            {"name", name}
            //{"inputManager",inputFieldManager}
        };
        itemcript.onInit(initData, null);
    }
    public List<MeasurementHistoryItem> GetFilteredAndSortedItems(MeasurementHistory history, string targetName)
    {
        return history.measurmentHistory
            .Where(item => item.name.ToLower() == targetName.ToLower())
            .OrderByDescending(item => DateTime.ParseExact(item.dateTime, "MMM dd, yyyy hh:mm tt", CultureInfo.InvariantCulture))
            .ToList();
    }
    public void Back()
    {
        StateManager.Instance.HandleBackAction(gameObject);
    }

}
