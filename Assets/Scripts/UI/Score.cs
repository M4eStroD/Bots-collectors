using TMPro;
using UnityEngine;

public class Score : MonoBehaviour
{
    [SerializeField] private Base _base;
    [SerializeField] private TMP_Text _score; 

    private void Start()
    {
        _base.ScoreChanged += UpdateScore;
    }

    private void UpdateScore(int score)
    {
        _score.text = score.ToString();
    }
}
