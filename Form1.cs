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
        public string ParseAnystr(Anystr e)
        {
            if (e.def[0] == "conststr")
            {
                if (e.def.Count <= 1) Debug("error: no enough parameters for conststr");
                return e.def[1];
            }
            if (e.def[0] == "constint")
            {
                if (e.def.Count <= 1) Debug("error: no enough parameters for constint");
                return e.def[1];
            }
            if (e.def[0] == "constpure" || e.def[0] == "const")
            {
                if (e.def.Count <= 1) Debug("error: no enough parameters for const");
                return e.def[1];
            }
            if (e.def[0] == "sum")
            {
                if (e.def.Count <= 2) Debug("error: no enough parameters for sum");
                return (Convert.ToInt32(e.def[1]) + Convert.ToInt32(e.def[2])).ToString();
            }
            if (e.def[0] == "rsum")
            {
                if (e.def.Count <= 2) Debug("error: no enough parameters for rsum");
                return (Convert.ToDouble(e.def[1]) + Convert.ToDouble(e.def[2])).ToString();
            }
            if(e.def[0] == "input")
            {
                saveFileDialog1.ShowDialog();
                return saveFileDialog1.FileName.Split('\\').Last();
            }
            if(e.def[0] == "equal")
            {
                if (e.def.Count <= 2) Debug("error: no enough parameters for equal");
                return (e.def[1] == e.def[2] ? "true" : "false");
            }
            if(e.def[0] == "if")
            {
                if (e.def.Count <= 2) Debug("error: no enough parameters for if");
                if (e.def[1] == "true") return e.def[2];
                if (e.def.Count >= 3 && e.def[1] == "false") return e.def[3];
                Debug("error: if neither true nor false");
                return "";
            }
            string ret = "<";
            for (int i = 0; i < e.def.Count; i++) ret += e.def[i] + ",";
            foreach (var par in e.info)
            {
                for (int j = 0; j < par.Value.Count; j++)
                    ret += par.Key + "=" + par.Value[j] + ",";
            }
            ret = ret.Remove(ret.Length - 1);
            ret += ">";
            return ret;
        }
        public bool RealCharMatch(string s, int i, char c)
        {
            return s[i] == c;
            //return i != s.Length - 1 && s[i] == c && s[i + 1] == c;
        }
        public string ParseMoncodt(string s) // Moncodt
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
            Debug(s + " -> " + ret);
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
                    tmp.def.Add(ParseMoncodt(doi[i]));
                }
                else
                {
                    string before = "", after = "";

                    for (int j = 0; j < p - 1; j++) before += doi[i][j];
                    for (int j = p; j < doi[i].Length; j++) after += doi[i][j];
                    //Debug("after is " + after);
                    if (!tmp.info.ContainsKey(before)) tmp.info.Add(before, new List<string>());
                    tmp.info[before].Add(ParseMoncodt(after));
                }
                //Debug(doi[i]);
            }
            if (tmp.def.Count == 0) tmp.def.Add("undefined");
            string ret = ParseAnystr(tmp);
            Debug(ss + " -> " + ret);
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
                Debug(ParseMoncodt(textBox1.Text));
                Debug("finished.");
            }
            catch {
                Debug("something wrong happened..");
            }
        }
    }
}
