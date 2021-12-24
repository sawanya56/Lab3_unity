using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyPlayerBehaviour : MonoBehaviour
{
    public float speed = 1.0f;
    //public List<WeaponBehaviour> weapons = new List<WeaponBehaviour>();
    public WeaponBehaviour[] weapons;
    public int indexs;
    public int selectedWeaponIndex;
    // Start is called before the first frame update
    void Start()
    {
        References.thePlayer = gameObject;
        selectedWeaponIndex = 0;
        weapons = new WeaponBehaviour[10];
        indexs = 0;
        for (int i = 0; i < weapons.Length; i++)
        {
            weapons[i] = new WeaponBehaviour();
        }
    }

    // Update is called once per frame
    void Update()
    {
        //WASD to move
        Vector3 inputVector = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        Rigidbody ourRigidBody = GetComponent<Rigidbody>();
        ourRigidBody.velocity = inputVector * speed;

        Ray rayFromCameraToCursor = Camera.main.ScreenPointToRay(Input.mousePosition);
        Plane playerPlane = new Plane(Vector3.up, transform.position);
        playerPlane.Raycast(rayFromCameraToCursor, out float distanceFromCamera);
        Vector3 cursorPosition = rayFromCameraToCursor.GetPoint(distanceFromCamera);

        //Face the new position
        Vector3 lookAtPosition = cursorPosition;
        transform.LookAt(lookAtPosition);

        //if (weapons.Count > 0 && Input.GetButton("Fire1"))
        if (weapons.Length > 0 && Input.GetButton("Fire1"))
        {
            //Tell our weapon to fire
            weapons[selectedWeaponIndex].Fire(cursorPosition);
        }

        //weapon switching
        if (Input.GetButtonDown("Fire2"))
        {
            ChangeWeaponIndex(selectedWeaponIndex + 1);

        }
    }

    private void ChangeWeaponIndex(int index)
    {

        //Change our index
        selectedWeaponIndex = index;
        //If it's gone too far, loop back around
        //if (selectedWeaponIndex >= weapons.Count)
        if (selectedWeaponIndex >= indexs)
        {

            selectedWeaponIndex = 0;
        }

        //For each weapon in our list
        for (
            int i = 0; //Declare a variable to keep track of how many iterations we've done
                       //i < weapons.Count; 
            i < indexs;//Set a limit for how high this variable can go
            i++ //Run this after each time we iterate - increase the iteration count
        )
        {
            if (i == selectedWeaponIndex)
            {
                //If it's the one we just selected, make it visible - 'enable' it
                weapons[i].gameObject.SetActive(true);
            }
            else
            {
                //If it's not the one we just selected, hide it - disable it.
                weapons[i].gameObject.SetActive(false);
            }
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        WeaponBehaviour theirWeapon = other.GetComponentInParent<WeaponBehaviour>();
        if (theirWeapon != null)
        {
            //Add it to our internal list
            //weapons.Add(theirWeapon);
            if (weapons[indexs] == null)
            {
                weapons[indexs] = theirWeapon;
            }
            else if (weapons[indexs] != null)
            {
                indexs++;
                weapons[indexs] = theirWeapon;
            }


            //Move it to our location
            theirWeapon.transform.position = transform.position;
            theirWeapon.transform.rotation = transform.rotation;

            //Parent it to us - attach it to us, so it moves with us
            theirWeapon.transform.SetParent(transform);

            //Select it!
            //ChangeWeaponIndex(weapons.Length - 1);

        }
    }
}
