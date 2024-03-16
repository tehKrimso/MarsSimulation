using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorkingZoneDivider : MonoBehaviour
{
    public float Height;
    public float Width;

    public GameObject Floor;
    
    public float Step; // different step for horizontal and vertical?

    public Vector3 StartingPoint; //center of plane


    private List<Vector3> _referencePoints;

    //add different types of startegies to divide from where starting point
    //center/left top corner/right top corner
    
    //describe working area by array of points where outer line curve?

    public List<Vector3> GetReferencePointsFromWorkingZone()
    {
        if (_referencePoints != null)
            return _referencePoints;
        
        _referencePoints = new List<Vector3>();

        var height = Floor.transform.localScale.z * 10;
        var width = Floor.transform.localScale.x * 10; //default plane with scale 1 is 10 meters

        int topBound = (int)(StartingPoint.z + height / 2);
        int bottomBound = (int)(StartingPoint.z - height / 2);
        int leftBound = (int)(StartingPoint.x - width / 2);
        int rightBound = (int)(StartingPoint.x + width / 2);

        for (float i = leftBound; i < rightBound; i += Step)
        {
            for (float j = bottomBound; j < topBound; j += Step)
            {
                _referencePoints.Add(new Vector3(i,0,j));
            }
        }

        return _referencePoints;
    }
}
