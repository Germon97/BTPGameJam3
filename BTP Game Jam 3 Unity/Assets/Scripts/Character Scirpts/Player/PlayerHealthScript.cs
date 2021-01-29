using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class PlayerHealthScript : HealthScript
{
    [HideInInspector]
    public UnityEvent<int> ChangedMaxHealth;

    private void Start()
    {
        Reset();
        GotKilled.AddListener(PlayerGotKilled);
        Init();
    }

    private void PlayerGotKilled()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void Reset()
    {
        maxHealth = startMaxHealth;
        currentHealth = maxHealth;

        if (ChangedHealth != null)
            ChangedHealth.Invoke(currentHealth);
        if (ChangedMaxHealth != null)
            ChangedMaxHealth.Invoke(maxHealth);
    }
}
