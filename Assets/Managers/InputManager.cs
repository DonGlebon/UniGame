using UnityEngine;

public class InputManager : MonoBehaviour
{


    public float Horizontal { get { return horizontal; } private set { horizontal = value; } }
    private float horizontal;
    public float Vertical { get { return vertical; } private set { vertical = value; } }
    private float vertical;

    // Use this for initialization
    private void Start()
    {

    }

    private void Update()
    {
        Horizontal = Input.GetAxis("Horizontal");
        Vertical = Input.GetAxis("Vertical");
    }
}
