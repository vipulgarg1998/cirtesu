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
			byte[] imageData = this.cameraCapture.getJPGFromCurrentCamera();
			
			RosMessageTypes.Sensor.CompressedImageMsg img = new RosMessageTypes.Sensor.CompressedImageMsg(new RosMessageTypes.Std.HeaderMsg(), this.encoding, imageData);
			
			this.ros.Publish(topic, img);
			
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
