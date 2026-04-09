using UnityEngine;
using System.Collections.Generic;

public class ObjectPlacementManager : MonoBehaviour
{
    public string environmentId;

    [Header("Prefabs")]
    public GameObject[] availablePrefabs; // Alle plaatsbare prefabs, index = PrefabId

    [Header("Dependencies")]
    public Object2DApiClient objectApiClient;
    public Environment2DApiClient environmentApiClient;

    private List<GameObject> spawnedObjects = new();
    private List<Object2D> loadedObject2Ds = new();

    // Objects laden bij het openen van een wereld
    public async void LoadObjects()
    {
        ClearSpawnedObjects();

        IWebRequestReponse response = await objectApiClient.ReadObject2Ds(environmentId);

        switch (response)
        {
            case WebRequestData<List<Object2D>> data:
                loadedObject2Ds = data.Data;
                foreach (var obj in loadedObject2Ds)
                {
                    SpawnFromData(obj);
                }
                Debug.Log($"Loaded {loadedObject2Ds.Count} objects");
                break;

            case WebRequestError error:
                Debug.LogError("Objects laden mislukt: " + error.ErrorMessage);
                break;
        }
    }

    // Nieuw object plaatsen (vanuit UI knoppen)
    public void PlaceObjectByPrefabIndex(int prefabIndex)
    {
        if (prefabIndex < 0 || prefabIndex >= availablePrefabs.Length) return;

        GameObject placed = Instantiate(availablePrefabs[prefabIndex]);
        var draggable = placed.GetComponent<Draggable>();
        draggable.StartDragging();
        draggable.trans = placed.transform;

        spawnedObjects.Add(placed);
    }

    // Alle objects opslaan naar de API (bij Home knop)
    public async void SaveAllObjects()
    {
        foreach (var obj in spawnedObjects)
        {
            if (obj == null) continue;

            int prefabIndex = GetPrefabIndex(obj);
            if (prefabIndex == -1) continue;

            // Check of dit object al in de database staat
            string existingId = FindExistingObjectId(obj);

            if (existingId != null)
            {
                // Update bestaand object
                Object2D updated = CreateObject2DFromGameObject(obj, prefabIndex);
                updated.Id = existingId;
                await objectApiClient.UpdateObject2D(updated);
            }
            else
            {
                // Nieuw object aanmaken
                Object2D newObj = CreateObject2DFromGameObject(obj, prefabIndex);
                IWebRequestReponse response = await objectApiClient.CreateObject2D(newObj);

                if (response is WebRequestData<Object2D> created)
                {
                    // Sla het ID op voor toekomstige updates
                    obj.name = created.Data.Id;
                }
            }
        }

        Debug.Log("Alle objects opgeslagen!");
        ScreenManager.Instance.SwitchTo(2); // Terug naar world overzicht
    }

    // Wereld verwijderen
    public async void DeleteEnvironment()
    {
        IWebRequestReponse response = await environmentApiClient.DeleteEnvironment(environmentId);

        switch (response)
        {
            case WebRequestData<string>:
                Debug.Log("Wereld verwijderd!");
                ClearSpawnedObjects();
                ScreenManager.Instance.SwitchTo(2);
                break;

            case WebRequestError error:
                Debug.LogError("Verwijderen mislukt: " + error.ErrorMessage);
                break;
        }
    }

    // === Helper methodes ===

    private void SpawnFromData(Object2D data)
    {
        int prefabIndex = int.Parse(data.PrefabId);
        if (prefabIndex < 0 || prefabIndex >= availablePrefabs.Length) return;

        GameObject obj = Instantiate(availablePrefabs[prefabIndex]);
        obj.transform.position = new Vector3(data.PositionX, data.PositionY, 0);
        obj.transform.localScale = new Vector3(data.ScaleX, data.ScaleY, 1);
        obj.transform.rotation = Quaternion.Euler(0, 0, data.RotationZ);
        obj.name = data.Id; // Sla ID op in de naam voor tracking

        var sr = obj.GetComponent<SpriteRenderer>();
        if (sr != null) sr.sortingOrder = data.SortingLayer;

        spawnedObjects.Add(obj);
    }

    private Object2D CreateObject2DFromGameObject(GameObject obj, int prefabIndex)
    {
        var sr = obj.GetComponent<SpriteRenderer>();
        return new Object2D
        {
            EnvironmentId = environmentId,
            PrefabId = prefabIndex.ToString(),
            PositionX = obj.transform.position.x,
            PositionY = obj.transform.position.y,
            ScaleX = obj.transform.localScale.x,
            ScaleY = obj.transform.localScale.y,
            RotationZ = obj.transform.rotation.eulerAngles.z,
            SortingLayer = sr != null ? sr.sortingOrder : 0
        };
    }

    private int GetPrefabIndex(GameObject obj)
    {
        string objName = obj.name.Replace("(Clone)", "").Trim();
        for (int i = 0; i < availablePrefabs.Length; i++)
        {
            if (availablePrefabs[i].name == objName)
                return i;
        }
        return -1;
    }

    private string FindExistingObjectId(GameObject obj)
    {
        // Als de naam een GUID is, dan is het een bestaand object
        if (System.Guid.TryParse(obj.name, out _))
            return obj.name;
        return null;
    }

    private void ClearSpawnedObjects()
    {
        foreach (var obj in spawnedObjects)
        {
            if (obj != null) Destroy(obj);
        }
        spawnedObjects.Clear();
        loadedObject2Ds.Clear();
    }
}