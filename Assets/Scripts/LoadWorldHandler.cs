using System.Collections.Generic;
using UnityEngine;

public class LoadWorldHandler : MonoBehaviour
{
    public Object2DApiClient object2DApiClient;
    public ObjectPlacementScript objectPlacementScript;
    public GameObject[] prefabs;

    private string environmentId;

    async void OnEnable()
    {
        environmentId = PlayerPrefs.GetString("ActiveWorldId");
        Debug.Log("Loading world ID: " + environmentId);
        await LoadObjects();
    }

    private async Awaitable LoadObjects()
    {
        objectPlacementScript.DeleteAllObjects();

        IWebRequestReponse response = await object2DApiClient.ReadObject2Ds(environmentId);

        switch (response)
        {
            case WebRequestData<List<Object2D>> dataResponse:
                dataResponse.Data.ForEach(obj => SpawnObject(obj));
                break;
            case WebRequestError errorResponse:
                Debug.Log("Load objects error: " + errorResponse.ErrorMessage);
                break;
            default:
                throw new System.NotImplementedException("No implementation for: " + response.GetType());
        }
    }

    private void SpawnObject(Object2D obj)
    {
        int prefabIndex = int.Parse(obj.PrefabId);
        if (prefabIndex < 0 || prefabIndex >= prefabs.Length) return;

        GameObject go = Instantiate(prefabs[prefabIndex]);
        go.transform.position = new Vector3(obj.PositionX, obj.PositionY, 0);
        go.transform.localScale = new Vector3(obj.ScaleX, obj.ScaleY, 1);
        go.transform.rotation = Quaternion.Euler(0, 0, obj.RotationZ);

        Draggable draggable = go.GetComponent<Draggable>();
        if (draggable != null)
            draggable.objectData = obj;

        objectPlacementScript.RegisterLoadedObject(go);
    }
}