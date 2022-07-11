using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovementController : MonoBehaviour
{
    Rigidbody mRigidbody;

    [SerializeField]
    float mSpeed = 5;

    [SerializeField]
    LayerMask mObstacleLayerMask;

    bool mCarryingFruit;

    GameObject mCarriedFruit;

    Transform mFruitHolder;

    GameObject mCurrentPlatform;

    List<GameObject> mWalkedOnPlatforms;

    public delegate void OnPointScored_Delegate();
    public static event OnPointScored_Delegate OnPointScored;

    bool mKnockedOut;

    [SerializeField]
    Transform mInitalPosition;

    bool mCanBeKnockedOut;


    void Start()
    {
        mWalkedOnPlatforms = new List<GameObject>();
        mRigidbody = GetComponent<Rigidbody>();
        mCarryingFruit = false;
        mKnockedOut = false;
        mFruitHolder = transform.Find("FruitHolder");
        mCanBeKnockedOut = false;
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        Vector3 walkDir = new Vector3(-Input.GetAxis("Vertical"), 0, Input.GetAxis("Horizontal"));

        walkDir = Vector3.ClampMagnitude(walkDir, 1);

        RaycastHit hit;

        if (!Physics.Raycast(transform.position, walkDir, out hit, 1.5f, mObstacleLayerMask) && !mKnockedOut)
        {
            mRigidbody.MovePosition( transform.position + walkDir * Time.fixedDeltaTime * mSpeed);

            
        }

        //transform.forward = walkDir; // Quaternion.Lerp(transform.rotation, Quaternion.FromToRotation(transform.forward, walkDir),0.2f);

        if (mCarryingFruit)
        mCarriedFruit.transform.position = mFruitHolder.position;
    }

    private void Update()
    {
        Debug.Log(Mathf.Abs((mInitalPosition.position - transform.position).magnitude));

        Vector3 walkDir = new Vector3(-Input.GetAxis("Vertical"), 0, Input.GetAxis("Horizontal"));

        if(walkDir.magnitude != 0)
        {
            Quaternion toRotation = Quaternion.LookRotation(walkDir, Vector3.up);

            transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, 360 * Time.deltaTime);
        }

        if(mKnockedOut)
        {
            transform.position = Vector3.Slerp(transform.position, mInitalPosition.position, 0.5f/ Mathf.Abs((mInitalPosition.position - transform.position).magnitude) );

            if (Mathf.Abs((mInitalPosition.position - transform.position).magnitude) < 0.01f)
            {
                mKnockedOut = false;
                GetComponent<Collider>().enabled = true;
                transform.position = new Vector3(transform.position.x, 1.5f, transform.position.z);
            }
        }
         
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (mCarryingFruit) return;

        mCarriedFruit = collision.gameObject;
        collision.gameObject.GetComponent<Rigidbody>().useGravity = false;
        collision.collider.enabled = false;
        mCarryingFruit = true;
        StartCoroutine(SetCanBeKnockedOut());

        for (int i = 0; i < mWalkedOnPlatforms.Count; i++)
        {
            mWalkedOnPlatforms[i].GetComponent<PlatformController>().ShineRight();
        }
    }

    IEnumerator SetCanBeKnockedOut()
    {
        yield return new WaitForSeconds(0.5f);
        mCanBeKnockedOut = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "PointCounter")
        {
            if (mCarriedFruit == null) return;

            if(other.transform.parent.GetComponent<BasketController>().mFruitType == mCarriedFruit.GetComponent<FruitController>().mFruitType)
            {
                for (int i = 0; i < mWalkedOnPlatforms.Count; i++)
                {
                    mWalkedOnPlatforms[i].GetComponent<PlatformController>().UnShine();
                }
                mWalkedOnPlatforms.Clear();
                OnPointScored();
            }

            Destroy(mCarriedFruit);
            mCarryingFruit = false;
            mCanBeKnockedOut = false;
        }
        else if(other.tag == "Platform")
        {
            if(mCurrentPlatform!=null)
            {
                mCurrentPlatform.GetComponent<PlatformController>().UnShine();
            }
            mCurrentPlatform = other.gameObject;
            other.GetComponent<PlatformController>().ShineRight();

            if (!mCarryingFruit)
            {

                if (!mWalkedOnPlatforms.Contains(mCurrentPlatform))
                {
                    mWalkedOnPlatforms.Add(mCurrentPlatform);
                }
            }
            else
            {
                if (mCanBeKnockedOut)
                {
                    if (!mWalkedOnPlatforms.Contains(mCurrentPlatform))
                    {
                        mCurrentPlatform.GetComponent<PlatformController>().ShineWrong();
                        StartCoroutine(StartKickOutTimer());
                    }
                }
            }
            
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Platform")
        {
            if (other.gameObject == mCurrentPlatform)
            {
                other.GetComponent<PlatformController>().UnShine();
                mCurrentPlatform = null;
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Platform")
        {
            if (mCurrentPlatform == null)
            {
                mCurrentPlatform = other.gameObject;
                other.GetComponent<PlatformController>().ShineRight();

                if (!mCarryingFruit)
                {

                    if (!mWalkedOnPlatforms.Contains(mCurrentPlatform))
                    {
                        mWalkedOnPlatforms.Add(mCurrentPlatform);
                    }
                }
                else
                {
                    if (mCanBeKnockedOut)
                    {
                        if (!mWalkedOnPlatforms.Contains(mCurrentPlatform))
                        {
                            mCurrentPlatform.GetComponent<PlatformController>().ShineWrong();
                        }
                    }

                }
            }
        }
    }

    IEnumerator StartKickOutTimer()
    {
        if (mKnockedOut) yield return null;

        yield return new WaitForSeconds(0.5f);
        GetComponent<Collider>().enabled = false;
        if(mCarriedFruit)
        {
            mCarriedFruit.GetComponent<Rigidbody>().useGravity = true;
            mCarriedFruit.GetComponent<Collider>().enabled = true;
        }

        if(mCurrentPlatform)
        mCurrentPlatform.GetComponent<PlatformController>().UnShine();

        mCurrentPlatform = null;
        mCarryingFruit = false;
        mCarriedFruit = null;
        mKnockedOut = true;
        mCanBeKnockedOut = false;

        for (int i = 0; i < mWalkedOnPlatforms.Count; i++)
        {
            mWalkedOnPlatforms[i].GetComponent<PlatformController>().UnShine();
        }
        mWalkedOnPlatforms.Clear();
    }
}
