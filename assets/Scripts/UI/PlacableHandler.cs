using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlacableHandler : MonoBehaviour {

    public GameObject placableObject;

    public void SetPlacableObject(GameObject obj)
    {
        placableObject = obj;
    }

    public void Update()
    {
        if (placableObject != null)
        {
            Vector2 mousePos = Input.mousePosition;
            Vector3 mousePosWorld = Camera.main.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, Camera.main.farClipPlane));

            placableObject.transform.position = mousePosWorld;

            if (Input.GetMouseButtonDown(0))
            {
                switch (placableObject.transform.name)
                {
                    case "Goal":
                        Swarm.SetGoalObj(placableObject);
                    break;

                    case "Obstacle":
                        Swarm.SetObstacleObj(placableObject);
                    break;                                                  
                }
                
                placableObject = null;
            }
        }
    }
    
}
