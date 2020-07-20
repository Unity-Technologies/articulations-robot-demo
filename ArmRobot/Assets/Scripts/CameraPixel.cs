using UnityEngine;
using System.Collections;

public class CameraPixel : MonoBehaviour
{
    public GameObject cube;
    public Camera _camera;


    public Vector2 GetPixelPosition()
    {   
        Vector3 position = cube.transform.position;
        Vector3 screenPos = _camera.WorldToScreenPoint(position);
        float width =  _camera.GetComponent<Camera>().pixelRect.width;
        float height = _camera.GetComponent<Camera>().pixelRect.height;
        //float positionX = screenPos.x/width;
        //float positionY = screenPos.y/height;
        float positionX = screenPos.x;
        float positionY = screenPos.y;
        Vector2 screenPosCenter = new Vector2(positionX, positionY);
        return screenPosCenter;
    }
}