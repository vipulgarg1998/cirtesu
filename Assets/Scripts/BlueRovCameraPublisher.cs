using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Robotics.ROSTCPConnector;


public class BlueRovCameraPublisher : MonoBehaviour
{
    private ROSConnection ros;
    
	public string topic = "camera";
	public string encoding = "jpg";
	public CameraCapture cameraCapture;
	private uint _count = 0;
	private int isBigEndian = 0;
	private int step = 4;
	private System.DateTime epochStart = new System.DateTime(1970, 1, 1, 0, 0, 0, System.DateTimeKind.Utc);
	
    // Publish the cube's position and rotation every N seconds
    public float publishMessageFrequency = 2f;
    // Used to determine how much time has elapsed since the last message was published
    private float timeElapsed;
	
    // Start is called before the first frame update
    void Start()
    {
    	this.cameraCapture = GetComponent<CameraCapture>();
        this.ros = ROSConnection.GetOrCreateInstance();
        this.ros.RegisterPublisher<RosMessageTypes.Sensor.CompressedImageMsg>(topic);
    }

    IEnumerator publish()
	{
        timeElapsed += Time.fixedDeltaTime;
        if (timeElapsed > publishMessageFrequency)
        {
			// Encode the texture as a PNG, and send to ROS
			/*
			var timeMessage = new RosMessageTypes.BuiltinInterfaces.TimeMsg((int)Time.realtimeSinceStartup, 0);
			byte[] imageBytes = this.cameraCapture.getCapturedJpegImage();
			RosMessageTypes.Sensor.ImageMsg img = new RosMessageTypes.Sensor.ImageMsg();
			RosMessageTypes.Std.HeaderMsg header = new RosMessageTypes.Std.HeaderMsg(timeMessage, "camera");
			img.header = header;
			img.width = (uint)this.cameraCapture.resWidth;
			img.height = (uint)this.cameraCapture.resHeight;
			img.encoding = this.encoding;
			img.step = 4;
			img.data = imageBytes;
			*/
			/*
			byte[] imageData = this.cameraCapture.getCapturedJpegImage();
	        RosMessageTypes.Sensor.ImageMsg img = new RosMessageTypes.Sensor.ImageMsg(
	        	new RosMessageTypes.Std.HeaderMsg(), 
	        	(uint)this.cameraCapture._camera.targetTexture.width, 
	        	(uint)this.cameraCapture._camera.targetTexture.height, 
	        	this.encoding, 
	        	(byte)this.isBigEndian, 
	        	(uint)this.step, 
	        	imageData);
        	*/
        	
            //Debug.Log("Asking Img Bytes");
			byte[] imageData = this.cameraCapture.getJPGFromCurrentCamera();
			
            //Debug.Log("Prep Msg");
			RosMessageTypes.Sensor.CompressedImageMsg img = new RosMessageTypes.Sensor.CompressedImageMsg(new RosMessageTypes.Std.HeaderMsg(), this.encoding, imageData);
			
            //Debug.Log("Sending Msg");
			this.ros.Publish(topic, img);
			
            //Debug.Log("Msg sent");
            timeElapsed = 0;
            this._count = this._count + 1;
            yield return null;
		}
	}

    // Update is called once per frame
    void FixedUpdate()
    { 
        StartCoroutine(publish());
    }
}
