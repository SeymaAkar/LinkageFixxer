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
        static string eglLinkagePath = @"E:\GITREPO\LinkageFix\LinkageFix\data\eglLinkage.txt";
        static string gui2PrdPath = @"E:\GITREPO\LinkageFix\LinkageFix\data\gui2prd.txt";
        static string mvs2PrdPath = @"E:\GITREPO\LinkageFix\LinkageFix\data\mvs2prd.txt";
        static string tst2ProdPath = @"E:\GITREPO\LinkageFix\LinkageFix\data\tst2prod.txt";
        static string webEglLinkage = @"E:\GITREPO\LinkageFix\LinkageFix\data\webegllinkage.txt";

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

        static void ListToFile(List<string> ls, string filePath)
        {

            try
            {

                StreamWriter file = new StreamWriter(filePath + "_new");
                for (int i = 0; i < ls.Count; i++)
                {
                    file.WriteLine(ls[i]);

                }
                file.Close();


            }
            catch (Exception ex)
            {
                Console.WriteLine("EglLinkage dosyasi satirları okunurken exception meydana geldi: " + ex.Message);

            }

        }
        static void EglLinkageFixer()
        {


            lineList.Clear();
            lineList = FileToList(eglLinkagePath);
            for (lineNumber = 0; lineNumber < lineList.Count; lineNumber++)
            {
                pgmName = "";
                luwControl = "";
                linkType = "";

                if (lineList[lineNumber].Contains("PGMNAME"))
                {
                   // repairedLineList.Add()
                    pgmName = lineList[lineNumber].Split(new string[] { "PGMNAME=\"" }, StringSplitOptions.None)[1].Split('\"')[0].Trim();

                    if (lineList[lineNumber].Contains("LUWCONTROL"))
                    {
                        luwControl = lineList[lineNumber].Split(new string[] { "LUWCONTROL=\"" }, StringSplitOptions.None)[1].Split('\"')[0].Trim();
                    }
                    if (lineList[lineNumber].Contains("LINKTYPE"))
                    {
                        linkType = lineList[lineNumber].Split(new string[] { "LINKTYPE=\"" }, StringSplitOptions.None)[1].Split('\"')[0].Trim();
                    }

                    for (int j = lineNumber + 1; j < lineList.Count; j++) // bir sonraki satirdan itibaren baslayarak diger satirlari tek tek 
                    {
                        _pgmName = "";
                        _luwControl = "";
                        _linkType = "";
                        if (lineList[j].Contains("PGMNAME"))
                        {

                            _pgmName = lineList[j].Split(new string[] { "PGMNAME=\"" }, StringSplitOptions.None)[1].Split('\"')[0].Trim();
                            if (lineList[j].Contains("LUWCONTROL"))
                            {
                                _luwControl = lineList[j].Split(new string[] { "LUWCONTROL=\"" }, StringSplitOptions.None)[1].Split('\"')[0].Trim();
                            }
                            if (lineList[j].Contains("LINKTYPE"))
                            {
                                _linkType = lineList[j].Split(new string[] { "LINKTYPE=\"" }, StringSplitOptions.None)[1].Split('\"')[0].Trim();
                            }

                            // onceki satirdaki pgmname'in yildiza kadar olan kismi sonraki satirdaki pgmName'in ilk karakterinden itibaren iceriliyorsa, iceren satir silinir
                            // 1.satir pgmName= RMQ*   2.satir _pmgName= RMQDGT , RMQ*
                            //if (_pgmName.Substring(0).Contains(pgmName.Split('*')[0]))
                            //{
                            //    if ((_pgmName != pgmName))
                            //    {
                            //        lineList.RemoveAt(j);
                            //    }
                            //    else if ((_luwControl == luwControl))
                            //    {
                            //        lineList.RemoveAt(j);
                            //    }
                            //    else if ((_pgmName == pgmName) && !string.IsNullOrEmpty(linkType) && !string.IsNullOrEmpty(_linkType) && (linkType == _linkType))
                            //    {
                            //        lineList.RemoveAt(j);
                            //    }
                            //}

                            if ((_pgmName == pgmName))
                            {
                                if ((_luwControl == luwControl))
                                {
                                    lineList.RemoveAt(j);
                                }
                                else if ((_pgmName == pgmName) && !string.IsNullOrEmpty(linkType) && !string.IsNullOrEmpty(_linkType) && (linkType == _linkType))
                                {
                                    lineList.RemoveAt(j);
                                }
                            }
                            else if (_pgmName.Substring(0).Contains(pgmName.Split('*')[0]))
                            {
                                lineList.RemoveAt(j);
                            }
                        }
                    }
                }
            }
            ListToFile(lineList, eglLinkagePath);
        }
    }




}




