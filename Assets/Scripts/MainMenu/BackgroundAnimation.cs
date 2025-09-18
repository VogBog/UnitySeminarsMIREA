using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace MainMenu
{
    public class BackgroundAnimation : MonoBehaviour
    {
        [SerializeField] private Image _backgroundImage;
        [SerializeField] private Color[] _colors;
        [SerializeField] private float _animationTime;

        private int _colorIndex = 0;

        private void Start()
        {
            StartCoroutine(CycleRoutine());
        }

        private IEnumerator CycleRoutine()
        {
            while (true)
            {
                _backgroundImage.DOColor(_colors[_colorIndex], _animationTime);

                yield return new WaitForSeconds(_animationTime);
                
                _colorIndex = (_colorIndex + 1) % _colors.Length;
            }
        }
    }
}