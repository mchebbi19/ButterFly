using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddAwards : MonoBehaviour
{
    public static AddAwards instance;
    public GameObject RewardPanel;
    public GameObject WatchVideoRewards;
    // Start is called before the first frame update
    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        RewardPanel.SetActive(false);
    }
    public void ReceiveRewad()
    {
        Debug.Log("received");
        //Score.TrySetNewMunitionCount(Score.GetMunitionCount() + GameAssets.MUNITION_AWARDES);
        RewardPanel.SetActive(false);
        WatchVideoRewards.SetActive(true);
    }

    
}
