using PaladinsCollectDemGems.game;
using System;

namespace PaladinsCollectDemGems.Paladins
{
	public class InGameRewardInstructionSet : InstructionSet
	{
		public InGameRewardInstructionSet(SteamGame game) : base(game)
		{
		}

		public override void Execute()
		{
			// just sitting on screen for twenty seconds before closing
			Wait(20000);
		}

		public override bool ValidateAsActiveInstruction()
		{
			return true;
		}
	}
}
