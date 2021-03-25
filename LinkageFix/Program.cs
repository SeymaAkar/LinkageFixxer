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
        static int duplicateLines;

        static List<string> lineList = new List<string>();
        static List<string> repairedLineList = new List<string>();
        static string pgmName;
        static string _pgmName;
        static string applName;
        static string _applName;
        static string str;




        static void Main(string[] args)
        {
            Console.WriteLine("LinkageFix Calıstırıldı");

            Console.WriteLine("EglLinkage kayitlari duzletiliyor");

            pgmName = "*";
            _pgmName = pgmName.Split('*')[0];

            //if (pgmName.Contains("*") && (pgmName.Split('*')[0].Length <= _pgmName.Length))
            //{
            //    if (_pgmName.Substring(0, pgmName.Split('*')[0].Length).Contains(pgmName.Split('*')[0]))
            //    {

            //    }
            //}
            //bool isContains = _pgmName.Substring(0, pgmName.Split('*')[0].Length).Contains(pgmName.Split('*')[0]);
            LinkageFixer(eglLinkagePath);
            Console.WriteLine("GUI kayitlari duzletiliyor");
            LinkageFixer(gui2PrdPath);
            Console.WriteLine("MVS kayitlari duzletiliyor");
            LinkageFixer(mvs2PrdPath);
            Console.WriteLine("TEST kayitlari duzletiliyor");
            LinkageFixer(tst2ProdPath);
            Console.WriteLine("Egl WEB Linkage kayitlari duzletiliyor");
            LinkageFixer(webEglLinkage);

            Console.ReadLine();

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
        static void LinkageFixer(string filePath)
        {
            Console.WriteLine("------Mukerrer Kayıtlar-----");
            lineList.Clear();
            lineList = FileToList(filePath);
            for (lineNumber = 0; lineNumber < lineList.Count; lineNumber++)
            {
                pgmName = "";

                if (lineList[lineNumber].Contains("PGMNAME") || lineList[lineNumber].Contains("APPLNAME"))
                {

                    if (lineList[lineNumber].Contains("PGMNAME"))
                    {
                        pgmName = lineList[lineNumber].Split(new string[] { "PGMNAME=\"" }, StringSplitOptions.None)[1].Split('\"')[0].Trim();
                    }
                    else if (lineList[lineNumber].Contains("APPLNAME"))
                    {
                        pgmName = lineList[lineNumber].Split(new string[] { "APPLNAME=" }, StringSplitOptions.None)[1].Split(' ')[0].Trim();
                    }


                    for (int j = lineNumber + 1; j < lineList.Count; j++) // bir sonraki satirdan itibaren baslayarak diger satirlari tek tek 
                    {
                        _pgmName = "";

                        if (lineList[j].Contains("PGMNAME") || lineList[lineNumber].Contains("APPLNAME"))
                        {
                            if (lineList[j].Contains("PGMNAME"))
                            {
                                _pgmName = lineList[j].Split(new string[] { "PGMNAME=\"" }, StringSplitOptions.None)[1].Split('\"')[0].Trim();
                            }
                            else if (lineList[j].Contains("APPLNAME"))
                            {
                                _pgmName = lineList[j].Split(new string[] { "APPLNAME=" }, StringSplitOptions.None)[1].Split(' ')[0].Trim();
                            }

                            if ((_pgmName == pgmName))
                            {
                                Console.WriteLine(lineList[j]);
                                lineList.RemoveAt(j);
                                j--;
                                duplicateLines++;
                            }
                            else if (pgmName.Contains("*") && (pgmName.Split('*')[0].Length <= _pgmName.Length))
                            {
                                if (_pgmName.Substring(0, pgmName.Split('*')[0].Length).Contains(pgmName.Split('*')[0]))
                                {
                                    Console.WriteLine(lineList[j]);
                                    duplicateLines++;
                                    lineList.RemoveAt(j);
                                    j--;
                                }
                            }
                        }
                    }


                }

            }
            Console.WriteLine("İlgili kayitlar icin duzeltme tamamlandi");
            Console.WriteLine("Toplam Tekrar Eden Kayıt: " + duplicateLines);
            ListToFile(lineList, filePath);
        }
    }
}










