using UnityEngine;

public class DiceThrower : MonoBehaviour
{
  /*  Rigidbody rb;

    [SerializeField] private float spinforce = 100f;
    [SerializeField] private float force = 100f;

    private float powerIncreaseRate = 1000f;
    private float minPower = 1000f;
    float maxPower = 10000f;

    private float currentPower;
    public Camera playerCamera;

    bool cyclicPower = true;
    private bool isChargingPower = false;
    private bool wasTossed = false;
    bool isRolling = false;

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

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Space) && !wasTossed)
        {
            isChargingPower = true;
            ChargePower();
        }

        if (Input.GetKeyUp(KeyCode.Space) && isChargingPower && !wasTossed)
        {
            isChargingPower = false;
            TossDice();
        }
    }

    void FixedUpdate()
    {
        if (!wasTossed) return;
        if (rb.IsSleeping())
        {
            GetTopSide();
        }
    }

    private void GetTopSide()
    {
        var up = Vector3.up;
        if (Vector3.Dot(sideUp.rotation.eulerAngles, up) <= 0.1f) RolledValue = sideUpValue;
        if (Vector3.Dot(sideDown.rotation.eulerAngles, up) <= 0.1f) RolledValue = sideDownValue;
        if (Vector3.Dot(sideLeft.rotation.eulerAngles, up) <= 0.1f) RolledValue = sideBackValue;
        if (Vector3.Dot(sideRight.rotation.eulerAngles, up) <= 0.1f) RolledValue = sideFrontValue;
        if (Vector3.Dot(sideFront.rotation.eulerAngles, up) <= 0.1f) RolledValue = sideLeftValue;
        if (Vector3.Dot(sideBack.rotation.eulerAngles, up) <= 0.1f) RolledValue = sideRightValue;
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

        currentPower = minPower;
    }

    public void AddRandomRotation()
    {
        float randomX = Random.Range(-10f, 10f);
        float randomY = Random.Range(-10f, 10f);
        float randomZ = Random.Range(-10f, 10f);

        rb.AddTorque(new Vector3(randomX, randomY, randomZ), ForceMode.Impulse);
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


        // Wyświetlanie mocy (do debugowania)
        Debug.Log($"Current Power: {currentPower}");
    }
    */
}
