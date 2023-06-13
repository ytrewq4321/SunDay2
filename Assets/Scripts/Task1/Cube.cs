using UnityEngine;

public class Cube : MonoBehaviour
{
    [SerializeField] private float speed;
    private Renderer rendererGO;
    private Transform transformGO;

    void Start()
    {
        transformGO = gameObject.transform;
        rendererGO = GetComponent<Renderer>();
    }

    void Update()
    {
        transformGO.Rotate(Vector3.up, speed * Time.deltaTime);

        if (Input.touchCount>0)
        {
            Touch touch = Input.GetTouch(0);
            if(touch.phase==TouchPhase.Began)
            {
                Vector2 touchPosition = touch.position;
                Ray ray = Camera.main.ScreenPointToRay(touchPosition);
                if (Physics.Raycast(ray,out RaycastHit hit))
                {
                    if (hit.transform == gameObject.transform)
                    {
                        rendererGO.material.color= Random.ColorHSV();
                    }
                }
            }
        }
 
    }
}
