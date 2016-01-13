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

        public MapFileHandler(ref MapFile mapFileToSave)
        {
            mapFileLoadSave = new MapFileLoadSave(ref mapFileToSave);
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
            while (keyPressed.Key != ConsoleKey.B && keyPressed.Key != ConsoleKey.Escape)
            {
                Console.Clear();
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
        MapFile mapFile;

        public MapFileLoadSave(ref MapFile mapFileToSave)
        {
            mapFile = mapFileToSave;
        }

        public void SaveMapFileBrowser()
        {
            string[] filesInDirectory = Directory.GetFiles(Directory.GetCurrentDirectory() + @"\Maps", "*.csr");

            string input = "";
            int tempInt = 0;

            while (string.Compare(input, "exit", true) != 0)
            {
                for (int index = 0; index < filesInDirectory.GetLength(0); index++)
                {
                    Console.WriteLine((index + 1) + " " + filesInDirectory[index]);
                }

                input = Console.ReadLine();

                if(int.TryParse(input, out tempInt) && tempInt != 0 && tempInt <= filesInDirectory.GetLength(0))
                {
                    SaveMap(filesInDirectory[tempInt - 1]);
                    Console.Clear();
                    return;
                }
                else if(string.Compare(input, "exit", true) != 0)
                {
                    SaveMap(@"Maps\" + input);
                    Console.Clear();
                    return;
                }
            }
        }

        public void LoadMapFileBrowser()
        {
            string[] filesInDirectory = Directory.GetFiles(Directory.GetCurrentDirectory() + @"\Maps", "*.csr");

            string input = "";
            int tempInt = 0;

            while (string.Compare(input, "exit", true) != 0)
            {
                for (int index = 0; index < filesInDirectory.GetLength(0); index++)
                {
                    Console.WriteLine((index + 1) + " " + filesInDirectory[index]);
                }

                input = Console.ReadLine();

                if (int.TryParse(input, out tempInt) && tempInt != 0 && tempInt <= filesInDirectory.GetLength(0))
                {
                    LoadMap(filesInDirectory[tempInt - 1]);
                    return;
                }
                else if (string.Compare(input, "exit", true) != 0)
                {
                    LoadMap(@"Maps\" + input);
                    return;
                }
            }
        }

        private void SaveMap(string filePath)
        {
            Stream fileStream = File.OpenWrite(filePath + ".csr");
            BinaryFormatter serializer = new BinaryFormatter();
            serializer.Serialize(fileStream, mapFile);
            fileStream.Close();
        }

        private void LoadMap(string filePath)
        {
            Stream fileStream = File.OpenRead(filePath + ".csr");
            BinaryFormatter deserializer = new BinaryFormatter();
            mapFile = (MapFile)deserializer.Deserialize(fileStream);
            fileStream.Close();
        }
    }
}
