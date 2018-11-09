using UnityEngine;

public class TouchController : MonoBehaviour
{








    public GameObject joystic;
    public static float radius;
    private static Vector2 stickPosition;
    private static GameObject basis = null;
    private static GameObject stick;
    private GameObject canvas;
    private Vector2 offset = Vector2.zero;
    private float height;

    private void Start()
    {
        height = Screen.height / 4;
        radius = height / 2;
        canvas = GameObject.FindGameObjectWithTag("Canvas");
        basis = Instantiate(joystic, canvas.transform);
        basis.transform.position = new Vector3(-999, -999);
        basis.GetComponent<RectTransform>().sizeDelta = new Vector2(height * 2, height * 2);
        stick = basis.transform.GetChild(0).gameObject;
        stick.GetComponent<RectTransform>().sizeDelta = new Vector2(radius * 1.5f, radius * 1.5f);
    }

    public static float GetAxis(string axis)
    {
        float velocity = 0;
        if (axis.Equals("Horizontal"))
        {
            velocity = (stick.transform.position.x - basis.transform.position.x) / radius;
        }
        else if (axis == "Vertical")
        {
            velocity = (stick.transform.position.y - basis.transform.position.y) / radius;
        }
        if (velocity > 1 || velocity < -1)
            velocity = 0;
        return velocity;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Debug.Log("tap");
            Ray ray = GameManager.Instance.Audio.camera.ScreenPointToRay(Input.mousePosition);
          
            if (Physics.Raycast(ray, out hit))
            {
                Transform objectHit = hit.transform;
                Debug.Log(objectHit.name);

            }
        }

        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            switch (touch.phase)
            {
                case TouchPhase.Began:
                    {
                        float margin = height / 1.5f;
                        offset = Vector2.zero;
                        if (touch.position.x < margin) 
                        {
                            offset.x = margin - touch.position.x;
                        }
                        if (touch.position.y < margin)
                        {
                            offset.y = margin - touch.position.y;
                        }
                        if (touch.position.x > Screen.width - margin)
                        {
                            offset.x -= touch.position.x - (Screen.width - margin);
                        }
                        if (touch.position.y > Screen.height - margin)
                        {
                            offset.y -= touch.position.y - (Screen.height - margin);
                        }
                        basis.transform.position = touch.position + offset;
                        break;
                    }
                case TouchPhase.Moved:
                    {
                        float distance = Vector2.Distance(basis.transform.position, touch.position);
                        float moveDistance = (distance < radius) ? distance : radius;
                        stick.transform.position = (Vector2)basis.transform.position + (touch.position - (Vector2)basis.transform.position).normalized * moveDistance;
                        break;
                    }
                case TouchPhase.Ended:
                    {
                        basis.transform.position = new Vector3(-999, -999);
                        stick.transform.position = new Vector3(-999, -999);
                        break;
                    }
            }
        }
    }
}
