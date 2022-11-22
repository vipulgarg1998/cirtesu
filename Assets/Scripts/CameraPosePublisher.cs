using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Robotics.ROSTCPConnector;

public class CameraPosePublisher : MonoBehaviour
{
    // Start is called before the first frame update
    
	public string topic = "camera/pose";
    // Publish the cube's position and rotation every N seconds
    public float publishMessageFrequency = 1.0f;
    // Used to determine how much time has elapsed since the last message was published
    private float timeElapsed;
    
    private ROSConnection ros;
	
    void Start()
    {
        this.ros = ROSConnection.GetOrCreateInstance();
        this.ros.RegisterPublisher<RosMessageTypes.Geometry.PoseMsg>(topic);
    }

    IEnumerator publish()
	{
        timeElapsed += Time.fixedDeltaTime;
        if (timeElapsed > publishMessageFrequency)
        {
            float x = transform.position[0];
            float y = transform.position[1];
            float z = transform.position[2];
            float p = transform.rotation.x;
            float q = transform.rotation.y;
            float r = transform.rotation.z;
            float w = transform.rotation.w;
            
            RosMessageTypes.Geometry.PointMsg position = new RosMessageTypes.Geometry.PointMsg(x, y, z);
            RosMessageTypes.Geometry.QuaternionMsg quaternion = new RosMessageTypes.Geometry.QuaternionMsg(p, q, r, w);
            RosMessageTypes.Geometry.PoseMsg pose = new RosMessageTypes.Geometry.PoseMsg(position, quaternion);
            //Debug.Log("Printing Pos");
            //Debug.Log(pose);
			this.ros.Publish(topic, pose);
			
            timeElapsed = 0;
            yield return null;
		}
	}


    // Update is called once per frame
    void FixedUpdate()
    {
        StartCoroutine(publish());
    }
}
