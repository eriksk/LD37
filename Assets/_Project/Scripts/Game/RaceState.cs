using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets._Project.Scripts.Game
{
    public class RaceState : MonoBehaviour
    {
        private List<Car> _cars;

        public Car PlayerCar;
        public Hud Hud;

        public bool GameOver = false;

        void Start()
        {
            Hud.DisableHudElements();
        }

        public int CarCount
        {
            get { return _cars == null ? 0 : _cars.Count; }
        }

        public int GetMyPosition(Car car)
        {
            var progressCars = _cars.OrderByDescending(x => x.GetComponent<RaceProgressState>().RaceProgress).ToList();
            if(progressCars.Contains(car))
                return progressCars.IndexOf(car) + 1;
            return -1;
        }

        public void AddCar(Car car, bool isPlayer)
        {
            if (_cars == null)
                _cars = new List<Car>();

            if (isPlayer)
            {
                PlayerCar = car;
            }

            _cars.Add(car);
        }


        public void BeginRace()
        {
            StartCoroutine(InitiateCountdown());
        }

        private IEnumerator InitiateCountdown()
        {
            Hud.MainMessage.enabled = true;
            Hud.MainMessage.text = "3";
            yield return new WaitForSeconds(1);
            Hud.MainMessage.text = "2";
            yield return new WaitForSeconds(1);
            Hud.MainMessage.text = "1";
            yield return new WaitForSeconds(1);
            Hud.MainMessage.text = "GO!";

            Hud.EnableHudElements();

            foreach (var car in _cars)
                car.EngageControl();

            yield return new WaitForSeconds(2);
            Hud.MainMessage.enabled = false;
        }

        void Update()
        {
            if (GameOver) return;
            if (PlayerCar == null) return;

            var state = PlayerCar.GetComponent<RaceProgressState>();
            if (state.LapCount > state.NumberOfLaps)
            {
                Hud.DisableHudElements();
                Hud.GameOver(GetMyPosition(PlayerCar), CarCount);
                GameOver = true;
            }
        }

        public void RaceAgain()
        {
            SceneManager.LoadScene("Game");
        }

        public void ExitGame()
        {
            Application.Quit();
        }

    }
}
