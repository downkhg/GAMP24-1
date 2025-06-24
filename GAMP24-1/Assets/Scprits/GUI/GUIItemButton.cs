using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class GUIItemButton : MonoBehaviour
{
    public TextMeshProUGUI textItemName;
    public ItemData itemData;

    public void Set(ItemData itemdata, GameObject target, UnityAction action)
    {
        this.itemData = itemdata;
        Button button = GetComponent<Button>();
        textItemName.text = itemData.name;
        button.onClick.AddListener(action);
    }

    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.T))
        //{
        //    ItemData itemData = GameManager.instance.itemDataManager.GetItem(0);
        //    GameObject target = GameManager.instance.responnerPlayer.objPlayer;
        //    Set(itemData, target);
        //}
    }
}
