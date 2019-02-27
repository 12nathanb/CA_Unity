using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
public static class Exporter {

	public static void SaveWorld (cell Cell)
	{
		BinaryFormatter formatter = new BinaryFormatter();
		string path = Application.persistentDataPath + "/Chunk" + Cell.getPosInArray().ToString() + ".ctp";
		FileStream stream = new FileStream(path, FileMode.Create);

		WorldData data = new WorldData(Cell);

		formatter.Serialize(stream, data);
		Debug.Log(Application.persistentDataPath);

		stream.Close();
	}

	public static WorldData LoadWorld (int num)
	{
		string path = Application.persistentDataPath + "/Chunk" + num.ToString() + ".ctp";

		if(File.Exists(path))
		{
			BinaryFormatter formatter = new BinaryFormatter();
			FileStream stream = new FileStream(path, FileMode.Open);
			WorldData data = formatter.Deserialize(stream) as WorldData;

			stream.Close();

			return data;
		}
		else
		{
			return null;
		}
	}

}