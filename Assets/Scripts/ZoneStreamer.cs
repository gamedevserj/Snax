using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

public class ZoneStreamer : MonoBehaviour
{

    public delegate void ZoneLoad(bool start);
    public static event ZoneLoad OnZoneLoad;

    [SerializeField] private bool _simulateSlowLoad;
    [SerializeField] private int _delayMilliseconds = 1000;
    [SerializeField] private ZoneSettings[] _zones;

    private AsyncOperationHandle<SceneInstance> _loadHandle;
    private CancellationTokenSource _source;
    private bool _isLoading;

    private void OnPlayerMoved(Vector3 position)
    {
        for (int i = 0; i < _zones.Length; i++)
        {
            var zone = _zones[i];
            if (Vector3.Distance(position, zone.Center) <= zone.EnterRadius && !zone.HasPlayer)
            {
                EnterZone(zone);
            }
            else if (Vector3.Distance(position, zone.Center) >= zone.ExitRadius && zone.HasPlayer)
            {
                ExitZone(zone);
            }
        }
    }

    private async Task EnterZone(ZoneSettings zone)
    {
        zone.HasPlayer = true;
        if (_isLoading)
            return;

        if (!_loadHandle.IsValid() || _loadHandle.IsDone)
        {
            Debug.Log($"Started loading {zone.Name}");
            _source = new CancellationTokenSource();
            OnZoneLoad?.Invoke(true);
            if (_simulateSlowLoad)
            {
                _isLoading = true;
                await Task.Delay(_delayMilliseconds, _source.Token);
            }

            _loadHandle = Addressables.LoadSceneAsync(zone.Scene, LoadSceneMode.Additive);
            _loadHandle.Completed += OnFinishedLoading;
        }
    }

    private void ExitZone(ZoneSettings zone)
    {
        zone.HasPlayer = false;
        OnZoneLoad?.Invoke(false);

        if (_isLoading)
        {
            _source.Cancel();
            _source.Dispose();
            _isLoading = false;
            Debug.Log($"Cancelled loading {zone.Name}");
        }
        else if (_loadHandle.IsValid() && _loadHandle.IsDone)
        {
            Debug.Log($"Started unloading {zone.Name}");
            var unloadhandle = Addressables.UnloadSceneAsync(_loadHandle);
            unloadhandle.Completed += (handle) => {
                Debug.Log($"Finished unloading {zone.Name}");
            };
        }
    }

    private void OnFinishedLoading(AsyncOperationHandle<SceneInstance> handle)
    {
        OnZoneLoad?.Invoke(false);
        _isLoading = false;
        Debug.Log($"Finished loading {handle.Result.Scene.name}");
    }

    private void OnEnable()
    {
        PuzzleController.OnPlayerMoved += OnPlayerMoved;
    }

    private void OnDisable()
    {
        PuzzleController.OnPlayerMoved -= OnPlayerMoved;
    }

    private void OnDrawGizmos()
    {
        if (_zones == null)
            return;

        for (int i = 0; i < _zones.Length; i++)
        {
            var zone = _zones[i];
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(zone.Center, zone.EnterRadius);
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(zone.Center, zone.ExitRadius);
        }
    }

}

[System.Serializable]
public class ZoneSettings
{

    [SerializeField] string _name;
    [SerializeField] private Vector3 _center;
    [SerializeField] private float _enterRadius;
    [SerializeField] private float _exitRadius;
    [SerializeField] private AssetReference _scene;

    public string Name => _name;
    public Vector3 Center => _center;
    public float EnterRadius => _enterRadius;
    public float ExitRadius => _exitRadius;
    public AssetReference Scene => _scene;

    public bool HasPlayer { get; set; }

}
