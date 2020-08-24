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
    // adds a requestor (Citizen) to a list and the requested item using a custom clas
    public StockPileRequest AddtoQueueandWaitForItem(GameObject Citizen , Item NeededItem)
    {
        StockPileRequest FoundItem = new StockPileRequest();
        if (!Queue.Contains(Citizen))
        {
            Queue.Add(Citizen);
        }
        if (Queue[0] == Citizen)
        {
        // if citizen is first in que checks for item
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
        foreach (GameObject item in PlayerOwnedManager.Instance.AllStoredItems)
        {
            Item itemScript = item.GetComponent<Item>();
            // checks if item is reserved or not
            if (itemScript.HauledBy == null || itemScript.HauledBy == Citizen)
            {
                if (itemScript.GetItemID == NeededItem.GetItemID)
                {
                // assigns who reserved the item to the item 
                    itemScript.HauledBy = Citizen;
                    return itemScript;
                }
            }
        }
        return null;
    }
}
