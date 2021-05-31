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
                    if (prevBeadStartMovingTime > parentController.timeOfMove)
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
            float partOfDistance = movingDuration / parentController.timeOfMove;
            if(partOfDistance > 1.0f)
            {
                partOfDistance = 1.0f;
            }
            Vector3 nextPosition = parentController.getPositionOnStepOfMovingTo(positionNumber, prevPositionNumber, partOfDistance);
            transform.Translate(nextPosition - transform.position);
        }

        private void checkForFinishOfWalking()
        {
            if (Vector3.Distance(parentController.getPositionForSphere(positionNumber), transform.position) < 10000 * bmv)
            {
                stateOfBead = allStates.Peace;
                movingDuration = 0.0f;
                Debug.Log("bead " + number + " is arrived to " + positionNumber);
            }
        }

        public void BeadsResized(int lengthBefore)
        {
            if (lengthBefore < GameManager.Instance.settings.lengthOfCircle)
            {
                int r = GameManager.Instance.settings.lengthOfCircle - lengthBefore;
                for (int i = 0; i < walkToPositions.Count; i++)
                {
                    walkToPositions[i] = (walkToPositions[i] + r) % GameManager.Instance.settings.lengthOfCircle;
                }
            }
            else
            {
                for (int i = 0; i < walkToPositions.Count; i++)
                {
                    walkToPositions[i] = walkToPositions[i] % GameManager.Instance.settings.lengthOfCircle;
                }
            }
        }

        public void OnPreviousBeadStartMoving(int fromPosition)
        {
            if(positionNumber != 0)
            {
                walkToPositions.Add(fromPosition);
                prevBeadStartMovingTime = 0.0f;
            }
            else
            {
                if(canWalkThroughZero.Count > 0)
                {
                    walkToPositions.Add(fromPosition);
                    prevBeadStartMovingTime = 0.0f;
                    canWalkThroughZero.RemoveAt(0);
                }
            }
        }

        private void OnStartMoving()
        {
            if(nextBead != null)
            {
                nextBead.OnPreviousBeadStartMoving(prevPositionNumber);
            }
        }

        public void MarkCanThroughZero()
        {
            canWalkThroughZero.Add(true);
        }

    }
}