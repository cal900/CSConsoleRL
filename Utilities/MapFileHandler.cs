using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using GameTiles;
using GameTiles.Tiles;

namespace Utilities
{
    public class MapFileHandler
    {
        MapFileLoadSave mapFileLoadSave;

        public MapFileHandler()
        {

            mapFileLoadSave = new MapFileLoadSave();
            OpenEditorMenu();
        }

        public void OpenEditorMenu()
        {
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
            Console.Write("B");
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("]ack\n");

            ConsoleKeyInfo keyPressed;
            keyPressed = Console.ReadKey(false);
            while (keyPressed.Key != ConsoleKey.B)
            {
                if (keyPressed.Key == ConsoleKey.S)
                {
                    mapFileLoadSave.SaveMapFileBrowser();
                }
                else if (keyPressed.Key == ConsoleKey.L)
                {
                    mapFileLoadSave.LoadMapFileBrowser();
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
                Console.Write("B");
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write("]ack\n");

                keyPressed = Console.ReadKey(false);
            }
        }
    }

    public class MapFileLoadSave
    {
        public void SaveMapFileBrowser()
        {

        }

        public void LoadMapFileBrowser()
        {

        }

        private void SaveMap(string filePath, MapFile mapFileToSave)
        {
            Stream fileStream = File.OpenWrite(filePath);
            BinaryFormatter serializer = new BinaryFormatter();
            serializer.Serialize(fileStream, mapFileToSave);
            fileStream.Close();
        }

        private MapFile LoadMap(string filePath)
        {
            MapFile mapFile = new MapFile();

            Stream fileStream = File.OpenRead(filePath);
            BinaryFormatter deserializer = new BinaryFormatter();
            mapFile = (MapFile)deserializer.Deserialize(fileStream);
            fileStream.Close();

            return mapFile;
        }
    }
}
