using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace easycmd
{
    internal class ListBoxCmd
    {
        public string Command { get; set; }
        public string Name { get; set; }

        public ListBoxCmd(string name, string command)
        {
            Name = name;
            Command = command;
        }
    }

    internal class Formats
    {
        public string Name { get; set; }
        public List<string> List { get; set; }

        public Formats(string name, List<string> lists)
        {
            Name = name;
            List = lists;
        }
    }

    internal class GetCmd
    {
        static string cmd;
        static List<string> fileNameList = new List<string>();
        static List<Formats> formatList = new List<Formats>();
        static List<string> fileExtensionList = new List<string>();
        static List<string> cmdInputList = new List<string>();
        static string cmdOutput = "";
        static List<string> fileFormatList = new List<string>();
        static List<int> indexList = new List<int>();
        static string dateTime = DateTime.Now.ToString().Replace('/', '-').Replace(' ', '-').Replace(':', '-');

        static List<Formats> InitFormats()
        {
            List<Formats> formats = new List<Formats>();
            //外部自定义格式优先
            using (StreamReader sr = new StreamReader("1.txt"))
            {
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    Formats format = new Formats(line.Split('<')[0], new List<string>(line.Split('<')[1].Split(',')));
                    formats.Add(format);
                }
            }
            //内部定义格式
            Formats any = new Formats("any", new List<string>());
            formats.Add(any);
            Formats ffsub = new Formats("ffsub", new List<string> { "ass", "srt" });
            formats.Add(ffsub);

            return formats;
        }

        static List<string> GetFileExtension(List<string> fileNameList)
        {
            List<string> list = new List<string>();
            foreach (var file in fileNameList)
            {
                list.Add(file.Split('.')[1]);
            }

            return list;
        }

        static List<string> GetCmdInputList()
        {
            int end = 0, start;
            if (cmd.Contains("[") && cmd.Contains("]"))
            {
                while ((start = cmd.IndexOf("[", end)) != -1)
                {

                    end = cmd.IndexOf("]", start);
                    cmdInputList.Add(cmd.Substring(start + 1, end - start - 1));
                }
            }

            return cmdInputList;
        }

        static string GetCmdOutput()
        {
            int end = 0, start;
            if (cmd.Contains("<") && cmd.Contains(">"))
            {
                start = cmd.IndexOf("<", end);
                end = cmd.IndexOf(">", start);
                cmdOutput = cmd.Substring(start + 1, end - start - 1);
            }

            return cmdOutput;
        }

        static List<string> GetFileFormatList()
        {
            List<string> fileFormatList = new List<string>();
            foreach (var file in fileExtensionList)
            {
                bool flag = true;
                for (int j = 0; j < formatList.Count && flag; j++)
                {
                    for (int i = 0; i < formatList[j].List.Count && flag; i++)
                    {
                        if (file == formatList[j].List[i])
                        {
                            fileFormatList.Add(formatList[j].Name);
                            flag = false;
                        }
                    }
                    if (j == formatList.Count - 1 && flag)
                    {
                        fileFormatList.Add(file);
                    }
                }
            }

            return fileFormatList;
        }

        static void HandleFFmpegSub()
        {
            //https://superuser.com/questions/1247197/ffmpeg-absolute-path-error
            for (int i = 0; i < fileFormatList.Count; i++)
            {
                if (fileFormatList[i] == "ffsub")
                {
                    string name = fileNameList[i];
                    fileNameList.RemoveAt(i);
                    fileNameList.Insert(i, name.Replace(@"\", @"\\\\").Replace(":", @"\\:"));
                }
            }
        }

        static void GetIndexList()
        {
            foreach (var input in cmdInputList)
            {
                for (int i = 0; i < fileFormatList.Count; i++)
                {
                    if (input == "any")
                    {
                        if (!indexList.Contains(i))
                        {
                            indexList.Add(i);
                            break;
                        }
                    }
                    else if (input == fileFormatList[i])
                    {
                        if (!indexList.Contains(i))
                        {
                            indexList.Add(i);
                            break;
                        }
                    }
                }
            }
        }

        static string GetRealCmd()
        {
            int count = 0;
            string realCmd = "";
            int end = 0, start;

            start = cmd.IndexOf("[", end);
            realCmd += cmd.Substring(end, start);
            realCmd += '"' + fileNameList[indexList[count]] + '"';
            end = cmd.IndexOf("]", start);

            while ((start = cmd.IndexOf("[", end)) != -1)
            {
                count++;
                realCmd += cmd.Substring(end + 1, start - end - 1);
                realCmd += '"' + fileNameList[indexList[count]] + '"';
                end = cmd.IndexOf("]", start);
            }

            start = cmd.IndexOf("<", end);
            realCmd += cmd.Substring(end + 1, start - end - 1);

            if (cmdOutput != "")
            {
                realCmd += '"' + fileNameList[indexList[0]] + dateTime + '.' + cmdOutput + '"' + cmd.Split('>')[1];
            }

            return realCmd;
        }

        public static string Get(string _cmd, List<string> _fileNameList)
        {
            cmd = _cmd;
            fileNameList = _fileNameList;

            formatList = InitFormats();
            fileExtensionList = GetFileExtension(fileNameList);
            cmdInputList = GetCmdInputList();
            if (cmdInputList.Count == fileExtensionList.Count)
            {
                cmdOutput = GetCmdOutput();
                fileFormatList = GetFileFormatList();
                HandleFFmpegSub();
                GetIndexList();
                return GetRealCmd();
            }
            else
            {
                return cmd;
            }
        }
    }

    internal class BulkCmd
    {
        public static string Get(string cmd, List<string> list)
        {
            string dateTime = DateTime.Now.ToString().Replace('/', '-').Replace(' ', '-').Replace(':', '-');
            string realCmd = cmd.Split('[')[0] + '"' + list[0] + '"' + cmd.Split(']')[1].Split('<')[0] + '"' + list[0].Split('.')[0] + '-' + dateTime + '.' + cmd.Split('<')[1].Split('>')[0] + '"' + cmd.Split('>')[1];
            for (int i = 1; i < list.Count; i++)
            {
                realCmd += " && " + cmd.Split('[')[0] + '"' + list[i] + '"' + cmd.Split(']')[1].Split('<')[0] + '"' + list[i].Split('.')[0] + '-' + dateTime + '.' + cmd.Split('<')[1].Split('>')[0] + '"' + cmd.Split('>')[1];
            }

            return realCmd;
        }
    }
}
