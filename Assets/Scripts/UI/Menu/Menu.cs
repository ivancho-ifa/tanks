using UnityEngine;
public class Menu : MonoBehaviour
{
	private GameObject currentMenu;
	protected GameObject CurrentMenu {
		get => this.currentMenu;
		set {
			if (this.currentMenu)
				this.currentMenu.SetActive(false);

			this.currentMenu = value;
			this.currentMenu.SetActive(true);
		}
	}
}
