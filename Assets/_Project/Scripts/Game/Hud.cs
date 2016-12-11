using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace Assets._Project.Scripts.Game
{
    public class Hud : MonoBehaviour
    {
        public Car Car;

        public Text PositionNumberText;
        public Text LapNumberText;
        public Text ProgressText;
        public Text MainMessage;
        public Text SpeedText;
        public GameObject ButtonPlayAgain, ButtonExit;

        public RaceState State;
        
        public void DisableHudElements()
        {
            ButtonPlayAgain.SetActive(false);
            ButtonExit.SetActive(false);
            PositionNumberText.transform.parent.gameObject.SetActive(false);
            LapNumberText.transform.parent.gameObject.SetActive(false);
            SpeedText.transform.parent.gameObject.SetActive(false);
            ProgressText.enabled = false;
        }

        public void EnableHudElements()
        {
            PositionNumberText.transform.parent.gameObject.SetActive(true);
            LapNumberText.transform.parent.gameObject.SetActive(true);
            SpeedText.transform.parent.gameObject.SetActive(true);
        }

        void Update()
        {
            if (Car == null) return;
            var state = Car.GetComponent<RaceProgressState>();

            if(LapNumberText != null)
                LapNumberText.text = state.LapCount + "/" + state.NumberOfLaps;

            if (ProgressText != null)
                ProgressText.text = state.RaceProgress + "%";

            PositionNumberText.text = State.GetMyPosition(Car) + "/" + State.CarCount;
            SpeedText.text = (Car.CurrentSpeed / 5) + " km/h";
        }

        public void GameOver(int finishingPosition, int carCount)
        {
            ButtonPlayAgain.SetActive(true);
            ButtonExit.SetActive(true);
            MainMessage.enabled = true;
            MainMessage.text = "Finished " + finishingPosition + " of " + carCount; 
        }
    }
}
