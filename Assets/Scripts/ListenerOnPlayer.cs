using UnityEngine;

// my camera is around 15m away from the player, so if the AudioListener was on
// the camera every 3D sound would sound far away. so I took it off the camera
// and put it on this empty object which just sits on the player.
// it turns the same way as the camera so left/right on screen match the speakers
public class ListenerOnPlayer : MonoBehaviour
{
    public Transform player;
    public Transform cameraTransform;

    void LateUpdate()
    {
        if (player == null) return;

        transform.position = player.position;
        if (cameraTransform != null)
            transform.rotation = Quaternion.Euler(0f, cameraTransform.eulerAngles.y, 0f);
    }
}
