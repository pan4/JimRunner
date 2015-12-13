using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

namespace JimRunner
{
    public class ScoreView : MonoBehaviour
    {

        [SerializeField]
        Text _scoreText;

        float _score;
        
        private void Start()
        {
            _score = 0f;
        } 

        void Update()
        {
            _score += 10 * Time.deltaTime; 
            _scoreText.text = Math.Round(_score, 0).ToString();
        }
    }
}
