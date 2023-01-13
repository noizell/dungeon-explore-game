using NPP.DE.Core.Character;
using NPP.DE.Core.Dungeon.Generator;
using NPP.DE.Core.Services;
using NPP.DE.Misc;
using UnityEngine;
using Zenject;

public class DitherFade : MonoBehaviour
{
    //for test purpose only.
    [SerializeField]
    SceneContext _context;

    //for test purpose only.
    [SerializeField]
    MazeSettings _settings;
    MazeRecursiveDFS _maze;

    //for test purpose only.
    [SerializeField]
    BaseCharacter _player;
    PlayerControllerFactory _playerFactory
    {
        get
        {
            return PersistentServices.Current.Get<AssetLoader>().Load<PlayerControllerFactory>("Player Controller Factory", "Factory");
        }
    }

    [Inject]
    private void Construct(MazeRecursiveDFS maze)
    {
        _maze = maze;
    }

    private void Start()
    {
        GlobalServices.InstallSceneContext(_context);

        _maze.StartMaze(_settings);

        foreach (var t in GameObject.FindGameObjectsWithTag("Wall"))
        {
            foreach (MeshRenderer mat in t.GetComponents<MeshRenderer>())
            {
                Material _mat = new Material(mat.material);
            }
        }

        var player = Instantiate(_player);
        player.GetComponent<BaseCharacter>().Initialize(_playerFactory);
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log($"on trigger with {other.name}");

        if (other.tag == "Wall" && other.GetComponent<MeshRenderer>() != null)
        {
            other.GetComponent<MeshRenderer>().material.SetFloat("_Transparency", 0.3f);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Debug.Log($"on exit trigger with {other.name}");
        
        if (other.tag == "Wall" && other.GetComponent<MeshRenderer>() != null)
        {
            other.GetComponent<MeshRenderer>().material.SetFloat("_Transparency", 1f);
        }

    }
}
