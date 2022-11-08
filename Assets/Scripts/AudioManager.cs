using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("There is more than one AudioManager in this scene! " + transform.position + " - " + Instance);
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void Start()
    {
        // Shooting SFX

        // Grenade SFX

        // Sword SFX

        // Open Door SFX

        // Close Door SFX

        // Death SFX

        // BGM
    }
}
