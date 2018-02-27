using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using FluentCoroutines;

public class BasicExample : MonoBehaviour
{
	[SerializeField]
	private Text text;

	[SerializeField]
	private Color[] colors = new Color[] { Color.red, Color.green, Color.magenta };

	private FluentCoroutine printMessage;

	void Start()
	{
		printMessage = this.FluentCoroutine()
			.Do(PrintHelloWorld)
			.WaitForSeconds(0.5f)
			.Do(CycleColors)
			.Do(ResetText)
			.Finalize();
	}

	public void PrintMessage()
	{
		if (!printMessage.IsExecuting)
		{
			printMessage.Execute();
		}
	}

	private IEnumerator CycleColors()
	{
		foreach (Color c in colors)
		{
			text.color = c;
			yield return new WaitForSeconds(0.5f);
		}
	}

	private void PrintHelloWorld()
	{
		text.text = "Hello, world!";
	}

	private void ResetText()
	{
		text.text = "";
		text.color = Color.white;
	}

	public void StopMessage()
	{
		printMessage.Stop();
		ResetText();
	}
}