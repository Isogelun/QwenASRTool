using System;
using System.IO;
using System.Windows;
using Microsoft.Web.WebView2.Core;
using Microsoft.Web.WebView2.Wpf;

namespace QwenASRAPP
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Loaded += async (s, e) =>
            {
                try
                {
                    await webView.EnsureCoreWebView2Async();

                    // 添加错误处理
                    webView.CoreWebView2.NavigationCompleted += (sender, args) =>
                    {
                        if (!args.IsSuccess)
                        {
                            MessageBox.Show($"页面加载失败: {args.WebErrorStatus}", "错误");
                        }
                    };

                    // 添加开发者工具（调试时取消注释）
                    // webView.CoreWebView2.OpenDevToolsWindow();

                    // 加载 dist 文件夹（与 EXE 同目录）
                    var distPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "dist");

                    if (!Directory.Exists(distPath))
                    {
                        MessageBox.Show($"找不到 dist 目录！\n请确保 dist 文件夹与程序在同一目录。\n\n当前目录:\n{AppDomain.CurrentDomain.BaseDirectory}", "错误");
                        return;
                    }

                    // 使用虚拟主机映射来正确处理本地文件路径
                    webView.CoreWebView2.SetVirtualHostNameToFolderMapping(
                        "app.local",
                        distPath,
                        CoreWebView2HostResourceAccessKind.Allow
                    );

                    // 使用虚拟主机 URL
                    webView.CoreWebView2.Navigate("https://app.local/index.html");
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"初始化失败:\n{ex.Message}", "错误");
                }
            };
        }
    }
}