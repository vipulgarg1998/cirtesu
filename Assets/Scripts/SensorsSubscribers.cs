using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Robotics.ROSTCPConnector;

public class SensorsSubscribers : MonoBehaviour
{
	public string leakTopic = "/leak";
	public string depthTopic = "/depth";
	public string imuTopic = "/mavros/imu/data";
	public Rigidbody rb;
	
	private float yaw;
	private float pitch;
	private float roll;
	private float depth;
	
	private bool blueROVConnected = false;
	
    // Start is called before the first frame update
    void Start()
    {
    	// Get the BlueROV
    	this.rb = GetComponent<Rigidbody>();
    	
    	// Create Subscribers
    	ROSConnection.GetOrCreateInstance().Subscribe<RosMessageTypes.Std.BoolMsg>(leakTopic, leakCallback);
    	ROSConnection.GetOrCreateInstance().Subscribe<RosMessageTypes.Sensor.ImuMsg>(imuTopic, imuCallback);
    	ROSConnection.GetOrCreateInstance().Subscribe<RosMessageTypes.Std.Float64Msg>(depthTopic, depthCallback);
    }

	void leakCallback(RosMessageTypes.Std.BoolMsg leakMsg)
	{
		if(leakMsg.data == true){
			Debug.Log("PANICCCCCC");
		}
	}
	
	
	void depthCallback(RosMessageTypes.Std.Float64Msg depthMsg)
	{
		this.depth = (float)depthMsg.data;
	}
	
	void imuCallback(RosMessageTypes.Sensor.ImuMsg imuMsg)
	{
		this.blueROVConnected = true;
		float x = (float)imuMsg.orientation.x;
		float y = (float)imuMsg.orientation.y;
		float z = (float)imuMsg.orientation.z;
		float w = (float)imuMsg.orientation.w;
		
        float sinr_cosp = 2 * (w * x + y * z);
        float cosr_cosp = 1 - 2 * (x * x + y * y);
        this.roll = Mathf.Atan2(sinr_cosp, cosr_cosp)*Mathf.Rad2Deg;
        
        float sinp = 2 * (w * y - z * x);
        this.pitch = Mathf.Asin(sinp)*Mathf.Rad2Deg;

        float siny_cosp = 2 * (w * z + x * y);
        float cosy_cosp = 1 - 2 * (y * y + z * z);
        this.yaw = Mathf.Atan2(siny_cosp, cosy_cosp)*Mathf.Rad2Deg;
        //Debug.Log("Yaw: " + yaw + " Pitch: " + pitch + " Roll: " + roll);
        
        //Vector3 eulerAngles	= new Vector3(-roll, yaw, -pitch);
        //Quaternion deltaRotation = Quaternion.Euler(eulerAngles);
        //this.rb.rotation = deltaRotation;
        //transform.Rotate(-roll, yaw, -pitch);
	}
	
    // Update is called once per frame
    void Update()
    {
    	if(blueROVConnected){
		    Vector3 eulerAngles	= new Vector3(180 + -this.roll, 180 + this.yaw, -this.pitch);
		    Quaternion deltaRotation = Quaternion.Euler(eulerAngles);
		    this.rb.rotation = deltaRotation;
		    
		    Vector3 position = new Vector3(10, 3 - this.depth, 6);
		    this.rb.position = position;
        }
        
    }
}
