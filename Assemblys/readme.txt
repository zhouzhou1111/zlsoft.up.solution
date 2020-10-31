20200121包主要解决问题
zltools下管理的SQL不能在netcore环境下获取问题
netcore需要升级到3.0.7.9

2020060101升级包主要解决：
1. 优化框架统一连接数据库及统一异常处理
2. 实现框架的执行脚本统一监控
   在服务端配置config/database.config文件下<appSettings>节点中<add key="ZLBaseServer" value="127.0.0.1:1100" />表示监听到的SQL统一发送到这个地址与端口
   在ZLTools中工具菜单打开”监听执行脚本“的窗口即可看到发送过来的脚本信息