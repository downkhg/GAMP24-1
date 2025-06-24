using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Iventory : MonoBehaviour
{
    public List<ItemData> itemList = new List<ItemData>();

    public void SetItem(Item item)
    {
        if (item)
            itemList.Add(item.itemData);
        else
            Debug.LogWarning($"SetItem:{item.gameObject.name}");
    }

    public void DeleteItem(ItemData itemData)
    {
        itemList.Remove(itemData);
    }

    public void ChatItem(ItemDataManager itemDataManage, int count)
    {
        Debug.Log("ChatItem:"+count);
        for (int i = 0; i < count; i++)
        {
            foreach (ItemData item in itemDataManage.ItemDatas)
            {
                Debug.Log("AddIventory:" + item.name);
                itemList.Add((ItemData)item);
            }
        }
    }

    public int idx = 3;

    private void OnGUI()
    {
        int w = 100;
        int h = 20;

        for (int i = 0; i < itemList.Count; i++)
        {
            if(GUI.Button(new Rect(w*idx,h*i,100,20),$"{itemList[i].name}"))
            {
                itemList[i].Use(this.gameObject);
                itemList.Remove(itemList[i]);
            }
        }
    }
}
