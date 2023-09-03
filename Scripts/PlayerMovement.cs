using UnityEngine;
using static UnityEditor.PlayerSettings;
using UnityEngine.UIElements;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float movementSpeed = 2f;
    [SerializeField] float turnSpeed = 5f;
    [SerializeField] Thruster[] thruster;

    [SerializeField] private float sensitivity = 0.005f;
    [SerializeField] private float boundLimit = 300f;
    [SerializeField] private float step = 10f;

    Transform myTransform;
    private Vector2 xyRotationVector;

    private void Awake()
    {
        myTransform = transform;
    }

    void Update()
    {
        Thrust();
        Rotations();
    }

    void Rotations()
    {
        UpdateAbsoluteXYRotationalVelocity();

        myTransform.Rotate(new Vector3(xyRotationVector.y *-1f, xyRotationVector.x, 0) * Time.deltaTime);
    }
    void Thrust()
    {
        // Activate and deactivate trail rendereer. 
        if (Input.GetAxis("Vertical") > 0)
        {
            myTransform.position += myTransform.forward * movementSpeed * Time.deltaTime * Input.GetAxis("Vertical");
        }
    }

    public float BindAndDescretize(float value)
    {
        var boundValue = value < 0
            ? value <  boundLimit *-1f
                ? boundLimit * -1f
                : value
            : value > boundLimit
                ? boundLimit
                : value;

        var result = (boundValue / step);

        return result * step;
    }

    private float SquareWithDirection(float input)
    {
        var square = input * input;
        if(input < 0)
        {
            square = square * -1f;
        }

        return square;
    }

    private Vector2 ApplySensitivitySetting(Vector2 input)
    {
        var quadratic = new Vector2(SquareWithDirection(input.x), SquareWithDirection(input.y));
        var scaled = quadratic * sensitivity;
        var descrete = new Vector2 (BindAndDescretize(scaled.x), BindAndDescretize(scaled.y));
        return descrete;
    }


    private void UpdateAbsoluteXYRotationalVelocity()
    {// TODO extend to include roll rotation

        // rotate based on where position of mouse is on the screen. Do not do it additive, abosolute based on the mouse poisition. We can consider doing the addative approach later. 

        float screenX = Screen.width;
        float screenY = Screen.height;
        float mouseX = Input.mousePosition.x;
        float mouseY = Input.mousePosition.y;


        if (mouseX < 0 || mouseX > screenX || mouseY < 0 || mouseY > screenY) return;

        var mouseLocation = new Vector2(mouseX, mouseY);
        var screenCenter = new Vector2(screenX/2, screenY/2);
        var movementVector = mouseLocation- screenCenter;
        var finalMovmentVector = ApplySensitivitySetting(movementVector);
        xyRotationVector = finalMovmentVector;
    }

        magnifiedMovementVector = finalMovmentVector;


    }
}
