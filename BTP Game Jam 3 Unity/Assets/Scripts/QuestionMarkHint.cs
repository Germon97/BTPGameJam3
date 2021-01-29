using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class QuestionMarkHint : MonoBehaviour
{
    [SerializeField]
    private TextMeshPro text;
    [SerializeField]
    private GameObject outline;

    private bool playerInRange = false;
    private float faceSpeed = 2f;
    private float currentValue = 0;

    private void Start()
    {
        toggle(false);


        Color col = text.color;
        col.a = 0;
        text.color = col;
    }

    private void Update()
    {
        if (playerInRange && currentValue < 1)
        {
            currentValue += faceSpeed * Time.deltaTime;

            Color col = text.color;
            col.a = currentValue;
            text.color = col;
        }
        else if (currentValue > 0)
        {
            currentValue -= faceSpeed * Time.deltaTime;
            Color col = text.color;
            col.a = currentValue;
            text.color = col;
        }
    }

    private void toggle(bool active)
    {
        playerInRange = active;
        outline.SetActive(active);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            toggle(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            toggle(false);
        }
    }
}
