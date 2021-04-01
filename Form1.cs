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
            e.def[0] = ParseMoncodt(e.def[0]);
            string ret = "";
            if (e.def[0] == "conststr")
            {
                if (e.def.Count <= 1) Debug("error: no enough parameters for conststr");
                ret += ParseMoncodt(e.def[1]);
            }
            else if (e.def[0] == "constint")
            {
                if (e.def.Count <= 1) Debug("error: no enough parameters for constint");
                ret += ParseMoncodt(e.def[1]);
            }
            else if (e.def[0] == "constpure" || e.def[0] == "const")
            {
                if (e.def.Count <= 1) Debug("error: no enough parameters for const");
                ret += ParseMoncodt(e.def[1]);
            }
            else if (e.def[0] == "pure")
            {
                if (e.def.Count <= 1) ret += "<pure>";
                else ret += e.def[1];
            }
            else if (e.def[0] == "sum")
            {
                if (e.def.Count <= 2) Debug("error: no enough parameters for sum");
                e.def[1] = ParseMoncodt(e.def[1]);
                e.def[2] = ParseMoncodt(e.def[2]);
                ret += (Convert.ToInt32(e.def[1]) + Convert.ToInt32(e.def[2])).ToString();
            }
            else if (e.def[0] == "rsum")
            {
                if (e.def.Count <= 2) Debug("error: no enough parameters for rsum");
                ret += (Convert.ToDouble(ParseMoncodt(e.def[1])) + Convert.ToDouble(ParseMoncodt(e.def[2]))).ToString();
            }
            else if (e.def[0] == "prod")
            {
                if (e.def.Count <= 2) Debug("error: no enough parameters for prod");
                e.def[1] = ParseMoncodt(e.def[1]);
                e.def[2] = ParseMoncodt(e.def[2]);
                ret += (Convert.ToInt32(e.def[1]) * Convert.ToInt32(e.def[2])).ToString();
            }
            else if (e.def[0] == "rprod")
            {
                if (e.def.Count <= 2) Debug("error: no enough parameters for rsum");
                ret += (Convert.ToDouble(ParseMoncodt(e.def[1])) * Convert.ToDouble(ParseMoncodt(e.def[2]))).ToString();
            }
            else if (e.def[0] == "input")
            {
                saveFileDialog1.ShowDialog();
                ret += saveFileDialog1.FileName.Split('\\').Last();
            }
            else if (e.def[0] == "equal")
            {
                if (e.def.Count <= 2) Debug("error: no enough parameters for equal");
                ret += (ParseMoncodt(e.def[1]) == ParseMoncodt(e.def[2]) ? "true" : "false");
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
                    e.def[1] = ParseMoncodt(e.def[1]);
                    if (e.def[1] == "true") ret += ParseMoncodt(e.def[2]);
                    else if (e.def[1] == "false")
                    {
                        if (e.def.Count > 3) ret += ParseMoncodt(e.def[3]);
                    }
                    else
                    {
                        Debug("error: if no proper return value");
                        //ret += "<if_error>";
                    }
                }
            }
            else if (e.def[0] == "repeat" || e.def[0] == "rep")
            {
                if (e.def.Count <= 2) Debug("error: no enough parameters for repeat");
                int x = Convert.ToInt32(ParseMoncodt(e.def[1]));
                while (x-- > 0)
                {
                    ret += ParseMoncodt(e.def[2]);
                }
            }
            else if (e.def[0] == "nl")
            {
                ret += "\r\n";
            }
            
            if(e.def[0] == "len" || e.def[0] == "length")
            {
                if (e.def.Count <= 1) Debug("error: no enough parameters for len");
                return e.def[1].Length.ToString();
            }

            {
                ret = "<";
                for (int i = 0; i < e.def.Count; i++) ret += ParseMoncodt(e.def[i]) + ",";
                foreach (var par in e.info)
                {
                    for (int j = 0; j < par.Value.Count; j++)
                        ret += par.Key + "=" + ParseMoncodt(par.Value[j]) + ",";
                }
                ret = ret.Remove(ret.Length - 1);
                ret += ">";
            }
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
                    ret += ParseAnystr(ParseSingleString(tas)); // tas is like "<...>"
                }
                else
                {
                    ret += s[i];
                }
            }
            //Debug(s + " -> " + ret);
            return ret;
        }
        public bool IsDigit(char c)
        {
            return c >= '0' && c <= '9';
        }
        public string Match(string t, string s)
        {
            s = ParseMoncodt(s);
            int ti = 0, si = 0;
            string ans = "true";
            for(; si < s.Length;)
            {
                if(RealCharMatch(s, si, '<'))
                {
                    int bed = 0;
                    string tas = "";
                    for (; si < s.Length; si++)
                    {
                        tas += s[si];
                        if (RealCharMatch(s, si, '<')) ++bed;
                        if (RealCharMatch(s, si, '>')) --bed;
                        if (bed == 0) break;
                    } // tas is like "<...>"
                    ++si;
                    Anystr e = ParseSingleString(tas);
                    e.def[0] = ParseMoncodt(e.def[0]);
                    if(e.def[0] == "digits")
                    {
                        while (ti < t.Length && IsDigit(t[ti])) ++ti;
                    }
                    if(e.def[0] == "digit")
                    {
                        int oti = ti;
                        if(e.info.ContainsKey("n"))
                        {
                            int n = Convert.ToInt32(ParseMoncodt(e.info["n"][0])); // here is a PM, so you have to think about it, but not now! 4.1
                            while(n > 0 && ti<t.Length && IsDigit(t[ti]))
                            {
                                --n;
                                ++ti;
                            }
                            if (ti - oti != n) return "false";
                        }else if(e.info.ContainsKey("from"))
                        {

                        }
                        else
                        {
                            if (ti >= t.Length || !IsDigit(t[ti])) return "false";
                            ++ti;
                        }
                    }
                }
                else
                {
                    if (ti >= t.Length || si >= s.Length) return "false";
                    if (t[ti] != s[si]) return "false";
                    ++ti; ++si;
                }

                if (ti == t.Length && si == s.Length) break;
                //if (ti == t.Length || si == s.Length) return "false";
            }
            return ans;
        }
        //<abc,def=gd,dsf=gt>
        public Anystr ParseSingleString(string s) // Anystring(string type)
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
                    if (doi[i][j] == '<') break;  // amazing
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
                    tmp.info[before].Add(ParseMoncodt(after));
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
            return e;
            /*
            string ret = ParseAnystr(e);
            return ret;
            */
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

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                Debug(Match(textBox3.Text, textBox1.Text));
                Debug("match finished.");
            }
            catch
            {
                Debug("something went wrong..");
            }
        }
    }
}
