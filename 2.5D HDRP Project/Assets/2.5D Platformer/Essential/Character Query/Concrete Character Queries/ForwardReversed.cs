﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Roundbeargames
{
    public class ForwardReversed : CharacterQuery
    {
        public override bool ReturnBool()
        {
            if (control.ANIMATION_DATA.LatestMoveForward.MoveOnHit)
            {
                if (control.GetBool(typeof(FacingAttacker)))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }

            if (control.ANIMATION_DATA.LatestMoveForward.Speed > 0f)
            {
                return false;
            }
            else if (control.ANIMATION_DATA.LatestMoveForward.Speed < 0f)
            {
                return true;
            }

            return false;
        }
    }
}