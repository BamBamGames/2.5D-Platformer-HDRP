﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Roundbeargames
{
    [CreateAssetMenu(fileName = "New State", menuName = "Roundbeargames/CharacterAbilities/GroundDetector")]
    public class GroundDetector : CharacterAbility
    {
        public float Distance;

        public override void OnEnter(CharacterState characterState, Animator animator, AnimatorStateInfo stateInfo)
        {

        }

        public override void UpdateAbility(CharacterState characterState, Animator animator, AnimatorStateInfo stateInfo)
        {
            if (IsGrounded(characterState.characterControl))
            {
                animator.SetBool(HashManager.Instance.ArrMainParams[(int)MainParameterType.Grounded], true);
            }
            else
            {
                animator.SetBool(HashManager.Instance.ArrMainParams[(int)MainParameterType.Grounded], false);
            }
        }

        public override void OnExit(CharacterState characterState, Animator animator, AnimatorStateInfo stateInfo)
        {

        }

        bool IsGrounded(CharacterControl control)
        {
            // physics check
            if (control.GROUND_DATA.BoxColliderContacts != null)
            {
                foreach (ContactPoint c in control.GROUND_DATA.BoxColliderContacts)
                {
                    float colliderBottom = (control.transform.position.y + control.boxCollider.center.y) -
                        (control.boxCollider.size.y / 2f);
                    float yDifference = Mathf.Abs(c.point.y - colliderBottom);

                    if (yDifference < 0.01f)
                    {
                        if (Mathf.Abs(control.RIGID_BODY.velocity.y) < 0.001f)
                        {
                            control.GROUND_DATA.Ground = c.otherCollider.transform.root.gameObject;
                            SetLandingPosition(control, c.point);
                            return true;
                        }
                    }
                }
            }

            // raycast check
            if (control.RIGID_BODY.velocity.y < 0f)
            {
                foreach (GameObject o in control.COLLISION_SPHERE_DATA.BottomSpheres)
                {
                    RaycastHit[] hits = Physics.RaycastAll(o.transform.position, Vector3.down, Distance);

                    foreach(RaycastHit h in hits)
                    {
                        if (!CollisionDetection.IgnoreCollision(control, h))
                        {
                            CharacterControl c = CharacterManager.Instance.GetCharacter(h.transform.root.gameObject);

                            if (c == null)
                            {
                                control.GROUND_DATA.Ground = h.transform.root.gameObject;
                                SetLandingPosition(control, h.point);
                                return true;
                            }
                        }
                    }
                }
            }

            control.GROUND_DATA.Ground = null;
            return false;
        }

        void SetLandingPosition(CharacterControl control, Vector3 pos)
        {
            control.BOX_COLLIDER_DATA.LandingPosition = new Vector3(
                0f,
                pos.y,
                pos.z);

            if (control.BLOCKING_DATA.DownBlockingObjs.Count < 5 &&
                control.BLOCKING_DATA.DownBlockingObjs.Count != 0)
            {
                foreach (KeyValuePair<GameObject, List<GameObject>> data in control.BLOCKING_DATA.DownBlockingObjs)
                {
                    if (data.Key.transform.position.z < control.transform.position.z)
                    {
                        break;
                    }

                    if (data.Key.transform.position.z > control.transform.position.z)
                    {
                        break;
                    }
                }
            }
        }
    }
}