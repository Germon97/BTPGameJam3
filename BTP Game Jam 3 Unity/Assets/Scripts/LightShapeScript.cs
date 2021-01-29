using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;

public class LightShapeScript : MonoBehaviour
{
    private UnityEngine.Experimental.Rendering.Universal.Light2D shapeLight;

    [SerializeField]
    private GameObject lightPrefab;
    [SerializeField]
    private float distance = 8;
    [SerializeField]
    private float angle = 8;
    [SerializeField]
    private LayerMask lightLayer;


    // Start is called before the first frame update
    void Start()
    {

    }


    private void Update()
    {
        UpdateLight();
    }

    [ContextMenu("UpdateLight")]
    private void UpdateLight()
    {
        if (shapeLight != null)
            Destroy(shapeLight.gameObject);

        GameObject light = Instantiate(lightPrefab, transform.position, Quaternion.identity);
        light.transform.SetParent(transform);

        shapeLight = light.GetComponent<UnityEngine.Experimental.Rendering.Universal.Light2D>();

        float startAngle = angle / 2;

        for (int i = 0; i < 10; i++)
        {
            startAngle += angle / 10;

            RaycastHit2D hit = Physics2D.Raycast(transform.position, Quaternion.Euler(0, 0, startAngle) * transform.rotation * Vector3.right, distance, lightLayer);

            if (hit.collider == null)
            {
                shapeLight.shapePath[i] = Quaternion.Euler(0, 0, startAngle) * transform.rotation * Vector3.right * distance;
            }
            else
            {
                shapeLight.shapePath[i] = transform.worldToLocalMatrix * hit.point;
            }
            
        }

        shapeLight.shapePath[10] = Vector3.zero;

        // SetFieldValue<Vector3[]>(shapeLight, "m_ShapePath", points);
    }

    void SetFieldValue<T>(object obj, string name, T val)
    {
        var field = obj.GetType().GetField(name, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
        field?.SetValue(obj, val);
    }

}
