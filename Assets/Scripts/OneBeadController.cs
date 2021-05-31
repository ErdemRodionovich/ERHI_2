using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BER_ERHI_c223901b45f74af0a160b6a254574b90
{
    public class OneBeadController : MonoBehaviour
    {
        public enum allStates { Peace, Moving };
        private allStates stateOfBead = allStates.Peace;
        public allStates State
        {
            get { return stateOfBead; }
        }
        private BeadsController parentController;
        private const float bmv = 0.000001f;
        private List<int> walkToPositions = new List<int>();
        private List<bool> canWalkThroughZero = new List<bool>();

        private int number = 0;
        private int positionNumber;
        public int positionIndex
        {
            get { return positionNumber; }
        }
        private int prevPositionNumber;
        private Vector3 positionsDiff = new Vector3(0.0f, 0.0f, 0.0f);
        private float movingDuration = 0.0f;
        private float prevBeadStartMovingTime = 0.0f;
        private OneBeadController nextBead;
        public OneBeadController next
        {
            get { return nextBead; }
            set { nextBead = value; }
        }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            if (stateOfBead == allStates.Moving)
            {
                WalkToNextStep();
                checkForFinishOfWalking();
            }
            else
            {
                if(walkToPositions.Count > 0)
                {
                    if (prevBeadStartMovingTime > parentController.intervalForMove)
                    {
                        StartWalk(walkToPositions[0]);
                        walkToPositions.RemoveAt(0);
                        prevBeadStartMovingTime = 0.0f;
                    }
                    else
                    {
                        prevBeadStartMovingTime += Time.deltaTime;
                    }
                }
            }
        }

        public void StartWalkToPosition(int newPositionNumber)
        {
            walkToPositions.Add(newPositionNumber);
            prevBeadStartMovingTime = parentController.TimeOfMove;
        }

        private void StartWalk(int newPositionNumber) {

            if (positionNumber != newPositionNumber)
            {
                prevPositionNumber = positionNumber;
                movingDuration = 0.0f;
                stateOfBead = allStates.Moving;
                positionNumber = newPositionNumber;

                if (prevPositionNumber == 0)
                {
                    GameManager.Instance.Tick();
                }
                OnStartMoving();
            }

        }

        public void Init(BeadsController parentController, int number, int positionNumber)
        {
            this.parentController = parentController;
            this.number = number;
            this.positionNumber = positionNumber;
            stateOfBead = allStates.Peace;
            walkToPositions.Clear();
        }

        private void WalkToNextStep()
        {
            movingDuration += Time.deltaTime;
            float partOfDistance = movingDuration / parentController.TimeOfMove;
            if(partOfDistance > 1.0f)
            {
                partOfDistance = 1.0f;
            }
            Vector3 nextPosition = parentController.getPositionOnStepOfMovingTo(positionNumber, prevPositionNumber, partOfDistance);
            transform.Translate(nextPosition - transform.position + positionsDiff * (1.0f - partOfDistance));
        }

        private void checkForFinishOfWalking()
        {
            if (Vector3.Distance(parentController.getPositionForSphere(positionNumber), transform.position) < 10000 * bmv)
            {
                stateOfBead = allStates.Peace;
                movingDuration = 0.0f;
                positionsDiff = new Vector3(0.0f, 0.0f, 0.0f);
            }
        }

        public void BeadsResized(int lengthBefore)
        {
            if (stateOfBead == allStates.Peace)
            {
                positionsDiff = parentController.getPositionForSphere(positionNumber) - transform.position;
                stateOfBead = allStates.Moving;
                movingDuration = 0.0f;
            }
            else
            {
                float partOfDistance = movingDuration / parentController.TimeOfMove;
                if (partOfDistance > 1.0f)
                {
                    partOfDistance = 1.0f;
                }
                Vector3 nextPosition = parentController.getPositionOnStepOfMovingTo(positionNumber, prevPositionNumber, partOfDistance);
                if (partOfDistance != 1.0f)
                {
                    positionsDiff = (nextPosition - transform.position) / (1.0f - partOfDistance);
                }
                else
                {
                    positionsDiff = new Vector3(0.0f, 0.0f, 0.0f);
                }
            }
        }

        public void OnPreviousBeadStartMoving(int fromPosition)
        {
            prevBeadStartMovingTime = 0.0f;
            if (positionNumber != 0 && fromPosition != positionNumber)
            {
                for (int i = positionNumber - 1; i >= fromPosition; i--)
                {
                    walkToPositions.Add(i);
                }
                if (!gameObject.activeInHierarchy)
                {
                    gameObject.SetActive(true);
                }
            }
        }

        private void OnStartMoving()
        {
            if (nextBead != null)
            {
                nextBead.OnPreviousBeadStartMoving(prevPositionNumber);
            }
            else
            {
                Debug.LogError("[OneBeadController] nextBead is NULL. OnStartMoving. position number:"+positionNumber+" number:"+number);
            }
        }

    }
}