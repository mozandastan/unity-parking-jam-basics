using UnityEngine;

public class InputManager : MonoBehaviour
{

    private Vector2 firstPressPos;
    private Vector2 secondPressPos;
    private Vector2 currentSwipe;
    private GameObject carObject;

    void Update()
    {
        Swipe();
    }
    public void Swipe()
    {
        if (MenuManager.InPutEnableBool)
        {
            if (Input.touches.Length > 0) //Touch Input
            {
                Touch t = Input.GetTouch(0);
                if (t.phase == TouchPhase.Began)
                {
                    carObject = null;

                    firstPressPos = new Vector2(t.position.x, t.position.y);

                    RaycastHit raycastHit;
                    Ray ray = Camera.main.ScreenPointToRay(firstPressPos);
                    if (Physics.Raycast(ray, out raycastHit, 100f))
                    {
                        if (raycastHit.transform != null)
                        {
                            if (raycastHit.transform.gameObject.tag == "car")
                            {
                                carObject = raycastHit.transform.gameObject;
                            }
                        }
                    }


                }
                if (t.phase == TouchPhase.Ended)
                {
                    Vector3 tempVector = Vector3.zero;

                    secondPressPos = new Vector2(t.position.x, t.position.y);
                    currentSwipe = new Vector3(secondPressPos.x - firstPressPos.x, secondPressPos.y - firstPressPos.y);
                    currentSwipe.Normalize();

                    if (currentSwipe.y > 0 && currentSwipe.x > -0.5f && currentSwipe.x < 0.5f)
                    {
                        Debug.Log("up swipe");
                        tempVector = Vector3.forward;
                    }

                    else if (currentSwipe.y < 0 && currentSwipe.x > -0.5f && currentSwipe.x < 0.5f)
                    {
                        Debug.Log("down swipe");
                        tempVector = Vector3.back;
                    }

                    else if (currentSwipe.x < 0 && currentSwipe.y > -0.5f && currentSwipe.y < 0.5f)
                    {
                        Debug.Log("left swipe");
                        tempVector = Vector3.left;
                    }

                    else if (currentSwipe.x > 0 && currentSwipe.y > -0.5f && currentSwipe.y < 0.5f)
                    {
                        Debug.Log("right swipe");
                        tempVector = Vector3.right;
                    }

                    if (carObject != null)
                    {

                        carObject.GetComponent<CarScript>().CheckDirectionAvailable(tempVector);
                    }
                }
            }
            else //Mouse Input
            {
                if (Input.GetMouseButtonDown(0))
                {
                    carObject = null;

                    RaycastHit raycastHit;
                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    if (Physics.Raycast(ray, out raycastHit, 100f))
                    {
                        if (raycastHit.transform != null)
                        {
                            if (raycastHit.transform.gameObject.tag == "car")
                            {
                                carObject = raycastHit.transform.gameObject;
                            }
                        }
                    }

                    firstPressPos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
                }
                if (Input.GetMouseButtonUp(0))
                {
                    Vector3 tempVector = Vector3.zero;

                    secondPressPos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
                    currentSwipe = new Vector2(secondPressPos.x - firstPressPos.x, secondPressPos.y - firstPressPos.y);
                    currentSwipe.Normalize();

                    if (currentSwipe.y > 0 && currentSwipe.x > -0.5f && currentSwipe.x < 0.5f)
                    {
                        Debug.Log("up swipe");
                        tempVector = Vector3.forward;
                    }

                    else if (currentSwipe.y < 0 && currentSwipe.x > -0.5f && currentSwipe.x < 0.5f)
                    {
                        Debug.Log("down swipe");
                        tempVector = Vector3.back;
                    }

                    else if (currentSwipe.x < 0 && currentSwipe.y > -0.5f && currentSwipe.y < 0.5f)
                    {
                        Debug.Log("left swipe");
                        tempVector = Vector3.left;
                    }

                    else if (currentSwipe.x > 0 && currentSwipe.y > -0.5f && currentSwipe.y < 0.5f)
                    {
                        Debug.Log("right swipe");
                        tempVector = Vector3.right;
                    }

                    if (carObject != null)
                    {

                        carObject.GetComponent<CarScript>().CheckDirectionAvailable(tempVector);
                    }
                }
            }

        }
    }

}
