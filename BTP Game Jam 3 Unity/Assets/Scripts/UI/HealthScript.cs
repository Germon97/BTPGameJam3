using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class HealthScript : MonoBehaviour
{
    [HideInInspector]
    public UnityEvent<int> ChangedHealth;
    [HideInInspector]
    public UnityEvent GotKilled;

    [SerializeField]
    protected int startMaxHealth = 3;
    [SerializeField]
    private float invinsibleTime = 0.2f;
    [SerializeField]
    private GameObject hitMask;
    [SerializeField]
    private SpriteRenderer normalSprite;

    [SerializeField]
    private AudioClip hitSound;
    [SerializeField]
    private AudioClip killSound;

    protected int currentHealth;
    protected int maxHealth;

    [SerializeField]
    private bool totalyInvisible = false;

    private bool invinsible = false;

    private void Start()
    {
        Init();
    }

    protected void Init()
    {
        maxHealth = startMaxHealth;
        currentHealth = maxHealth;
    }

    public void ChangeHealth(int change)
    {
        if (currentHealth <= 0)
            return;

        if (change < 0)
        {
            // Can't take any damage because the character is invisible.
            if (invinsible || totalyInvisible)
                return;
            else
                StartCoroutine(OnHitRoutine());
        }

        currentHealth += change;

        if (ChangedHealth != null)
        {
            GlobalAudioSource.PlaySoundEffect(hitSound);
            ChangedHealth.Invoke(currentHealth);
        }

        if (currentHealth <= 0 && GotKilled != null)
        {
            GlobalAudioSource.PlaySoundEffect(killSound);
            GotKilled.Invoke();
        }
    }

    private IEnumerator OnHitRoutine()
    {
        hitMask.GetComponent<SpriteMask>().sprite = normalSprite.sprite;
        invinsible = true;
        hitMask.SetActive(true);
        yield return new WaitForSeconds(invinsibleTime);
        hitMask.SetActive(false);
        invinsible = false;
    }
}
