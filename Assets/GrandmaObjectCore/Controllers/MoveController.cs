using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Grandma.Core
{
    [RequireComponent(typeof(GroundMovement))]
    [RequireComponent(typeof(ZeroGMovement))]
    public class MoveController: GrandmaComponent
    {
        public Moveable active;

        private Moveable inactive;
        protected override void Awake()
        {
            base.Awake();
            active = GetComponent<GroundMovement>();
            inactive = GetComponent<ZeroGMovement>();
        }

        public void SwitchMode(Moveable switchTo)
        {
            active.Deactivate();
            Moveable temp = active;
            active = inactive;
            inactive = temp;
            active.Activate();
        }
    }
    //this would all have to be in another file ?
    [RequireComponent(typeof(MoveController))]
    public class InputController: GrandmaComponent
    {
        private MoveController mc;
        private Vector3 velocity;
        protected override void Awake()
        {
            base.Awake();
            mc = GetComponent<MoveController>();
        }
        private void FixedUpdate()
        { 
           
            mc.active.Move(velocity);
        }

        private Vector3 ZeroGVelocity()
        {
            float x = Input.GetAxisRaw("Horizontal");
            float z = Input.GetAxisRaw("Vertical");
            //regular x movement
            Vector3 moveHorizontal = transform.right * x;

            //move in the direction you're facing
            Vector3 moveVertical = Camera.main.transform.forward * z;

            //move vertically using the jump button (pharah-esque ??)
            Vector3 moveUp = transform.up * (Input.GetButton("Jump") ? 1 : 0);

            //TODO create a data field for movement to include scalar value 
            //(originally called "thrust")
            float scalar = 1.0f;
            //calculate velocity
            Vector3 newVelocity = (moveHorizontal + moveVertical + moveUp).normalized * scalar;
            return newVelocity;
        }

        private Vector3 GroundMovementVelocity()
        {
            float x = Input.GetAxisRaw("Horizontal");
            float z = Input.GetAxisRaw("Vertical");
            Vector3 moveHorizontal = transform.right * x;
            Vector3 moveVertical = transform.forward * z;
            //TODO create a data field for movement to include scalar value
            float scalar = 1.0f;

            //calculate velocity
            Vector3 newVelocity = (moveHorizontal + moveVertical).normalized * scalar;
            return newVelocity;
        }
    }
}
