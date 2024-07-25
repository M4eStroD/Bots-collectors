using TMPro;
using UnityEngine;

public class ViewResource : MonoBehaviour
{
    [SerializeField] private Storage _storage;
    [SerializeField] private TMP_Text _resourcesCount; 

    private void Start()
    {
        _storage.ResourceChanged += ChangeInfo;
    }

    private void ChangeInfo(int score)
    {
        _resourcesCount.text = score.ToString();
    }
}
