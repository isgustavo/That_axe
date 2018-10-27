using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HornsBehaviour : MonoBehaviour {

    [SerializeField]
    private AudioSource hornsAudio;
    [SerializeField]
    private AudioSource troopsAudio;


    public void OnStartGame()
    {
        if (hornsAudio != null)
        {
            hornsAudio.Play();
        }

        if (troopsAudio != null)
        {
            Invoke("OnPlayTroops", 2f);
        }
        
    }

    private void OnPlayTroops()
    {
        if(troopsAudio.isActiveAndEnabled)
        {
            troopsAudio.Play();
        }
    }
}
