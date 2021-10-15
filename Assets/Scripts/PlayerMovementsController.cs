using Mirror;
using UnityEngine;
using UnityEngine.AI;

public class PlayerMovementsController : NetworkBehaviour
{
    [SerializeField] private NavMeshAgent navMeshAgent;
    private Camera _camera;
    
    #region Client

    public override void OnStartAuthority()
    {
        _camera = Camera.main;
    }

    [ClientCallback]
    private void Update()
    {
        if (!hasAuthority) return;
        if (!Input.GetMouseButtonDown(1)) return;
        var ray = _camera.ScreenPointToRay(Input.mousePosition);
        if (!Physics.Raycast(ray, out var raycastHit, Mathf.Infinity)) return;
        CmdMove(raycastHit.point);
    }

    #endregion

    #region Server

    [Command]
    private void CmdMove(Vector3 position)
    {
        if (!NavMesh.SamplePosition(position, out var navMeshHit, 1f, NavMesh.AllAreas)) return;
        navMeshAgent.SetDestination(navMeshHit.position);
    }

    #endregion
}
