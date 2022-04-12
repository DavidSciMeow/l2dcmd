# l2dcmd
## live2d in bandori - download command line
```csharp
var help = "在使用前请检查您是否能连接BestDori.com \n" +
	"[check internet connection before using software]\n"+
	"------------------可用参数-------------------\n" +
	"-d (*download) [下载全部live2d] \n" +
	"-f (*force update) [强制文件完整性检查] \n" +
	"-v (*verbose mode) [详细模式] \n" +
	"-d [-f] [-v] [PATH] [全部下载主构型]\n" +
	"====================================== \n" +
	"-l (*list live2d) [列表所有可用模型] \n" +
	"-fc [string] (*find chara by charaid) [使用charaid搜索] \n" +
	"-fr [string] (*find chara by AssetBundleName using Regex) [使用正则表达式进行包名称搜索] \n" +
	"-fd [numkey] [path] (*file download) [下载指定的live2d到..]\n" +
	"--------------------------------------------\n" +
	"软件版本: 2.0.0 - 使用安全快捷 .net6 实现,链接更加稳定. \n" +
	"[software version:2.0.0, using .net6 for more secure, fast and stable build]"
```

例句: 下载全部的主构型文件到 当前目录 并且重构目录结构 ./l2dcmd -d ./  
例句: 列表当前的live2d ./l2dcmd -l


