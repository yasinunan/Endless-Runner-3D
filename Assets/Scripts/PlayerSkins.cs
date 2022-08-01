using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LootLocker.Requests;


public class PlayerSkins : MonoBehaviour
{

    [SerializeField]
    private Transform scrollViewContentTransform;
    [SerializeField]
    private GameObject prefab;


    public void GetSkins()
    {
        LootLockerSDKManager.GetInventory((response) =>
        {
            if (response.success)
            {
                Debug.Log("Succesfully got the player inventory.");
                LootLockerInventory[] items = response.inventory;
                for(int i = 0; i < items.Length; ++i)
                {
                    if(items[i].asset.context == "Unlockables")
                    {
                        GameObject item = Instantiate(prefab, scrollViewContentTransform);
                        item.GetComponent<PlayerSkinItem>().SetColor(items[i].asset.name);
                    }
                }
            }
            else
            {
                Debug.Log("UnSuccesfully got the player inventory.");
            }
        });
    }
}
