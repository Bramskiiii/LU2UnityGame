using UnityEngine;

public class RaceCreatorScreenLogic : MonoBehaviour
{
    public GameObject CreateTrackManager;
    
    //OnEnabled is 
    void OnEnabled()
    {
        var tracks = CreateTrackManager.GetComponent<CreateTrackManager>().tracks;
        for (int i = 0; i <= tracks.Count; i++)
        {
            Debug.Log("Track: " + tracks[i].name);
        }
    }
}
