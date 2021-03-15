using UnityEngine;
using System.Collections;
using System.IO;
using UnityEngine.UI;

using UnityEditor;

public class TakeImg : MonoBehaviour 
{
    
    public int FileCounter = 0;
    
    WebCamTexture webCamTexture;


    public GameObject sender;


    public void DeletePicture()
    {
        string path = "Assets/Resources/" + FileCounter + ".png";
        File.Delete(path);
        File.Delete(path+".meta");
        Debug.Log("delete picture");
        AssetDatabase.Refresh();
    }
    void Start() 
    {
        DeletePicture();
        GetComponent<Button>().onClick.AddListener(() => TakePhoto());
        webCamTexture = new WebCamTexture();
        GetComponent<Renderer>().material.mainTexture = webCamTexture; //Add Mesh Renderer to the GameObject to which this script is attached to
        webCamTexture.Play();
    }

    public void TakePhoto()  // Start this Coroutine on some button click
    {

    // NOTE - you almost certainly have to do this here:
 

    // it's a rare case where the Unity doco is pretty clear,
    // http://docs.unity3d.com/ScriptReference/WaitForEndOfFrame.html
    // be sure to scroll down to the SECOND long example on that doco page 

        Texture2D photo = new Texture2D(webCamTexture.width, webCamTexture.height);
        photo.SetPixels(webCamTexture.GetPixels());
        photo.Apply();

        string path = "Assets/Resources/" + FileCounter + ".png";
        //Encode to a PNG
        byte[] bytes = photo.EncodeToPNG();
        //Write out the PNG. Of course you have to substitute your_path for something sensible
        
        //async process 비동기 처리  System Tasks 임포트해야함.
        //이 과정이 끝난 뒤, 아래 사진 보내기 처리
        File.WriteAllBytes(path, bytes);
        AssetDatabase.Refresh();
        AssetDatabase.ImportAsset(path);
        TextureImporter importer = AssetImporter.GetAtPath(path) as TextureImporter;
        importer.textureType = TextureImporterType.Sprite;
        AssetDatabase.WriteImportSettingsIfDirty(path);
        Invoke("OpenSender",2f) ;
    }

    
    public void OpenSender()
    {
        AssetDatabase.Refresh();
        sender.SetActive(true);
    }
    public void CloseSender()
    {
        sender.SetActive(false);
    }
}