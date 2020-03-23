using UnityEngine;

public class CameraBobbing : MonoBehaviour
{
    public float speedX;
    public float speedY;

    public float bobbingAmountX;
    public float bobbingAmountY;

    public PlayerController controller;

    private float originalY = 0;
    private float originalX = 0;

    private float timerX = 0;
    private float timerY = 0;

    // Start is called before the first frame update
    void Start()
    {
        originalY = transform.localPosition.y;
        originalX = transform.localPosition.x;
    }

    // Update is called once per frame
    void Update()
    {
        if (controller.state == PlayerController.MovementState.running)
        {
            bobbingAmountY = 0.05f;
            bobbingAmountX = 0.03f;
            speedX = 9.0f;
            speedY = 14.0f;
        }
        else if (controller.state == PlayerController.MovementState.walking)
        {
            bobbingAmountY = 0.03f;
            bobbingAmountX = 0.02f;
            speedX = 7.0f;
            speedY = 11.0f;
        }
        else
        {
            bobbingAmountY = 0.02f;
            bobbingAmountX = 0.01f;
            speedX = 0.2f;
            speedY = 0.7f;
        }

        timerX += Time.deltaTime * speedX;
        timerY += Time.deltaTime * speedY;

        transform.localPosition = new Vector3(originalX + Mathf.Sin(timerX) * bobbingAmountX, originalY + Mathf.Sin(timerY) * bobbingAmountY, transform.localPosition.z);
    }
}