using NPP.DE.Core.Dungeon.Generator;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class DitherFade : MonoBehaviour
{
    [SerializeField]
    MazeSettings _settings;

    MazeRecursiveDFS _maze;

    [Inject]
    private void Construct(MazeRecursiveDFS maze)
    {
        _maze = maze;
    }

    private void Start()
    {
        _maze.StartMaze(_settings);

        foreach (var t in GameObject.FindGameObjectsWithTag("Wall"))
        {
            foreach (MeshRenderer mat in t.GetComponents<MeshRenderer>())
            {
                Material _mat = new Material(mat.material);
            }
        }
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
