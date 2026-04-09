using System;
using UnityEngine;

public class Draggable : MonoBehaviour
{
    public Transform trans;
    public Object2D objectData;

    public float rotateSpeed = 90f;
    public float scaleSpeed = 1f;
    public float minScale = 0.1f;

    private bool isDragging = false;
    private ObjectPlacementScript objectPlacementScript;

    void Awake()
    {
        if (trans == null)
            trans = transform;
        objectPlacementScript = FindObjectOfType<ObjectPlacementScript>();
    }

    public void StartDragging()
    {
        isDragging = true;
    }

    public void Update()
    {
        if (!isDragging) return;

        trans.position = GetMousePosition();

        // rotate with left right arrow keys
        if (Input.GetKey(KeyCode.LeftArrow))
            trans.Rotate(0, 0, rotateSpeed * Time.deltaTime);
        if (Input.GetKey(KeyCode.RightArrow))
            trans.Rotate(0, 0, -rotateSpeed * Time.deltaTime);

        // scale with scroll wheel
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll != 0)
        {
            float newScale = Mathf.Max(minScale, trans.localScale.x + scroll * scaleSpeed);
            trans.localScale = new Vector3(newScale, newScale, 1);
        }

        // sorting layer with arrow up/down
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (sr != null)
        {
            if (Input.GetKeyDown(KeyCode.UpArrow))
                sr.sortingOrder++;
            if (Input.GetKeyDown(KeyCode.DownArrow))
                sr.sortingOrder--;
        }

        // delete with right mouse button
        if (Input.GetMouseButtonDown(1))
        {
            objectPlacementScript.UnregisterObject(gameObject);
            Destroy(gameObject);
        }
    }

    private void OnMouseUpAsButton()
    {
        if (!enabled) return;
        isDragging = !isDragging;
    }

    private Vector3 GetMousePosition()
    {
        Vector3 positionInWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        positionInWorld.z = 0;
        return positionInWorld;
    }
}