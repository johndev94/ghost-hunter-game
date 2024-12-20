using UnityEngine;


public class MusicManager : MonoBehaviour
{
    public AudioSource backgroundMusic;
    public AudioSource combatMusic;
    public float transitionSpeed = 1.0f;

    private bool isInCombat = false;


    void Update()
    {
        if (isInCombat)
        {
            // Fade out background music and fade in combat music
            backgroundMusic.volume = Mathf.Lerp(backgroundMusic.volume, 0, Time.deltaTime * transitionSpeed);
            combatMusic.volume = Mathf.Lerp(combatMusic.volume, 1, Time.deltaTime * transitionSpeed);
        }
        else
        {
            // Fade in background music and fade out combat music
            backgroundMusic.volume = Mathf.Lerp(backgroundMusic.volume, 1, Time.deltaTime * transitionSpeed);
            combatMusic.volume = Mathf.Lerp(combatMusic.volume, 0, Time.deltaTime * transitionSpeed);
        }
    }

    public void EnterCombat()
    {
        isInCombat = true;
    }

    public void ExitCombat()
    {
        isInCombat = false;
    }

}

