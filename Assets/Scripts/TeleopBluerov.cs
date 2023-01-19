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
	
	void stop_robot(){
    	this.velocity_msg.linear.x = 0.0;
    	this.velocity_msg.linear.y = 0.0;
    	this.velocity_msg.linear.z = 0.0;
    	this.velocity_msg.angular.x = 0.0;
    	this.velocity_msg.angular.y = 0.0;
    	this.velocity_msg.angular.z = 0.0;
	}
    
    // Update is called once per frame
    void Update()
    {
            
        if(Input.GetKeyDown(KeyCode.W) || Input.GetKey(KeyCode.W))
        {
        	this.velocity_msg.linear.x = (float)0.1;
        }
        else if(Input.GetKeyDown(KeyCode.S) || Input.GetKey(KeyCode.S))
        {
        	this.velocity_msg.linear.x = -(float)0.1;
        }
        else if(Input.GetKeyDown(KeyCode.A) || Input.GetKey(KeyCode.A))
        {
        	this.velocity_msg.linear.y = (float)0.1;
        }
        else if(Input.GetKeyDown(KeyCode.D) || Input.GetKey(KeyCode.D))
        {
        	this.velocity_msg.linear.y = -(float)0.1;
        }
        else if(Input.GetKeyDown(KeyCode.U) || Input.GetKey(KeyCode.U))
        {
        	this.velocity_msg.linear.z = (float)0.1;
        }
        else if(Input.GetKeyDown(KeyCode.J) || Input.GetKey(KeyCode.J))
        {
        	this.velocity_msg.linear.z = -(float)0.1;
        }
        else if(Input.GetKeyDown(KeyCode.H) || Input.GetKey(KeyCode.H))
        {
        	this.velocity_msg.angular.z = (float)1;
        }
        else if(Input.GetKeyDown(KeyCode.K) || Input.GetKey(KeyCode.K))
        {
        	this.velocity_msg.angular.z = -(float)1;
        }
        else {
        	stop_robot();
        }
        StartCoroutine(publish());
    }
}
