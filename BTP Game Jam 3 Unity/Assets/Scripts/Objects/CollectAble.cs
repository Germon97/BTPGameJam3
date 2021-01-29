using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectAble : MonoBehaviour
{
    public CollectableData data;

    [SerializeField]
    private GameObject outline;

    private void Start()
    {
        Init(data);
    }

    public void Init(CollectableData data)
    {
        this.data = data;
        GetComponent<SpriteRenderer>().sprite = data.sprite;
        outline.GetComponent<SpriteRenderer>().sprite = data.sprite;
        outline.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        outline.SetActive(true);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        outline.SetActive(false);
    }
}
