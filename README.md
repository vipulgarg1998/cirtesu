# cirtesu

A Unity based ROS project developed to simulate the BLUE ROV in the swimming pool. The robot can be moved using the ROS1 or ROS2 "teleop_twist_keyboard" package 
by running the following command.
```
ros2 run teleop_twist_keyboard teleop_twist_keyboard
```

The robot can also be teleoperated using the keyboard as well. Assuming the coordinate frame attached to BueROV follows the standard notation where x axis
is from stern to bow, y axis is from port to starboard, and z axis is from top to botton, then, 
- w for surge in positve direction
- s for surge in negative direction
- d for sway in positive direction
- a for sway in negative direction
- u for heave in negative direction
- j for heave in positive direction
- k for yaw in positive direction
- h for yaw in negative direction

## Orientation Mimicin
If the simulator is linked to the actual robot, then the simulated robot will attain the same orientation as of the robot.
