using UnityEngine;
using UnityEngine.UI;

public class SingleDice : MonoBehaviour
{
    public DiceManager diceManager;
    [SerializeField] private Slider powerSlider;
    [SerializeField] private Color lowPowerColor = Color.green;
    [SerializeField] private Color highPowerColor = Color.red;
    
    // Zarządzanie mocą rzutu kości
    public float powerIncreaseRate = 1000f;
    public float minPower = 1000f;
    public float maxPower = 10000f;

    public float currentPower;
    private bool cyclicPower = true;
    private bool wasTossed = false;
    private bool isRolling = false;

    private Vector3 diceTop;
    public Camera playerCamera;

    private Rigidbody rb;

    public int RolledValue { get; private set; }

    [SerializeField] private int sideUpValue;
    [SerializeField] private int sideDownValue;
    [SerializeField] private int sideLeftValue;
    [SerializeField] private int sideRightValue;
    [SerializeField] private int sideFrontValue;
    [SerializeField] private int sideBackValue;

    [SerializeField] private Transform sideUp;
    [SerializeField] private Transform sideDown;
    [SerializeField] private Transform sideLeft;
    [SerializeField] private Transform sideRight;
    [SerializeField] private Transform sideFront;
    [SerializeField] private Transform sideBack;

    private Camera currentCamera;

    void Start()
    {
        currentCamera = Camera.main;
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = true;
        currentPower = minPower;
    }

    private void FixedUpdate()
    {
        if (isRolling && rb.IsSleeping())
        {
            GetTopSide();
            ReportResult();
            isRolling = false;
            wasTossed = false; // Resetowanie po rzucie
        }
    }

    private void ReportResult()
    {
        if (diceManager != null)
        {
            diceManager.RecordDiceResult(this, RolledValue);
        }
    }

    public void ChargePower()
    {
        if (cyclicPower)
        {
            currentPower += powerIncreaseRate * Time.deltaTime;
            if (currentPower > maxPower)
            {
                currentPower = maxPower;
            }
        }

        UpdateSliderColor();

        Debug.Log($"Current Power: {currentPower}");
    }

    public void TossDice()
    {
        Ray ray = currentCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        Vector3 throwDirection;

        if (Physics.Raycast(ray, out hit))
        {
            throwDirection = (hit.point - transform.position).normalized;
        }
        else
        {
            throwDirection = playerCamera.transform.forward;
        }

        rb.isKinematic = false;
        rb.AddForce(throwDirection * (currentPower * Time.deltaTime), ForceMode.Impulse);
        AddRandomRotation();

        wasTossed = true;
        isRolling = true;
        currentPower = minPower;
    }

    
    
    public void AddRandomRotation()
    {
        float randomX = Random.Range(-10f, 10f);
        float randomY = Random.Range(-10f, 10f);
        float randomZ = Random.Range(-10f, 10f);

        rb.AddTorque(new Vector3(randomX, randomY, randomZ), ForceMode.Impulse);
    }

    private void GetTopSide()
    {
        var up = Vector3.up;

        if (Vector3.Dot(sideUp.up, up) >= 0.1f) RolledValue = sideUpValue;
        if (Vector3.Dot(sideDown.up, up) >= 0.1f) RolledValue = sideDownValue;
        if (Vector3.Dot(sideLeft.up, up) >= 0.1f) RolledValue = sideLeftValue;
        if (Vector3.Dot(sideRight.up, up) >= 0.1f) RolledValue = sideRightValue;
        if (Vector3.Dot(sideFront.up, up) >= 0.1f) RolledValue = sideFrontValue;
        if (Vector3.Dot(sideBack.up, up) >= 0.1f) RolledValue = sideBackValue;

        
    }
    
    private void UpdateSliderColor()
    {
        if (powerSlider != null)
        {
            // Normalizujemy moc (0 to minPower, 1 to maxPower)
            float normalizedPower = (currentPower - minPower) / (maxPower - minPower);

            // Interpolacja koloru między lowPowerColor a highPowerColor
            Color currentColor = Color.Lerp(lowPowerColor, highPowerColor, normalizedPower);

            // Zmieniamy kolor wypełnienia suwaka
            powerSlider.fillRect.GetComponent<Image>().color = currentColor;
        }
    }
}
