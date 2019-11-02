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
using CefSharp;
using CefSharp.WinForms;
using System.Text.RegularExpressions;

namespace QZAlbumTool
{
    public partial class FormMain : Form
    {
        private ChromiumWebBrowser web = null;
        public FormMain()
        {
            InitializeComponent();
        }

        private FolderBrowserDialog m_dlg = new FolderBrowserDialog();

        private void FormMain_Load(object sender, EventArgs e)
        {
            //web.Url = new Uri("");
            InitBrowser();
        }
        private void InitBrowser()
        {
            Cef.Initialize(new CefSettings());
            web = new ChromiumWebBrowser("https://qzone.qq.com");
            tabPageLogin.Controls.Add(web);
            web.Dock = DockStyle.Fill;
        }

        private string GetPSkey()
        {
            string p_skey =  GetCookieValue("p_skey");
            return p_skey;
        }
        private string GetQQNumber()
        {
            string qqnumber = int.Parse(GetCookieValue("uin").Substring(1)).ToString();
            return qqnumber;
        }
        private string GetCookieValue(string key)
        {
            string val = "";
            try
            {
                var cookies = web.GetCookieManager().VisitAllCookiesAsync().Result.Where(p => p.Name == key).ToList();
                if (cookies.Count > 0)
                {
                    val = cookies[0].Value;
                }
            }
            catch { }

            return val;
        }
        private void CopyCookie(ChromiumWebBrowser src, WebClient dst)
        {
            var cookies = src.GetCookieManager().VisitAllCookiesAsync().Result;
            string strcookies = "";
            foreach (var cookie in cookies)
            {
                strcookies += cookie.Name + "=" + cookie.Value + ";";
            }
            dst.Headers.Add("Cookie", strcookies);
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

        private JObject ParseCallbackJson(string html)
        {
            JObject jobj = null;
            if (string.IsNullOrWhiteSpace(html))
                return null;

            string start_key = "shine0_Callback(";
            string end_key = ");";

            int start = html.IndexOf(start_key);
            int end = html.LastIndexOf(end_key);

            if (start >= 0 && end >= 0 && end > start)
            {
                html = html.Substring(start+ start_key.Length, end - start- start_key.Length);

                try { jobj = JObject.Parse(html); } catch { }

            }

            return jobj;
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
            wc.Headers.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/74.0.3729.169 Safari/537.36");
            CopyCookie(web,wc);
            string json = string.Empty;

            try { json = wc.DownloadString(url); } catch { Thread.Sleep(1000); }
            if(json == null) try { json = wc.DownloadString(url); } catch { }

            JObject jobj = ParseCallbackJson(json);

            try
            {
                do
                {
                    if ((int)jobj["code"] != 0 || (int)jobj["subcode"] != 0)
                        break;

                    listJAlbumList = new List<JObject>();

                    var jalbumListModeClass = jobj["data"]["albumListModeClass"];

                    if (jalbumListModeClass == null)
                        break;


                    foreach (var jelem in jalbumListModeClass)
                    {
                        foreach (var jelem2 in jelem["albumList"])
                        {
                            listJAlbumList.Add((JObject)jelem2);
                        }
                    }
                    Debug.WriteLine("完成");
                } while (false);

            }
            catch { listJAlbumList = null; }


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
                string filetype = "";
                byte[] data = GetFile(pre,out filetype, type, save);

                using (MemoryStream mStream = new MemoryStream(data))
                {
                    img = Image.FromStream(mStream);
                }
            }
            catch { }

            return img;
        }
        private byte[] GetFile(string pre,out string filetype,string type = null, bool save = false)
        {
            byte[] data = null;

            filetype = "";
            try
            {
                WebClient wc = new WebClient();
                CopyCookie(web,wc);
                //wc.Headers.Add("cookie", web.GetCookieManager().ToString());
                //wc.Headers.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/74.0.3729.169 Safari/537.36");
                //string url = pre + "&rf=albumlist&t=5";
                //string url = pre + "&d=0";

                string url = pre;

                if (!string.IsNullOrWhiteSpace(type))
                {
                    if (url.Contains("/psb?/"))
                    {
                        if(url.Contains("/a/"))
                            url = url.Replace("/a/", "/" + type + "/");
                        else if(url.Contains("/b/"))
                            url = url.Replace("/b/", "/" + type + "/");
                    }
                    url += "&d=1";
                    if (save)
                        url += "&save=1";

                    //把url换成原图url更换host就行
                    url = Regex.Replace(url, @"//([a-z 0-9 .])*/", "//r.photo.store.qq.com/");
                    //http://r.photo.store.qq.com/psb?/V12fsSln18e8BB/8*DJV4OpdWekCnDzBw3vNgCPDbUY4TrTvNOCwMmA14Y!/b/dC8BAAAAAAAA&bo=AAqgBQAKoAURBzA!
                    //http://r.photo.store.qq.com/psb?/V12fsSln18e8BB/u623hhimDbAeYuMkulM6JFuTmCTIFqlr0h*ArmMrPrc!/r/dA0BAAAAAAAA

                }
                else
                {
                    url += "&t=0";
                }

                data = wc.DownloadData(url);

                try 
                {

                    filetype = wc.ResponseHeaders["Content-Disposition"].Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries)[1].Split('=')[1].Split('.')[1];

                }
                catch { }

            }
            catch
            {

            }

            return data;
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

                    if(list == null)
                    {
                        MessageBox.Show("获取相册列表失败");
                    }else if(list.Count == 0)
                    {
                        MessageBox.Show("相册为空");
                    }
                    else
                    {
                        MessageBox.Show("显示中,共" + list.Count + "个相册，载入缓慢请耐心等待");
                        ShowAlbumList(list);
                    }
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
            //wc.Headers.Add("cookie", web.GetCookieManager().ToString());
            CopyCookie(web, wc);
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
                    string photo_batchid = (string)jphoto["batchId"];
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

                    photo_name = photo_name.Replace('\\', '_').Replace('/', '_').Replace(':', '_');

                    string filename = path + "\\" + counter++  + "_" +photo_name;

                    filename = filename
                        .Replace('*', '_')
                        .Replace('?', '_')
                        .Replace('<', '_')
                        .Replace('>', '_')
                        .Replace('|', '_');

                    

                    string filetype = "";
                    byte[] data = GetFile(photo_url,out filetype, "r", true);


                    if(string.IsNullOrWhiteSpace(filetype))
                    {
                        filetype = ".png";   //取类型失败时默认就是png
                    }
                    else
                    {
                        filetype = "." + filetype;
                    }

                    File.WriteAllBytes(filename + filetype, data);

                    if (cbUseSize.Checked)
                    {
                        Image img = Image.FromFile(filename + filetype);
                        string filename2 = filename + "_" + img.Width + "x" + img.Height + filetype;
                        img.Dispose();
                        File.Move(filename + filetype, filename2); //重命名
                    }
                }

                if(!string.IsNullOrWhiteSpace(err2))
                {
                    err += jitem.name + "[" + err2 + "]\r\n";
                }
            }
            MessageBox.Show("处理完毕");
        }

    }


    public class AA : ICookieVisitor
    {
        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public bool Visit(CefSharp.Cookie cookie, int count, int total, ref bool deleteCookie)
        {
            throw new NotImplementedException();
        }
    }

}
