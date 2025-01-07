using UnityEngine;

public class DiceThrower : MonoBehaviour
{
    [SerializeField] private float spinforce = 100f;
    [SerializeField] private float force = 100f;
    Rigidbody rb;

    private bool wasTossed=false;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !wasTossed)
        {
            TossDice();
        }
    }
    
    
    void TossDice()
    {
        rb.isKinematic = false;
        rb.AddForce(Vector3.forward * (force * Time.deltaTime), ForceMode.Impulse);
        AddRandomRotation();
        wasTossed = true;
    }

    void AddRandomRotation()
    {
        float randomX = Random.Range(-10f, 10f);
        float randomY = Random.Range(-10f, 10f);
        float randomZ = Random.Range(-10f, 10f);
        
        rb.AddTorque(new Vector3(randomX,randomY, randomZ), ForceMode.Impulse);
    }

}

