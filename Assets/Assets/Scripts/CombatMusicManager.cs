using UnityEngine;

public class CombatMusicManager : MonoBehaviour
{
    public AudioSource combatMusic;
    public AudioSource normalMusic;

    private bool isCombatMusicPlaying = false;

    // Play combat music
    public void PlayCombatMusic()
    {
        if (isCombatMusicPlaying == false)
        {
            normalMusic.Stop();
            combatMusic.Play();
            isCombatMusicPlaying = true;
        }
    }

    // Stop combat music and resume normal music
    public void StopCombatMusic()
    {
        if (isCombatMusicPlaying)
        {
            combatMusic.Stop();
            normalMusic.Play();
            isCombatMusicPlaying = false;
        }
    }
}
