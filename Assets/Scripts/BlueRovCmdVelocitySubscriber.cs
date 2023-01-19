using UnityEngine;
using Unity.Robotics.ROSTCPConnector;

public class BlueRovCmdVelocitySubscriber : MonoBehaviour
{
	public string topic = "cmd_vel";
	public BlueRovVelocityControl blueRovVelocityControl;
	
    // Start is called before the first frame update
    void Start()
    {
    	this.blueRovVelocityControl = GetComponent<BlueRovVelocityControl>();
    	ROSConnection.GetOrCreateInstance().Subscribe<RosMessageTypes.Geometry.TwistMsg>(topic, VelocityChange);
    }

	void VelocityChange(RosMessageTypes.Geometry.TwistMsg velocityMsg)
	{
		//Debug.Log("" + velocityMsg);
		this.blueRovVelocityControl.moveVelocity(velocityMsg);
	}
    // Update is called once per frame
    void Update()
    {
        
    }
}
