using UnityEngine;

public class SpatulaDetection : MonoBehaviour
{
    [SerializeField] private CaldronMerger caldronMerger;
    
    private int _nbHalfTurnsToMerge = 6;
    private int _nbHalfTurns = 0;
    
    public void InitNbHalfTurnsToMerge(int nbHalfTurnsToMerge)
    {
        _nbHalfTurnsToMerge = nbHalfTurnsToMerge;
    }
    
    public void ResetNbHalfTurns()
    {
        _nbHalfTurns = 0;
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Caldron_Spatula"))
        {
            _nbHalfTurns++;
            if (_nbHalfTurns >= _nbHalfTurnsToMerge)
            {
                caldronMerger.Merge();
                _nbHalfTurns = 0;
            }
        }
    }
}
