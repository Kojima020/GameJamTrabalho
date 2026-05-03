using UnityEngine;

public class GrapplingGun : MonoBehaviour
{
    private LineRenderer lr;
    private Vector3 grapplePoint;

    //Ver como o Kojima fez com layers no codigo de movimento - dia 3
    public LayerMask whatIsGrappleable;

    //Lembrar de mudar isso depois para private, ver como o Kojima faz
    private PlayerMovement pm;
    public Transform gunTip;
    public Transform parentCamera;
    public Transform player;

    private SpringJoint joint;
   
    public float overshootYAxis;

    private float maxDistance = 30f;
    private bool grappling;

    private void Awake() 
    {
        pm = player.GetComponent<PlayerMovement>();
        lr = GetComponent<LineRenderer>();
    }

    private void Update() 
    {

        if (Input.GetMouseButtonDown(0)) 
        {
            StartGrapple();
        }
        else if (grappling && Input.GetMouseButtonUp(0)) {
            StopGrapple();
        }

        if (Input.GetMouseButton(1) && grappling) {
            print("OLHA EU AI");
            ExecuteGrapple();
        }

    }

    private void LateUpdate() 
    {
        DrawRope();
    }

    void StartGrapple() 
    {
        RaycastHit hit;
        if (Physics.Raycast(parentCamera.position, parentCamera.forward,out hit, maxDistance)) 
        {
            grapplePoint = hit.point;
            joint = player.gameObject.AddComponent<SpringJoint>();
            joint.autoConfigureConnectedAnchor = false;
            joint.connectedAnchor = grapplePoint;

            float distanceFromPoint = Vector3.Distance(player.position, grapplePoint);

            joint.maxDistance = distanceFromPoint * 0.8f;
            joint.minDistance = distanceFromPoint * 0.25f;

            joint.spring = 4.5f;
            joint.damper = 7f;
            joint.massScale = 4.5f;

            lr.positionCount = 2;

            grappling = true;
        }

    }

    void ExecuteGrapple()     
    {
        if (!grappling) return;

        Vector3 lowestPoint = new Vector3(transform.position.x, transform.position.y - 1f, transform.position.z);

        float grapplePointRelativeYPos = grapplePoint.y - lowestPoint.y;
        float highestPointOnArc = grapplePointRelativeYPos + overshootYAxis;

        if (grapplePointRelativeYPos < 0) highestPointOnArc = overshootYAxis;

        pm.JumpToPosition(grapplePoint, highestPointOnArc);
    }

    void DrawRope() 
    {
        if (!joint) return;

        lr.SetPosition(0, gunTip.position);
        lr.SetPosition(1, grapplePoint);

    }

    void StopGrapple() 
    {
        lr.positionCount = 0;
        Destroy(joint);
        grappling = false;
    }

}
