using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StockPileManager : MonoBehaviour
{
    private static StockPileManager _instance;

    public static StockPileManager Instance { get { return _instance; } }

    List<GameObject> Queue = new List<GameObject>();

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }
    public StockPileRequest AddtoQueueandWaitForItem(GameObject Citizen , Item NeededItem)
    {
        StockPileRequest FoundItem = new StockPileRequest();
        if (!Queue.Contains(Citizen))
        {
            Queue.Add(Citizen);
        }
        if (Queue[0] == Citizen)
        {
            FoundItem.NeededItem = FindNeededItem(Citizen, NeededItem);
            if (FoundItem != null)
            {
                Queue.Remove(Citizen);
                return FoundItem;
            }
        }
        FoundItem.NeededItem = null;
        return FoundItem;
    }
    Item FindNeededItem(GameObject Citizen, Item NeededItem)
    {
        foreach (GameObject item in GameManager.Instance.playerOwnedManager.AllStoredItems)
        {
            Item itemScript = item.GetComponent<Item>();
            if (itemScript.HauledBy == null || itemScript.HauledBy == Citizen)
            {
                if (itemScript.GetItemID == NeededItem.GetItemID)
                {
                    itemScript.HauledBy = Citizen;
                    return itemScript;
                }
            }
        }
        return null;
    }
}
