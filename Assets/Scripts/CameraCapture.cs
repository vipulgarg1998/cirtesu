using System;
using UnityEngine;

public class CameraCapture : MonoBehaviour
{
	
	public bool isCaptureEnable = false;
	public Camera _camera;
	public int resWidth = 256;
	public int resHeight = 256;
	public byte[] jpg;
	
	
    private RenderTexture renderTexture;
	
    // Start is called before the first frame update
    void Start()
    {
        this._camera = GetComponent<Camera>();
        Debug.Log(this._camera.projectionMatrix);
        
        // Render texture 
        renderTexture = new RenderTexture(this._camera.pixelWidth, this._camera.pixelHeight, 24, UnityEngine.Experimental.Rendering.GraphicsFormat.R8G8B8A8_SRGB);
        renderTexture.Create();
    }
    
    private void FixedUpdate(){
    	if(this.isCaptureEnable){
    		this.jpg = getJPGFromCurrentCamera();
    		this.isCaptureEnable = false;
		}
	}
	
	public byte[] getCapturedJpegImage(){
		this.isCaptureEnable = true;
		while(this.isCaptureEnable) { }
		if(this.jpg != null){
			return this.jpg;
		}
		
		return null;
	}
	
	public byte[] getJPGFromCurrentCamera(){
		try{
			RenderTexture rt = new RenderTexture(resWidth, resHeight, 24);
			_camera.targetTexture = rt;
			Texture2D screenshot = new Texture2D(resWidth, resHeight, TextureFormat.RGB24, false);
			_camera.Render();
			RenderTexture.active = rt;
			screenshot.ReadPixels(new Rect(0, 0, resWidth, resHeight), 0, 0);
            Debug.Log(_camera.targetTexture.width);
            Debug.Log(_camera.targetTexture.height);
			_camera.targetTexture = null;
			RenderTexture.active = null;
			Destroy(rt);
			byte[] bytes = screenshot.EncodeToJPG();
            //Debug.Log("Retuning bytes");
            //Debug.Log("Cam Width");
            //Debug.Log("Cam Height");
			return bytes;
		}
		catch (Exception e){
			return null;
		}
	}
	
    private byte[] CaptureScreenshot(){
		try{
		    this._camera.targetTexture = renderTexture;
		    RenderTexture currentRT = RenderTexture.active;
		    RenderTexture.active = renderTexture;
		    this._camera.Render();
		    Texture2D mainCameraTexture = new Texture2D(renderTexture.width, renderTexture.height);
		    mainCameraTexture.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);
		    mainCameraTexture.Apply();
		    RenderTexture.active = currentRT;
		    // Get the raw byte info from the screenshot
		    byte[] imageBytes = mainCameraTexture.GetRawTextureData();
		    this._camera.targetTexture = null;
		    return imageBytes;
		}
		catch (Exception e){
			return null;
		}
    }
}
