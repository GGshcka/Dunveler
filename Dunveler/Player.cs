using System.Diagnostics;
using System.Numerics;
using Raylib_cs;
using static Raylib_cs.Raylib;

namespace Dunveler;

public class Player
{
    private static bool cur = false;

    public static Camera3D camera = new Camera3D();

    public static void PlayerCameraStart()
    {
        camera.Position = new Vector3(0.0f, 0.0f, 0.0f); // Camera position
        camera.Target = new Vector3(10.0f, 8.0f, 0.0f);      // Camera looking at point
        camera.Up = new Vector3(0.0f, 1.0f, 0.0f);          // Camera up vector (rotation towards target)
        camera.FovY = 45.0f;                                // Camera field-of-view Y
        camera.Projection = CameraProjection.Perspective;

        CameraYaw(ref camera, -135 * DEG2RAD, true);
        CameraPitch(ref camera, -45 * DEG2RAD, true, true, false);
    }

    public static void PlayerCamera()
    {
        UpdateCamera(ref camera, CameraMode.FirstPerson);
    }

    public static void Controls()
    {
        if (IsKeyPressed(KeyboardKey.C))
        {
            if (cur == false)
            {
                EnableCursor();
                cur = true;
            }
            else
            {
                DisableCursor();
                cur = false;
            }
        }
    }
}