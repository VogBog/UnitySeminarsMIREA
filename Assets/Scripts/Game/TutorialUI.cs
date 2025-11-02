using System;
using System.Collections;
using TMPro;
using UnityEngine;

namespace Game
{
    public class TutorialUI : MonoBehaviour
    {
        [SerializeField] private TMP_Text _text;
        [SerializeField] private PlayerMovement _movement;

        private IEnumerator Start()
        {
            FindFirstObjectByType<PlayersSpawner>().End += OnGameEnded;
            
            yield return new WaitForSeconds(0.4f);

            try
            {
                _text.text = $"{PlayerController.GetPlayerCodes(_movement.Controller.PlayerIndex)} for move";
            }
            catch (Exception)
            {
                if(_text != null)
                    _text.gameObject.SetActive(false);
            }

            yield return new WaitForSeconds(2f);
            
            _text.gameObject.SetActive(false);
        }

        private void OnGameEnded()
        {
            _text.text = "THE END";
            _text.gameObject.SetActive(true);
        }
    }
}