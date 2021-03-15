using System.Collections;
using System.Collections.Generic;
using TextSpeech;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Android;
public class VoiceController : MonoBehaviour
{
    const string LANG_CODE = "ko-KR";
    public Text uiText;
    private void Start() {
        Setup(LANG_CODE);
#if UNITY_ANDROID
    SpeechToText.instance.onPartialResultsCallback = OnPartialSpeechResult;
#endif
        SpeechToText.instance.onResultCallback = OnFinalSpeechResult;
        TextToSpeech.instance.onStartCallBack = OnSpeakStart;
        TextToSpeech.instance.onDoneCallback = OnSpeakStop;
    
        CheckPermission();
    }

    void CheckPermission() {
#if UNITY_ANDROID 

        if(!Permission.HasUserAuthorizedPermission(Permission.Microphone))
        {
            Permission.RequestUserPermission(Permission.Microphone);
        }
#endif
    }
    #region Text to Speech 
    public void StartSpeaking(string message) {
        TextToSpeech.instance.StartSpeak(message);
    }

    public void stopSpeaking()
    {
        TextToSpeech.instance.StopSpeak();
    }
    void OnSpeakStart()
    {
        Debug.Log("Speak start");
    }
    void OnSpeakStop()
    {
        Debug.Log("Speak Stop");
    }
    #endregion

    #region Speech To Text

    public void StartListening()
    {
        SpeechToText.instance.StartRecording();
    }
    public void StopListening()
    {
        SpeechToText.instance.StopRecording();
    }

    void OnFinalSpeechResult(string result)
    {
        uiText.text = result;
    }
    void OnPartialSpeechResult(string result)
    {
        uiText.text = result;
    }
    #endregion
    void Setup(string code) {
        TextToSpeech.instance.Setting(code,1,1);
        SpeechToText.instance.Setting(code);
    }
}
