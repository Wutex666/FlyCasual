﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Upgrade;
using Tokens;

namespace ActionsList
{

    public class ReloadAction : GenericAction
    {

        public ReloadAction() {
            Name = EffectName = "Reload";
            ImageUrl = "https://raw.githubusercontent.com/guidokessels/xwing-data/master/images/reference-cards/ReloadActionAndJamTokens.png";
        }

        public override void ActionTake()
        {
            foreach (var upgrade in Selection.ThisShip.UpgradeBar.GetInstalledUpgrades())
            {
                if (upgrade.Type == UpgradeType.Missile || upgrade.Type == UpgradeType.Torpedo)
                {
                    if (upgrade.isDiscarded) upgrade.FlipFaceup();
                }
            }

            Selection.ThisShip.AssignToken(new WeaponsDisabledToken(), Phases.CurrentSubPhase.CallBack);
        }

        public override int GetActionPriority()
        {
            int result = 0;
            return result;
        }

    }

}
