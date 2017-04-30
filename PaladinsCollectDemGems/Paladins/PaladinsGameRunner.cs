using PaladinsCollectDemGems.game;

namespace PaladinsCollectDemGems.Paladins
{
	public class PaladinsGameRunner : GameRunner
	{
		private const int PALADINS_STEAM_GAME_ID = 444090;
		
		// To find out which step we are on
		private InstructionSet[] _instructionSets;
		private int _currInstrSetIndex = -1;

		public PaladinsGameRunner() : base(PALADINS_STEAM_GAME_ID)
		{
		}

		protected override void Execute() {
			Initialize();

			// Start the next instruction set
			InstructionSet currentSet = GetNextInstructionSet();
			while (currentSet != null) {
				// Make sure the game is foreground and activated
				// before each instruction set
				BringGameToFront();

				currentSet.Execute();
				currentSet = GetNextInstructionSet();
			}
		}

		private void Initialize() {
			// First load any instruction sets
			_currInstrSetIndex = -1;
			_instructionSets = new InstructionSet[] {
				new LoginInstructionSet(Game),
				new LoadingGameScreenInstructionSet(Game),
				new InGameRewardInstructionSet(Game)
			};

			// Foreground & activate the game
			BringGameToFront();
		}

		private InstructionSet GetNextInstructionSet() {
			while (++_currInstrSetIndex < _instructionSets.Length) {
				InstructionSet candidate = _instructionSets[_currInstrSetIndex];
				if (candidate.ValidateAsActiveInstruction())
					return candidate;
			}

			_currInstrSetIndex = -1;
			return null;
		}

		protected override void OnExiting()
		{
			foreach (InstructionSet instrSet in _instructionSets) {
				instrSet.Dispose();
			}

			_currInstrSetIndex = -1;
			_instructionSets = null;
	
			base.OnExiting();
		}
	}
}
