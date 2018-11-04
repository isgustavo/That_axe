using UnityEngine;

public class HornsBehaviour : MonoBehaviour {

    [SerializeField]
    private AudioSource hornsAudio;
    [SerializeField]
    private AudioSource[] troopsAudio;
    [SerializeField]
    private AudioSource environment;


    public void OnStartGame()
    {
        if (hornsAudio != null)
        {
            hornsAudio.Play();
        }

        if (environment != null)
        {
            environment.Stop();
        }

        if (troopsAudio != null)
        {
            Invoke("OnPlayTroops", 2f);
        }
        
    }

    public void OnEndGame()
    {
        hornsAudio.Stop();
        for (int i = 0; i < troopsAudio.Length; i++)
        {
            troopsAudio[i].Stop();
        }

        if (environment != null)
        {
            environment.Play();
        }

    }

    private void OnPlayTroops()
    {
        for (int i = 0; i < troopsAudio.Length; i++)
        {
            if (troopsAudio[i].isActiveAndEnabled)
            {
                troopsAudio[i].Play();
            }
        }
    }
}
