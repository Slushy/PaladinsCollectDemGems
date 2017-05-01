using PaladinsCollectDemGems.game;

namespace PaladinsCollectDemGems.Paladins
{
	/// <summary>
	/// Controls and runs the steam game Paladins to allow us to automate logging into the game everyday to get our daily reward. 
	/// The following sequence the runner executes:
	/// 
	/// 1.) Logs into the launcher if not already logged in.
	/// 2.) Once logged in, press play to launch the actual game.
	/// 3.) Once the game is opened and loaded select the daily reward.
	/// 4.) Close the game.
	/// 
	/// </summary>
	public class PaladinsGameRunner : GameRunner
	{
		private const int PALADINS_STEAM_GAME_ID = 444090;
		
		// To find out which step we are on
		private InstructionSet[] _instructionSets;
		private int _currInstrSetIndex = -1;

		/// <summary>
		/// Constructs a Paladins game runner
		/// </summary>
		public PaladinsGameRunner() : base(PALADINS_STEAM_GAME_ID)
		{
		}

		/// <summary>
		/// Starts execution of the Paladins runner sequence.
		/// 
		/// 1.) Initializes the runner
		/// 2.) Gets the next instruction set to run and executes it.
		/// 3.) Once out of instruction sets, return to base class.
		/// 
		/// </summary>
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

		/// <summary>
		/// Initializes the instruction sets and brings the game to the foreground of the desktop
		/// </summary>
		private void Initialize() {
			// First load any instruction sets
			_currInstrSetIndex = -1;
			_instructionSets = new InstructionSet[] {
				new LoginInstructionSet(Game),
				new LoadingGameScreenInstructionSet(Game),
				new InGameRewardInstructionSet(Game)
			};

			// Foreground & focus the game
			BringGameToFront();
		}

		/// <summary>
		/// Loops over each instruction set to get the next one that should be ran.
		/// </summary>
		/// <returns>the next instruction set to run, or null if none.</returns>
		private InstructionSet GetNextInstructionSet() {
			while (++_currInstrSetIndex < _instructionSets.Length) {
				InstructionSet candidate = _instructionSets[_currInstrSetIndex];
				if (candidate.ValidateAsActiveInstruction())
					return candidate;
			}

			_currInstrSetIndex = -1;
			return null;
		}

		/// <summary>
		/// Called when the game has been closed and we are exiting the runner. We dispose the instruction
		/// sets and reset our data so this instance can be restarted without the need to create a new runner.
		/// </summary>
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
