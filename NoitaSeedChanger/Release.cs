using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using System.Threading;
using System.Xml;

namespace NoitaSeedChanger
{
    class Release
    {
        private static Dictionary<string, List<string>> data = new Dictionary<string, List<string>>();

        public static string currentHash = string.Empty;
        public static List<IntPtr> currentTargets = new List<IntPtr>();

        public static void Init()
        {
            GetCurrentHash();
            ReadXML("ReleaseData.xml");
            GetCurrentTargets();
        }

        private static void ReadXML(string fileName)   // Reads XML and fills data dictionary
        {
            XmlDocument doc = new XmlDocument();
            try
            {
                doc.Load(fileName);
            }
            catch (Exception e)
            {
                Helper.Error(e.Message);
                Thread.Sleep(10000);
                throw;
            }

            XmlNodeList Nodes = doc.DocumentElement.SelectNodes("/Release/Data");

            foreach (XmlNode node in Nodes)
            {
                string hash = "";
                List<string> targets = new List<string>();

                for (int i = 0; i < node.ChildNodes.Count; i++)
                {
                    if (i == 0)
                    {
                        hash = node.ChildNodes[i].InnerText;
                    }
                    else
                    {
                        targets.Add(node.ChildNodes[i].InnerText);
                    }
                }
                data.Add(hash, targets);
            }
        }

        public static void GetCurrentTargets()
        {
            List<IntPtr> targetList = new List<IntPtr>();

            foreach (var item in data)
            {
                if (item.Key == currentHash)
                {
                    foreach (var target in item.Value)
                    {
                        targetList.Add((IntPtr)int.Parse(target, NumberStyles.AllowHexSpecifier));
                    }
                }
            }
            currentTargets = targetList;
        }

        public static void GetCurrentHash()
        {
            try
            {
                string hash = File.ReadAllLines(GetHashFilePath(), Encoding.UTF8)[0];
                currentHash = hash;
            }
            catch (Exception e)
            {
                Helper.Error(e.Message);
                Thread.Sleep(10000);
                throw;
            }

        }

        private static string GetHashFilePath()
        {
            return Path.GetDirectoryName(Program.game.MainModule.FileName) + "/_version_hash.txt";
        }
    }
}
