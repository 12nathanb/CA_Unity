using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
public static class Exporter {

	public static void SaveWorld (cell[,] Cell, int width, int height, string name)
	{
		BinaryFormatter formatter = new BinaryFormatter();
		string path = Application.persistentDataPath + "/" + name + ".ctp";
		FileStream stream = new FileStream(path, FileMode.Create);

		WorldData data = new WorldData(Cell, width, height);

		formatter.Serialize(stream, data);
		Debug.Log(Application.persistentDataPath);

		stream.Close();
	}

}