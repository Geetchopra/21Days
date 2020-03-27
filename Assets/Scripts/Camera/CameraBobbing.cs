using UnityEngine;

/// <summary>
/// Simulates human like camera movement.
/// </summary>
public class CameraBobbing : MonoBehaviour
{
    //Speed at which the camera bobs.
    private float speedX;
    private float speedY;

    //How much should the camera bob.
    private float bobbingAmountX;
    private float bobbingAmountY;

    public PlayerController controller;

    private float originalY;
    private float originalX;

    private float timerX = 0;
    private float timerY = 0;

    /// <summary>
    /// Called before the first frame update.
    /// </summary>
    void Start()
    {
        originalY = transform.localPosition.y;
        originalX = transform.localPosition.x;
    }

    /// <summary>
    /// Called once every frame.
    /// </summary>
    void Update()
    {
        float input = Input.GetAxis("Vertical");

        if (controller.state == PlayerController.MovementState.idle)
        {
            bobbingAmountY = 0.005f;
            bobbingAmountX = 0.008f;
            speedX = 0.1f;
            speedY = 0.2f;
        }
        else if (controller.state == PlayerController.MovementState.running)
        {
            bobbingAmountY = 0.05f;
            bobbingAmountX = 0.03f;
            speedX = 9.0f;
            speedY = 14.0f;
        }
        else 
        {   
            //Walking straight or backward
            if (input > 0.1f || input < -0.1f)
            {
                bobbingAmountY = 0.03f;
                bobbingAmountX = 0.02f;
                speedX = 7.0f;
                speedY = 11.0f;
            }
            //Walking fully left or right
            else
            {
                bobbingAmountY = 0.01f;
                bobbingAmountX = 0.01f;
                speedX = 2.0f;
                speedY = 3.0f;
            }
        }
        
        timerX += Time.deltaTime * speedX;
        timerY += Time.deltaTime * speedY;

        float newX = originalX + Mathf.Sin(timerX) * bobbingAmountX;
        float newY = originalY + Mathf.Sin(timerY) * bobbingAmountY;

        transform.localPosition = new Vector3(newX, newY, transform.localPosition.z);
    }
}