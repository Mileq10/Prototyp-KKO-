using DataModels;
using UnityEngine;
using UnityEngine.UI;

public class SingleDice : MonoBehaviour
{
    const float MinDifference = 1.0f - 0.1f;
    public DiceManager diceManager;
    [SerializeField] private Slider powerSlider;
    [SerializeField] private Color lowPowerColor = Color.green;
    [SerializeField] private Color highPowerColor = Color.red;

    // Zarządzanie mocą rzutu kości
    public float powerIncreaseRate = 1000f;
    public float minPower = 1000f;
    public float maxPower = 10000f;

    public float currentPower;
    private bool _cyclicPower = true;
    private bool _wasTossed = false;
    private bool _isRolling = false;

    private Vector3 _diceTop;
    /*
    public Camera playerCamera;
    public Camera boardView;
    */
    private Rigidbody _rb;

    public EDiceSide RolledValue { get; private set; }
    public bool HasResult => _hasResult;

    [SerializeField] private EDiceSide sideUpValue;
    [SerializeField] private EDiceSide sideDownValue;
    [SerializeField] private EDiceSide sideLeftValue;
    [SerializeField] private EDiceSide sideRightValue;
    [SerializeField] private EDiceSide sideFrontValue;
    [SerializeField] private EDiceSide sideBackValue;

    [SerializeField] private Transform sideUp;
    [SerializeField] private Transform sideDown;
    [SerializeField] private Transform sideLeft;
    [SerializeField] private Transform sideRight;
    [SerializeField] private Transform sideFront;
    [SerializeField] private Transform sideBack;

 
    private bool _hasResult = false;


    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _rb.isKinematic = true;
        currentPower = minPower;
    }

    private void FixedUpdate()

    {
        if (_isRolling && _rb.IsSleeping())
        {
            GetTopSide();
            _hasResult = true;
            _isRolling = false;
            _wasTossed = false; // Resetowanie po rzucie
        }
    }




    public void Reset()
    {
        _isRolling = false;
        currentPower = minPower;
        _wasTossed = false;
        _hasResult = false;
        _rb.isKinematic = true;
    }
    public void ChargePower()
    {
        if (_cyclicPower)
        {
            currentPower += powerIncreaseRate * Time.deltaTime;
            if (currentPower > maxPower)
            {
                currentPower = maxPower;
            }
        }

        UpdateSliderColor();

        //Debug.Log($"Current Power: {currentPower}");
    }

    public void TossDice(Vector3 forceValue)
    {

        forceValue = forceValue * Time.deltaTime * currentPower;
        _rb.isKinematic = false;
        _rb.AddForce(forceValue, ForceMode.Impulse);
        AddRandomRotation();

        _wasTossed = true;
        _isRolling = true;
        currentPower = minPower;
    }


    public void AddRandomRotation()
    {
        float randomX = Random.Range(-10f, 10f);
        float randomY = Random.Range(-10f, 10f);
        float randomZ = Random.Range(-10f, 10f);

        _rb.AddTorque(new Vector3(randomX, randomY, randomZ), ForceMode.Impulse);
    }

    private void GetTopSide()
    {
        var inverse = Quaternion.Inverse(transform.rotation);
        var up = inverse * Vector3.up;

        if (CompareAngle(up,inverse* sideUp.forward, sideUpValue)) return;
        if (CompareAngle(up,inverse* sideDown.forward, sideDownValue)) return;
        if (CompareAngle(up,inverse* sideLeft.forward, sideLeftValue)) return;
        if (CompareAngle(up,inverse* sideRight.forward, sideRightValue)) return;
        if (CompareAngle(up,inverse* sideFront.forward, sideFrontValue)) return;
        if (CompareAngle(up,inverse* sideBack.forward, sideBackValue)) return;
        Debug.LogError($"{name} no value was detected {up}");
        
    }

    [ContextMenu("Debug values")]
    private void DebugValues()
    {
        Debug.Log(sideUp.eulerAngles);
        Debug.Log(sideDown.eulerAngles);
        Debug.Log(sideLeft.eulerAngles);
        Debug.Log(sideRight.eulerAngles);
        Debug.Log(sideFront.eulerAngles);
        Debug.Log(sideBack.eulerAngles);
    }

    /// <summary>
    /// Compares angle between 2 vectors and assigns result to <see cref="RolledValue"/>
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <param name="result"></param>
    /// <returns>True if dot product was close to 1(Greater or equal to <see cref="MinDifference"/>>) </returns>
    private bool CompareAngle(Vector3 a, Vector3 b, EDiceSide result)
    {
        //Debug.Log($"{a} {b} {result} ");
        var aNorm = a.normalized;
        //b = new Vector3(aNorm.x * b.x, aNorm.y * b.y, aNorm.z * b.z);
        if (!(Vector3.Dot(a, b) >= MinDifference)) return false;
        RolledValue = result;
        //Debug.Log("Sukces");
        return true;
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

    public DiceData GetResult()
    {
        if(!_hasResult) return null;
        var result = new DiceData(RolledValue);
        return result;
    }
}