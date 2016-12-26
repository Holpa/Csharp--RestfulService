using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace JSONTest1
{
    class Parser
    {
        string[] SEPARATOR = new string[]{"\r\n"};
        private Dictionary<string, string> datas;
        public Dictionary<string, Tag> LstTags { private set; get; }

        public Parser(string sJSON)
        {
            datas = new Dictionary<string, string>();
            LstTags = new Dictionary<string, Tag>();
            readReport(sJSON);
        }

        public Parser()
        {
            datas = new Dictionary<string, string>();
            LstTags = new Dictionary<string, Tag>();
        }

        /// <summary>
        /// Parse a JSON report and extract datas from it. Since the Parser object is valid for 1 type of object, it also
        /// ignore any "object" type from the JSON
        /// </summary>
        /// <remarks>ToDo: The parser is currently case sensitive. We might need to resolve this.</remarks>
        /// <param name="sJSON">The JSON report sent by the client</param>
        public void readReport(string sJSON)
        {
            List<string> lst = sJSON.Split(SEPARATOR, StringSplitOptions.RemoveEmptyEntries).ToList();
            for (int i = 0; i < lst.Count(); i++)
            {
                string line = lst[i];
                if (isToIgnore(line)) continue;
                if (line.Contains("type=\"array\"") == false)
                {
                    string[] line_split = line.Split(new string[] { "type=" }, StringSplitOptions.RemoveEmptyEntries);
                    string sKey = line_split[0].Trim().Remove(0, 1);
                    string sValue = line_split[1].Split(new char[] { '>', '<' }, StringSplitOptions.RemoveEmptyEntries)[1];
                    setProperty(sKey, sValue);
                }
                else
                {
                    bool bTagList = false;
                    if (line.Contains("tags"))
                        bTagList = true;
                    if (bTagList)
                    {
                        line = lst[++i];
                        do
                        {
                            string[] tag_split = line.Split(new char[] { '>', '<' }, StringSplitOptions.RemoveEmptyEntries);
                            addTag(tag_split[tag_split.Length - 2]);
                            line = lst[++i];
                        } while (line.Contains("/tags") == false);
                    }
                }
            }
        }

        private bool isToIgnore(string line)
        {
            bool cond1 = line.Contains("root");
            bool cond2 = line.Contains("type=\"object\"");
            bool cond3 = line.Contains("type") == false;
            return cond1 || cond2 || cond3;
        }

        public string getProperty(string sKey)
        {
            if (datas.ContainsKey(sKey))
                return datas[sKey];
            else
                return "";
        }

        public void setProperty(string sKey, string sValue)
        {
            datas.Remove(sKey);
            datas.Add(sKey, sValue);
        }

        public Tag getTagByID(string sID)
        {
            if (LstTags.ContainsKey(sID))
                return LstTags[sID];
            else
                return null;
        }

        /// Used to create and add a tag based on a string built on the following protocol:
        /// ID Status Location (optional)
        private void addTag(string sTagInfos)
        {
            string[] sRawData = sTagInfos.Split(' ');
            Tag tag = new Tag();
            tag.ID = sRawData[0];
            tag.Status = sRawData[1];
            //ToDo: validate location
            if (sRawData.Length > 2)
                tag.Location = sRawData[2];
            else
                tag.Location = "0,0";
            LstTags.Add(tag.ID, tag);
        }
    }
}
