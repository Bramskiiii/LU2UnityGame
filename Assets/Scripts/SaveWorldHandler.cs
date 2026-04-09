using System.Collections.Generic;
using UnityEngine;

public class SaveWorldHandler : MonoBehaviour
{
    public Object2DApiClient object2DApiClient;
    public GameObject saveMessage;

    public async void SaveAll()
    {
        Draggable[] allObjects = FindObjectsByType<Draggable>(FindObjectsSortMode.None);

        foreach (var draggable in allObjects)
        {
            draggable.objectData.PositionX = draggable.trans.position.x;
            draggable.objectData.PositionY = draggable.trans.position.y;
            draggable.objectData.RotationZ = draggable.trans.eulerAngles.z;
            draggable.objectData.ScaleX = draggable.trans.localScale.x;
            draggable.objectData.ScaleY = draggable.trans.localScale.y;

            if (string.IsNullOrEmpty(draggable.objectData.Id))
            {
                IWebRequestReponse response = await object2DApiClient.CreateObject2D(draggable.objectData);
                switch (response)
                {
                    case WebRequestData<Object2D> dataResponse:
                        draggable.objectData.Id = dataResponse.Data.Id;
                        break;
                    case WebRequestError errorResponse:
                        Debug.Log("Save error: " + errorResponse.ErrorMessage);
                        break;
                }
            }
            else
            {
                IWebRequestReponse response = await object2DApiClient.UpdateObject2D(draggable.objectData);
                switch (response)
                {
                    case WebRequestError errorResponse:
                        Debug.Log("Update error: " + errorResponse.ErrorMessage);
                        break;
                }
            }
        }

        saveMessage.SetActive(true);


        Debug.Log("Saved " + allObjects.Length + " objects.");
    }
}