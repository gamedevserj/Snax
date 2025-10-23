using UnityEngine;

public class LoadingIndicator : MonoBehaviour
{

    [SerializeField] private GameObject _loadingIndicator;

    public void OnZoneLoad(bool show)
    {
        _loadingIndicator.SetActive(show);
    }

    private void OnEnable()
    {
        ZoneStreamer.OnZoneLoad += OnZoneLoad;
    }

    private void OnDisable()
    {
        ZoneStreamer.OnZoneLoad -= OnZoneLoad;
    }

}
