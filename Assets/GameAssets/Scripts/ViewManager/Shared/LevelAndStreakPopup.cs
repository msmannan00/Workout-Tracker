using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LevelAndStreakPopup : MonoBehaviour, IPrefabInitializer
{
    public TextMeshProUGUI titleText;
    public TextMeshProUGUI descriptionText;
    public ParticleSystem particles;
    public GameObject coins;
    public TextMeshProUGUI coinText;
    bool goalComplete;
    public void InitPrefab(Action<List<object>> onFinish, List<object> data)
    {
        goalComplete = (bool)data[0];
        titleText.text = (string)data[1];
        descriptionText.text= (string)data[2];
        if (goalComplete)
        {
            coins.SetActive(true);
            if (IAPManager.Instance.isSubscripted)
                coinText.text = "+10";
            else
                coinText.text = "0";
            Invoke("PlayParticle", 0.5f);
        }
        else
        {
            coins.SetActive(false);
        }
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Continue();
        }
    }
    void PlayParticle()
    {
        AudioController.Instance.OnAchievement();
        particles.Play();
    }

    public void Continue()
    {
        AudioController.Instance.OnButtonClick();
        PopupController.Instance.ClosePopup("LevelAndStreakPopup");
    }
}

