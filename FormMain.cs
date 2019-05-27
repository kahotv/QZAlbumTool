using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json.Linq;

namespace QZAlbumTool
{
    public partial class FormMain : Form
    {
        public FormMain()
        {
            InitializeComponent();
        }

        private void FormMain_Load(object sender, EventArgs e)
        {
            web.Url = new Uri("https://qzone.qq.com");
            
        }

        private string GetPSkey()
        {
            string p_skey = null;
            try
            {
                var cookies = web.Document.Cookie.Split(';');
                p_skey = cookies.Where(p => p.StartsWith(" p_skey=")).First().Split('=').ToArray()[1];
            }
            catch { }

            return p_skey;
        }
        private string GetQQNumber()
        {
            string qqnumber = null;

            try
            {
                var cookies = web.Document.Cookie.Split(';');
                qqnumber = cookies.Where(p => p.StartsWith(" uin_cookie=")).First().Split('=').ToArray()[1];
            }
            catch { }

            return qqnumber;
        }
        private int GetG_TK(string p_skey)
        {
            string e = p_skey;
            var t = 5381;
            for (int n = 0, r = e.Length; n < r; ++n)
            {
                t += (t << 5) + e[n];
            }
            return t & 0x7FFFFFFF;
        }

        private List<JObject> GetAlbumList(string p_skey,string qqnumber)
        {
            //检查登录 跳转到相册，失败就没登陆
            List<JObject> listJAlbumList = null;

            //string g_tk = WebBeginInvoke("return QZFL.pluginsDefine.getACSRFToken();");
            string g_tk = GetG_TK(p_skey).ToString();
            string url = "https://h5.qzone.qq.com/proxy/domain/photo.qzone.qq.com/fcgi-bin/fcg_list_album_v3?" +
                "g_tk=" + g_tk +
                "&callback=shine0_Callback" +
                "&t=628680498" +
                "&hostUin=" + qqnumber +
                "&uin="+ qqnumber +
                "&appid=4" +
                "&inCharset=utf-8" +
                "&outCharset=utf-8" +
                "&source=qzone" +
                "&plat=qzone" +
                "&format=jsonp" +
                "&notice=0" +
                "&filter=1" +
                "&handset=4" +
                "&pageNumModeSort=40" +
                "&pageNumModeClass=15" +
                "&needUserInfo=1" +
                "&idcNum=4" +
                "&callbackFun=shine0" +
                "&_=" + DateTime.UtcNow.ToFileTimeUtc().ToString();

            WebClient wc = new WebClient() { Encoding = Encoding.UTF8 };
            wc.Headers.Add("cookie", web.Document.Cookie);
            wc.Headers.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/74.0.3729.169 Safari/537.36");
            string json = string.Empty;

            try { json = wc.DownloadString(url); } catch { Thread.Sleep(1000); }
            if(json == null) try { json = wc.DownloadString(url); } catch { }

            if(json != null)
            {
                if (json.Contains("albumListModeClass"))
                {
                    // 相册列表
                    int start = json.IndexOf("albumListModeClass");
                    int end = json.LastIndexOf("}\r\n}\r\n);");
                    if(end == -1)
                        end = json.LastIndexOf("}\n}\n);");
                    if (start >= 0 && end >= 0)
                    {
                        string jsAlbumListModeClass = "{\"" + json.Substring(start, end - start) + "}";

                        JObject jobj = JObject.Parse(jsAlbumListModeClass);
                        listJAlbumList = new List<JObject>();

                        foreach (var jelem in jobj["albumListModeClass"])
                        {
                            foreach (var jelem2 in jelem["albumList"])
                            {
                                listJAlbumList.Add((JObject)jelem2);
                            }
                        }
                        Debug.WriteLine("完成");
                    }

                    //显示在列表里
                    /*
                {
                   "allowAccess" : 1,
                   "anonymity" : 0,
                   "bitmap" : "11000010",
                   "classid" : 100,
                   "comment" : 0,
                   "createtime" : 123456,
                   "desc" : "",
                   "handset" : 0,
                   "id" : "xxxxx",
                   "lastuploadtime" : 123456,
                   "modifytime" : 123456,
                   "name" : "坦克大战",
                   "order" : 1,
                   "pre" : "http:\/\/b323.photo.store.qq.com\/psb?\/xxxxxx\/xxxxxxxx!\/a\/xxxxx",
                   "priv" : 3,
                   "pypriv" : 3,
                   "total" : 6,
                   "viewtype" : 0
                },
                    */
                }
            }

            //获取相册列表 相册名称+封面缩略图
            //获取相册内容列表  照片名称+缩略图+对应的原图
            //下载原图

            return listJAlbumList;
        }
        private Image GetAlbumFace(string pre,string name)
        {
            Image img = null;

            try
            {
                WebClient wc = new WebClient();
                wc.Headers.Add("cookie", web.Document.Cookie);
                //wc.Headers.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/74.0.3729.169 Safari/537.36");
                //string url = pre + "&rf=albumlist&t=5";
                string url = pre + "&d=0";
                if(url.Contains("/psb?/"))
                {
                    url = url.Replace("/a/", "/m/");
                }
                byte[] data = wc.DownloadData(url);
                
                using (MemoryStream mStream = new MemoryStream(data))
                {
                    img = Image.FromStream(mStream);
                }
            }
            catch(Exception e)
            {

            }

            return img;
        }

        private void TabControl1_Selected(object sender, TabControlEventArgs e)
        {
            if(e.TabPageIndex == 1)
            {
                string p_skey = GetPSkey();
                string qq = GetQQNumber();
                if(string.IsNullOrWhiteSpace(p_skey) || string.IsNullOrWhiteSpace(qq))
                {
                    MessageBox.Show("未登录，请重新登录");
                    tabControl1.TabIndex = 0;
                }
                else
                {
                    var list = GetAlbumList(p_skey, qq);
                    Debug.WriteLine("显示中,共" + list.Count + "个相册");
                    ShowAlbumList(list);
                }
            }
        }

        private void ShowAlbumList(List<JObject> list)
        {
            listView1.Items.Clear();
            listView1.LargeImageList = new ImageList();
            listView1.LargeImageList.ImageSize = new Size(146, 110);
            listView1.View = View.LargeIcon;
            int i = 0;
            foreach (var item in list)
            {
                dynamic jitem = item;
                string name = jitem.name;
                Image face = GetAlbumFace((string)jitem.pre, name);
                //Debug.WriteLine("添加图片" + i + "| url:" + (string)jitem.pre);

                if (face == null)
                {
                    Thread.Sleep(500);
                    face = GetAlbumFace((string)jitem.pre, name);
                }

                if (face == null)
                {
                    Debug.WriteLine("添加图片" + i + "失败(" + (string)jitem.name + ")");
                }
                else
                {
                    listView1.LargeImageList.Images.Add(face);
                    listView1.Items.Add(name, i++);
                }


            }
        }

        private void BtnSelectAll_Click(object sender, EventArgs e)
        {
        }
    }
}
