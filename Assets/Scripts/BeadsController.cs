using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BER_ERHI_c223901b45f74af0a160b6a254574b90
{
    public class BeadsController : MonoBehaviour
    {
        private List<GameObject> childSpheres = new List<GameObject>();
        private List<OneBeadController> childBeadsControllers = new List<OneBeadController>();
        private List<OneBeadController> sphereControllersToActive = new List<OneBeadController>();
        private Vector3 center = new Vector3(0, -5, -2);
        private int countToStartTick = 0;
        private float prevMoveInterval = 0.0f;
        private bool tickCourutineIsRunning = false;
        private bool nowIsResizing = false;

        private float radiusY = 1.0f;
        private float radiusX = 1.0f;
        private float radiusZ = 1.0f;
        private int length = 12;
        private int newLength = 12;
        private bool needResize = false;
        private float moveInterval = 0.3f;
        private float timeForStepOfBead = 1.0f;
        public float timeOfMove
        {
            get { return timeForStepOfBead; }
        }
        private float originTimeForStepOfBead = 1.0f;
        private float prevTickTime;
        private OneBeadController curBeadForMove;

        public GameObject spherePrefab;

        private void Awake()
        {
            GameManager.Instance.OnGameStarted.AddListener(InitBeads);
            GameManager.Instance.OnClickForTick.AddListener(Tick);
            GameManager.Instance.settings.OnCircleLengthChanged.AddListener(HandleCircleLengthChange);
        }

        // Start is called before the first frame update
        void Start()
        {
            prevTickTime = Time.time;
        }

        // Update is called once per frame
        void Update()
        {
            if(countToStartTick > 1)
            {
                if(!Global.Equal(moveInterval, 0.0f))
                {
                    prevMoveInterval = moveInterval;
                    moveInterval = 0.0f;
                }
                timeForStepOfBead = originTimeForStepOfBead / countToStartTick;
                if(timeForStepOfBead < 0.1f)
                {
                    timeForStepOfBead = 0.1f;
                }
            }
            else if(countToStartTick > 0)
            {
                if(Global.Equal(moveInterval, 0.0f) && !Global.Equal(prevMoveInterval, 0.0f))
                {
                    moveInterval = prevMoveInterval;
                }
                timeForStepOfBead = originTimeForStepOfBead;
            }else if (!tickCourutineIsRunning)
            {
                if (needResize)
                {
                    ResizeBeads();
                    needResize = false;
                }
            }

            if (!tickCourutineIsRunning && countToStartTick > 0)
            {
                if (needResize)
                {
                    ResizeBeads();
                    needResize = false;
                }
                StartCoroutine(MoveBeadsForOneTick());
            }

        }

        private void InitBeads()
        {
            length = GameManager.Instance.settings.lengthOfCircle;
            checkRadiuses();
            calculateCenter();
            childBeadsControllers.Clear();
            
            for (int i = 0; i < length; i++)
            {
                Vector3 positionOfSphere = getPositionForSphere(i);
                GameObject sphere;

                if (childSpheres.Count <= i)
                {
                    sphere = Instantiate(spherePrefab, positionOfSphere, spherePrefab.transform.rotation, transform);
                    childSpheres.Add(sphere);
                }
                else
                {
                    sphere = childSpheres[i];
                    sphere.SetActive(true);
                    sphere.transform.Translate(positionOfSphere - sphere.transform.position);
                }
                OneBeadController childBeadController = sphere.GetComponent<OneBeadController>();
                childBeadsControllers.Add(childBeadController);
                childBeadController.Init(this, i, i);
                if (i > 0)
                {
                    childBeadsControllers[i - 1].next = childBeadController;
                }
            }

            childBeadsControllers[childBeadsControllers.Count - 1].next = childBeadsControllers[0];
            curBeadForMove = childBeadsControllers[0];

            if(childSpheres.Count > length)
            {
                for(int i = length; i < childSpheres.Count; i++)
                {
                    childSpheres[i].SetActive(false);
                }
            }

        }

        private void checkRadiuses()
        {
            float minRadius = ((length + 2) * spherePrefab.GetComponent<SphereCollider>().radius * 2.001f) / (2 * Mathf.PI);

            radiusX = minRadius; // checkAndGetRadius(radiusX, minRadius);
            radiusY = minRadius; // checkAndGetRadius(radiusY, minRadius);
            radiusZ = minRadius; // checkAndGetRadius(radiusZ, minRadius);
        }

        private float checkAndGetRadius(float radiusForCheck, float minRadius)
        {
            if (radiusForCheck < minRadius)
            {
                return minRadius;
            }
            return radiusForCheck;
        }

        private void calculateCenter()
        {
            float x = 0;
            float y = -radiusY;
            float z = radiusZ;

            center = new Vector3(x, y, z);
        }

        public Vector3 getPositionForSphere(int numberOfPosition)
        {
            float angleOfPosition = getAngleForPosition(numberOfPosition);
            return getPositionByAngle(angleOfPosition);
        }

        public Vector3 getPositionByAngle(float angleOfPosition)
        {
            float x = radiusX * Mathf.Cos(angleOfPosition) + center.x;
            float y = radiusY * Mathf.Sin(angleOfPosition) + center.y;
            float z = radiusZ * Mathf.Cos(angleOfPosition) + center.z;

            return new Vector3(x, y, z);
        }

        public float getAngleForPosition(int numberOfPosition)
        {
            return (Mathf.PI / 2 - numberOfPosition * (2 * Mathf.PI / (length + 2)));
        }

        private void Tick()
        {
            countToStartTick++;
        }

        private IEnumerator MoveBeadsForOneTick() {

            tickCourutineIsRunning = true;

            curBeadForMove.MarkCanThroughZero();
            if (curBeadForMove.positionIndex == 0)
            { 
                curBeadForMove.OnPreviousBeadStartMoving(length); 
            }
            curBeadForMove = curBeadForMove.next;

            if(countToStartTick > 0)
            {
                countToStartTick--;
            }

            tickCourutineIsRunning = false;

            yield return null;
        }

        private IEnumerator WaitInterval()
        {
            if (!Global.Equal(moveInterval, 0.0f))
            {
                yield return new WaitForSeconds(moveInterval);
            }
            else
            {
                yield return null;
            }
        }

        private IEnumerator WaitTimeForMove()
        {
            yield return new WaitForSeconds(timeForStepOfBead);
        }

        public Vector3 getPositionOnStepOfMovingTo(int numberOfPosition, int fromPosition, float partOfDistance)
        {
            float angleOfPosition = getAngleForPosition(numberOfPosition);
            float angleOfPrevPosition = getAngleForPosition(fromPosition);

            while (angleOfPosition < angleOfPrevPosition)
            {
                angleOfPosition += 2 * Mathf.PI;
            }

            float currentAngle = (angleOfPosition - angleOfPrevPosition) * partOfDistance +
                angleOfPrevPosition;

            return getPositionByAngle(currentAngle);

        }

        private void HandleCircleLengthChange()
        {
            newLength = GameManager.Instance.settings.lengthOfCircle;
            needResize = true;
        }

        private void ResizeBeads()
        {
            nowIsResizing = true;
            int lengthBefore = length;
            length = GameManager.Instance.settings.lengthOfCircle;
            checkRadiuses();
            calculateCenter();
            
            //TODO
            bool canPlaceToPosition = false;
            bool isRunningBeads = false;
            foreach (OneBeadController beadController in childBeadsControllers)
            {
                if (beadController.State == OneBeadController.allStates.Moving)
                {
                    isRunningBeads = true;
                    break;
                }
            }
            if (countToStartTick > 0 || isRunningBeads)
            {
                canPlaceToPosition = false;
            }
            else
            {
                canPlaceToPosition = true;
            }

            if (length > lengthBefore)
            {
                AddNewBeads(lengthBefore, canPlaceToPosition);
            }
            else
            {
                HideSomeBeads();
            }

            if (canPlaceToPosition)
            {
                for (int i = 0; i < childBeadsControllers.Count; i++)
                {
                    GameObject sphete = childBeadsControllers[i].gameObject;
                    sphete.transform.Translate(getPositionForSphere(i) - sphete.transform.position);
                }
            }

            foreach (OneBeadController beadController in childBeadsControllers)
            {
                beadController.BeadsResized(lengthBefore);
            }

            nowIsResizing = false;

        }

        private void AddNewBeads(int lengthBefore, bool canPlaceToPosition)
        {
            for (int i = lengthBefore; i < length; i++)
            {
                GameObject sphere = null;
                if (childSpheres.Count <= i)
                {
                    Vector3 positionToPlace;
                    if (canPlaceToPosition)
                    {
                        positionToPlace = getPositionForSphere(i);
                    }
                    else
                    {
                        positionToPlace = getPositionForSphere(length - 1);
                    }
                    sphere = Instantiate(spherePrefab, positionToPlace, spherePrefab.transform.rotation, transform);
                    sphere.SetActive(false);
                }
                else
                {
                    sphere = childSpheres[i];
                }

                OneBeadController childController = sphere.GetComponent<OneBeadController>();
                
                if (!canPlaceToPosition)
                {
                    Debug.Log("Add sphere to INACTIVE position " + (length-1));
                    sphere.transform.Translate(getPositionForSphere(length - 1) - sphere.transform.position);
                    childController.Init(this, i, length - 1);
                    sphere.SetActive(false);
                    sphereControllersToActive.Add(childController);
                }
                else
                {
                    Debug.Log("Add sphere to position " + i);
                    childController.Init(this, i, i);
                    sphere.SetActive(true);
                    sphere.transform.Translate(getPositionForSphere(i) - sphere.transform.position);
                }
                childBeadsControllers.Add(childController);

            }
        }

        private void HideSomeBeads()
        {
            for(int i = childBeadsControllers.Count-1; i >= length ; i--)
            {
                OneBeadController curController = childBeadsControllers[i];
                curController.gameObject.SetActive(false);
                childSpheres.Remove(curController.gameObject);
                childSpheres.Add(curController.gameObject);
                childBeadsControllers.RemoveAt(i);
            }
        }

    }
}