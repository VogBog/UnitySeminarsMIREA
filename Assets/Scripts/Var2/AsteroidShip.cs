using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Var2
{
    public class AsteroidShip : MonoBehaviour
    {
        public float Speed;

        [SerializeField] private TMP_Text _healthTxt;
        [SerializeField] private int _health;

        private void Start()
        {
            _healthTxt.text = _health.ToString();
        }

        private void Update()
        {
            var y = Input.GetAxis("Vertical");
            
            var pos = transform.position;
            pos += Time.deltaTime * Speed * y * Vector3.up;
            pos.y = Mathf.Clamp(pos.y, -4.5f, 4.5f);
            transform.position = pos;
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            _health--;
            _healthTxt.text = _health.ToString();

            if (_health == 0)
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
    }
}