using UnityEngine;

public class TestUIState : MonoBehaviour {

	public static EventSystem.Dispatcher Events = new EventSystem.Dispatcher();

	public FiniteStateMachine FSM = new FiniteStateMachine();

	public void Awake() {
	
		FSM.Register("MainMenu", new MainMenuUI());
		FSM.Register("AudioMenu", new AudioMenuUI());
		FSM.Register("MainGame", new MainGame(FSM));
		FSM.Register("QuitGame", new QuitGameUI());

		FSM.EnterPoint("MainMenu"); 


		FSM.State("MainMenu").On("OPEN_AUDIO").Enter("AudioMenu")
			.On("PLAY_GAME").Push("MainGame")
			.On("QUIT_GAME").Enter("QuitGame");

		FSM.State("QuitGame").On("PROCESS_QUIT", delegate(bool sure) {
				if (sure) {
					gameObject.GetComponent<TestUIState>().enabled = false;
					Camera.main.backgroundColor = Color.black;
				} else { FSM.Enter("MainMenu"); }
			});


		FSM.State("AudioMenu").On("BACK_TO_MENU").Enter("MainMenu");

	
		Events.On("OpenMainGame", delegate() { FSM.CurrentState.Trigger("PLAY_GAME"); });
		Events.On("OpenAudioMenu", delegate() { FSM.CurrentState.Trigger("OPEN_AUDIO"); });
		Events.On("QuitGame", delegate() { FSM.CurrentState.Trigger("QUIT_GAME"); });

		Events.On("ConfirmQuit", delegate() { FSM.CurrentState.Trigger("PROCESS_QUIT", true); });
		Events.On("CancelQuit", delegate() { FSM.CurrentState.Trigger("PROCESS_QUIT", false); });

		Events.On("BackToMenu", delegate() { FSM.CurrentState.Trigger("BACK_TO_MENU", false); });
	}

	public void Update() {

		FSM.Update();
	}

	void OnGUI() {
		if (FSM.CurrentState == null)
			return;

		MenuUI ui = (MenuUI)FSM.CurrentState.StateObject;

		ui.DoGUI();
	}
}
