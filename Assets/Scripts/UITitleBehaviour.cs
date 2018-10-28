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
    private string speechWithoutAxeText = "Call u axe at first";
    private string grabAxeAfterSpeechText = "Now say <<Start>>";
    private string endGameText = "There is no failure in training! Say<<Start>> to play";

	void Start () {

        string[] keyword = {"Start"};

        recognizer = new KeywordRecognizer(keyword);
        recognizer.OnPhraseRecognized += OnPhraseRecognized;
        recognizer.Start();

        subtext.text = initText;

    }

    public void OnRestart()
    {
        recognizer.Start();
        subtext.text = endGameText;
        panel.SetActive(true);
        
    }

    public void OnFirstGrab()
    {
        isAxeGrabbed = true;
        
        subtext.text = grabAxeAfterSpeechText;
        
    }

    void OnPhraseRecognized(PhraseRecognizedEventArgs args)
    {
        if (!isAxeGrabbed)
        {
            isSpeechOnce = true;
            subtext.text = speechWithoutAxeText;
        }
        else
        {
            recognizer.Stop();
            panel.SetActive(false);
            OnStartGame.Invoke();
        }
    }
}
