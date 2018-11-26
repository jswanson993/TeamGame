using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using System.Windows.Forms;

public class Player_Controller : MonoBehaviour {

    [Header("Player Movement")] [Space(15)]
    public float MouseSensitivity;
    public float MoveSpeed;
    public float JumpForce;
    public float groundCheckDistance;
    public float wallRunSnapDistance;
    public float MaxMoveSpeed;
    public float maxAirSpeed;
    static bool playerCanJump;
    public bool hasGun;
    public Animator animator;
    public Rigidbody Rigid;
    private Vector2 rotation = new Vector2(0, 0);
    public Transform shotPoint;
    private Vector3 wallRunVector;
    private Rigidbody p_rigidbody;
    public GameObject FPGUN;
    public GameObject GUN;
    Vector3 VShift;
    Vector3 HShift;
    public bool isWallLeaping;
    public bool is3D = true;

    public enum JumpState {Grounded, InAir, Wallrunning};
    public JumpState jState;
    public float stickToGroundForce = 100;

    private Grapple playerGrapple;
    public float time;
    public Texture2D mouseTex;
    private float recoil;
    // Use this for initialization
    void Start () {
        rotation =Camera.main.transform.eulerAngles;
        playerGrapple = GetComponent<Grapple>();
        p_rigidbody = GetComponent<Rigidbody>();
        jState = JumpState.Grounded;
        Rigid = GetComponent<Rigidbody>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        playerCanJump = true;
        FPGUN = GameObject.Find("FP_Gun");
        GUN = GameObject.Find("Gun");
        time = 0;
        getPickup("Gun Pickup");

        if (hasGun) {
            if (is3D) {
                GameObject.Find("FP_Gun").SetActive(true);
            } else {
                GameObject.Find("Gun").SetActive(true);
            }
        } else {
            if (is3D) {
                GameObject.Find("FP_Gun").SetActive(false);
            } else {
                GameObject.Find("Gun").SetActive(false);
            }
        }

        if (!is3D) {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.Confined;
        }

        //shotPoint = transform.Find("Camera/FP_Gun/Gun/FirePoint");
        //is3D = true;


        VShift = Vector3.zero;
        HShift = Vector3.zero;
    }
	
    public void refreshIs3D()
    {
        if (!is3D)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.SetCursor(mouseTex, new Vector2(mouseTex.height/2,mouseTex.width/2), CursorMode.Auto);
            //SendKeys.Send("{ESCAPE}");
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }
	// Update is called once per frame
	void Update () {

        
        processButtonInput();

        /*
        if(jState == JumpState.Wallrunning)
        {
            wallRun();
        }
        else
        {
            groundTest();
        }
        */
        
        groundTest();
        if (!groundTest())
        {
            if (Rigid.velocity.y < 0) {
                time += Time.deltaTime;
            }
        }
        else {
            if (time >= 2 && time < 3) {
                GetComponent<PlayerHealth>().takeDamage(25);
                Debug.Log(time);

            } else if( time >= 3 && time >= 4) {
                GetComponent<PlayerHealth>().takeDamage(100);
                Debug.Log(time);
            }

            time = 0;
            
        }

        if (jState == JumpState.Wallrunning)
        {
            time = 0;
        }
        


        //Debug.Log(jState.ToString());
	}

    void FixedUpdate()
    {
        if (is3D && !playerGrapple.fired) { processFPMovementInput(); }

        else if (!playerGrapple.fired)
        { process2DMovementInput();
            aim2d();
        }
        if (is3D)
        {
            TestRotation();
        }
    }

    private void wallRun()
    {
        time = 0;
        RaycastHit filteredHit;
        Vector3 rayDirection;

        RaycastHit hitR;

        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.right, out hitR, wallRunSnapDistance))
        {
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.right) * hitR.distance, Color.yellow);
            Debug.Log("RightSide WallRunHit");
            
        }

        RaycastHit hitL;
        if (Physics.Raycast(Camera.main.transform.position, -Camera.main.transform.right, out hitL, wallRunSnapDistance))
        {
            Debug.DrawRay(transform.position, transform.TransformDirection(-Vector3.right) * hitR.distance, Color.yellow);
            Debug.Log("LeftSide WallRunHit");
            
        }

        if(hitL.distance > hitR.distance)
        {
            filteredHit = hitR;
            rayDirection = Camera.main.transform.right;
        }
        else
        {
            filteredHit = hitL;
            rayDirection = -Camera.main.transform.right;
        }

        if (Physics.Raycast(Camera.main.transform.position, rayDirection, wallRunSnapDistance))
        {
            

            //Vector3 surfaceParallel = Vector3.ProjectOnPlane(rayDirection, filteredHit.normal);
            Vector3 surfaceParallel = Quaternion.Euler(0, -45, 0) * filteredHit.normal;
            Debug.DrawRay(Camera.main.transform.position, surfaceParallel * 20, Color.red, 1f);

            if (filteredHit.distance > wallRunSnapDistance)
            {
                jState = JumpState.InAir;
                p_rigidbody.useGravity = true;
            }

        }



        
    }

    private void processButtonInput()
    {
        if (Input.GetButtonDown("Fire1") && hasGun)
        {
            if (is3D) {
                shoot();
            } else {
                shoot2D();
            }
        }



        if (Input.GetButtonDown("Jump") && groundTest())
        {
            Vector3 haltVel = new Vector3(p_rigidbody.velocity.x, 0, p_rigidbody.velocity.z);
            p_rigidbody.velocity = haltVel;
            if (is3D)
            {
                Rigid.AddForce(Vector3.up * JumpForce);
            }
            else
            {
                Rigid.AddForce(Vector3.up * JumpForce * 1.3f);
            }
            jState = JumpState.InAir;
            animator.SetBool("Jump", !groundTest());
            Debug.Log("Jump");
        }

        /*
        if (Input.GetButtonDown("Jump") && jState == JumpState.InAir && CanWallRun() && jState != JumpState.Wallrunning)
        {
            jState = JumpState.Wallrunning;
            //Debug.Log(p_rigidbody.velocity.to)
            p_rigidbody.velocity = new Vector3(p_rigidbody.velocity.x, 0, p_rigidbody.velocity.z);
            //p_rigidbody.useGravity = false;
        }
        */

    }

    private void aim2d() {
       Vector3 mouse_pos = Input.mousePosition;
        mouse_pos.z = -20;
        Vector3 object_pos = Camera.main.WorldToScreenPoint(GUN.transform.position);
        mouse_pos.x = mouse_pos.x - object_pos.x;
        mouse_pos.y = mouse_pos.y - object_pos.y;
        float angle = Mathf.Atan2(mouse_pos.y, mouse_pos.x) * Mathf.Rad2Deg;
        if (mouse_pos.x > this.transform.position.x + ((float)Screen.width * .04)) {
            this.transform.rotation = Quaternion.Euler(0, 0, 0);
        } else if(mouse_pos.x < this.transform.position.x - ((float)Screen.width * .04)) {
            this.transform.rotation = Quaternion.Euler(0, 180, 0);
        }
       GUN.transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    private bool CanWallRun()
    {
        RaycastHit hitR;
        if(Physics.Raycast(Camera.main.transform.position, Camera.main.transform.right, out hitR, wallRunSnapDistance))
        {
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.right) * hitR.distance, Color.yellow);
            Debug.Log("RightSide WallRunHit");
            return true;
        }

        RaycastHit hitL;
        if (Physics.Raycast(Camera.main.transform.position, -Camera.main.transform.right, out hitL, wallRunSnapDistance))
        {
            Debug.DrawRay(transform.position, transform.TransformDirection(-Vector3.right) * hitR.distance, Color.yellow);
            Debug.Log("LeftSide WallRunHit");
            return true;
        }

        return false;
    }

    private void setRecoil(float maxXRecoil, float recoilSpeed){
        float maxYRecoil = UnityEngine.Random.Range(-maxXRecoil, maxXRecoil);

        if (recoil > 0f)
        {

            Quaternion maxRecoil = Quaternion.Euler(-maxXRecoil, maxYRecoil, 0f);
            // Dampen towards the target rotation
            GUN.transform.localRotation = Quaternion.Slerp(GUN.transform.localRotation, maxRecoil, Time.deltaTime * recoilSpeed);
            recoil -= Time.deltaTime;
        }
        else
        {
            recoil = 0f;
            // Dampen towards the target rotation
            GUN.transform.localRotation = Quaternion.Slerp(GUN.transform.localRotation, Quaternion.identity, Time.deltaTime * recoilSpeed / 2);
        }

    }

    private void shoot()
    {
       
        Vector3 endpoint;
        RaycastHit hit;
        // Does the ray intersect any objects excluding the player layer
        if (Physics.Raycast(Camera.main.transform.position + Camera.main.transform.forward, transform.TransformDirection(Camera.main.transform.forward), out hit, Mathf.Infinity))
        {
            Debug.DrawRay(Camera.main.transform.position, (Camera.main.transform.forward) * hit.distance, Color.yellow);
            Debug.Log("Did Hit");
            endpoint = hit.point;
            if (hit.collider.isTrigger) {
                var hitReciever = hit.collider.gameObject.GetComponent<HitTrigger>();
                if (hitReciever) {
                    hitReciever.InvokeTrigger();
                }
                

            }
            if (hit.collider.GetComponent<Killable>())
            {
                hit.collider.GetComponent<Killable>().TakeDamage(5);
            }
            recoil = UnityEngine.Random.Range(0, 100);
            float maxXRecoil = 20;
            float recoilSpeed = 20;
            setRecoil(maxXRecoil, recoilSpeed);
            
            

            /*
            GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
            cube.transform.position = endpoint;
            Destroy(cube, 1);
            */

        }
        else
        {
            Debug.DrawRay(Camera.main.transform.position, transform.TransformDirection(Camera.main.transform.forward) * 1000, Color.white);
            Debug.Log("Did not Hit");
            endpoint = Camera.main.transform.position + Camera.main.transform.forward * 1000;
        }
        
        transform.GetChild(0).GetComponent<LineRenderer>().enabled = true;
        transform.GetChild(0).GetComponent<LineRenderer>().SetPositions(new Vector3[] {shotPoint.position, endpoint});
        Invoke("RemoveTrail", .06f);
        /*
        GameObject cube2 = GameObject.CreatePrimitive(PrimitiveType.Cube);
        cube2.transform.position = endpoint;
        Destroy(cube2, 1);
        */

    }

    private void shoot2D() {
        int layerMask = 1 << 11;
        layerMask = ~layerMask;
        Vector3 endpoint;
        RaycastHit hit;
        if(Physics.Raycast(shotPoint.position, shotPoint.forward, out hit, Mathf.Infinity, layerMask)) {
            Debug.DrawRay(shotPoint.position, shotPoint.forward * hit.distance, Color.red, 1f);
            endpoint = hit.point;
            Debug.Log("RayHit");
            Debug.Log(hit.transform.name);
            if (hit.collider.isTrigger) {
                var hitReciever = hit.collider.gameObject.GetComponent<HitTrigger>();
                if (hitReciever) {
                    hitReciever.InvokeTrigger();
                }

            }
            if (hit.collider.GetComponent<Killable>())
            {
                hit.collider.GetComponent<Killable>().TakeDamage(5);
            }

        } else {
            Debug.DrawRay(shotPoint.position, transform.TransformDirection(shotPoint.forward) * 1000, Color.white);
            endpoint = (shotPoint.forward) * 1000;
            Debug.Log("RayMissed");
        }

        transform.GetChild(0).GetComponent<LineRenderer>().enabled = true;
        transform.GetChild(0).GetComponent<LineRenderer>().SetPositions(new Vector3[] { shotPoint.position, endpoint });
        Invoke("RemoveTrail", .06f);
        ///*
    }

   

    private void RemoveTrail()
    {
        transform.GetChild(0).GetComponent<LineRenderer>().enabled = false;
    }

    private void processFPMovementInput()
    {
        
        

        if (jState == JumpState.Grounded)
        {
            VShift = Vector3.Normalize(Vector3.Scale(Camera.main.transform.forward, new Vector3(1, 0, 1))) * Input.GetAxis("Vertical") * MoveSpeed;
            HShift = Vector3.Normalize(Vector3.Scale(Camera.main.transform.right, new Vector3(1, 0, 1))) * Input.GetAxis("Horizontal") * MoveSpeed;


            //Rigid.MovePosition(transform.position + VShift + HShift);
            //Rigid.AddForce(Vector3.ClampMagnitude((transform.position + VShift + HShift) - transform.position, 4f) * MoveSpeed);
            //Rigid.velocity = new Vector3(Vector3.ClampMagnitude(Rigid.velocity, MaxMoveSpeed).x, Rigid.velocity.y, Vector3.ClampMagnitude(Rigid.velocity, MaxMoveSpeed).z);
            Vector3 moveVec;
            if (!isWallLeaping)
            {
                moveVec = (Vector3.ClampMagnitude((transform.position + VShift + HShift) - transform.position, 4f) * MoveSpeed);
            }
            else
            {
                moveVec = (Vector3.ClampMagnitude((transform.position + VShift + HShift) - transform.position, Mathf.Infinity) * MoveSpeed);
            }

            Rigid.velocity = new Vector3(moveVec.x, Rigid.velocity.y, moveVec.z);
            if (!Input.GetButton("Jump"))
            {
                RaycastHit hit;
                if (Physics.Raycast(transform.position, Vector3.down, out hit, 3F))
                {
                    Rigid.AddForce(-hit.normal * stickToGroundForce);
                }
                else
                {
                    //Rigid.AddForce(Vector3.down * stickToGroundForce);
                }
            }

        }

        else if (jState == JumpState.InAir || jState == JumpState.Wallrunning)
        {
            /*
            VShift = Vector3.Normalize(Vector3.Scale(Camera.main.transform.forward, new Vector3(1, 0, 1))) * Input.GetAxis("Vertical") * MoveSpeed;
            HShift = Vector3.Normalize(Vector3.Scale(Camera.main.transform.right, new Vector3(1, 0, 1))) * Input.GetAxis("Horizontal") * MoveSpeed;
            //if (Math.Abs(Input.GetAxis("Vertical")) > .01f) { VShift = Vector3.Normalize(Vector3.Scale(Camera.main.transform.forward, new Vector3(1, 0, 1))) * Input.GetAxis("Vertical") * MoveSpeed/2; }
            //if(Math.Abs(Input.GetAxis("Horizontal")) > .01f) { HShift = Vector3.Normalize(Vector3.Scale(Camera.main.transform.right, new Vector3(1, 0, 1))) * Input.GetAxis("Horizontal") * MoveSpeed/2; }
            Vector3 moveVec = (Vector3.ClampMagnitude((transform.position + VShift + HShift) - transform.position, 100f) * MoveSpeed);
            Debug.DrawRay(transform.position, moveVec, Color.red);
            
            Rigid.velocity = Rigid.velocity + new Vector3(moveVec.x, 0, moveVec.z);
            Rigid.velocity = new Vector3(Mathf.Clamp(Rigid.velocity.x, -maxAirSpeed, maxAirSpeed), Rigid.velocity.y, Mathf.Clamp(Rigid.velocity.z, -maxAirSpeed, maxAirSpeed));

            //Vector3 TempVec = Vector3.Normalize(new Vector3(Camera.main.transform.forward.x, 0, Camera.main.transform.forward.y)) * Rigid.velocity.magnitude;
            //Debug.DrawRay(transform.position, TempVec);
            //Rigid.velocity = new Vector3(TempVec.x, Rigid.velocity.y, TempVec.z);
            */

            VShift = Vector3.Normalize(Vector3.Scale(Camera.main.transform.forward, new Vector3(1, 0, 1))) * Input.GetAxis("Vertical") * MoveSpeed;
            HShift = Vector3.Normalize(Vector3.Scale(Camera.main.transform.right, new Vector3(1, 0, 1))) * Input.GetAxis("Horizontal") * MoveSpeed;


            //Rigid.MovePosition(transform.position + VShift + HShift);
            //Rigid.AddForce(Vector3.ClampMagnitude((transform.position + VShift + HShift) - transform.position, 4f) * MoveSpeed);
            //Rigid.velocity = new Vector3(Vector3.ClampMagnitude(Rigid.velocity, MaxMoveSpeed).x, Rigid.velocity.y, Vector3.ClampMagnitude(Rigid.velocity, MaxMoveSpeed).z);
            Vector3 moveVec;
            if (!isWallLeaping)
            {
                moveVec = (Vector3.ClampMagnitude((transform.position + VShift + HShift) - transform.position, 4f) * maxAirSpeed);
            }
            else
            {
                moveVec = (Vector3.ClampMagnitude((transform.position + VShift + HShift) - transform.position, Mathf.Infinity) * maxAirSpeed);
            }
            Vector3 tVel = Vector3.ClampMagnitude(Vector3.ClampMagnitude(new Vector3(moveVec.x, 0, moveVec.z), maxAirSpeed), maxAirSpeed);
            //Rigid.velocity = Rigid.velocity +new Vector3(tVel.x, 0, tVel.z);
            if(Vector3.Magnitude(new Vector3(Rigid.velocity.x, 0, Rigid.velocity.z)) < maxAirSpeed)
                Rigid.AddForce(tVel * 10);

            if(jState == JumpState.Wallrunning && Rigid.velocity.y > 0f)
            {
                Rigid.velocity = Vector3.Scale(Rigid.velocity , new Vector3(1, 0, 1));
            }

        }

        
        //Debug.Log(moveVec.ToString());
        

        //Invoke("groundTest", .3f);



    }

    private void process2DMovementInput()
    {
        if (jState != JumpState.Wallrunning)
        {
            float Shift = Input.GetAxis("Horizontal") * MoveSpeed;
          
            //Changes the rotation of the player based on the direction they are moving
            //if (Shift > 0) {
            //    this.transform.rotation = Quaternion.Euler(0, 0, 0);
                //shotPoint.transform.rotation = Quaternion.Euler(0, 90, 0);
            //} else if (Shift < 0) {
            //   this.transform.rotation = Quaternion.Euler(0, 180, 0);
                //shotPoint.transform.rotation = Quaternion.Euler(0, -90, 0);
            //}

            
            //Debug.Log(Shift.ToString());
            Rigid.velocity = new Vector3(Shift, Rigid.velocity.y, 0);
            animator.SetFloat("Speed", Shift);
            if (!Input.GetButton("Jump") && jState == JumpState.Grounded)
            {
                
                RaycastHit hit;
                if (Physics.Raycast(transform.position, Vector3.down, out hit, 3F))
                {
                    Rigid.AddForce(-hit.normal * stickToGroundForce);
                }
                else
                {
                    //Rigid.AddForce(Vector3.down * stickToGroundForce);
                }
            }

        }
    }

    private bool groundTest()
    {
        if (!playerCanJump)
        {
            return false;
        }
        else
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, transform.TransformDirection(-transform.up), out hit, Mathf.Infinity))
            {
                Debug.DrawRay(transform.position, transform.TransformDirection(-transform.up) * hit.distance, Color.yellow);
                //Debug.Log("Did Hit");
                if(hit.distance < groundCheckDistance)
                {
                    jState = JumpState.Grounded;
                    isWallLeaping = false;
                    return true;
                }
                else
                {
                    if(jState != JumpState.Wallrunning)
                        jState = JumpState.InAir;
                    return false;
                }
            }
            return false;
        }
    }

    private void TestRotation()
    {
        rotation.x += -Input.GetAxis("Mouse Y") * MouseSensitivity;
        rotation.x = Mathf.Clamp(rotation.x, -90, 90);
        rotation.y += Input.GetAxis("Mouse X") * MouseSensitivity;
        //rotation.y = Mathf.Clamp(rotation.y, 0, 90);
        Camera.main.transform.eulerAngles = (Vector2)rotation;
    }

    public void getPickup(System.String pickup) {
        if (pickup == "Gun Pickup") {
            hasGun = true;
            //GameObject gun = GameObject.Find("Gun");
            if (!is3D) {
                GUN.SetActive(true);
            } else {
                FPGUN.SetActive(true);
            }
        } else if (pickup == "Grapple Pickup") {
            GetComponent<Grapple>().enableGrapple();
        }


            
    }


    public void cameraTilt(Transform wall)
    {
        //if(transform)
    }
}
