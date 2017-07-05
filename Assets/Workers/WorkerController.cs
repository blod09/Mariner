using System;
using System.Collections.Generic;
using UnityEngine;

public class WorkerController : MonoBehaviour
{

    #region Editor Fields

    [SerializeField]
    Transform[] spawnPoints;
    [SerializeField]
    private GameObject _workerPrefab;
    [SerializeField]
    private Material _normalMaterial;
    [SerializeField]
    private Material _hoveredMaterial;

    #endregion


    private GameObject _currentHoveredWorker;

    private GameObject _currentSelectedWorker;

    private Vector3 _workerLastLegalPosition;

    public List<Worker> WorkerList { get; private set; }

    private static Dictionary<GameObject, Worker> _gameobjectToDataMap;

    private static WorkerTooltip _workerTooltip;

    public int TotalWorkerSalary
    {
        get
        {
            int salary = 0;
            foreach (Worker worker in WorkerList)
            {
                salary += worker.salary;
            }
            return salary;
        }
    }

    private void Awake ()

    {
        WorkerList = new List<Worker> ();
        _gameobjectToDataMap = new Dictionary<GameObject, Worker> ();
        _workerTooltip = FindObjectOfType (typeof (WorkerTooltip)) as WorkerTooltip;

        if (_workerTooltip == null)
            throw new Exception ("Error: Can't find the worker tooltip object.");
    }

    private void Start ()
    {
        for (int i = 0; i < Constants.Instance.NumberOfWorkersAtStart; i++)
        {
            AddWorker ();
        }
    }

#if UNITY_EDITOR
    private void Update ()
    {
        if (Input.GetButtonDown ("Jump") && _currentSelectedWorker != null)
        {
            Worker w = _gameobjectToDataMap[_currentSelectedWorker];
            w.AddExp (20f);
        }
    }
#endif

    #region Action Subscription

    private void OnEnable ()
    {
        MouseManager.onHover += OnHover;
        MouseManager.onClick += OnClick;
        MouseManager.onDrag += OnDrag;
        MouseManager.onRelease += OnRelease;

    }

    private void OnDisable ()
    {
        MouseManager.onHover -= OnHover;
        MouseManager.onClick -= OnClick;
        MouseManager.onDrag -= OnDrag;
        MouseManager.onRelease -= OnRelease;

    }

    #endregion

    #region Mouse Event Handlers

    private void OnHover (GameObject go)
    {

        // If we already have a worker that is being hovered over and it's not this one, then reset the material.
        if (_currentHoveredWorker != null && go != _currentHoveredWorker)
        {
            ChangeToNormalMaterial (_currentHoveredWorker);
            _currentHoveredWorker = null;
        }

        // If the object being hovered over is one of the workers this script controlls, change it's material to _hoveredMaterial.
        if (_gameobjectToDataMap.ContainsKey (go))
        {
            _currentHoveredWorker = go;
            ChangeToHoveredMaterial (_currentHoveredWorker);
        }
    }


    // Same as the hover function above, but instead of setting the material, we toggle the projector on and off.
    private void OnClick (GameObject go)
    {
        if (_currentSelectedWorker != null && go != _currentSelectedWorker)
        {
            _currentSelectedWorker.GetComponentInChildren<Projector> ().enabled = false;
            _currentSelectedWorker = null;
            _workerTooltip.Hide ();
        }

        if (_gameobjectToDataMap.ContainsKey (go))
        {
            go.GetComponentInChildren<Projector> ().enabled = true;
            _currentSelectedWorker = go;
            _workerTooltip.Show (_gameobjectToDataMap[go], go.transform);
            _workerLastLegalPosition = go.transform.position;
        }
    }

    private void OnDrag (GameObject go, Vector3 startPos, Vector3 endPos, GameObject ground)
    {
        if (_gameobjectToDataMap.ContainsKey (go) == false)
            return;
        if (_gameobjectToDataMap[go].CurrentJob != null || _currentSelectedWorker != go)
            return;

        endPos.y += 2.0f;

        // TODO: If user is trying to place worker in illegal position, give feedback!
        _currentSelectedWorker.transform.position = endPos;
        if (isWalkableGround (ground))
        {
            _workerLastLegalPosition = endPos;
        }
    }

    private void OnRelease (GameObject go, GameObject releaseArea)
    {
        if (go != _currentSelectedWorker || go == null)
            return;
        if (_gameobjectToDataMap[_currentSelectedWorker].CurrentJob != null)
            return;


        _currentSelectedWorker.transform.position = _workerLastLegalPosition;
        if (releaseArea != null)
        {
            BaseJob job = releaseArea.GetComponent<BaseJob> ();
            if (job != null)
            {
                job.TryToStartJob (_gameobjectToDataMap[_currentSelectedWorker]);
            }


        }
    }


    #endregion

    public void HireWorker ()
    {
        if (MasterManager.TimeAndScoreMan.Gold > ProgressionFormulas.CurrentWorkerCost (WorkerList.Count))
        {
            MasterManager.TimeAndScoreMan.Gold -= ProgressionFormulas.CurrentWorkerCost (WorkerList.Count);
            AddWorker ();

        }
    }

    public void AddWorker ()
    {
        if (spawnPoints.Length <= 0)
        {
            throw new Exception ("HireWorker() - No spawn points registered");
        }

        Vector3 spawnPoint = spawnPoints[UnityEngine.Random.Range (0, spawnPoints.Length)].position;
        Vector2 randomFactor = UnityEngine.Random.insideUnitCircle * 3.0f;
        spawnPoint.x += randomFactor.x;
        spawnPoint.z += randomFactor.y;


        Worker worker = new Worker (NameGenerator.GetRandomPirateName (), Constants.Instance.WorkerBaseSalary);

        GameObject go = Instantiate (_workerPrefab, spawnPoint, Quaternion.identity, gameObject.transform);
        go.name = worker.Name;

        WorkerVisualData workerVisuals = WorkerVisuals.GetVisuals (worker.type);

        go.GetComponentInChildren<Renderer> ().material = workerVisuals.normalMat;
        go.GetComponentInChildren<MeshFilter> ().mesh = workerVisuals.mesh;



        // Find the object in the hirerachy that actually holds the collider.
        go = go.GetComponentInChildren<Collider> ().gameObject;
        WorkerList.Add (worker);
        _gameobjectToDataMap.Add (go, worker);
    }

    #region Helper Functions

    private bool isWalkableGround (GameObject ground)
    {
        if (ground == null)
            return false;

        MapArea area = ground.GetComponent<MapArea> ();
        if (area == null)
            return false;
        if (area.Type != TerrainType.Walkable)
            return false;

        return true;
    }

    public static void HideWorker (Worker w)
    {
        foreach (KeyValuePair<GameObject, Worker> worker in _gameobjectToDataMap)
        {
            if (worker.Value == w)
            {
                worker.Key.SetActive (false);
                _workerTooltip.Hide ();

            }
        }
    }
    public static void ShowWorker (Worker w)
    {
        foreach (KeyValuePair<GameObject, Worker> worker in _gameobjectToDataMap)
        {
            if (worker.Value == w)
                worker.Key.SetActive (true);
        }
    }

    private void ChangeToNormalMaterial (GameObject workerGO)
    {
        Worker worker;
        _gameobjectToDataMap.TryGetValue (workerGO, out worker);
        WorkerVisualData workerVisualData = WorkerVisuals.GetVisuals (worker.type);
        workerGO.GetComponentInChildren<Renderer> ().material = workerVisualData.normalMat;
    }

    private void ChangeToHoveredMaterial (GameObject workerGO)
    {
        Worker worker;
        _gameobjectToDataMap.TryGetValue (workerGO, out worker);
        WorkerVisualData workerVisualData = WorkerVisuals.GetVisuals (worker.type);
        workerGO.GetComponentInChildren<Renderer> ().material = workerVisualData.hoveredMat;
    }

    public int GetCurrentNumberOfWorkers ()
    {
        if (WorkerList == null)
            return 0;
        return WorkerList.Count;
    }

    #endregion





}
