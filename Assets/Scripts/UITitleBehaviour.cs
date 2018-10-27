using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Windows.Speech;

public class UITitleBehaviour : MonoBehaviour {

    [SerializeField]
    private GameObject panel;
    [SerializeField]
    private TextMeshProUGUI subtext;
    [SerializeField]
    private UnityEvent OnStartGame;

    private KeywordRecognizer recognizer;
    private bool isAxeGrabbed = false;
    private bool isSpeechOnce = false;

    private string initText = "Call u axe like Thor God of Thunder or Kratos God of War and say <<Start>> to play";
    private string speechWithoutAxe = "Call u axe first";
    private string grabAxeAfterSpeech = "Now say <<Start>>";

	void Start () {

        string[] keyword = {"Start"};

        recognizer = new KeywordRecognizer(keyword);
        recognizer.OnPhraseRecognized += OnPhraseRecognized;
        recognizer.Start();

        subtext.text = initText;

    }

    public void OnFirstGrab()
    {
        isAxeGrabbed = true;
        
        subtext.text = grabAxeAfterSpeech;
        
    }

    void OnPhraseRecognized(PhraseRecognizedEventArgs args)
    {
        if (!isAxeGrabbed)
        {
            isSpeechOnce = true;
            subtext.text = speechWithoutAxe;
        }
        else
        {
            panel.SetActive(false);
            OnStartGame.Invoke();
        }
    }
}
