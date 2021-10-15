using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using GameTiles;
using Utilities;

namespace MapFileHandler
{
  public class MapFileHandler
  {
    MapFileLoadSave mapFileLoadSave;

    public MapFileHandler(ref MapFile mapFileToSave, string mapIndex = null)
    {
      mapFileLoadSave = new MapFileLoadSave();
      OpenEditorMenu(ref mapFileToSave, mapIndex);
    }

    public void OpenEditorMenu(ref MapFile mapFileToSave, string mapIndex = null)
    {
      // mapIndex having a value indicates we pass in on app launch
      if (!String.IsNullOrWhiteSpace(mapIndex))
      {
        mapFileLoadSave.LoadMapFileBrowser(ref mapFileToSave, mapIndex);
        return;
      }

      Console.Clear();

      Console.ForegroundColor = ConsoleColor.White;
      Console.Write("[");
      Console.ForegroundColor = ConsoleColor.Yellow;
      Console.Write("S");
      Console.ForegroundColor = ConsoleColor.White;
      Console.Write("]ave Map\n[");
      Console.ForegroundColor = ConsoleColor.Yellow;
      Console.Write("L");
      Console.ForegroundColor = ConsoleColor.White;
      Console.Write("]oad Map\n[");
      Console.ForegroundColor = ConsoleColor.Yellow;
      Console.Write("M");
      Console.ForegroundColor = ConsoleColor.White;
      Console.Write("]ap Editor\n[");
      Console.ForegroundColor = ConsoleColor.Yellow;
      Console.Write("B");
      Console.ForegroundColor = ConsoleColor.White;
      Console.Write("]ack\n");

      ConsoleKeyInfo keyPressed;
      keyPressed = Console.ReadKey(false);
      while (keyPressed.Key != ConsoleKey.B && keyPressed.Key != ConsoleKey.Escape)
      {
        Console.Clear();
        if (keyPressed.Key == ConsoleKey.S)
        {
          mapFileLoadSave.SaveMapFileBrowser(ref mapFileToSave);
          return;
        }
        else if (keyPressed.Key == ConsoleKey.L)
        {
          mapFileLoadSave.LoadMapFileBrowser(ref mapFileToSave);
          return;
        }
        else if (keyPressed.Key == ConsoleKey.M)
        {
          throw new NotImplementedException("Haven't implemented Map Editor yet");
        }

        Console.ForegroundColor = ConsoleColor.White;
        Console.Write("[");
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.Write("S");
        Console.ForegroundColor = ConsoleColor.White;
        Console.Write("]ave Map\n[");
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.Write("L");
        Console.ForegroundColor = ConsoleColor.White;
        Console.Write("]oad Map\n[");
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.Write("M");
        Console.ForegroundColor = ConsoleColor.White;
        Console.Write("]ap Editor\n[");
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.Write("B");
        Console.ForegroundColor = ConsoleColor.White;
        Console.Write("]ack\n");

        keyPressed = Console.ReadKey(false);
      }
    }
  }

  public class MapFileLoadSave
  {
    public MapFileLoadSave()
    {

    }

    public void SaveMapFileBrowser(ref MapFile mapFileToSave)
    {
      string[] filesInDirectory = Directory.GetFiles(@"G:\Programming\CSConsoleRL\CSConsoleRL\Data\Maps", "*.csr");

      string input = "";
      int tempInt = 0;

      while (string.Compare(input, "exit", true) != 0)
      {
        for (int index = 0; index < filesInDirectory.GetLength(0); index++)
        {
          Console.WriteLine((index + 1) + ": " + filesInDirectory[index]);
        }

        input = Console.ReadLine();

        if (int.TryParse(input, out tempInt) && tempInt != 0 && tempInt <= filesInDirectory.GetLength(0))
        {
          SaveMap(filesInDirectory[tempInt - 1], ref mapFileToSave);
          Console.Clear();
          return;
        }
        else if (string.Compare(input, "exit", true) != 0)
        {
          SaveMap(@"G:\Programming\CSConsoleRL\CSConsoleRL\Data\Maps\" + input, ref mapFileToSave);
          Console.Clear();
          return;
        }
      }
    }

    public void LoadMapFileBrowser(ref MapFile mapFileToLoadTo, string mapIndex = null)
    {
      var mapDir = GameGlobals.Instance().Get<string>("mapDir");
      if (!Directory.Exists(mapDir)) mapDir = @"H:\Programming\CSConsoleRL\Data\Maps\";
      string[] filesInDirectory = Directory.GetFiles(mapDir, "*.csr");
      int tempInt = 0;
      string input = "";

      if (!String.IsNullOrWhiteSpace(mapIndex))
      {
        if (int.TryParse(mapIndex, out tempInt) && tempInt != 0 && tempInt <= filesInDirectory.GetLength(0))
        {
          LoadMap(filesInDirectory[tempInt - 1], ref mapFileToLoadTo);
          return;
        }
      }

      while (string.Compare(input, "exit", true) != 0)
      {
        for (int index = 0; index < filesInDirectory.GetLength(0); index++)
        {
          Console.WriteLine((index + 1) + ": " + filesInDirectory[index]);
        }

        input = Console.ReadLine();

        if (int.TryParse(input, out tempInt) && tempInt != 0 && tempInt <= filesInDirectory.GetLength(0))
        {
          LoadMap(filesInDirectory[tempInt - 1], ref mapFileToLoadTo);
          return;
        }
        else if (string.Compare(input, "exit", true) != 0)
        {
          LoadMap(mapDir + input, ref mapFileToLoadTo);
          return;
        }
      }
    }

    private void SaveMap(string filePath, ref MapFile mapFileToSave)
    {
      if (filePath.Substring(filePath.Length - 4) != ".csr") filePath = filePath + ".csr";
      Stream fileStream = File.OpenWrite(filePath);
      BinaryFormatter serializer = new BinaryFormatter();
      serializer.Serialize(fileStream, mapFileToSave);
      fileStream.Close();
    }

    private void LoadMap(string filePath, ref MapFile mapFileToLoadTo)
    {
      if (filePath.Substring(filePath.Length - 4) != ".csr") filePath = filePath + ".csr";
      MapFile loadedMapFile;
      Stream fileStream = File.OpenRead(filePath);
      BinaryFormatter deserializer = new BinaryFormatter();
      loadedMapFile = (MapFile)deserializer.Deserialize(fileStream);
      fileStream.Close();

      mapFileToLoadTo = (MapFile)loadedMapFile.Clone();
    }
  }
}
