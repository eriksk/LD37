using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace Assets._Project.Scripts.Menu
{
    public class FadeOutImage : MonoBehaviour
    {
        public float Duration = 2f;

        void Start()
        {
            var fader = GetComponent<Image>();
            if (fader == null) return;

            StartCoroutine(FadeOut(fader));
        }

        private IEnumerator FadeOut(Image fade)
        {
            var current = 0f;
            while(current < Duration)
            {
                var color = fade.color;
                color.a = 1f - (current / Duration);
                fade.color = color;
                yield return new WaitForEndOfFrame();
                current += Time.deltaTime;
            }
            Destroy(gameObject);
        }
    }
}
