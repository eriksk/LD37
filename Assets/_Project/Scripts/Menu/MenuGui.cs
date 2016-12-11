using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Assets._Project.Scripts.Menu
{
    public class MenuGui : MonoBehaviour
    {
        public void StartGame()
        {
            SceneManager.LoadScene("Game");
        }
    }
}
