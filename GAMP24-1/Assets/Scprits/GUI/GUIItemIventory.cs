using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.GraphicsBuffer;

public class GUIItemIventory : MonoBehaviour
{
    public GameObject prefabButton;
    public List<GUIItemButton> guiBtnItems;
    public GridLayoutGroup gridLayoutGroup;

    public void InitItemButton(Iventory iventory, GameObject target)
    {
        Debug.Log("InitItemButton:"+iventory.itemList.Count);
        foreach (ItemData itemData in iventory.itemList)
        {
            Debug.Log("InitItemButton_ButtonAdd:" + itemData.name);
            GameObject objButton = Instantiate(prefabButton, gridLayoutGroup.transform);
            GUIItemButton guiItemButton = objButton.GetComponent<GUIItemButton>();
            guiItemButton.gameObject.name = itemData.name;
            Button button = objButton.GetComponent<Button>();
            guiItemButton.Set(itemData, target, () => EventItemButton(itemData,target,guiItemButton,iventory));
            guiBtnItems.Add(guiItemButton);
        }
        ResizeContentSize();
    }

    public void ClearButton()
    {
        Debug.Log("ClearButton:" + guiBtnItems.Count);
        for (int i = guiBtnItems.Count - 1; i >= 0; i--)
        {
            Destroy(guiBtnItems[i].gameObject);
        }
        guiBtnItems.Clear();
    }

    public void EventItemButton(ItemData itemData, GameObject target, GUIItemButton guiItemButton, Iventory iventory)
    {
        Debug.Log($"itemData:{itemData.name},Target: {target.name}, Button:{guiItemButton.gameObject.name}");
        itemData.Use(target); 
        iventory.itemList.Remove(itemData); 
        guiBtnItems.Remove(guiItemButton);
        Destroy(guiItemButton.gameObject);
    }

    public void ResizeContentSize()
    {
        RectTransform rectTransform = gridLayoutGroup.GetComponent<RectTransform>();
        Vector2 vGridSize = gridLayoutGroup.cellSize;
        Vector2 vContentSize = rectTransform.sizeDelta;
        
        float fCol = vContentSize.x / vGridSize.x; //300/100 = 3
        float fRow = Mathf.Ceil(guiBtnItems.Count / fCol); //9/3 = 3
        Debug.Log($"Grid/ContentSize[{fCol},{fRow}]:{vGridSize}/{vContentSize}");
        vContentSize.y = vGridSize.y * fRow;
        rectTransform.sizeDelta = vContentSize;
        Debug.Log($"Grid/ContentSize:{vGridSize}/{vContentSize}");
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            //Iventory iventory = GameManager.instance.responnerPlayer.objPlayer.GetComponent<Iventory>();
            //GameObject target = GameManager.instance.responnerPlayer.objPlayer;
            //InitItemButton(iventory, target);
            //ResizeContentSize();
        }
    }
}
