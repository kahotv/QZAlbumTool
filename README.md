# QZAlbumTool 

​	QQ Zone album export tool

​	QQ空间相册批量导出工具

## 更新日志

### 2019.05.28

​	version	0.1	只能看相册列表

​	version	1.0	新增导出相册功能

## 当前功能

- 登录到QQ空间
  - 扫码登录
  - 快捷登录
  - 账号密码登录
- 获取相册列表
  - 展示到TagPage2里

## 已知BUG和问题

- （BUG）无封面的相册不能获取
  - url转换没处理好。

## 编译

​	本项目依赖了Newtonsoft.Json，使用NuGet添加这个库，并编译。

## 使用

1. 登录

   扫码、快捷登录、账号密码登录都行

2. 点击2.相册

   要多等一会儿，有点慢。

   

![](QZAlbum.png)



## 感想

- 看起来是个小工具，但我写这个README.md就花了近一小时。做一件事和做好一件事是两码事。

- QQ相册的图片竟然不是https的，g_tk参数也是可重用的，且不需要cookie就能访问。这意味着：

  - 就算QQ主人空间锁了，你可以中间人拿到相册图片的URL，并进行重放攻击拿到图片。

  **说通俗点，你把空间设置为仅自己可见，当你打开相册里隐私照片的时候，黑客已经看到你的相册了。**

  这不是我遇到的首例安全漏洞，软件工程师们还是要认真对待安全问题呀。

  