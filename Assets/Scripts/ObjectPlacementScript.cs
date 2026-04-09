using UnityEngine;
using System.Collections.Generic;

public class ObjectPlacementScript : MonoBehaviour
{
    public List<GameObject> objectsList = new();
    private List<GameObject> placedObjects = new();
    private int currentOrder = 0;

    public void PlaceObjectByIndex(int index)
    {
        var placedObject = Instantiate(objectsList[index]);

        var sr = placedObject.GetComponent<SpriteRenderer>();
        if (sr != null)
            sr.sortingOrder = ++currentOrder;

        var draggable = placedObject.GetComponent<Draggable>();
        draggable.objectData = new Object2D
        {
            EnvironmentId = PlayerPrefs.GetString("ActiveWorldId"),
            PrefabId = index.ToString(),
            ScaleX = placedObject.transform.localScale.x,
            ScaleY = placedObject.transform.localScale.y,
            SortingLayer = currentOrder
        };
        draggable.trans = placedObject.transform;
        draggable.StartDragging();

        placedObjects.Add(placedObject);
    }

    public void RegisterLoadedObject(GameObject go) => placedObjects.Add(go);
    public void UnregisterObject(GameObject go) => placedObjects.Remove(go);
    public List<GameObject> GetAllPlacedObjects() => placedObjects;

    public void DeleteAllObjects()
    {
        placedObjects.ForEach(o => Destroy(o));
        placedObjects.Clear();
    }
}