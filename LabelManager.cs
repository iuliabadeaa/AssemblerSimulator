using System.Collections.Generic;

namespace proiectAsm
{
	public class LabelManager
	{
		private Dictionary<string, int> labels = new Dictionary<string, int>();

		public void Clear()
		{
			labels.Clear();
		}

		public void AddLabel(string label, int address)
		{
			if (!labels.ContainsKey(label))
			{
				labels[label] = address;
			}
		}

		public int GetAddress(string label)
		{
			if (!labels.ContainsKey(label))
				throw new KeyNotFoundException($"Label '{label}' not found.");
			return labels[label];
		}

		public bool Contains(string label)
		{
			return labels.ContainsKey(label);
		}

		public void LoadLabelsFromList(List<string> labelsList)
		{
			for (int i = 0; i < labelsList.Count; i++)
			{
				labels[labelsList[i]] = i;
			}
		}
	}
}
