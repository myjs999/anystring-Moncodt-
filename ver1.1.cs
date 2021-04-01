using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace anystring
{
    public partial class Form1 : Form
    {
        public void Debug(object obj)
        {
            //MessageBox.Show(obj.ToString());
            textBox2.Text += obj.ToString() + "\r\n";
        }
        public Random rand = new Random();



        public class Anystr
        {
            //bool isAnyStr = true; // otherwise it's Moncodt
            public List<string> def = new List<string>();
            public Dictionary<string, List<string>> info = new Dictionary<string, List<string>>();
            public Anystr() { }
            public void AddInfo(string key, string value)
            {
                if (!info.ContainsKey(key)) info.Add(key, new List<string>());
                info[key].Add(value);
            }
            public Anystr(string s)
            {
                def.Add("conststr");
                AddInfo("val", s);
            }
            public Anystr Parse_()
            {
                if (def[0] == "consthelloworld") return new Anystr("helloworld");
                if (def[0] == "sum")
                {

                }
                return this;
            }

        }
       
        public bool RealCharMatch(string s, int i, char c)
        {
            return s[i] == c;
            //return i != s.Length - 1 && s[i] == c && s[i + 1] == c;
        }
        public string Parse(string s) // Moncodt
        {
            //Debug("Parsing " + s);
            string ret = "";
            for (int i = 0; i < s.Length; i++)
            {
                if (RealCharMatch(s, i, '<'))
                {
                    int bed = 0;
                    string tas = "";
                    for (; i < s.Length; i++)
                    {
                        tas += s[i];
                        if (RealCharMatch(s, i, '<')) ++bed;
                        if (RealCharMatch(s, i, '>')) --bed;
                        if (bed == 0) break;
                    }
                    ret += ParseSingleString(tas);
                }
                else
                {
                    ret += s[i];
                }
            }
            //Debug(s + " -> " + ret);
            return ret;
        }
        //<abc,def=gd,dsf=gt>
        public string ParseSingleString(string s) // Anystring(string type)
        {
            //Debug("Parsing "+s);
            string ss = s; s = "";
            for (int i = 1; i < ss.Length - 1; i++) s += ss[i];
            List<string> doi = new List<string>();
            string c = ""; int bed = 0;
            for (int i = 0; i < s.Length; i++)
            {
                if (s[i] == ',' && bed == 0)
                {
                    doi.Add(c);
                    c = "";
                }
                else c += s[i];
                if (s[i] == '<') ++bed;
                if (s[i] == '>') --bed;
            }
            doi.Add(c);
            Anystr tmp = new Anystr();
            for (int i = 0; i < doi.Count; i++)
            {
                int p = -1;
                for (int j = 0; j < doi[i].Length; j++)
                {
                    if (doi[i][j] == '<') break;
                    if (doi[i][j] == '=')
                    {
                        p = j + 1;
                        break;
                    }
                }
                if (p == -1)
                {
                    //tmp.def.Add(ParseMoncodt(doi[i]));
                    tmp.def.Add(doi[i]);
                }
                else
                {
                    // now I'm not getting with "=" section...
                    // know that this may lead a long-time nonuse of "="... 4.1
                    // mik led'n Nep'"=" lme Zuz
                    string before = "", after = "";

                    for (int j = 0; j < p - 1; j++) before += doi[i][j];
                    for (int j = p; j < doi[i].Length; j++) after += doi[i][j];
                    //Debug("after is " + after);
                    if (!tmp.info.ContainsKey(before)) tmp.info.Add(before, new List<string>());
                    tmp.info[before].Add(Parse(after));
                }
                //Debug(doi[i]);
            }
            if (tmp.def.Count == 0) tmp.def.Add("undefined");
            /*
            string ret = ParseAnystr(tmp);
            Debug(ss + " -> " + ret);
            return ret;
            */
            Anystr e = tmp;
            e.def[0] = Parse(e.def[0]);
            string ret = "";
            if (e.def[0] == "conststr")
            {
                if (e.def.Count <= 1) Debug("error: no enough parameters for conststr");
                ret += Parse(e.def[1]);
            }
            else if (e.def[0] == "constint")
            {
                if (e.def.Count <= 1) Debug("error: no enough parameters for constint");
                ret += Parse(e.def[1]);
            }
            else if (e.def[0] == "constpure" || e.def[0] == "const")
            {
                if (e.def.Count <= 1) Debug("error: no enough parameters for const");
                ret += Parse(e.def[1]);
            }
            else if(e.def[0] == "pure")
            {
                if (e.def.Count <= 1) ret += "<pure>";
                else ret += e.def[1];
            }
            else if (e.def[0] == "sum")
            {
                if (e.def.Count <= 2) Debug("error: no enough parameters for sum");
                e.def[1] = Parse(e.def[1]);
                e.def[2] = Parse(e.def[2]);
                ret += (Convert.ToInt32(e.def[1]) + Convert.ToInt32(e.def[2])).ToString();
            }
            else if (e.def[0] == "rsum")
            {
                if (e.def.Count <= 2) Debug("error: no enough parameters for rsum");
                ret += (Convert.ToDouble(Parse(e.def[1])) + Convert.ToDouble(Parse(e.def[2]))).ToString();
            }
            else if(e.def[0] == "prod")
            {
                if (e.def.Count <= 2) Debug("error: no enough parameters for prod");
                e.def[1] = Parse(e.def[1]);
                e.def[2] = Parse(e.def[2]);
                ret += (Convert.ToInt32(e.def[1]) * Convert.ToInt32(e.def[2])).ToString();
            }
            else if (e.def[0] == "rprod")
            {
                if (e.def.Count <= 2) Debug("error: no enough parameters for rsum");
                ret += (Convert.ToDouble(Parse(e.def[1])) * Convert.ToDouble(Parse(e.def[2]))).ToString();
            }
            else if (e.def[0] == "input")
            {
                saveFileDialog1.ShowDialog();
                ret += saveFileDialog1.FileName.Split('\\').Last();
            }
            else if (e.def[0] == "equal")
            {
                if (e.def.Count <= 2) Debug("error: no enough parameters for equal");
                ret += (Parse(e.def[1]) == Parse(e.def[2]) ? "true" : "false");
            }
            else if (e.def[0] == "if")
            {
                if (e.def.Count <= 2)
                {
                    Debug("error: no enough parameters for if");
                    //ret += "<if_error>";
                }
                else
                {
                    e.def[1] = Parse(e.def[1]);
                    if (e.def[1] == "true") ret += Parse(e.def[2]);
                    else if (e.def[1] == "false")
                    {
                        if (e.def.Count > 3) ret += Parse(e.def[3]);
                    }
                    else
                    {
                        Debug("error: if no proper return value");
                        //ret += "<if_error>";
                    }
                }
            }
            else if(e.def[0] == "repeat" || e.def[0] == "rep")
            {
                if (e.def.Count <= 2) Debug("error: no enough parameters for repeat");
                int x = Convert.ToInt32(Parse(e.def[1]));
                while(x-- > 0)
                {
                    ret += Parse(e.def[2]);
                }
            }
            else if(e.def[0] == "nl")
            {
                ret += "\r\n";
            }
            else
            {
                ret = "<";
                for (int i = 0; i < e.def.Count; i++) ret += Parse(e.def[i]) + ",";
                foreach (var par in e.info)
                {
                    for (int j = 0; j < par.Value.Count; j++)
                        ret += par.Key + "=" + par.Value[j] + ",";
                }
                ret = ret.Remove(ret.Length - 1);
                ret += ">";
            }
            return ret;
        }



    
 
        
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                Debug(Parse(textBox1.Text));
                Debug("finished.");
            }
            catch {
                Debug("something wrong happened..");
            }
        }
    }
}
