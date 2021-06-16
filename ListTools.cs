using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ListTools
{
    public static void RemoveNullItems(List<GameObject> gameObjects)
    {
        foreach (var item in gameObjects)
        {
            if (item == null)
            {
                gameObjects.Remove(item);
                break;
            }
        }
    }
    public static GameObject FindClosestObject(List<GameObject> gameObjects, string tag, GameObject gameObject)
    {
        float closestFoodDistance = float.MaxValue;
        GameObject ClosestFoodObject = null;
        if (gameObjects.Count > 0)
        {
            foreach (GameObject item in gameObjects)
            {
                if (item != null)
                {
                    if (item.tag == tag)
                    {
                        if (item.GetComponent<Interactable>().reservedStatus == Interactable.ReservedStatuses.UnReserved)
                        {
                            float tempDistance = Vector3.Distance(gameObject.transform.position, item.transform.position);
                            if (closestFoodDistance > tempDistance)
                            {
                                closestFoodDistance = tempDistance;
                                ClosestFoodObject = item;
                            }
                        }
                    }
                }
            }
        }
        return ClosestFoodObject;
    }
    public static void DestoryListofGameObjects(List<GameObject> ListofGameObjects)
    {
        if (ListofGameObjects != null)
        {
            foreach (GameObject item in ListofGameObjects) Object.Destroy(item);
            ListofGameObjects.Clear();
        }
    }
    public static void AddOneToList(List<GameObject> AddingTo, GameObject thingToAdd)
    {
        if (!AddingTo.Contains(thingToAdd)) AddingTo.Add(thingToAdd);
    }
    public static GameObject FindInListByID(GameObject Item, List<GameObject> list)
    {
        GameObject temp = null;
        for (int i = 0; i < list.Count; i++)
        {
            if (list[i].GetComponent<Interactable>().Id == Item.GetComponent<Interactable>().Id)
            {
                temp = list[i];
                break;
            }
        }
        return temp;
    }
    public static GameObject FindInListByName(string Name, List<GameObject> list)
    {
        GameObject temp = null;
        for (int i = 0; i < list.Count; i++)
        {
            if (list[i].name == Name)
            {
                temp = list[i];
                break;
            }
        }
        return temp;
    }
    public static List<GameObject> ArrayToList(GameObject[] array)
    {
        List<GameObject> objects = new List<GameObject>();
        for (int i = 0; i < array.Length; i++)
        {
            objects.Add(array[i]);
        }
        return objects;
    }
    public static void DeActivateAllInList(List<GameObject> objects)
    {
        for (int i = 0; i < objects.Count; i++)
        {
            objects[i].SetActive(false);
        }
    }
    public static void ActivateAllInList(List<GameObject> objects)
    {
        for (int i = 0; i < objects.Count; i++)
        {
            objects[i].SetActive(true);
        }
    }
    public static bool AllIsActiveInList(List<GameObject> objects)
    {
        for (int i = 0; i < objects.Count; i++)
        {
            if (objects[i].activeInHierarchy == false)
            {
                return false;
            }
        }
        return true;
    }
    public static bool CompareLists(List<GameObject> list1, List<GameObject> list2)
    {
        list1.Sort(SortByName);
        list2.Sort(SortByName);
        if (list1.Count == list2.Count)
        {
            for (int i = 0; i < list1.Count; i++)
            {
                if (list1[i].GetComponent<Item>().Type != list2[i].GetComponent<Item>().Type)
                {
                    return false;
                }
            }
            return true;
        }
        return false;
    }
    static int SortByName(GameObject name1, GameObject name2)
    {
        return name1.GetComponent<Item>().Type.CompareTo(name2.GetComponent<Item>().Type);
    }
}
