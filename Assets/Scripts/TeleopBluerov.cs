using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Robotics.ROSTCPConnector;

public class TeleopBluerob : MonoBehaviour
{
    private ROSConnection ros;
	public string topic = "cmd_vel";
    
    // Publish the cube's position and rotation every N seconds
    public float publishMessageFrequency = 0.1f;

    // Used to determine how much time has elapsed since the last message was published
    private float timeElapsed;
    private RosMessageTypes.Geometry.TwistMsg velocity_msg = new RosMessageTypes.Geometry.TwistMsg();
	
    // Start is called before the first frame update
    void Start()
    {
        this.ros = ROSConnection.GetOrCreateInstance();
        this.ros.RegisterPublisher<RosMessageTypes.Geometry.TwistMsg>(this.topic);
    }
    
    IEnumerator publish()
	{
        this.timeElapsed += Time.deltaTime;

        if (timeElapsed > publishMessageFrequency)
        {
		    // Finally send the message to server_endpoint.py running in ROS
		    this.ros.Publish(this.topic, this.velocity_msg);
            this.timeElapsed = 0;
            yield return null;
        }
	}
    
    // Update is called once per frame
    void Update()
    {
            
        if(Input.GetKeyDown(KeyCode.W) || Input.GetKey(KeyCode.W))
        {
        	this.velocity_msg.linear.x = (float)0.1;

            Debug.Log("Publishing Linear Velocity");
        }
        if (Input.GetKeyUp(KeyCode.W))
        {
        	this.velocity_msg.linear.x = 0.0;
        }
        StartCoroutine(publish());
    }
}
