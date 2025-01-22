using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class FriendModel
{
    public List<FriendData> friendData = new List<FriendData>();
}
[Serializable]
public class FriendData
{
    public string userName;
    public string badgeName;
    public int streak;
    public int level;
    public int goal;
    public string joiningDate;
    public string clothe;
    public string profileImageUrl;
    public Sprite profileImage;
    public AchievementData achievementData = new AchievementData();
    public PersonalBestData personalBestData = new PersonalBestData();
}