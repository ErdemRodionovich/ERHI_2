using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BER_ERHI_c223901b45f74af0a160b6a254574b90
{
    public class BeadsController : MonoBehaviour
    {
        private List<GameObject> childSpheres = new List<GameObject>();
        private List<OneBeadController> childBeadsControllers = new List<OneBeadController>();
        private Vector3 center = new Vector3(0, -5, -2);
        private int countToStartTick = 0;
        private float prevMoveInterval = 0.0f;
        private bool tickCourutineIsRunning = false;
        private OneBeadController lastMovedBead = null;
        private Vector3 scaleOfBead = new Vector3(1.0f, 1.0f, 1.0f);

        private float radiusY = 1.0f;
        private float radiusX = 1.0f;
        private float radiusZ = 1.0f;
        private int length = 12;
        private List<float> anglesTable = new List<float>();
        private int freeCount = 2;
        private bool needResize = false;
        private float originMoveInterval = 0.3f;
        private float moveInterval = 0.3f;
        public float intervalForMove
        {
            get { return moveInterval; }
        }
        private float timeForStepOfBead = 1.0f;
        public float TimeOfMove
        {
            get { return timeForStepOfBead; }
        }
        private float originTimeForStepOfBead = 0.5f;
        private float prevTickTime;
        
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
            scaleOfBead = spherePrefab.transform.localScale;
        }

        // Update is called once per frame
        void Update()
        {   
            if (!tickCourutineIsRunning && countToStartTick > 0)
            {
                StartCoroutine(MoveBeadsForOneTick());
            }
        }

        private void InitBeads()
        {
            length = GameManager.Instance.settings.lengthOfCircle;
            checkRadiuses();
            calculateCenter();
            childBeadsControllers.Clear();
            originMoveInterval = 1.0f / length;
            
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
            float radiusOfBead = spherePrefab.GetComponent<SphereCollider>().radius;
            float minRadius = ((length + freeCount) * radiusOfBead * 2.001f) / (2 * Mathf.PI);
            scaleOfBead = new Vector3(1.0f, 1.0f, 1.0f);
            float lengthEll = 0.0f;
            float reqRadiusOfBead = radiusOfBead;

            if (minRadius > 7.2f)
            {
                minRadius = 7.2f;
                radiusZ = minRadius * 3;
                radiusX = radiusZ * Mathf.Tan(Mathf.PI / 12);
                radiusY = radiusZ * Mathf.Tan(Mathf.PI / 8);
                lengthEll = LengthOfEllipse(radiusX, radiusY, radiusZ);
                
                reqRadiusOfBead = lengthEll / ((length + freeCount)*2.0f);
                float resizeCoeff = reqRadiusOfBead / radiusOfBead;
                if (resizeCoeff < 1.0f)
                {
                    scaleOfBead = new Vector3(resizeCoeff, resizeCoeff, resizeCoeff);
                }
            }
            spherePrefab.transform.localScale = scaleOfBead;
            for(int i=0; i < childBeadsControllers.Count; i++)
            {
                childBeadsControllers[i].scale = scaleOfBead;
            }

            radiusZ = minRadius * 3;
            radiusX = radiusZ * Mathf.Tan(Mathf.PI/12);
            radiusY = radiusZ * Mathf.Tan(Mathf.PI/8);
            
            Debug.Log("[BeadsController] Min radius = " + minRadius.ToString() + " rZ=" + radiusZ.ToString() + " rY=" + radiusY.ToString() + " rX=" + radiusX.ToString()+
                    " radiusOfBead="+radiusOfBead.ToString()+" scaleOfBead="+scaleOfBead.ToString()+" reqFadius="+reqRadiusOfBead.ToString());

            InitAngleTable(reqRadiusOfBead);

        }

        private float LengthOfEllipse(float rX, float rY, float rZ)
        {
            float a = Mathf.Sqrt(Mathf.Pow(rZ, 2.0f)+Mathf.Pow(rY, 2.0f));
            float b = rX;

            return (Mathf.PI*(a+b)*(Mathf.Abs(1.0f+(3.0f*Mathf.Pow((a-b)/(a+b),2.0f))/(10.0f+Mathf.Sqrt(4.0f-3.0f*Mathf.Pow((a-b)/(a+b),2.0f))))));

        }

        private void InitAngleTable(float reqRadius)
        {
            anglesTable.Clear();
            float reqDistance = reqRadius * 2.01f;
            float curAngle = Mathf.PI / 2;
            float nextAngle = curAngle;
            float step = 2.0f * Mathf.PI / ((length + freeCount)*10.0f);
            anglesTable.Add(curAngle);
            Vector3 curPosition = getPositionByAngle(curAngle);
            float dist = 0.0f;
            
            Debug.Log("[BeadController] reqDist="+reqDistance.ToString()+" anglesTable = ");
            for (int i=1; i <= (length+freeCount); i++)
            {
                do
                {
                    nextAngle -= step;
                    dist = Vector3.Distance(getPositionByAngle(nextAngle), curPosition);
                }
                while (dist < reqDistance);
                
                curAngle = nextAngle;
                curPosition = getPositionByAngle(curAngle);
                anglesTable.Add(curAngle);
                Debug.Log("angle = " + curAngle+" dist="+dist+" i="+i);
            }
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
            float z = radiusZ * Mathf.Cos(angleOfPosition + Mathf.PI/2) + center.z;

            return new Vector3(x, y, z);
        }

        public float getAngleForPosition(int numberOfPosition)
        {
            if (anglesTable.Contains(numberOfPosition))
            {
                return anglesTable[numberOfPosition];
            }

            return (Mathf.PI / 2 - numberOfPosition * (2 * Mathf.PI / (length + freeCount)));
        }

        private void Tick()
        {
            countToStartTick++;
        }

        private void BootBeadsMoving(int forceCount)
        {
            if (forceCount >= (freeCount-1))
            {
                timeForStepOfBead = originTimeForStepOfBead / (float)length;
                moveInterval = originMoveInterval / (float)length;
            }
            else
            {
                timeForStepOfBead = originTimeForStepOfBead / (float)(forceCount + 2);
                moveInterval = originMoveInterval / (float)(forceCount + 2);
            }
        }

        private void RelaxBeadMoving()
        {
            moveInterval = originMoveInterval;
            timeForStepOfBead = originTimeForStepOfBead;
        }

        private IEnumerator MoveBeadsForOneTick() {

            tickCourutineIsRunning = true;

            if (needResize)
            {
                ResizeBeads();
            }

            int maxPosition = 0;
            bool isMovingBeads = false;
            foreach (OneBeadController curBeadForMove in childBeadsControllers)
            {
                if (curBeadForMove.State == OneBeadController.allStates.Moving)
                {
                    isMovingBeads = true;
                }

                if(maxPosition < curBeadForMove.positionIndex)
                {
                    maxPosition = curBeadForMove.positionIndex;
                }
            }

            if ((maxPosition+1) < (length + freeCount))
            {
                maxPosition++;
            }

            if (maxPosition > length)
            {
                BootBeadsMoving(maxPosition - length);
            }
            else { 
                RelaxBeadMoving();
            }

            foreach (OneBeadController curBeadForMove in childBeadsControllers)
            {
                if (curBeadForMove.positionIndex == 0 && curBeadForMove != lastMovedBead)
                {
                    curBeadForMove.StartWalkToPosition(maxPosition);
                    lastMovedBead = curBeadForMove;
                    if (countToStartTick > 0)
                    {
                        countToStartTick--;
                    }
                    break;
                }
            }

            yield return WaitInterval();

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
            needResize = true;
        }

        private void ResizeBeads()
        {
            int lengthBefore = length;
            length = GameManager.Instance.settings.lengthOfCircle;
            freeCount = length / 10 + 1;
            checkRadiuses();
            calculateCenter();
            originMoveInterval = 1.0f / length;

            if (length > lengthBefore)
            {
                AddNewBeads(lengthBefore);
            }
            else
            {
                HideSomeBeads();
            }

            foreach (OneBeadController beadController in childBeadsControllers)
            {
                beadController.BeadsResized(lengthBefore);
            }
            needResize = false;
        }

        private void AddNewBeads(int lengthBefore)
        {
            OneBeadController last = null, nextOfLast = null;
            int maxIndex = 0;
            int lastIndex = length - 1;
            foreach(OneBeadController chController in childBeadsControllers)
            {
                if(maxIndex < chController.positionIndex)
                {
                    last = chController;
                    maxIndex = chController.positionIndex;
                }
            }
            nextOfLast = last.next;
            Vector3 positionToPlace = getPositionForSphere(last.positionIndex);
            int numberOfPosition = last.positionIndex;

            for (int i = lengthBefore; i < length; i++)
            {
                GameObject sphere = null;
                if (childSpheres.Count > i)
                {
                    foreach(GameObject chSphere in childSpheres)
                    {
                        if (!chSphere.activeInHierarchy && !childBeadsControllers.Contains(chSphere.GetComponent<OneBeadController>()))
                        {
                            sphere = chSphere;
                            break;
                        }
                    }
                }

                if(sphere == null) { 
                    sphere = Instantiate(spherePrefab, positionToPlace, spherePrefab.transform.rotation, transform);
                    sphere.SetActive(false);
                }
                numberOfPosition = last.positionIndex;
                bool positionIsBusy = false;
                foreach(OneBeadController chController in childBeadsControllers)
                {
                    if (!chController.IsPositionFree(numberOfPosition + 1))
                    {
                        positionIsBusy = true;
                        break;
                    }
                }
                bool placeActived = false;
                if (!positionIsBusy && (numberOfPosition+1) < (length+freeCount))
                {
                    placeActived = true;
                    numberOfPosition++;
                    positionToPlace = getPositionForSphere(numberOfPosition);
                }

                OneBeadController childController = sphere.GetComponent<OneBeadController>();
                if (placeActived)
                {
                    sphere.SetActive(true);
                }
                else
                {
                    sphere.SetActive(false);
                }
                sphere.transform.Translate(positionToPlace - sphere.transform.position);
                childController.Init(this, i, numberOfPosition);
                childController.scale = scaleOfBead;
                childBeadsControllers.Add(childController);
                last.next = childController;
                last = childController;
            }

            last.next = nextOfLast;

        }

        private void HideSomeBeads()
        {
            List<OneBeadController> beadControllers = new List<OneBeadController>();
            OneBeadController currentController = childBeadsControllers[0];
            int minPosition = length;
            foreach (OneBeadController chController in childBeadsControllers)
            {
                if (chController.positionIndex < minPosition)
                {
                    currentController = chController;
                    minPosition = chController.positionIndex;
                }
            }

            for (int i = 0; i < childBeadsControllers.Count; i++)
            {
                beadControllers.Add(currentController);
                currentController = currentController.next;
            }
            
            for(int i = childBeadsControllers.Count-1; i >= length ; i--)
            {
                OneBeadController curController = beadControllers[i];
                curController.gameObject.SetActive(false);
                childSpheres.Remove(curController.gameObject);
                childSpheres.Add(curController.gameObject);
                childBeadsControllers.Remove(curController);
                beadControllers.Remove(curController);
            }

            beadControllers[length - 1].next = beadControllers[0];

        }

    }
}