using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.IO;
using System;
// this is for Encoding method
using System.Text;
using UnityEngine.UI;
using UnityEditor;
public class WebSender : MonoBehaviour
{    
    private SpriteRenderer spriteImage;
    private Text result_emotion_text;
    private Text result_prob_text;  
    private string url = "http://localhost:5000/";
    string path;
    private string FileCounter = "0";
 
    
    private void Awake() {
        spriteImage = GameObject.Find("SpriteImage").GetComponent<SpriteRenderer>();
        result_emotion_text = GameObject.Find("result_emotion_text").GetComponent<Text>();
        result_prob_text = GameObject.Find("result_prob_text").GetComponent<Text>();
        
    }
    void Start()
    {
        Debug.Log("샌더 시작");
        StartCoroutine(StartUploading()); 
    }
    private void OnEnable() {
        Debug.Log("재시작 샌더");
        StartCoroutine(StartUploading());
    }
  
    public void DeletePicture()
    {
        path = "Assets/Resources/" + FileCounter + ".png";
        File.Delete(path);
        File.Delete(path+".meta");
        Debug.Log("delete picture");
        AssetDatabase.Refresh();
    }
    public void StopCo()
    {
        StopCoroutine("StartUploading");
    }
    IEnumerator StartUploading()
    {
        spriteImage.sprite = Resources.Load<Sprite>("0");
        WWWForm form = new WWWForm();
        byte[] textureBytes = null;
        Texture2D imageTexture = GetTextureCopy (spriteImage.sprite.texture);
        textureBytes = imageTexture.EncodeToPNG();

        form.AddBinaryData("myimage", textureBytes, "imageFromUnity.png","image/png");
        
        UnityWebRequest www = UnityWebRequest.Post(url, form);
       
        yield return www.SendWebRequest();
     
        if(www.isNetworkError || www.isHttpError) {
            Debug.Log(www.error);
        }
        else {
            Debug.Log("Form upload complete!");
            Debug.Log(www.downloadHandler.text);

            string data = www.downloadHandler.text ;
            string[] results = data.Split('T');
            string result_emotion = results[1];
            string result_prob = results[2];

            result_emotion_text.text = "Emotion is :: \n"+  result_emotion;
            result_prob_text.text = "probability is :: \n" + result_prob;
        }
        spriteImage.sprite = null;
        gameObject.SetActive(false);
        DeletePicture();

        // WWWForm form = new WWWForm();
        
        // WWW w = new WWW(url, form);

        // yield return w;

        // if(w.error!= null)
        // {
        //     Debug.Log("error: " + w.error);
        // }
        // else {
        //     Debug.Log(w.text);
        // }
        // w.Dispose();
    }
    Texture2D GetTextureCopy (Texture2D source)
	{
		//Create a RenderTexture
		RenderTexture rt = RenderTexture.GetTemporary (
			                   source.width,
			                   source.height,
			                   0,
			                   RenderTextureFormat.Default,
			                   RenderTextureReadWrite.Linear
		                   );

		//Copy source texture to the new render (RenderTexture) 
		Graphics.Blit (source, rt);

		//Store the active RenderTexture & activate new created one (rt)
		RenderTexture previous = RenderTexture.active;
		RenderTexture.active = rt;

		//Create new Texture2D and fill its pixels from rt and apply changes.
		Texture2D readableTexture = new Texture2D (source.width, source.height);
		readableTexture.ReadPixels (new Rect (0, 0, rt.width, rt.height), 0, 0);
		readableTexture.Apply ();

		//activate the (previous) RenderTexture and release texture created with (GetTemporary( ) ..)
		RenderTexture.active = previous;
		RenderTexture.ReleaseTemporary (rt);

		return readableTexture;
	}
}
