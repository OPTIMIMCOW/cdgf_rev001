using Unity.VisualScripting;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float movementSpeed = 0.001f;
    [SerializeField] Thruster[] thruster;

    [SerializeField] private float sensitivity = 0.005f;
    [SerializeField] private float boundLimit = 300f;
    [SerializeField] private float step = 10f;

    Transform myTransform;
    private Vector2 xyRotationVector;
    private float zRotationVector;

    private Vector3 translationWorldVector; 

    private void Awake()
    {
        myTransform = transform;
    }

    void Update()
    {
        Thrust();
        Rotations();
        NullifyVectors();
    }

    void Rotations()
    {
        UpdateAbsoluteXYRotationalVelocity();
        UpdateAbsoluteZRoationalVelocity();

        //myTransform.Translate(new Vector3(0.001f,0f,0f), Space.World);

        myTransform.Rotate(new Vector3(xyRotationVector.y *-1f, xyRotationVector.x, zRotationVector) * Time.deltaTime);
    }
    void Thrust()
    {
        // calculate what a movement forward vector is in the world space and translate using that. 
        // just compound the position.forward vectors and update position on that or translate using that. 
        // Activate and deactivate trail rendereer. 
        UpdateTranslationVector();
        myTransform.Translate(translationWorldVector * Time.deltaTime, Space.World);

            //myTransform.position += thrustTranslationVector * Time.deltaTime;
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
    {
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

    private void UpdateAbsoluteZRoationalVelocity()
    {
        var input = Input.mouseScrollDelta.y;

        if (Mathf.Approximately(0, input) ) return;

        zRotationVector += input * step;
    }

    private void UpdateTranslationVector()
    {
        if (Input.GetAxis("Boost") > 0)
        {
            translationWorldVector += myTransform.forward * movementSpeed*5 * Input.GetAxis("Boost");
        }

        if (Input.GetAxis("Vertical") != 0)
        {
            translationWorldVector += myTransform.up * movementSpeed * Input.GetAxis("Vertical");
        }

        if (Input.GetAxis("Horizontal") != 0)
        {
            translationWorldVector += myTransform.right * movementSpeed * Input.GetAxis("Horizontal");
        }
    }

    private void NullifyVectors()
    {
        Debug.Log("called");
        if (Input.GetAxis("Reset") > 0)
        {
            Debug.Log("Hit");
            xyRotationVector = new Vector2 (0, 0);
        zRotationVector = 0f;
        translationWorldVector = new Vector3 (0, 0, 0);
        }
    }
}
