﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Upgrade;

namespace UpgradesList
{

    public class FlightInstructor : GenericUpgrade
    {

        public FlightInstructor() : base()
        {
            Type = UpgradeType.Crew;
            Name = "Flight Instructor";
            Cost = 4;
        }

        public override void AttachToShip(Ship.GenericShip host)
        {
            base.AttachToShip(host);

            host.AfterGenerateAvailableActionEffectsList += FlightInstructorActionEffect;
        }

        private void FlightInstructorActionEffect(Ship.GenericShip host)
        {
            ActionsList.GenericAction newAction = new ActionsList.FlightInstructorActionEffect();
            newAction.ImageUrl = ImageUrl;
            host.AddAvailableActionEffect(newAction);
        }

    }
}

namespace ActionsList
{

    public class FlightInstructorActionEffect : GenericAction
    {

        public FlightInstructorActionEffect()
        {
            Name = EffectName = "Flight Instructor";
        }

        public override bool IsActionEffectAvailable()
        {
            bool result = false;
            if (Combat.AttackStep == CombatStep.Defence) result = true;
            return result;
        }

        public override int GetActionEffectPriority()
        {
            int result = 0;

            if (Combat.AttackStep == CombatStep.Defence)
            {
                int defenceFocuses = Combat.DiceRollDefence.FocusesNotRerolled;
                int defenceBlanks = Combat.DiceRollDefence.BlanksNotRerolled;

                if (Combat.Defender.GetAvailableActionEffectsList().Count(n => n.IsTurnsAllFocusIntoSuccess) > 0 )
                {
                    if (defenceBlanks > 0) result = 90;
                }
                else
                {
                    if (defenceBlanks + defenceFocuses > 0) result = 90;
                }
            }

            return result;
        }

        public override void ActionEffect(System.Action callBack)
        {
            // Can reroll focus if Attacker PS > 2, focus or blank if Attacker PS <= 2
            System.Collections.Generic.List<DieSide> allowedRerolls =
                (Combat.Attacker.PilotSkill > 2) ?
                new System.Collections.Generic.List<DieSide> { DieSide.Focus } :
                new System.Collections.Generic.List<DieSide> { DieSide.Blank, DieSide.Focus };

            DiceRerollManager diceRerollManager = new DiceRerollManager
            {
                SidesCanBeRerolled = allowedRerolls,
                CallBack = callBack
            };

            diceRerollManager.Start();
        }

    }

}

