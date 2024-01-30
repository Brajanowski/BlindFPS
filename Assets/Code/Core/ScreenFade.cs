using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Core
{
    public class ScreenFade : SingletonMonoBehaviour<ScreenFade>
    {
        [SerializeField]
        private Image _image;

        protected override void Awake()
        {
            base.Awake();
            _image.gameObject.SetActive(false);
        }

        public IEnumerator FadeIn(float duration = 0.3f)
        {
            _image.gameObject.SetActive(true);
            yield return _image.DOFade(1.0f, duration).WaitForCompletion();
        }

        public IEnumerator FadeOut(float duration = 0.3f)
        {
            if (!_image.gameObject.activeSelf)
            {
                yield break;
            }
            
            yield return _image.DOFade(0.0f, duration).WaitForCompletion();
            _image.gameObject.SetActive(false);
        }
    }
}