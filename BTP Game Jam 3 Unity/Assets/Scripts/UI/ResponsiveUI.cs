using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// Adds little things to make the UI more responsive.
/// </summary>
[RequireComponent(typeof(Outline))]
public class ResponsiveUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField]
    private AudioClip hoverSound;
    [SerializeField]
    private AudioClip clickSound;

    private Outline outline;

    private void Start()
    {
        outline = GetComponent<Outline>();
        outline.enabled = false;
    }

    public void Click()
    {
        GlobalAudioSource.PlaySoundEffect(clickSound);
        outline.enabled = false;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        outline.enabled = true;
        GlobalAudioSource.PlaySoundEffect(hoverSound);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        outline.enabled = false;
    }
}
