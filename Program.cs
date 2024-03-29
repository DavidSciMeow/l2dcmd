﻿using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace l2dcmd
{
    class Program
    {
        static readonly string help = 
            "在使用前请检查您是否能连接BestDori.com \n" +
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
            "[software version:2.0.0, using .net6 for more secure, fast and stable build]";
        static bool verbose = false;
        static bool download = false;
        static bool forcecheck = false;
        static Meow.Rinko.Core.Live2d.Live2dList j;
        static void Main(string[] args)
        {
            Console.WriteLine("");
            Console.WriteLine("l2dcmd - RinkoBotCore - 2.2.2 [1011]");
            Console.WriteLine("Allright reserved * Bestdori - Bushiroad / GBP * Electronicute");
            Console.WriteLine("");
            if(args.Length > 0)
            {
                if (args[0] != null)
                {
                    var a = args[0];
                    if (a == "-h")
                    {
                        Console.WriteLine(help); 
                        return;
                    }

                    Console.WriteLine("** TRY TO CONNECT BESTDORI **");
                    j = new Meow.Rinko.Core.Live2d.Live2dList();
                    Console.WriteLine("** CONNECT COMPLETE **");
                    
                    foreach (var k in args)
                    {
                        if (k == "-l")
                        {
                            Console.WriteLine("正在获取最新列表 [Getting Newest List()]");
                            foreach (var d in j.Data)
                            {
                                Console.WriteLine($"{d.Value.assetBundleName} :: {d.Key}");
                            }
                            Console.WriteLine("列表模式结束 [List mode complete]");
                            return;
                        }
                        else if (k == "-fc")
                        {
                            if (int.TryParse(args[1], out int patternnum))
                            {
                                Console.WriteLine("正在获取最新列表 [Getting Newest List()]");
                                var result = from aa in j.Data where aa.Value.characterId == patternnum select aa;
                                foreach (var d in result)
                                {
                                    Console.WriteLine($"[{d.Key}] : {d.Value.assetBundleName}");
                                }
                                Console.WriteLine("查找模式已列出所有可能 [Search list complete]");
                            }
                            else
                            {
                                Console.WriteLine("您的输入不是数字 [No Number Present input Unvalid]");
                            }
                            return;
                        }
                        else if(k == "-fr")
                        {
                            var pattern = args[1];
                            Console.WriteLine("正在获取最新列表 [Getting Newest List()]");
                            Console.WriteLine("正在搜索核对 [Validating]");
                            var result = from aa in j.Data where Regex.IsMatch(aa.Value.assetBundleName, pattern) select aa;
                            foreach (var d in result)
                            {
                                Console.WriteLine($"[{d.Key}] : {d.Value.assetBundleName}");
                            }
                            Console.WriteLine("查找模式已列出所有可能 [Search list complete]");
                            return;
                        }
                        else if(k == "-fd")
                        {
                            if (string.IsNullOrWhiteSpace(args[2]))
                            {
                                Console.WriteLine("下载路径为空,程序退出 [PATH Empty Prog exit]");
                                return;
                            }
                            Console.WriteLine($"下载路径 [PATH]: {args[2]}");
                            Console.WriteLine("正在获取最新列表核对下载 [Getting Newest List for ()]");
                            if (int.TryParse(args[1], out int patternnum))
                            {
                                var kk = j.Data[patternnum].getLive2dPack().Data.DownloadModel(args[2]).GetAwaiter().GetResult();
                                foreach(var (f, FileStatus) in kk)
                                {
                                    Console.WriteLine($"{f} :: {FileStatus}");
                                }
                                return;
                            }
                            else
                            {
                                Console.WriteLine("您输入的key非正常数字,请检查 [Key is not a number]");
                                return;
                            }
                        }

                        if (k == "-v")
                        {
                            verbose = true;
                            Console.WriteLine("详细信息模式已开启 [verbose mode on]");
                        }
                        if (k == "-f")
                        {
                            forcecheck = true;
                            Console.WriteLine("强制文件完整性筛查已启用 [force Update mode on]");
                        }
                        if (k == "-d")
                        {
                            download = true;
                            Console.WriteLine("已启用下载 [Task Download Start]");
                        }
                    }

                    if (download)
                    {
                        if (args.Last() != null 
                            && args.Last() != "-v"
                            && args.Last() != "-d"
                            && args.Last() != "-f")
                        {
                            Download(args);
                            return;
                        }
                        else
                        {
                            Console.WriteLine("无下载路径 程序已退出 [No Download Path]");
                        }
                    }
                    else
                    {
                        Console.WriteLine("未发现 -d 参数, 不进行下载 [parameter -d isn't present, download cancelled]");
                    }
                }
                else
                {
                    Console.WriteLine("无操作 - 没有第一参数 [1st Parameter not present]");
                }
            }
            else
            {
                Console.WriteLine("无输入 - 请携带参数 [No Input Parameter]");
            }
        }
        private static void Download(string[] args)
        {
            int num = 0;
            int numx = 0;
            Console.WriteLine($"下载路径 [PATH] : {args[^1]}");
            Console.WriteLine("正在获取最新列表 [Getting Newest List()]");
            Console.WriteLine($"执行下载中 [On Download] - 总计 [Total]:{j.Data.Count}");
            foreach (var x in j.Data)
            {
                if (!forcecheck && System.IO.Directory.Exists(System.IO.Path.Combine(args[^1], "live2d", "chara", x.Value.assetBundleName)))
                {
                    Console.WriteLine($":E: {num++} / {j.Data.Count} [{x.Value.assetBundleName}]");
                    continue;
                }
                else
                {
                    try
                    {
                        Meow.Rinko.Core.Live2d.Live2dSingle dt = x.Value.getLive2dPack();//for doc gen.
                        var ax = dt.Data.DownloadModel(args[^1]).GetAwaiter().GetResult();
                        ax.ForEach((k) =>
                        {
                            if (verbose)
                            {
                                Console.WriteLine($"{k.FileStatus} :: {k.f}");
                            }
                            numx++;
                        });
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($":ERR: [{num}] [{x.Value.assetBundleName}] : {ex.Message}");
                    }
                    Console.WriteLine($":D: {num} / {j.Data.Count} [{numx}] [{x.Value.assetBundleName}]");
                    num++;
                }
            }
            Console.WriteLine();
            Console.WriteLine("---已完成 [Complete] ---");
        }
    }
}
