using UnityEngine;

public class InputManager : MonoBehaviour
{
    private ActionsMap actions;

    private void OnEnable()
    {
        actions = new ActionsMap();
        actions.Player.Enable();
    }

    private void OnDisable()
    {
        actions.Player.Disable();
    }

    public Vector2 Move 
    {
        get { return SetMove(); }
    }

    public Vector2 Look
    {
        get { return SetLook(); }
    }

    public bool Jump
    {
        get { return SetJump(); }
    }

    public bool Run
    {
        get { return SetRun(); }
    }

    public bool Shoot
    {
        get { return SetShoot(); }
    }

    private bool SetJump()
    {
        return actions.Player.Jump.triggered;
    }

    private bool SetRun()
    {
        return actions.Player.Run.IsPressed();
    }

    private bool SetShoot()
    {
        return actions.Player.Shoot.IsPressed();
    }
     
    private Vector2 SetMove()
    {
        return actions.Player.Move.ReadValue<Vector2>();
    }

    private Vector2 SetLook()
    {
        return actions.Player.Look.ReadValue<Vector2>();
    }
}
