# QZAlbumTool 

​	QQ Zone album export tool

​	QQ空间相册批量导出工具

## 更新日志



## Version 1.5

时间：2025.04.07

日志：

1. 修改：更新获取原图的方式。
2. 修改：修复相册名没有处理特殊字符导致创建相册目录失败的bug。
3. 修改：升级`CefShrp`库到134.3.9。
4. 修改：升级`.net framework`到4.8。

## Version 1.4

时间：2019.11.03

日志：

1. 新增：保存时可选择以相册名作为文件夹存放相片。（感谢@ [h25019871990](https://github.com/h25019871990)在[issues1]( https://github.com/kahotv/QZAlbumTool/issues/1 )中的建议）
2. 修改：修复相册预览图失真的bug。
3. 修改：优化获取原图的方式。（感谢@ [h25019871990](https://github.com/h25019871990)在[issues1]( https://github.com/kahotv/QZAlbumTool/issues/1 )中的技术方案）



此项目后续应该不会再更新了，打算重构到另一个项目（[QZAlbumTool2]( https://github.com/kahotv/QZAlbumTool2)），会有更高的效率和更小的文件。



## Version 1.3

时间：2019.11.03

日志：

1. 修改：web控件由C#的原生WebBrowser换成CefSharp的ChromiumWebBrowser，解决js报错等兼容性问题。
2. 新增：下载**真正的原图**，之前版本下载的是比原图更次一级的大图。
3. 新增：下载原格式，上传的时候是什么格式下载就是什么格式**（例如.gif）**。
4. 新增：保存时可以选择加入图片尺寸。



CefSharp的文件可真多，不舒服



## Version 1.2

时间：2019.05.31

日志：

1. 修复严重bug，因取QQ号方法不对导致取不到相册列表。
2. 新增判断空相册逻辑。


## Version 1.1

时间：2019.05.28

日志：

​	1.优化UI布局

​	2.更新文档和图片

## Version 1.0

时间：2019.05.28

日志：

​	1.新增导出相册功能

​	2.优化查看相册列表速度

​	3.完善"关于"页面

## Version 0.1

时间：2019.05.28

日志：

​	1.只能看相册列表

## 当前功能

- 登录到QQ空间
  - 扫码登录
  - 快捷登录
  - 账号密码登录
- 获取相册列表
  - 展示到TagPage2里
- 导出选中的相册
  - 导出到指定的文件夹内

## 已知BUG和问题

- （BUG）无封面的相册不能获取
  - url转换没处理好。
- （问题）UI卡顿
  - 切换到相册时会卡UI，没有进度条显示。
  - 导出相册时会卡UI，没有进度条显示。

## 编译

​	本项目依赖了Newtonsoft.Json、CefSharp、CefSharp.Core、CefSharp.WinForms，自行使用NuGet添加这些库来编译。

## 使用

1. 登录

   扫码、快捷登录、账号密码登录都行。

2. 点击2.相册

   要多等一会儿，有点慢。界面不能拖动表示正在处理中，不要关闭程序。

   

![](QZAlbum.png)



## 感想

目前全程已经是https通信，就算照片实际上仍然可以通过http访问，但由于中间人拿不到url所以没啥问题，safe。

