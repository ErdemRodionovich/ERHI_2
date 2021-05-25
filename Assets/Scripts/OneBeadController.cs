using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BER_ERHI_c223901b45f74af0a160b6a254574b90
{
    public class OneBeadController : MonoBehaviour
    {
        private enum allStates { Peace, Moving };
        private allStates stateOfBead = allStates.Peace;
        private BeadsController parentController;
        private int stepOfMoving = 0;
        private int totalSteps = 100;
        private Vector3 destinationPosition;
        private float bmv = 0.000001f;
        private List<int> walkToPositions = new List<int>();

        private int number = 0;
        private int positionNumber;
        private int prevPositionNumber;

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
                    StartWalk(walkToPositions[0]);
                    walkToPositions.RemoveAt(0);
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
                stepOfMoving = 0;
                stateOfBead = allStates.Moving;
                positionNumber = newPositionNumber;
                destinationPosition = parentController.getPositionForSphere(newPositionNumber);
            }

            if(prevPositionNumber == 0)
            {
                if(GameManager.Instance.OnTick != null)
                {
                    GameManager.Instance.OnTick.Invoke();
                }
            }

        }

        public void Init(BeadsController parentController, int number, int positionNumber)
        {
            this.parentController = parentController;
            this.number = number;
            this.positionNumber = positionNumber;
        }

        private void WalkToNextStep()
        {
            stepOfMoving++;
            float partOfDistance = (float)stepOfMoving / (float)totalSteps;
            Vector3 nextPosition = parentController.getPositionOnStepOfMovingTo(positionNumber, prevPositionNumber, partOfDistance);
            transform.Translate(nextPosition - transform.position);
        }

        private void checkForFinishOfWalking()
        {
            if (stepOfMoving >= totalSteps)
            {
                stateOfBead = allStates.Peace;
                stepOfMoving = 0;
            }
        }

    }
}