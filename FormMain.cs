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

        private FolderBrowserDialog m_dlg = new FolderBrowserDialog();

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
        /// <summary>
        /// 获取图片
        /// </summary>
        /// <param name="pre">url</param>
        /// <param name="type">a、m、b</param>
        /// <returns></returns>
        private Image GetImage(string pre,string type = null,bool save = false)
        {
            Image img = null;

            try
            {
                WebClient wc = new WebClient();
                wc.Headers.Add("cookie", web.Document.Cookie);
                //wc.Headers.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/74.0.3729.169 Safari/537.36");
                //string url = pre + "&rf=albumlist&t=5";
                //string url = pre + "&d=0";

                string url = pre;

                if(!string.IsNullOrWhiteSpace(type))
                {
                    if (url.Contains("/psb?/"))
                    {
                        url = url.Replace("/a/", "/" + type + "/");
                    }
                    url += "&d=1";
                    if (save)
                        url += "&save=1";
                }
                else
                {
                    url += "&t=0";
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
            foreach (dynamic jitem in list)
            {
                string name = jitem.name;
                Image img = GetImage((string)jitem.pre);
                //Debug.WriteLine("添加图片" + i + "| url:" + (string)jitem.pre);

                if (img == null)
                {
                    Thread.Sleep(500);
                    img = GetImage((string)jitem.pre);
                }

                if (img == null)
                {
                    Debug.WriteLine("添加图片" + i + "失败(" + (string)jitem.name + ")");
                }
                else
                {
                    ListViewItem item = new ListViewItem()
                    {
                        Tag = jitem,
                        Text = name,
                        ImageIndex = i++
                    };

                    listView1.LargeImageList.Images.Add(img);

                    listView1.Items.Add(item);
                }

            }
        }

        private void BtnSelectAll_Click(object sender, EventArgs e)
        {
            foreach(ListViewItem item  in listView1.Items)
            {
                item.Selected = true;
            }
        }

        private void BtnSelectNot_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in listView1.Items)
            {
                item.Selected = !item.Selected;
            }
        }

        private List<JObject> GetPhotoList(string p_skey, string qqnumber,string id)
        {
            string g_tk = GetG_TK(p_skey).ToString();

            string url = "https://h5.qzone.qq.com/proxy/domain/photo.qzone.qq.com/fcgi-bin/cgi_list_photo?" +
                "g_tk=" + g_tk +
                "&callback=shine0_Callback" +
                "&t=160252680" +
                "&mode=0" +
                "&idcNum=4" +
                "&hostUin=" + qqnumber +
                "&topicId=" + id +
                "&noTopic=0" +
                "&uin="+ qqnumber +
                "&pageStart=0" +
                "&pageNum=9999" +
                "&skipCmtCount=0" +
                "&singleurl=1" +
                "&batchId=" +
                "&notice=0" +
                "&appid=4" +
                "&inCharset=utf-8" +
                "&outCharset=utf-8" +
                "&source=qzone" +
                "&plat=qzone" +
                "&outstyle=json" +
                "&format=jsonp" +
                "&json_esc=1" +
                "&question=" +
                "&answer=" +
                "&callbackFun=shine0" +
                "&_=" + DateTime.UtcNow.ToFileTimeUtc().ToString();

            WebClient wc = new WebClient() {  Encoding = Encoding.UTF8};
            wc.Headers.Add("cookie", web.Document.Cookie);

            string resp = wc.DownloadString(url);

            List<JObject> list = new List<JObject>();

            do
            {
                if (string.IsNullOrWhiteSpace(resp))
                    break;

                string s_key = "shine0_Callback(";
                string e_key = ");";
                int start = resp.IndexOf(s_key) + s_key.Length;
                int end = resp.LastIndexOf(e_key);

                resp = resp.Substring(start, end - start);
                JObject jobj = JObject.Parse(resp);

                if ((int)jobj["code"] != 0 || (int)jobj["subcode"] != 0)
                    break;

                JArray photoList = (JArray)jobj["data"]["photoList"];

                foreach(JObject jphoto in photoList)
                {
                    list.Add(jphoto);
                }
            } while (false);

            return list;
        }

        private void BtnExport_Click(object sender, EventArgs e)
        {
            if (m_dlg.ShowDialog() != DialogResult.OK)
                return;

            string path = m_dlg.SelectedPath;

            string err = "这些图片下载失败:\r\n";

            foreach (ListViewItem item in listView1.Items)
            {
                if (!item.Selected)
                    continue;

                dynamic jitem = item.Tag;

                string err2 = "";

                var list = GetPhotoList(GetPSkey(), GetQQNumber(), jitem.id.ToString());

                if(list == null)
                {
                    MessageBox.Show("打开相册[" + jitem.name + "]失败");
                    continue;
                }
                int counter = 0;
                foreach(JObject jphoto in list)
                {
                    //string photo_batchid = (string)jphoto["batchId"];
                    string photo_url = (string)jphoto["url"];
                    string photo_name = (string)jphoto["name"];
                    //int photo_witdh = (int)jphoto["width"];
                    //int photo_height = (int)jphoto["height"];
                    bool photo_isvideo = (bool)jphoto["is_video"];

                    if(photo_isvideo)
                    {
                        //视频不能下载
                        err2 += photo_name + "(视频),";
                        continue;
                    }

                    Image img = null;

                    try { img = GetImage(photo_url, "b",true); } catch { }
                    if (img == null) { Thread.Sleep(500); try { img = GetImage(photo_url, "b", true); } catch { } }

                    if (img == null)
                    {
                        err2 += photo_name + ",";
                        continue;
                    }

                    photo_name = photo_name.Replace('\\', '_').Replace('/', '_').Replace(':', '_');

                    string filename = path + "\\" + counter++  + "_" +photo_name + ".png";

                    Bitmap bmp = new Bitmap(img.Width, img.Height);
                    Graphics g = Graphics.FromImage(bmp);
                    g.DrawImage(img, 0, 0);



                    filename = filename
                        .Replace('*', '_')
                        .Replace('?', '_')
                        .Replace('<', '_')
                        .Replace('>', '_')
                        .Replace('|', '_');

                    bmp.Save(filename);
                }

                if(!string.IsNullOrWhiteSpace(err2))
                {
                    err += jitem.name + "[" + err2 + "]\r\n";
                }
            }
            MessageBox.Show("处理完毕");
        }

    }
}
