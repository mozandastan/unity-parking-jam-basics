using UnityEngine;

public class CarScript : MonoBehaviour
{
    private bool isCarReverse = false;
    private bool isCarHorizontal = false;

    private float moveSpeed = 5f;
    private float turnSpeed = 20f;
    private enum MoveStates { Idle, MoveToRoad, MoveOnRoad, Rotate, RotateCorner };
    MoveStates moveStates = MoveStates.Idle;

    private Vector3 moveDirection;
    private Vector3 moveTarget;
    private GameObject currentRoadObj;
    private Quaternion rotTarget;

    private BoxCollider boxCollider;

    void Start()
    {
        boxCollider = GetComponent<BoxCollider>();
    }

    void Update()
    {
        MoveStatesUpdate();

    }

    public void CheckDirectionAvailable(Vector3 dir) // Controls if direction available
    {
        if (dir == Vector3.right || dir == Vector3.left)
        {
            isCarHorizontal = true;
        }

        if (dir == this.transform.right || -dir == this.transform.right) // If the swipe way == car way
        {
            Debug.Log("DIR OK" + dir);

            if (-dir == this.transform.right) // If the swipe direction != car direction
            {
                Debug.Log("DIR REV");
                isCarReverse = true; // Car is reverse
            }


            if (Physics.Raycast(transform.position, dir, out var hit, 50f)) // Swipe direction raycast
            {
                if (hit.collider.gameObject.tag == "road") // car sees road, can move
                {
                    moveDirection = dir;
                    moveTarget = hit.collider.transform.position;

                    moveStates = MoveStates.MoveToRoad; // Move to road state activated
                }
                else if (hit.collider.gameObject != null) // car sees another object, cant move
                {
                    Debug.Log($"Obstaacle: {hit.collider.gameObject}");

                }

            }
        }

    }

    private void OnTriggerEnter(Collider other) // Detecting next road object
    {
        if (other.gameObject.tag == "road")
        {
            currentRoadObj = other.gameObject;
            Debug.Log("Trigger Obj =" + other.gameObject);
        }
    }

    private void MoveStatesUpdate()
    {
        switch (moveStates)
        {
            case MoveStates.Idle:

                break;

            case MoveStates.MoveToRoad: // To ther road

                transform.position += moveDirection * Time.deltaTime * moveSpeed;

                if (isCarHorizontal)
                {
                    if (Mathf.Abs(transform.position.x - moveTarget.x) <= 0.5f && boxCollider.enabled)
                    {
                        PreRotate();
                    }

                }
                else
                {
                    if (Mathf.Abs(transform.position.z - moveTarget.z) <= 0.5f && boxCollider.enabled)
                    {
                        PreRotate();
                    }

                }
                break;

            case MoveStates.MoveOnRoad: // On road, to the anothe road

                transform.position = Vector3.MoveTowards(transform.position, moveTarget, Time.deltaTime * moveSpeed);

                if (Vector3.Distance(transform.position, moveTarget) <= 0.5f)
                {
                    float rotDegree = 90;
                    rotTarget = Quaternion.Euler(0, rotDegree, 0) * this.transform.rotation;

                    moveStates = MoveStates.RotateCorner;
                }

                break;

            case MoveStates.Rotate: // first rotate

                if (isCarHorizontal)
                {
                    if (Mathf.Abs(transform.position.x - moveTarget.x) <= 0.1f)
                    {
                        transform.position = new Vector3(moveTarget.x, transform.position.y, transform.position.z);
                    }
                    else
                    {
                        transform.position += moveDirection * Time.deltaTime * moveSpeed;
                    }

                }
                else
                {
                    if (Mathf.Abs(transform.position.z - moveTarget.z) <= 0.1f)
                    {
                        transform.position = new Vector3(transform.position.x, transform.position.y, moveTarget.z);
                    }
                    else
                    {
                        transform.position += moveDirection * Time.deltaTime * moveSpeed;
                    }
                }

                transform.rotation = Quaternion.Lerp(transform.rotation, rotTarget, Time.deltaTime * turnSpeed);

                if (Mathf.Abs(transform.rotation.eulerAngles.y - rotTarget.eulerAngles.y) <= 1f)
                {
                    moveTarget = new Vector3(currentRoadObj.transform.GetChild(0).transform.position.x, transform.position.y, currentRoadObj.transform.GetChild(0).transform.position.z);

                    moveStates = MoveStates.MoveOnRoad;

                    transform.rotation = rotTarget;
                    boxCollider.enabled = true;
                }

                break;
            case MoveStates.RotateCorner: // road corner, rotating

                if (Vector3.Distance(transform.position, moveTarget) <= 0.1f)
                {
                    transform.position = moveTarget;
                }
                else
                {
                    transform.position = Vector3.MoveTowards(transform.position, moveTarget, Time.deltaTime * moveSpeed);
                }

                transform.rotation = Quaternion.Lerp(transform.rotation, rotTarget, Time.deltaTime * turnSpeed);

                if (Mathf.Abs(transform.rotation.eulerAngles.y - rotTarget.eulerAngles.y) <= 1f)
                {
                    moveTarget = new Vector3(currentRoadObj.transform.GetChild(0).transform.position.x, transform.position.y, currentRoadObj.transform.GetChild(0).transform.position.z);

                    moveStates = MoveStates.MoveOnRoad;

                    transform.rotation = rotTarget;
                    boxCollider.enabled = true;
                }

                break;
        }
    }
    private void PreRotate() // first rotate when car is going to first road
    {
        boxCollider.enabled = false;

        float rotDegree = isCarReverse ? -90 : 90;
        rotTarget = Quaternion.Euler(0, rotDegree, 0) * this.transform.rotation;

        moveStates = MoveStates.Rotate;

        Debug.Log("Rotate Started");

    }
}
