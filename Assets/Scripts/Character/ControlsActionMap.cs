using UnityEngine;
using UnityEngine.InputNew;

// GENERATED FILE - DO NOT EDIT MANUALLY
public class ControlsActionMap : ActionMapInput {
	public ControlsActionMap (ActionMap actionMap) : base (actionMap) { }
	
	public ButtonInputControl @jumpDash { get { return (ButtonInputControl)this[0]; } }
	public ButtonInputControl @tossPunch { get { return (ButtonInputControl)this[1]; } }
	public AxisInputControl @move { get { return (AxisInputControl)this[2]; } }
	public AxisInputControl @upDown { get { return (AxisInputControl)this[3]; } }
	public Vector2InputControl @aimDirection { get { return (Vector2InputControl)this[4]; } }
}
