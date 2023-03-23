using Godot;
using System;

public partial class Camera3D : Godot.Camera3D
{
    private Vector3 cameraOrigin = new Vector3(0, 0, 0);
    private Vector2 lastMousePos = new Vector2();
    private Node3D target = null;
    private bool enableRotation = false;
    private bool enablePan = false;
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        lastMousePos = GetViewport().GetMousePosition();
        target = GetParent().GetNode<Node3D>("Helmet");
        LookAt(target.Position, Vector3.Up);
        cameraOrigin = Position;
    }

    public override void _Input(InputEvent @event)
    {
        if (@event is InputEventMouseButton)
        {
            InputEventMouseButton mouseEvent = (InputEventMouseButton)@event;
            if (mouseEvent.ButtonIndex == MouseButton.Left)
            {
                if (mouseEvent.Pressed)
                {
                    enableRotation = true;
                    lastMousePos = GetViewport().GetMousePosition();
                }
                else
                {
                    enableRotation = false;
                }
            }

            if (mouseEvent.ButtonIndex == MouseButton.WheelUp)
            {
                if (mouseEvent.Pressed)
                {
                    TranslateObjectLocal(Vector3.Forward * 0.1f);
                }
            }

            if (mouseEvent.ButtonIndex == MouseButton.WheelDown)
            {
                if (mouseEvent.Pressed)
                {
                    TranslateObjectLocal(Vector3.Forward * -0.1f);
                }
            }


            if (mouseEvent.ButtonIndex == MouseButton.Right)
            {
                if (mouseEvent.Pressed)
                {
                    enablePan = true;
                    lastMousePos = GetViewport().GetMousePosition();
                }
                else
                {
                    enablePan = false;
                }
            }

            if (mouseEvent.ButtonIndex == MouseButton.Middle)
            {
                if (mouseEvent.Pressed)
                {
                    Position = cameraOrigin;
                    LookAt(target.Position, Vector3.Up);
                }
            }
        }
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
        if (enableRotation)
        {
            Vector2 mousePos = GetViewport().GetMousePosition();
            Vector2 mouseDelta = mousePos - lastMousePos;
            Vector3 point = target.Position;
            rotateAround(point, Vector3.Up, -mouseDelta[0] * (float)delta * 0.01f);
            rotateAround(point, Basis.X.Normalized(), -mouseDelta[1] * (float)delta * 0.01f);
            // LookAt(point, Basis.Y.Normalized());
        }

        if (enablePan)
        {
            Vector2 mousePos = GetViewport().GetMousePosition();
            Vector2 mouseDelta = mousePos - lastMousePos;
            Vector3 point = GetParent().GetNode<Node3D>("Helmet").Position;
            TranslateObjectLocal(new Vector3(-mouseDelta[0] * (float)delta * 0.01f, -mouseDelta[1] * (float)delta * 0.01f, 0));
            // LookAt(point, Basis.Y.Normalized());
        }
    }

    private void rotateAround(Vector3 point, Vector3 axis, float angle)
    {
        Quaternion q = new Quaternion(axis, angle);
        Vector3 diff = Position - point;
        diff = q * diff;
        Position = point + diff;
        //Transform = Transform.Rotated(axis, angle);
        Rotate(axis, angle);

    }


}
