using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;




namespace LinkageFix
{
    class Program
    {
        static string eglLinkagePath = @"C:\Users\grawgraw\source\repos\LinkageFix\LinkageFix\data\eglLinkage.txt";
        static string gui2PrdPath = @"C:\Users\grawgraw\source\repos\LinkageFix\LinkageFix\data\gui2prd.txt";
        static string mvs2PrdPath = @"C:\Users\grawgraw\source\repos\LinkageFix\LinkageFix\data\mvs2prd.txt";
        static string tst2ProdPath = @"C:\Users\grawgraw\source\repos\LinkageFix\LinkageFix\data\tst2prod.txt";
        static string webEglLinkage = @"C:\Users\grawgraw\source\repos\LinkageFix\LinkageFix\data\webegllinkage.txt";

        static int lineNumber = 0;
        static string line;

        static List<string> lineList = new List<string>();
        static List<string> repairedLineList = new List<string>();
        static string pgmName;
        static string luwControl;
        static string linkType;
        static string _pgmName;
        static string _luwControl;
        static string _linkType;


        static void Main(string[] args)
        {
            Console.WriteLine("LinkageFix Calıstırıldı");


            EglLinkageFixer();
            Console.WriteLine(line);
        }

        static List<string> FileToList(string filePath)
        {
            List<string> tmpList = new List<string>();
            string tmpLine;
            try
            {
                StreamReader file = new StreamReader(filePath);
                while ((tmpLine = file.ReadLine()) != null)
                {
                    tmpLine = tmpLine.Trim();
                    tmpLine = tmpLine.ToUpper();
                    tmpLine = tmpLine.Replace('İ', 'I');
                    tmpList.Add(tmpLine);
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine("EglLinkage dosyasi satirları okunurken exception meydana geldi: " + ex.Message);
                return null;
            }
            return tmpList;
        }

        static void EglLinkageFixer()
        {


            lineList.Clear();
            lineList = FileToList(eglLinkagePath);
            for (lineNumber = 0; lineNumber < lineList.Count; lineNumber++)
            {
                if (lineList[lineNumber].Contains("PGMNAME"))
                {
                    pgmName = lineList[lineNumber].Split(new string[] { "PGMNAME=\"" }, StringSplitOptions.None)[1].Split('\"')[0].Trim();
                    if (lineList[lineNumber].Contains("REMOTECALL"))
                    {
                        luwControl = lineList[lineNumber].Split(new string[] { "LUWCONTROL=\"" }, StringSplitOptions.None)[1].Split('\"')[0].Trim();
                        for (int j = lineNumber + 1; j < lineList.Count; j++)
                        {
                            if (lineList[lineNumber].Contains("PGMNAME"))
                            {
                                _pgmName = lineList[j].Split(new string[] { "PGMNAME=\"" }, StringSplitOptions.None)[1].Split('\"')[0].Trim();
                                if (lineList[j].Contains("REMOTECALL"))
                                {
                                    _luwControl = lineList[j].Split(new string[] { "LUWCONTROL=\"" }, StringSplitOptions.None)[1].Split('\"')[0].Trim();
                                }
                                //pgmName = RMQ*  , pgmName=RMQDGT
                                if ((_pgmName == pgmName) && (_luwControl == luwControl))
                                {
                                    lineList.RemoveAt(j);
                                }
                                else if (pgmName.Contains("*"))
                                {
                                    if (_pgmName.Substring(0).Contains(pgmName.Split('*')[0]))
                                    {

                                    }


                                }
                            }
                        }
                    }
                    else if (lineList[lineNumber].Contains("LOCALCALL"))
                    {
                        linkType = lineList[lineNumber].Split(new string[] { "LINKTYPE=\"" }, StringSplitOptions.None)[1].Split('\"')[0].Trim();
                        for (int j = lineNumber + 1; j < lineList.Count; j++)
                        {
                            _pgmName = lineList[j].Split(new string[] { "PGMNAME=\"" }, StringSplitOptions.None)[1].Split('\"')[0].Trim();
                            if (lineList[j].Contains("LOCALCALL"))
                            {
                                _linkType = lineList[j].Split(new string[] { "LINKTYPE=\"" }, StringSplitOptions.None)[1].Split('\"')[0].Trim();
                            }
                            if ((_pgmName == pgmName) && (_linkType == linkType))
                            {
                                lineList.RemoveAt(j);
                            }
                        }
                    }
                }
            }




        }

    }
}

