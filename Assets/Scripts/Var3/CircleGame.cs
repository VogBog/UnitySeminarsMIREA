using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Var3
{
    public class CircleGame : MonoBehaviour
    {
        [SerializeField] private Image _circle;
        [SerializeField] private float _speed;
        [SerializeField] private TMP_Text _attemptsText;

        private float _size = 0f;
        
        [SerializeField] private int _attempts = 3;

        [SerializeField] private Image _menu;

        private bool _menuOpened = false;

        private void Start()
        {
            Time.timeScale = 1;
            _attemptsText.text = _attempts.ToString();
        }

        public void QuitGame()
        {
            SceneManager.LoadScene("CircleMainMenu");
        }

        private void Update()
        {
            _size += _speed * Time.deltaTime;
            _circle.transform.localScale = Vector3.one * _size;

            if (_size > 1.2f)
            {
                SubAttempt();
            }

            if (Input.GetMouseButtonDown(0))
            {
                if (Mathf.Abs(_size - 1f) > .05f)
                {
                    SubAttempt();
                }

                _size = 0f;
            }

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                _menuOpened = !_menuOpened;
                Time.timeScale = _menuOpened ? 0 : 1;
                _menu.gameObject.SetActive(_menuOpened);
            }
        }

        private void SubAttempt()
        {
            _attempts--;
            _attemptsText.text = _attempts.ToString();
            if (_attempts == 0)
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
    }
}