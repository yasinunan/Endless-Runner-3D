using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using LootLocker.Requests;

public class PlayerSkinItem : MonoBehaviour, IPointerClickHandler
{
    private string colorName;

    public void OnPointerClick(PointerEventData eventData)
    {
        LootLockerSDKManager.UpdateOrCreateKeyValue("skin", colorName, (response) => {
            if (response.success)
            {
                Debug.Log("Succesfully set the skin");
              
            }
            else
            {
                Debug.Log("Error setting the skin");
              
            }
        });
    }

    public void SetColor( string color)
    {
        if (ColorUtility.TryParseHtmlString(color, out Color newColor))
        {
            GetComponent<Image>().color = newColor;
        }

        colorName = color;
    }
}
 